using UnityEngine;
using System.Collections;
using System.IO;
namespace WsFrame{
    public static class FileHelper  {
        public static string GetTextFromResource(string fileName, bool unloadAfterFnish = true)
        {
            TextAsset tex = Resources.Load(fileName) as TextAsset;

            if (tex==null)
            {
                Debug.Log("<color=red> get text from "+ fileName+ " fail(Resource)</color>");
                return null;
            }
            else
            {
				string ret = tex.text;
                if (unloadAfterFnish) {
                    
                    Resources.UnloadAsset(tex);
                }
				return ret;
            }
        }
        public static string GetTextFromOrderPath(string path) { 
            if( !File.Exists(path)){
                Debug.Log("<color=red> "+path +" not exists </color>");
                return null;
            }
            StreamReader reader = File.OpenText(path);

            string text = reader.ReadToEnd();
            reader.Close();
            return text;
        }

    }
}