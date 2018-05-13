using System;
using DFloat = System.Single;
public class Timer  {
 //property
    public DFloat remainingTime { get; protected set; }
    /// <summary>
    /// 判断当前是否暂停
    /// </summary>
     public bool isRunning{get;protected set;}
    /// <summary>
    /// 判断计时器是否已经结束，结束后会被管理类移除
    /// </summary>
     public bool isStop{get;protected set;}

     protected Action m_onTimingOver;

//method
    /// <summary>
    /// </summary>
    /// <param name="time">计时时间</param>
    /// <param name="timeOverAction">计时结束回调（不包括中途stop）</param>
    /// <param name="stopAction">stop时回调，包括计时结束，和调用stop</param>
     public Timer(DFloat time, Action timeOverAction=null){
          isRunning = true;
          isStop = false;
          remainingTime = time;
          m_onTimingOver = timeOverAction;
     }

    public virtual void Pause(){
        isRunning = false;
    }
    public virtual void Resume(){
        isRunning  = true;
    }
    
    public virtual void Stop(){
        if(isStop) return; 
        isStop = true;
        isRunning = false;
        remainingTime = 0;
    }
    /// <summary>
    /// override时全重写（代码拷过去，以防暂停失效）
    /// </summary>
    /// <param name="delta"></param>
    public virtual bool Tick(DFloat delta){
        if (!isRunning || isStop) return false;
         remainingTime -= delta;
         if (remainingTime <= 0){
             Stop();
             if (m_onTimingOver != null) { m_onTimingOver(); }             
         }
         return true;
      }
     public virtual void Reset(DFloat time){
         remainingTime = time;
         isRunning = true;
         isStop = false;
     }
     public virtual void AddTime(DFloat time){
         remainingTime += time;
     }
}
