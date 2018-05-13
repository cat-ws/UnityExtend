using System;
using DFloat = System.Single;

public class IntervalTimer : Timer
{
    Action m_onInterval;
    DFloat m_Interval;
    DFloat m_curPassInterval;
    public IntervalTimer(DFloat time, DFloat interval, Action intervalAction ,Action timeOverAction=null) : base(time,timeOverAction) {
        m_Interval = interval;
        m_onInterval = intervalAction;
        m_curPassInterval = 0;
    }
    public override bool Tick(DFloat delta)
    {
        if (!isRunning || isStop) return false;
        remainingTime -= delta;
        m_curPassInterval += delta;
        if (m_curPassInterval > m_Interval && m_onInterval != null){
            m_curPassInterval -= m_Interval;
            m_onInterval();
        }

        if (remainingTime <= 0) {
            if (m_onTimingOver != null) { m_onTimingOver(); }
            Stop();
        }
        return true;
    }
    public override void Reset(DFloat time) {
        base.Reset(time);
        m_curPassInterval = 0;
    }

}

