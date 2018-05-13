using System.Collections.Generic;
using UnityEngine;
using UIFrame;
    public partial class UIManager
    {
        static UIManager _Instance = null;
        public static UIManager Instance{
            get{
                if(_Instance == null){
                    _Instance = new UIManager();
                }
                return _Instance;
            }
        }

        class PrefabPaths{
            Dictionary<string,string> m_paths;
            PrefabPaths(){
                m_paths = new Dictionary<string,string>();
            }

            public bool TryGetPath(string uiname,out string path){
                path = "";  
                return false;
            }
        }
        class ManagerData {
            Dictionary<RootCanvasType,RootCanvas> m_rootCanvases;
            Dictionary<RootCanvasType, List<UIBase>> m_displayUIs;

        List<string> m_back;
        }
        UIManager() { 
            
        }
        public void PreloadUIs (string [] uinames){
        
        }
        public void OpenUI (string uiname){
        
        }
        /// <summary>
        /// return specified ui that is exsiting or null
        /// </summary>
        /// <param name="uiname"></param>
        /// <returns></returns>
        public UIBase FindUI(string uiname) {

            return null;
        }
        public void CloseUI(UIBase ui) {


        }

    }
