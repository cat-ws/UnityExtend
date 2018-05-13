using UnityEngine;

namespace WsFrame
{
    /// <summary>
    /// 只需继承此类则会成为单例，在游戏过程中只有一个实例
    /// 注意：
    /// 1可以直接建好容器拖放脚本，也可以通过CreateInstance方法自动创建(创建多个单例的方法也是单例)；！！！不要用两种方法创建一个单例
    /// 2CreateInstance创建单例前设置ContainerName,可以修改自动生成容器的名字（当然是在没指定容器的情况下）
    /// 3需要Awake 处理事件时override实现Awake（需要每次加载都执行的初始化，请实现 新SceneManager.sceneLoaded  旧OnLevelWasLoaded,）
    /// 
    /// </summary>
    /// <typeparam name="T">
    /// T为希望成为单例的类型
    /// </typeparam>
    [DisallowMultipleComponent]
    public abstract class USingleton<T> : MonoBehaviour
    where T : Component
    {
        private static T _instance;
        [HideInInspector]
        public static string ContainerName = typeof(T).ToString() + "Singleton";
        public static T Instance {
            get{
                if (_instance==null)
                {
                      CreateInstance();
                }
                return _instance;
            }
        }
        /// <summary>
        /// 主动创建一个单例，如果已存在则返后false
        /// </summary>
        /// <param name="container"></param>
        /// (可选参数）指定容器,未指定则根据期望的ContainerName，创建容器
        /// <returns></returns>
        public static bool CreateInstance(GameObject container = null)
        {
            if (_instance != null) {
                return false; 
            }
            if (container == null) { 
                container = new GameObject(ContainerName);
            }
            // obj.hideFlags = HideFlags.HideAndDontSave;
            _instance = container.AddComponent<T>();
            return true;
        }
        /// <summary>
        /// 只删除组件，不删除容器
        /// </summary>
        public static void  DeleteInstanceOnly(){
            if(_instance!=null){
                Destroy(_instance);
            }
        }

       protected virtual void Awake()
        {       
            if (_instance==null){
                _instance = this as T;
                ContainerName = gameObject.name;
            }
            else {
                Destroy(gameObject);
                return;
            }
            DontDestroyOnLoad(gameObject);
        }
    }
}