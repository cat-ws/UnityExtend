using UnityEngine;
using System.Collections;
using System.Diagnostics;
using System.Text;
namespace WsFrame.Extend
{
    
    /// <summary>
    /// 使用时引入别名简化
    /// using Debug = WsFrame.Tool.Debug（不使用则调用UnityEngine.Debug.Log输出）;
    /// 根据宏定义屏蔽
    /// DEBUG 总开关（开启调试输出）
    /// LOG_NONE 
    /// </summary>
    public static class Debug 
    {
        public static void Log(string str = "") {
#if !DEBUG
            return;
#endif

#if LOG_NONE  
            UnityEngine.Debug.Log("  " + str);
#elif LOG_ALL 
            StackTrace t = new StackTrace();
            StackFrame[] fs = t.GetFrames();
            StringBuilder _str = new StringBuilder();
            for (int i =fs.Length- 1; i > 0; --i) { 
                var _f =  fs[i].GetMethod();
                _str.Append(_f.DeclaringType);
                _str.Append("(");
                _str.Append(_f);
                _str.Append(")-");
            }
            UnityEngine.Debug.Log(_str.ToString() + ":  " + str);   
#else
            StackTrace t = new StackTrace();
            StackFrame f = t.GetFrame(1);
            var m = f.GetMethod();
            UnityEngine.Debug.Log(m.DeclaringType + "-(" + m + "):  " + str);

#endif

        }
        /// <summary>
        /// 方法开始
        /// </summary>
        public static void BEGIN()
        {
#if !DEBUG
            return;
#endif
            StackTrace t = new StackTrace();
            StackFrame f = t.GetFrame(1);
            var m = f.GetMethod();
            UnityEngine.Debug.Log(m.DeclaringType + "-(" + m + "):  " + "Begin");  
        }
        public static void END()
        {
#if !DEBUG
            return;
#endif
            StackTrace t = new StackTrace();
            StackFrame f = t.GetFrame(1);
            var m = f.GetMethod();
            UnityEngine.Debug.Log(m.DeclaringType + "-(" + m + "):  " + "End");  
        }
        
    }



}