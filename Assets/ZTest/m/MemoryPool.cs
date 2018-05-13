using System.Collections;
using System.Collections.Generic;

public class MemoryPool<T> {
    private int m_capacity ;
    protected List<T> m_recollectPool; 
    /// <summary>
    /// 
    /// </summary>
    /// <param name="capacity"><0时为无限（默认-1）</param>
    public MemoryPool(int capacity = -1){
        m_capacity = capacity;
        m_recollectPool = new List<T>();
    }
    /// <summary>
    /// 当超过capacity时回收将失败，注意处理释放
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public bool Recollect(T obj) {
        if (m_capacity < 0 || m_capacity > m_recollectPool.Count){
            AddToPool(obj);
            return true;
        }
        else {
            return false;
        }
    }
    protected virtual void AddToPool(T obj)
    {
        m_recollectPool.Add(obj);
    }
      
    public T TryGetObject() { 
        T ret = default(T);
        if(m_recollectPool.Count>0){
            ret = m_recollectPool[m_recollectPool.Count-1];
            m_recollectPool.RemoveAt(m_recollectPool.Count-1); 
        }
        return ret;
    }
    public List<T> GetRecollectPool(){
        return m_recollectPool;
    }
    /// <summary>
    /// 只是简单的清除了list，缓存的对象可能没有释放
    /// </summary>
    public void ClearRef(){
        m_recollectPool.Clear();
        m_recollectPool.TrimExcess();
    }
}
