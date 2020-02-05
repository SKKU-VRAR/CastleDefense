using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : Magic
{
    public float speed = 15.0f;
    public float boom = 0.4f;
    public float decreasedDamage = 0;
    public float decreasedSpeed = 0; 
    // Start is called before the first frame update
    void Start()
    {
        //불속성
        Element = ElementType.Fire;
        //마나
        Cost = 0;
        //데미지
        Damage = 80;
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.y < 0)
        {
            Destroy(gameObject);
        }
        
        transform.Translate(transform.up * -1*speed * Time.deltaTime);
        if(transform.position.y < 10 && speed > 3f)
        {
            speed -= 0.1f;
        }
        if (transform.position.y < 0.5f)
        {
            decreasedDamage += 1;
            transform.localScale += new Vector3(boom, boom, boom);
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        // collider가 Monster 스크립트를 가지고 있으면 진입, 아니면 안함
        if (collider.TryGetComponent(out Monster mon))
        {
            // 몬스터에게 상태이상 - 에어본을 추가함
            mon.AddCrowdControl(new CC_Burn());
            if( decreasedDamage < 80 )
            {
                mon.AddCrowdControl(new CC_Stun(5));
            }
            mon.AddCrowdControl(new CC_Burn());
            mon.AddCrowdControl(new CC_Stun(5));
            // 몬스터에게 데미지만큼의 피해를 입힘
            mon.Hp -= Damage-decreasedDamage;
        }
    }
}
