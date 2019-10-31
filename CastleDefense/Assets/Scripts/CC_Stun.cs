using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CC_Stun : CrowdControl
{
    private float originalSpeed;

    public CC_Stun(float time)
    {
        Time = time;
    }

    public override void InitialEffect(Monster g)
    {
        originalSpeed = g.Speed;
        g.Speed = 0;
    }
    public override void PersistingEffect(Monster g)
    {

    }
    public override void FinalEffect(Monster g)
    {
        g.Speed = originalSpeed;
    }
}
