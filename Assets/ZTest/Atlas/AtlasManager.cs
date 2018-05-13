using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AtlasManager
{
    #region 配置
    //Resources下的目录
    public const string AtlasCollisionInResDir = "Atlas";
    #endregion


    const string m_atlasInResPath = AtlasCollisionInResDir + "/";
    static Dictionary<string, AtlasCollection> m_cachedAtlas = new Dictionary<string, AtlasCollection>();

    public static Sprite GetSprite(string atlasName, string spriteName) {
        Sprite ret = null;
        AtlasCollection atlas = GetAtlas(atlasName);

        if (atlas != null)
        {
            ret = atlas.Find(spriteName);
        }
        atlas = null;
       return ret;
    }
    /// <summary>
    /// 注意返回的引用不要保留
    /// </summary>
    /// <param name="atlasName"></param>
    /// <returns></returns>
    static AtlasCollection GetAtlas(string atlasName)
    {
        return Resources.Load<AtlasCollection>(m_atlasInResPath + atlasName);
    }
    /// <summary>
    /// 预先加载atlas
    /// </summary>
    /// <param name="atlasNames"></param>
    public static void PreLoad(string[] atlasNames){
        AtlasCollection ac=null;
        for (int i = 0; i < atlasNames.Length; ++i) {
            ac = GetAtlas(atlasNames[i]);
        }
        ac = null;
    }
    /// <summary>
    /// （慎用）保留atlas的资源引用，使得Resource.UnloadUnusedAssets不能将其卸载
    /// </summary>
    /// <param name="atlasNames"></param>
    public static void Cache(string[] atlasNames)
    {
        for (int i = 0; i < atlasNames.Length;++i )
        {
            if (!m_cachedAtlas.ContainsKey(atlasNames[i]))
            {
                AtlasCollection ac = GetAtlas(atlasNames[i]);
                if (ac != null){
                    m_cachedAtlas.Add(atlasNames[i], ac);
                    ac = null;
                }
            }
        }
    }
    public static void ClearCache(){
        m_cachedAtlas.Clear();
        Resources.UnloadUnusedAssets();
    }
}
