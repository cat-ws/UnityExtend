using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace WsFrame
{
    /// <summary>
    /// 使用（）：
    ///1预设体：在Resource中建预设体GameObject对象,(gameObject上必须有一个并且只有一个继承UMemoryItem的脚步)
    ///2创建对象：使用UMemoryManager.Instantiate<T>创建实例(会调用Reset和 SetActive（true）)，
    ///3回收对象：UMemoryManager.Destroy (会将gameobject禁用SetActive(fase：当对象禁用时将执行内存的回收）
    ///
    ///注意：
    ///SetCapcity可以设置各最大池容量
    ///使用GameObject.Destroy 摧毁对象 将从管理中移除（不再复用）
    /// </summary>
    public class UMemoryManager:USingleton<UMemoryManager>
    {   //单例部分代码
        /*
        private static UMemoryManager _instance;

        public static UMemoryManager Instance {
            get { 
                if(_instance == null){
                    _instance = new UMemoryManager();
                }
                return _instance;
            }
        }
        private UMemoryManager() { }
        */
        //内部类
       public abstract class  UMemoryItem : MonoBehaviour {
            /// <summary>
            /// 任何时候不要修改（管理内存使用）
            /// </summary>
            [HideInInspector]
            public string ResorceName;
            /// <summary>
            /// 任何时候不要修改（管理内存使用）
            /// </summary>
            [HideInInspector]
            public bool IsUnderManager = false;
            public virtual void Reset(){}

            //protected virtual void  OnEnable(){
        
           // }

            protected virtual void  OnDisable(){
                if (IsUnderManager)
                {
                    UMemoryManager.Instance.RecollectInstance(this);
                }
            }
            protected virtual void OnDestroy() {
                if (IsUnderManager)
                {
                    UMemoryManager.Instance.RemoveObject(this);
                }
            }
        }

        //实现部分
        /// <summary>
        /// 维护由管理类生成的对象
        /// </summary>
        private List<UMemoryItem> createItems = new List<UMemoryItem>();
        /// <summary>
        /// 备用的对象
        /// </summary>
        private Dictionary<string, List<UMemoryItem>> memoryPools = new Dictionary<string, List<UMemoryItem>>();
        private Dictionary<string, GameObject> loaded = new Dictionary<string, GameObject>();
        private Dictionary<string, int> capacity = new Dictionary<string, int>();
       //外部使用接口
        public static T Instantiate<T>(string resourceName) where T:UMemoryItem
        {
            UMemoryItem ret = Instance.GetAInstance(resourceName,typeof(T).Name);
            if (ret == null) {
                return null;
            }
            else {
                return ret as T;
            }
        }
        public static void Destroy(UMemoryItem item) {
            if (item.IsUnderManager)
            {
                item.gameObject.SetActive(false);
            }
            else {
                GameObject.Destroy(item);
            }
        }
        public static void SetCapcity(string resorceName, int value)
        {
            Instance.capacity[resorceName] = value;
        }
        //内部实现

        /// <summary>
       /// 通过对象池创建对象
       /// </summary>
       /// <param name="resorceName"></param>
       /// Resource中 的路径
       /// <param name="itemName"></param>
        /// 该gameobject 下继承UMemoryItem的脚本名
       /// <returns></returns>
        private UMemoryItem GetAInstance(string resorceName,string itemName)
        {

            //初始池信息
            if (!memoryPools.ContainsKey(resorceName))
            {          
                UMemoryItem  item = tryCreateItem(resorceName,itemName);
                if (item)
                {
                    AddToPool(item.ResorceName,item);              
                }
                else {
                    return null;
                }
                
            }
            List<UMemoryItem> pool = memoryPools[resorceName];
            //尝试获得对象
            UMemoryItem it ;
           
            if (pool.Count == 0)
            {
                it = tryCreateItem(resorceName,itemName);
            }
            else {

                it = pool[pool.Count - 1];
                
                if (it.GetType().Name == itemName)
                {
                    pool.RemoveAt(pool.Count - 1);
                }
                else {
                
                    Debug.Log("<color=red> " + itemName + "not in *" + resorceName + " </color>");
                    it = null;
                }
            }

            if(it == null){
                return null;
            }
            else{
                it.Reset();
                it.gameObject.SetActive(true);
                return it;
            }
        }
        private void RecollectInstance(UMemoryItem item) {

            AddToPool(item.ResorceName, item);
        }
        private void RemoveObject(UMemoryItem item) {
            RemoveFromPool(item.ResorceName, item);
        }

       

        private void AddToPool(string rename, UMemoryItem item)
        {
			if (string.IsNullOrEmpty(rename) || !item.IsUnderManager) {
                return;
            }
            
            if (!memoryPools.ContainsKey(rename))
            {
                memoryPools.Add(rename,new List<UMemoryItem>());
            }
            List<UMemoryItem> pool = memoryPools[rename];
            if (capacity.ContainsKey(rename) && pool.Count >= capacity[rename])
            {
                GameObject.Destroy(item.gameObject);
                return;
            }
      
            pool.Add(item);
        }
        private void RemoveFromPool(string rename,UMemoryItem item) {
            if (item.IsUnderManager)
            {
                item.IsUnderManager = false;
                createItems.Remove(item);
            }


            if (!memoryPools.ContainsKey(rename)) {
                return;
            }
            List<UMemoryItem> pool = memoryPools[rename];
            pool.Remove(item);
        }
        private UMemoryItem tryCreateItem(string resorceName,string itemName) {
            if (!loaded.ContainsKey(resorceName))
            {

                GameObject obj = (GameObject)Resources.Load(resorceName);
                if (obj == null)
                {
                  
                    Debug.Log("<color=red> " + resorceName + "   not in Resource </color>");
                    return null;
                }
                else
                {

                    var item = obj.GetComponent(itemName) as UMemoryItem;
                    if (item == null)
                    {
                        Debug.Log("<color=red> " + itemName + " not in " + resorceName + " </color>");
          
                        return null;
                    }
                    else {
                        item.ResorceName = resorceName;
                        item.IsUnderManager = true;
                        loaded.Add(resorceName,obj);
                    }

                }
            }
            GameObject go = MonoBehaviour.Instantiate(loaded[resorceName]) as GameObject;
            UMemoryItem ret = go.GetComponent(itemName) as UMemoryItem;
            if (ret == null)
            {
                Debug.Log("<color=red> " + itemName + "not in " + resorceName + " </color>");
                GameObject.Destroy(go);
                return null;
            }
            createItems.Add(ret);
            return ret;
        }
        void OnDestroy() {
            
            foreach (UMemoryItem v in createItems)
            {
                v.IsUnderManager = false;
                GameObject.Destroy(v.gameObject);
            }
            createItems.Clear();
            memoryPools.Clear();
        }
    }
    
        
}
