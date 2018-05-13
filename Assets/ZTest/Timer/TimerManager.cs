using System.Collections.Generic;
using DFloat = System.Single;

public class TimerManager  {
    public int timerCount { get { return m_timers.Count; } }

    List<Timer> m_timers = new List<Timer>();
    public void AddTimer(Timer timer) {
        if(timer.remainingTime>=0&&!m_timers.Contains(timer))
            m_timers.Add(timer);
    }

    public void TimersTick(DFloat delta){
        for (int i = m_timers.Count - 1; i >= 0; i--){
            Timer timer = m_timers[i];
            timer.Tick(delta);
            if (timer.isStop) m_timers.RemoveAt(i);
        }
    }
    public bool Conains(Timer timer ) {
       return  m_timers.Contains(timer);
    }

    public void Clear(){
        foreach (Timer timer in m_timers) { timer.Stop(); }
        m_timers.Clear();
        m_timers.TrimExcess();
    }
}
