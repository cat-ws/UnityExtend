using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
/// <summary>
/// 1、创建ExtraToggle 见其说明
/// 2、使用RegisterToggle方法加toggle，不要用toggle.group赋值
/// 3、监听事件onToggleValueChanged
/// </summary>
public class ExtraToggleGroup : ToggleGroup {
    public event Action<ExtraToggle, bool> onToggleValueChanged;
    public List<ExtraToggle> toggles = new List<ExtraToggle>();
    protected override void Awake()
    {
        base.Awake();
        foreach (ExtraToggle tg in toggles)
            tg.InitGroupCallback(this);
    }
    public void RegisterToggle(ExtraToggle toggle) {
        if (toggle == null) return;
        //base.RegisterToggle(toggle);//原方法无效的
        toggle.InitGroupCallback(this);
        toggles.Add(toggle);
    }
    public void UnregisterToggle(ExtraToggle toggle) {
        //base.RegisterToggle(toggle);//原方法无效的
        if (toggle){
            toggle.RemoveGroupCallback();
            toggles.Remove(toggle);
        }
    }
    public void ValueChanged(ExtraToggle toggle,bool isOn) {
        if(onToggleValueChanged != null){
            onToggleValueChanged(toggle,isOn);
        }
    }
}
