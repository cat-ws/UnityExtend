using UnityEngine;
namespace UIFrame
{
    public abstract class UIBase:MonoBehaviour
    {
        public bool isCloseOnUnloadScene = true;
        public int layer = 0;
        public void Close() {
            UIManager.Instance.CloseUI(this);
        }
        
    }

}