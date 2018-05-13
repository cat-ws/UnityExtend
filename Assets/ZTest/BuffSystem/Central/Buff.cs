using System;
using System.Collections.Generic;

public abstract class Buff  { 
    /// <summary>
    /// 用于区分,作用哪个或几个BridgeData数据
    /// </summary>
    public int id { get; private set; }
    /// <summary>
    /// 作用相同数据时，存在差异时使用
    /// 如：add方式（始终只有一个或几个存在等），区分不同类型做不同处理
    /// </summary>
    public int subid { get; private set; }
    /// <summary>
    /// 同id时计算时使用的优先级，（如某些部分需要先计算等）
    /// </summary>
    public int subPriority { get; private set; }
    /// <summary>
    /// 如果需要，可以记录buff创建者
    /// </summary>
    public object creater { get; protected set; }


    /// <summary>
    /// 标记当前buff是否生效,即在buffManager的list中
    /// </summary>
    public bool isValid { get; private set; }

    protected BuffManager m_manager;
    #region Collection define
    public class Collection
    {
        public int Count
        {
            get { return m_buffs.Count; }
        }

        List<Buff> m_buffs;

        public Collection()
        {
            m_buffs = new List<Buff>();
        }
        /// <summary>
        /// 加入列表，同一个buff不能重复添加
        /// </summary>
        /// <param name="buff"></param>
        /// <returns>如果是false说明buff对象已加入过</returns>
        public bool Add(Buff buff)
        {
            if (buff.isValid)
            {
                return false;
            }
            else
            {
                m_buffs.Add(buff);
                buff.isValid = true;
                return true;
            }
        }
        public void Remove(Buff buff)
        {
            buff.isValid = false;
            m_buffs.Remove(buff);
        }
        public void Clear() {
            m_buffs.Clear();
            m_buffs.TrimExcess();
        }
        public bool Contains(int id)
        {
            for (int i = 0; i < m_buffs.Count; ++i)
            {
                if (m_buffs[i].id == id)
                {
                    return true;
                }
            }
            return false;
        }
        public bool Contains(int id,int subid)
        {
            for (int i = 0; i < m_buffs.Count; ++i)
            {
                if (m_buffs[i].id == id && m_buffs[i].subid == subid)
                {
                    return true;
                }
            }
            return false;
        }
        public void Find(int id, ref List<Buff> ret)
        {
            ret.Clear();
            for (int i = 0; i < m_buffs.Count; ++i)
            {
                if (m_buffs[i].id == id)
                {
                    ret.Add(m_buffs[i]);
                }
            }
        }
        public void Find(int id,int subid, ref List<Buff> ret)
        {
            ret.Clear();
            for (int i = 0; i < m_buffs.Count; ++i)
            {
                if (m_buffs[i].id == id && m_buffs[i].subid == subid)
                {
                    ret.Add(m_buffs[i]);
                }
            }
        }
        public void Find(object creater, ref List<Buff> ret)
        {
            ret.Clear();
            for (int i = 0; i < m_buffs.Count; ++i)
            {
                if (m_buffs[i].creater == creater)
                {
                    ret.Add(m_buffs[i]);
                }
            }
        }
        public Buff this[int idx]
        {
            get
            {
                return m_buffs[idx];
            }
        }
       
    }
    #endregion
    public Buff(BuffManager mgr,int id_ , int subid_= -1,int subPriority_= 0, object creater_ = null)  {
        m_manager = mgr;
        creater = creater_;
        id = id_;
        subid = subid_;
        subPriority = subPriority_;
        isValid = false;
    }
    /// <summary>
    /// buff如何影响数值在这里实现
    /// </summary>
 
    public virtual void AddEffect() {
        m_manager.collection.Add(this);
    }
    /// <summary>
    /// 去掉buff的影响，以及buff自己的清理（计时器等）
    /// </summary>
    public virtual void RemoveEffect() {
        m_manager.collection.Remove(this);
    }



//以下为实现buff时可能用到的
    /// <summary>
    /// 如果需要更新当前已存在的buff时使用
    /// </summary>
    /// <param name="buff"></param>
    public virtual void Renewal(Buff buff) { }
    /// <summary>
    /// 更新buff中的数据，比如以次数限制buff，buff中的数值会发生改变等
    /// </summary>
    public virtual void UpdateEffect(){}
    /// <summary>
    /// 重置bridge中的关联数据
    /// </summary>
    public virtual void ResetRelBriData() { }
    /// <summary>
    /// 使buff修改中间值，（注意是分布的方式计算）
    /// </summary>
    public virtual void Apply() { }
    /// <summary>
    /// 注意需要实现 ResetRelBriData  Apply
    /// </summary>
    public void EffectsCalculateHelper(bool isIgnorePriority = true)
    {
        List<Buff> buffs = m_manager.GetBuffs(id,true);
        if (!isIgnorePriority) { buffs.Sort(Buff.ComparePriority); }
        
        ResetRelBriData();
        foreach (Buff buff in buffs) {
            buff.Apply();
        }
    }
    public static int ComparePriority(Buff a,Buff b){
        return b.subPriority - a.subPriority;
    }

}
