using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TidalWave : Magic
{
    // 해일의 속도
    public float speed = 10.0f;

    void Start()
    {
        // 해일의 원소 타입 = 물
        Element = ElementType.Water;
        // 해일의 마나 (아직 마나 미구현이므로 0)
        Cost = 0;
        // 해일의 데미지
        Damage = 80;
    }

    void Update()
    {
        // 단순히 해일을 앞으로 이동
        transform.Translate(transform.forward * speed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider collider)
    {
        // 몬스터 스크립트 참조를 위한 변수
        Monster mon;
        // collider가 Monster 스크립트를 가지고 있으면 진입, 아니면 안함
        if (collider.TryGetComponent(out mon))
        {
            // 몬스터에게 상태이상 - 에어본을 추가함
            mon.AddCrowdControl(new CC_Airborn());
            // 몬스터에게 데미지만큼의 피해를 입힘
            mon.Hp -= Damage;
        }
    }
}