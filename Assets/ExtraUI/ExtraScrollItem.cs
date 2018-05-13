using UnityEngine;
/// <summary>
/// 1、重写Reset根据数据初始显示
/// 2、初始需要数据,可存储在manager中
/// </summary>
[RequireComponent(typeof(RectTransform))]
public abstract class ExtraScrollItem : MonoBehaviour {
    //propertys
    RectTransform _rect;
    public RectTransform rect { get { 
        if(!_rect ){
            _rect = GetComponent<RectTransform>();
        }
        return _rect;
    } }
    int _id = 0;
    public int id { get { return _id; } protected set { _id = value; } }
	//methods
    public virtual void SetManager(UnityEngine.Object manager) { }
    public virtual void Reset(int vId) {
        id = vId;
    }

}
