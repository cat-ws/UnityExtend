using System;
using DFloat = System.Single;

public class TickTImer : Timer
{
    Action<DFloat> m_onTick;
    public TickTImer(DFloat time, Action<DFloat> tickAction ,Action timeOverAction=null):base(time,timeOverAction){
        m_onTick = tickAction;
    }
    public override bool Tick(DFloat delta)
    {
        if (!base.Tick(delta)) { return false; };

        if (m_onTick != null&&!isStop) m_onTick(delta);
        return true;
    }

}
