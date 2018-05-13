using UnityEngine;
using UnityEngine.UI;

public class scrollItem : ExtraScrollItem { 
    public Text text;
    public void OnEnable() {
        Debug.Log("dddd");
    }
    public override void Reset(int vId)
    {
        base.Reset(vId);
        text.text = id.ToString();
    }
}
