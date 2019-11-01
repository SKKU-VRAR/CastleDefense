using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YasuoQ : Magic
{
    // 해일의 속도
    public float speed = 10.0f;

    void Start()
    {
        // 해일의 원소 타입 = 물
        Element = ElementType.Air;
        // 해일의 마나 (아직 마나 미구현이므로 0)
        Cost = 0;
        // 해일의 데미지
        Damage = 0;

        Destroy(gameObject, 2);

        
    }

    void OnTriggerStay(Collider collider)
    {
        // 몬스터 스크립트 참조를 위한 변수
        Monster mon;
        // collider가 Monster 스크립트를 가지고 있으면 진입, 아니면 안함
        
        if (collider.TryGetComponent(out mon))
        {
            mon.Hp -= Damage;
            mon.AddCrowdControl(new CC_Stun(.5f));
            collider.transform.position = Vector3.Lerp(collider.transform.position, transform.position, .5f);
        }
    }
}