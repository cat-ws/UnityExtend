using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
/// <summary>
/// 功能：只创建有限个item，在显示范围变化时不断刷新
/// 1、创建继承自ExtrScrollItem的item，实现其reset过程
///     （scrollView会用用户提供的id调用item的Reset）
///     （初始需要数据,可存储在静态数据中（注意释放），或其他地方）
/// 2、在编辑器中创建一个ScrollView ,替代为ExtrVerticalScrollView，
///     （注意滑条不要设置Auto Hide And Expand ViewPort，需要可以用Auto Hide加调整滑条位置代替）
/// 3、可选(可在编辑界面设置)：SetConfig 如果需要设置方向，间距，item缓存数量 在ResetScroll前
/// 4、ResetScroll
/// 5、SetItemIds 可以实现删除，添加，重排
/// </summary>
public class ExtraScrollView : ScrollRect
{
	//public members
    [Header("Extend参数")]
    [SerializeField] int m_cacheNum = 0;
    [Space(5)]
    [SerializeField] LayoutDirection m_layoutDirection = LayoutDirection.Vertical;
    [SerializeField] Layout m_layout = new Layout(0,0,0,0);
    [Space(5)][Header("             (0,0)不强制修改item预设尺寸")]
    [SerializeField] Vector2 m_assignItemSize=Vector2.zero;
	//property
	//privite or protected members
    bool m_isValid = false;
    List<int> m_itemIds = new List<int>();
    ExtraScrollItem m_itemPre;
    List<ExtraScrollItem> m_cacheitems = new List<ExtraScrollItem>();
    protected Vector2 m_itemSize;
    protected Vector2 m_viewportSize;

    UnityEngine.Object m_manager;//用于事件统一处理
    IndexRange m_curContentRange = IndexRange.NullRange;//现有缓存条目的显示范围

    //methods
    /// <summary>
    /// 设置布局，缓冲数量等
    /// 注意：在ResetScroll前调用
    /// </summary>
    /// <param name="dir"></param>
    /// <param name="cachenum"></param>
    /// <param name="layout"></param>
    public void SetConfig(LayoutDirection dir = LayoutDirection.Vertical, int cachenum=0, Layout layout=new Layout())
    {
        if (m_isValid) return;
        m_layoutDirection = dir;
        m_layout = layout;
        m_cacheNum = cachenum;
    }
    /// <summary>
    /// 重置Scroll，
    /// </summary>
    /// <param name="ids"></param>
    /// <param name="pre"></param>
    public void ResetScroll( List<int> ids,ExtraScrollItem pre, UnityEngine.Object manager = null)
    {
        Clear();
        if (pre == null) return;
        if (viewRect.rect == Rect.zero)
        {
            Debug.LogError("viewRect为0：如果使用了Auto Hide And Expand ViewPort，请用Auto Hide加调整滑条位置代替");
            return;
        }

        m_manager = manager;
        m_itemPre = pre;
        m_itemPre.rect.pivot = new Vector2(0.5f,0.5f);
        if (m_assignItemSize != Vector2.zero) {
            m_itemPre.rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,m_assignItemSize.x);
            m_itemPre.rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, m_assignItemSize.y);
        }

        m_itemIds = ids;//new List<int>(ids);
        m_itemSize = m_itemPre.rect.rect.size;
        m_viewportSize = viewRect.rect.size;

        m_isValid = true;
        resetContentSize();
        createCacheItems();
        UpdateView();     
    }
    /// <summary>
    /// 如果是ref，并修改过内容，需要刷新后生效
    /// </summary>
    /// <param name="isRef"></param>
    /// <returns></returns>
    public List<int> GetItemIds(bool isRef=true) {
        if (isRef){
            return m_itemIds;
        }
        else {
            return new List<int>(m_itemIds);
        }
    }
    /// <summary>
    /// 强制刷新显示
    /// </summary>
    public void Refrush() {
        resetContentSize();
        m_curContentRange = IndexRange.NullRange;
        UpdateView();
    }
    /// <summary>
    ///  可以实现删除，添加，重排的效果
    ///  注意：在resetScroll 后使用
    /// </summary>
    /// <param name="list"></param>
    public void SetItemIds(List<int> list) {
        if (!m_isValid) return;
        
        m_itemIds = list;//new List<int>(list);
        Refrush(); 
    }
    
    /// <summary>
    /// 清空所以数据和view
    /// </summary>
    public void Clear()
    {
        m_itemPre = null;
        m_itemSize = Vector2.zero;
        m_curContentRange = IndexRange.NullRange;
        m_itemIds.Clear();
        foreach (ExtraScrollItem item in m_cacheitems)
        {
            GameObject.Destroy(item.gameObject);
        }

        m_cacheitems.Clear();
        content.transform.localPosition = Vector2.zero;
        m_manager = null;
        m_isValid = false;
    }
    protected override void Awake()
    {
        base.Awake();
        //以防不小心修改了pivot
        content.pivot = new Vector2(0,1);
        viewRect.pivot = new Vector2(0,1);
        onValueChanged.AddListener(ValueChanged);
    }
    
    protected virtual void ValueChanged(Vector2 vec)
    {
        UpdateView();
    }

    protected void UpdateView() {
        if (!m_isValid ) return;
        //计算view range
        Vector2 curContentPos = content.localPosition;
        int minIndex = -1;
        int maxIndex = -1;
        switch (m_layoutDirection)
        {
            case LayoutDirection.Horizontal:
                float minViewX = -curContentPos.x-m_layout.originSpace;
                float maxViewX = minViewX + m_viewportSize.x;
                minIndex = Mathf.CeilToInt(minViewX / (m_itemSize.x+m_layout.spacing))-1;
                maxIndex = Mathf.CeilToInt(maxViewX / (m_itemSize.x+m_layout.spacing))-1;
                break;
            case LayoutDirection.Vertical:
                float minViewY = curContentPos.y-m_layout.originSpace;
                float maxViewY = minViewY + m_viewportSize.y;
                minIndex = Mathf.CeilToInt(minViewY / (m_itemSize.y+m_layout.spacing))-1;
                maxIndex = Mathf.CeilToInt(maxViewY / (m_itemSize.y+m_layout.spacing))-1;
                break;     
        }

        IndexRange viewRange = new IndexRange(minIndex, maxIndex);
        viewRange.Clamp(0,m_itemIds.Count-1);
        //更新view
        if (viewRange.IsInRange(m_curContentRange)) return;

        int dif = m_cacheitems.Count - viewRange.count;
        dif = Mathf.Max(0, dif);
        int startIdx = viewRange.minIndex - dif / 2;
        m_curContentRange = new IndexRange(startIdx, startIdx + m_cacheitems.Count-1);
        m_curContentRange.Clamp(0,m_itemIds.Count-1);
        showItems();
    }
    #region inner method
    void resetContentSize()
    {
        float xsize = 0;
        float ysize = 0;
        switch (m_layoutDirection)
        {
            case LayoutDirection.Horizontal:
                xsize = m_itemSize.x * m_itemIds.Count + m_layout.spacing * (m_itemIds.Count -1);
                ysize = m_viewportSize.y;
                xsize += m_layout.originSpace + m_layout.trailSpace;
                break;
            case LayoutDirection.Vertical:
                xsize = m_viewportSize.x;
                ysize = m_itemSize.y * m_itemIds.Count + m_layout.spacing * (m_itemIds.Count - 1);
                ysize += m_layout.originSpace + m_layout.trailSpace;
                break;
        }

        content.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, xsize);
        content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, ysize);
    }
    void createCacheItems()
    {
        if (m_cacheitems.Count != 0 || m_itemSize.y == 0 || m_itemSize.x == 0) {
            Debug.LogError("Scrollview:初始item失败，可能size为0，或存在未清除缓存");
            return; }
        int needNum = -2;
        if (m_layoutDirection == LayoutDirection.Horizontal)
        { needNum = Mathf.CeilToInt(m_viewportSize.x /( m_itemSize.x + m_layout.spacing)); }
        else if (m_layoutDirection == LayoutDirection.Vertical)
        { needNum = Mathf.CeilToInt(m_viewportSize.y / (m_itemSize.y + m_layout.spacing)); }


        needNum = Mathf.Max(m_cacheNum, needNum + 2);
        for (int i = 0; i < needNum; i++)
        {
            ExtraScrollItem item = GameObject.Instantiate<ExtraScrollItem>(m_itemPre, content.transform, false);
            item.SetManager(m_manager);
            m_cacheitems.Add(item);
            item.gameObject.SetActive(false);
        }
    }


    void showItems() {

        for (int i = 0; i < m_curContentRange.count; ++i)
        {
            int idx = m_curContentRange.minIndex + i; 
            ExtraScrollItem it = m_cacheitems[i];
            it.Reset(m_itemIds[idx]);
            it.rect.localPosition = getPosAtIndex(idx);
            it.gameObject.SetActive(true);
        }
        for (int i = m_curContentRange.count; i < m_cacheitems.Count;++i ) {
            ExtraScrollItem it = m_cacheitems[i];
            it.gameObject.SetActive(false);
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="index">从0开始</param>
    /// <returns></returns>
    Vector2 getPosAtIndex(int index)
    {
        
        float x = 0;
        float y = 0;
        switch (m_layoutDirection)
        {
            case LayoutDirection.Horizontal:
                x = m_itemSize.x * (index + 1 - 0.5f)+ index * m_layout.spacing + m_layout.originSpace;
                y = -content.rect.height * 0.5f + m_layout.offsetInOppsite;
                break;
            case LayoutDirection.Vertical:
                x = content.rect.width * 0.5f + m_layout.offsetInOppsite;
                y = m_itemSize.y * (0.5f - index - 1) -index * m_layout.spacing -m_layout.originSpace ;
                break;
        }
        return new Vector2(x,y);
    }
    #endregion

    #region  define inner type
    public enum LayoutDirection
    {
        Horizontal,
        Vertical,
    }
    [Serializable]
    public struct Layout {
        public float spacing;
        public float originSpace;
        public float trailSpace;
        public float offsetInOppsite ;
        public Layout(float pSpacing, float pOriginSpace, float pTrailSpace, float pOffsetInOppsite)
        {
            spacing = pSpacing;
            originSpace = pOriginSpace;
            trailSpace = pTrailSpace;
            offsetInOppsite = pOffsetInOppsite;
        }
    }
    /// <summary>
    /// id从0开始
    /// </summary>
    struct IndexRange
    {
        public static readonly IndexRange NullRange = new IndexRange(-1, -2);
        public int minIndex { get; private set; }
        public int maxIndex { get; private set; }
        public int count { get { return maxIndex - minIndex + 1; } }
        public IndexRange(int min, int max)
        {
            minIndex = min;
            maxIndex = max;
        }
        public bool IsInRange(IndexRange otherRange)
        {
            if (otherRange == IndexRange.NullRange || this == IndexRange.NullRange) 
                return false;

            bool ret = minIndex < otherRange.minIndex || maxIndex > otherRange.maxIndex;
            return !ret;
        }
        public void Clamp(int min, int max)
        {
            if (min > max)
            {
                this = IndexRange.NullRange;
            }
            else
            {
                minIndex = Mathf.Clamp(minIndex, min, max);
                maxIndex = Mathf.Clamp(maxIndex, min, max);
            }
        }
        public override string ToString()
        {
            return String.Format("range:({0},{1})",minIndex,maxIndex);
        }
        public static IndexRange Clamp(IndexRange src, int min, int max)
        {
            src.Clamp(min, max);
            return src;
        }
        public static bool operator ==(IndexRange a, IndexRange b)
        {

            return a.minIndex == b.minIndex && a.maxIndex == b.maxIndex;
        }
        public static bool operator !=(IndexRange a, IndexRange b)
        {
            return !(a == b);
        }
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
    #endregion
}
