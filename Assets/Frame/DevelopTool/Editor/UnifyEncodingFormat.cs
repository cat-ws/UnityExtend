using UnityEngine;
using UnityEditor;
using System.Text;
using System.Collections;
namespace WsFrame
{
    public class UnifyEncodingFormat : MonoBehaviour
    {

        [MenuItem("Custom Editor/ToUTF8")]
        static void UnifySelected()
        {
            UnityEngine.Object[] SelectedAsset = Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.DeepAssets);
            string path = null;
            foreach (var a in SelectedAsset)
            {
                path = AssetDatabase.GetAssetPath(a);
                if (!path.Contains(".csv") || path.Contains(".meta"))
                {
                    continue;
                }
                /*
                byte[] bs = System.IO.File.ReadAllBytes(path);
                if (bs[0] != 0xEF || bs[1] != 0xBB)
                {
                    byte[] new_bs = Encoding.Convert(Encoding.GetEncoding("gb2312"), Encoding.UTF8, bs);
                    System.IO.File.WriteAllBytes(path, new_bs);
                    Debug.Log("文件 " + path + " 编码格式转至UTF8");
                    EditorUtility.SetDirty(a);
                }
                 */

                System.IO.File.WriteAllText(path,  System.IO.File.ReadAllText(path, Encoding.Default), Encoding.UTF8); 
                Debug.Log("文件 " + path + " 编码格式转至UTF8");
                EditorUtility.SetDirty(a);
          

            }
            AssetDatabase.SaveAssets();
            EditorUtility.DisplayDialog("UTF8", "UTF8格式转化完成", "确定");
        }
    }
}