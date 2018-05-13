using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DFloat = System.Single;
public partial class BridgeData
{
    public Player owner{get;protected set;}
#region 伤害类
    public bool isProtected;//保护状态,不伤害，也不造成伤害
    public bool isDamageImmune;//伤害免疫
    public bool isDamageless;//无伤害（0伤害）
#endregion
#region 运动相关
    public DFloat XSpeedLimitAdd;//速度极限增加值
    public DFloat YSpeedLimitAdd;
    public DFloat XSpeedLimitScale;//速度极限缩放值
    public DFloat YSpeedLimitScale;
    public DFloat XSpeedScaleFactor;//速度缩放值
    public DFloat YSpeedScaleFactor;
#endregion
#region 状态
    public bool isSpeedUp;//加速状态
    

#endregion
    public BridgeData(Player player){
        owner = player;
        Reset();
    }
    public void Reset() {
        isProtected = false;
        isDamageImmune = false;
        isDamageless = false;
        isSpeedUp = false;
        XSpeedLimitAdd = 0;
        YSpeedLimitAdd = 0;
        XSpeedLimitScale = 1;
        YSpeedLimitScale = 1;
        XSpeedScaleFactor = 1;
        YSpeedScaleFactor = 1;
    }
}
