using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
#region  ExtrScrollViewEdtor
[CustomEditor(typeof(ExtraScrollView))]
public class ExtrScrollViewEdtor : Editor
{
    ExtraScrollView m_target;
    void OnEnable() {
        m_target = target as ExtraScrollView;
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (m_target.horizontalScrollbarVisibility == ScrollRect.ScrollbarVisibility.AutoHideAndExpandViewport)
            m_target.horizontalScrollbarVisibility = ScrollRect.ScrollbarVisibility.AutoHide;
        if (m_target.verticalScrollbarVisibility == ScrollRect.ScrollbarVisibility.AutoHideAndExpandViewport)
            m_target.verticalScrollbarVisibility = ScrollRect.ScrollbarVisibility.AutoHide;
    }
}
#endregion


#region  ExtrToggle
[CustomEditor(typeof(ExtraToggle))]
public class ExtrToggleEdtor : Editor {
    ExtraToggle m_target;
    void OnEnable() { m_target = target as ExtraToggle; }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (m_target.group != null) m_target.group = null;
    }
}
#endregion