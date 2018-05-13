using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//[CreateAssetMenu(fileName = "newAtlas", menuName = "AtlasCollection")]
[System.Serializable]
public class AtlasCollection : ScriptableObject {

    public List<Sprite> sprites = new List<Sprite>();

    Dictionary<string,int> _queryTable = null;
    Dictionary<string, int> queryTable {
        get {
            if (_queryTable == null) {
                Debug.Log("加载图集:"+this.name);
                _queryTable = new Dictionary<string, int>();
                for (int i = 0; i < sprites.Count;++i ) {
                    if(sprites[i]==null){
                        Debug.Log("<color=red>图集：" + this.name+" 存在null的sprite引用</color>" );
                        continue;
                    }

                    if (!_queryTable.ContainsKey(sprites[i].name)) {
                        _queryTable.Add(sprites[i].name,i);
                    }
                }   
            }
            return _queryTable;
        }
    
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="spriteName"></param>
    /// <returns></returns>
    public Sprite Find(string spriteName)
    {
        return findByDictionary(spriteName);
    }


    Sprite findByDictionary(string spriteName)
    {
        int index;
        if (queryTable.TryGetValue(spriteName, out index))
        {
            return sprites[index];
        }
        else
        {
            return null;
        }
    }
    //暂不使用
    Sprite findByList(string name) { 
        for(int i =0;i<sprites.Count;++i){
            if (sprites[i] == null) continue;
            if (sprites[i].name == name) {
                return sprites[i];
            }
        }
        return null;
    }
     
}
