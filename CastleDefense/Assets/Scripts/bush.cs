using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bush : Magic
{ 
    public float etime = 1f;
    // Start is called before the first frame update
    public float speed = 30f;

    void Start()
    {
        Element = ElementType.Earth;
        Cost = 0;
        Damage = 160;
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.y > -2)
        {
            speed -= 1f;
        }
        speed -= 0.1f;
        if(transform.position.y < 0) transform.Translate(transform.up*speed*Time.deltaTime);
    }
    void OnTriggerEnter(Collider collider)
    {
        if(true)
        {
            if (collider.TryGetComponent(out Monster mon))
            {
            // 몬스터에게 상태이상 - 에어본을 추가함
                mon.AddCrowdControl(new CC_Airborne());
            // 몬스터에게 데미지만큼의 피해를 입힘
            
                mon.Hp -= Damage;
           
            }
        }
        // collider가 Monster 스크립트를 가지고 있으면 진입, 아니면 안함
        
    }
}
