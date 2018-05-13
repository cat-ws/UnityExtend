using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace UIFrame
{
    [RequireComponent(typeof(Canvas))]
    [RequireComponent(typeof(CanvasScaler))]
    public class RootCanvas:MonoBehaviour
    {
        public Canvas canvas { get; private set; }
        public CanvasScaler canvasScaler { get; private set; }
        
        void Awake() {
            canvas = GetComponent<Canvas>();
            canvasScaler = GetComponent<CanvasScaler>();

            
        }

    }
}