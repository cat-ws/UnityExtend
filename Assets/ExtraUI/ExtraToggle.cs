using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// unity编辑器创建Toggle，然后替换为ExtrToggle
/// </summary>
public class ExtraToggle : Toggle {
    [Header("Extrend参数")]
    public int id=0;

    bool m_preIsOn;

    public void InitGroupCallback(ToggleGroup gp )
    {
          group = gp;
          isOn = false;
          m_preIsOn = isOn;
          this.onValueChanged.AddListener(GroupCallback);
    }
    public void RemoveGroupCallback()
    {
         group = null;
         this.onValueChanged.RemoveListener(GroupCallback);
    }
    protected override void OnDestroy(){
        base.OnDestroy();
        ExtraToggleGroup vGroup = group as ExtraToggleGroup;
        if (vGroup != null) {
            vGroup.UnregisterToggle(this);
        }
    }
    public void GroupCallback(bool isOn) {

        ExtraToggleGroup vGroup = group as ExtraToggleGroup;
        if (vGroup != null && m_preIsOn != isOn)
        {
            vGroup.ValueChanged(this, isOn);
        }
        m_preIsOn = isOn;
    } 
}
