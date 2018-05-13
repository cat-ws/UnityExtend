using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Sprites;
using System.IO;
public class AtlasTool : MonoBehaviour
{
    #region 配置：图片格式，path等
    //配置信息
    static string SpritesDir = Application.dataPath + "/Sprites/Atlas";//图集图片所在全路径
    //以下通常不修改
    //导出的AtlasCollection文件 全路径（在Resources下的,使用AtlasManager中的值，保证读取时目录一致）
    static string AtlasCollectionsDir = Application.dataPath + "/Resources/" + AtlasManager.AtlasCollisionInResDir;
    #endregion
    const string format = "t:Sprite";// 匹配的图片格式

    [MenuItem("Custom Editor/ExportAtlasPfb")]
    public static void ExportAtlasPfb()
    {
        if (!Directory.Exists(SpritesDir))
        {
            EditorUtility.DisplayDialog("提示","图集图片所在目录不存在","知道了");
            return;
        }

        EditorUtility.DisplayProgressBar("", "删除之前的AtlasCollection文件", 0f);
        Dictionary<string, AtlasCollection> pres = new Dictionary<string, AtlasCollection>();
        if (Directory.Exists(AtlasCollectionsDir))
        {
            Directory.Delete(AtlasCollectionsDir, true);
            AssetDatabase.SaveAssets();
        }

        Directory.CreateDirectory(AtlasCollectionsDir);
        EditorUtility.DisplayProgressBar("", "Rebuild图集", 0.2f);
        //Rebuild
        Packer.RebuildAtlasCacheIfNeeded(EditorUserBuildSettings.activeBuildTarget,false);

        EditorUtility.DisplayProgressBar("", "重新生成图集的AtlasCollection文件", 0.5f);


        //收集数据
        #region c#方法获得文件
        /*
        DirectoryInfo rootDirInfo = new DirectoryInfo(atlasDir);
        foreach (FileInfo pngFile in rootDirInfo.GetFiles(".png", SearchOption.AllDirectories))
        {
            string allPath = pngFile.FullName;
            string assetPath = allPath.Substring(allPath.IndexOf("Assets"));
        */
        #endregion

        string[] atlasSpritePath = new string[] { SpritesDir.Substring(SpritesDir.IndexOf("Assets")) };
        string[] result = AssetDatabase.FindAssets(format, atlasSpritePath);

        for (int i = 0; i < result.Length; ++i)
        {
                string assetPath = AssetDatabase.GUIDToAssetPath(result[i]);
                Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(assetPath);

                if (sprite == null || !sprite.packed)
                {
                    continue;

                }
                Texture2D tmp;
                string atlasName;

                Packer.GetAtlasDataForSprite(sprite, out atlasName, out tmp);
                //没有相应的图集问价AtlasCollection，则新建一个
                if (!pres.ContainsKey(atlasName))
                {
                    AtlasCollection atl = createAsset(atlasName);
                    pres.Add(atlasName,atl);
                }           
                pres[atlasName].sprites.Add(sprite);
            }
        EditorUtility.DisplayProgressBar("", "保存资源", 0.8f);
        //保存
        foreach (AtlasCollection obj in pres.Values)
        {
            EditorUtility.SetDirty(obj);   
        }
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        EditorUtility.ClearProgressBar();
    }
    static AtlasCollection createAsset(string atlasName )
    {
        string prefabPath = AtlasCollectionsDir + "/" + atlasName + ".asset";
        prefabPath = prefabPath.Substring(prefabPath.IndexOf("Assets"));
        AtlasCollection scriptableObj = ScriptableObject.CreateInstance<AtlasCollection>();
        UnityEditor.AssetDatabase.CreateAsset(scriptableObj, prefabPath);

        //  UnityEditor.AssetDatabase.SaveAssets();
        //  UnityEditor.AssetDatabase.Refresh();

        return AssetDatabase.LoadAssetAtPath<AtlasCollection>(prefabPath);
    } 

}
