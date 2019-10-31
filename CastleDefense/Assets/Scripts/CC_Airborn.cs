using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CC_Airborn : CrowdControl
{
    private float amount;

    // 생성자
    public CC_Airborn(float time = 1f)
    {
        Time = time;
        // 물리식
        amount = 0.5f * Physics.gravity.magnitude * time;
    }

    public override void InitialEffect(Monster g)
    {
        g.CurSpeed = 0;
        g.gameObject.GetComponent<Rigidbody>().velocity += Vector3.up * amount;
        g.StartCoroutine(Apply(g, 1f));
    }
    public override void PersistingEffect(Monster g)
    {

    }
    public override void FinalEffect(Monster g)
    {
        g.CurSpeed = g.Speed;
    }
}
