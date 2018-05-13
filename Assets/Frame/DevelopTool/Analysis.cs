using UnityEngine;
using System.Collections;
namespace WsFrame
{
    public class Analysis : USingleton<Analysis>
    {
        public float StatInterval = 0.1f;
        private float lastRealtime;
        private int frames = 0;
        private float fps;
        public static int verts;
        public static int tris;

        void Start() {
            lastRealtime = Time.realtimeSinceStartup;
            frames = 0;
        }
        void Update() {
            ++frames;
            if (Time.realtimeSinceStartup > lastRealtime + StatInterval)
            {
                fps = frames / (Time.realtimeSinceStartup - lastRealtime);
                frames = 0;
                lastRealtime = Time.realtimeSinceStartup;
                GetObjectsStats();
            }
        }
        void OnGUI()
        {
            GUI.skin.label.normal.textColor = Color.black;
           string fpsDisplay = "fps: " + fps.ToString("f2");
            GUILayout.Label(fpsDisplay);
            string vertsdisplay = verts.ToString("verts: #.##0 ");
            GUILayout.Label(vertsdisplay);
            string trisdisplay = tris.ToString("tris: #.##0 ");
            GUILayout.Label(trisdisplay);
        }
        void GetObjectsStats() {
            verts = 0;
            tris = 0;
            GameObject[] objs = FindObjectsOfType(typeof(GameObject)) as GameObject[];
            foreach(GameObject obj in objs){
                GetObjectStats(obj);
            }
        }
        void GetObjectStats(GameObject obj) {
            Component[] filters;
            filters = obj.GetComponentsInChildren<MeshFilter>();
            foreach(MeshFilter f in filters){
                tris += f.sharedMesh.triangles.Length / 3;
                verts += f.sharedMesh.vertexCount;
            }
        }
    }
}