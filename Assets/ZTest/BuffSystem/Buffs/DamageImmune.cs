using UnityEngine;
using DFloat = System.Single;
namespace Buffs { 
public class DamageImmune : Buff {
    Timer m_timer;
    public DamageImmune(BuffManager mgr,DFloat time):base(mgr,(int)BuffTypes.DamageImmune) {
        m_timer = new Timer(time,()=>RemoveEffect());
    }
    public override void AddEffect()
    {
         base.AddEffect();
         m_manager.bridgeData.owner.timerManager.AddTimer(m_timer);
         m_manager.bridgeData.isDamageImmune = true;
    }
    public override void RemoveEffect()
    {
        base.RemoveEffect();
        m_timer.Stop();
        m_manager.bridgeData.isDamageImmune = m_manager.Contains(id);
    }

}
}