using UnityEngine;
 using DFloat = System.Single;
namespace Buffs{
public class Protection:Buff  {
    Timer m_timer;
    public Protection(BuffManager mgr,DFloat protectionTime):base(mgr,(int)BuffTypes.Protection) {
        m_timer = new Timer(protectionTime, () => RemoveEffect());
    }
    public override void AddEffect()
    {
        base.AddEffect();
        m_manager.bridgeData.owner.timerManager.AddTimer(m_timer);

        m_manager.bridgeData.isProtected = true;
    }
    public override void RemoveEffect()
    {
        base.RemoveEffect();
        m_timer.Stop();

        m_manager.bridgeData.isProtected = m_manager.Contains(id);

       
    }
}
}