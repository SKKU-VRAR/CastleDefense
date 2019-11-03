using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 뚜벅이 몬스터
public class Monster_Walker : Monster
{
    // 코루틴 Start, Stop을 하기 위한 변수
    private IEnumerator coroutine;

    public void Start()
    {
        base.Start();
        // Start 함수에서 몬스터의 spec 초기화
        Hp = 100f;
        Speed = 5f;
        Damage = 1f;
        AttackRange = 5f;
        AttackSpeed = 1f;

        Agent.stoppingDistance = AttackRange;
        Agent.speed = Speed;
        coroutine = Attack(AttackSpeed);
    }
    
    public void Update()
    {
        base.Update();
        if (!CheckRange())
        {
            if (IsAttacking)
            {
                IsAttacking = false;
                StopCoroutine(coroutine);
            }
        }
        else
        {
            if (!IsAttacking)
            {
                StartCoroutine(coroutine);
                IsAttacking = true;
            }
        }
    }

    public override IEnumerator Attack(float interval)
    {
        while (true)
        {
            yield return new WaitForSeconds(interval);
            Debug.Log("Attack to " + NearestCastle.name + "!");
            NearestCastle.GetComponent<Entity>().Hp -= Damage;
        }
    }
}
