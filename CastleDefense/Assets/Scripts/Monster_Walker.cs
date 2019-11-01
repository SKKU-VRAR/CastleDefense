using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 뚜벅이 몬스터
public class Monster_Walker : Monster
{
    public void Start()
    {
        base.Start();
        // Start 함수에서 몬스터의 spec 초기화
        Hp = 100f;
        CurSpeed = Speed = 10f;
        Damage = 1f;
        AttackRange = 2f;
        AttackSpeed = 1f;
        StartCoroutine(Attack(AttackSpeed));
    }
    
    public void Update()
    {
        base.Update();
        if (!CheckRange())
            transform.Translate(transform.forward * CurSpeed * Time.deltaTime);
    }

    public override IEnumerator Attack(float interval)
    {
        while (true)
        {
            if (CheckRange())
            {
                yield return new WaitForSeconds(interval);
                castle.hp -= Damage;
            }
        }
    }
}
