using System.Collections.Generic;
using UnityEngine;

public class BuffManager {
    /// <summary>
    /// 除了buffbase 不要随意add或remove
    /// </summary>
    public Buff.Collection collection { get { return m_collection; } }
    public BridgeData bridgeData { get { return m_bridgeData; } }

    Buff.Collection m_collection = new Buff.Collection();
    BridgeData m_bridgeData;

    List<Buff> m_findedTmp = new List<Buff>();//用于查询优化
    public BuffManager(BridgeData bData) {
        m_bridgeData = bData;
        m_bridgeData.Reset();
    }
    public void Clear()
    {
        for (int i = 0; i < m_collection.Count; )
        {
            Buff buff = m_collection[i];
            buff.RemoveEffect();
        }
        m_collection.Clear();
        m_bridgeData.Reset();
    }

//auxiliary method 如果特殊需求，可以直接使用collection的方法查询
    public bool Contains(int id)
    {
        return m_collection.Contains(id);
    }
    public bool Contains(int id, int subid) {
        return m_collection.Contains(id,subid);
    }

    /// <param name="isUseTmp">true使用tmp来返回结果，false新建一个list返回结果</param>
    public List<Buff> GetBuffs(int id, bool isUseTmp)
    {
        List<Buff> ret = isUseTmp ? m_findedTmp : new List<Buff>();
        m_collection.Find(id, ref ret);
        return ret;
    }
    /// <param name="isUseTmp">true使用tmp来返回结果，false新建一个list返回结果</param>
    public List<Buff> GetBuffs(int id,int subid, bool isUseTmp )
    {
        List<Buff> ret = isUseTmp ? m_findedTmp : new List<Buff>();
        m_collection.Find(id,subid,ref ret);
        return ret;
    }
    /// <param name="isUseTmp">true使用tmp来返回结果，false新建一个list返回结果</param>
    public List<Buff> GetBuffs(object creater, bool isUseTmp)
    {
        List<Buff> ret = isUseTmp ? m_findedTmp : new List<Buff>();
        m_collection.Find(creater, ref ret);
        return ret;
    }
    public void RemoveBuffs(int id) {
        List<Buff> result = GetBuffs(id, false);
        for (int i = 0; i < result.Count;++i ) {
            result[i].RemoveEffect();
        }
    }
    public void RemoveBuffs(int id ,int subid) {
        List<Buff> result = GetBuffs(id,subid, false);
        for (int i = 0; i < result.Count; ++i)
        {
            result[i].RemoveEffect();
        }
    }
    public void RemoveBuffs(object creater)
    {
        List<Buff> result = GetBuffs(creater, false);
        for (int i = 0; i < result.Count; ++i)
        {
            result[i].RemoveEffect();
        }
    }
}
