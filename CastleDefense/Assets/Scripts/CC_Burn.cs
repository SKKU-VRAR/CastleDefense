using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CC_Burn : CrowdControl
{
    // 화상 틱당 데미지 (1초당)
    private float damage;

    // 지속시간과 데미지를 매개변수로 받음
    public CC_Burn(float time = 4f, float damage = 5f)
    {
        Time = time;
        this.damage = damage;
    }

    public override void InitialEffect(Monster g)
    {
        g.StartCoroutine(Apply(g, 1f));
    }
    public override void PersistingEffect(Monster g)
    {
        g.Hp -= damage;
    }
    public override void FinalEffect(Monster g)
    {

    }
}
