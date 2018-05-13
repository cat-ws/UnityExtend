using UnityEngine;
using DFloat = System.Single;
namespace Buffs{
public class Damageless : Buff
{
    Timer m_timer;
    public Damageless(BuffManager mgr, DFloat time) : base(mgr, (int)BuffTypes.Damageless)
    {
        m_timer = new Timer(time,()=>RemoveEffect());
    }
    public override void AddEffect()
    {
         base.AddEffect();
         m_manager.bridgeData.owner.timerManager.AddTimer(m_timer);
         m_manager.bridgeData.isDamageless = true;
    }
    public override void RemoveEffect()
    {
        base.RemoveEffect();
        m_timer.Stop();
        m_manager.bridgeData.isDamageless = m_manager.Contains(id);
    }

}
}
