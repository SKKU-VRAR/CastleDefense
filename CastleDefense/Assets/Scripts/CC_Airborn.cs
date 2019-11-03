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
        // NavMeshAgent를 사용하게 되면 rigidbody가 제대로 작동을 안함
        // 따라서 에어본을 맞을 때, 잠시 NavMeshAgent를 비활성화 하고
        g.Agent.enabled = false;
        // RigidBody 속성의 isKinematic을 풀어줌
        g.GetComponent<Rigidbody>().isKinematic = false;
        //g.CurSpeed = 0;
        g.gameObject.GetComponent<Rigidbody>().velocity += Vector3.up * amount;
        g.StartCoroutine(Apply(g, 1f));
    }
    public override void PersistingEffect(Monster g)
    {

    }
    public override void FinalEffect(Monster g)
    {
        //g.CurSpeed = g.Speed;
        // 에어본이 끝나면 다시 길을 추적하기 위해 NavMeshAgent를 활성화하고
        g.Agent.enabled = true;
        // Rigidbody의 isKinematic을 다시 걸어줌
        g.GetComponent<Rigidbody>().isKinematic = true;
    }
}
