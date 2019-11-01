using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 뚜벅이 몬스터
public class Monster_Walker : Monster
{
    private IEnumerator coroutine;

    public void Start()
    {
        base.Start();
        // Start 함수에서 몬스터의 spec 초기화
        Hp = 100f;
        CurSpeed = Speed = 10f;
        Damage = 1f;
        AttackRange = 10f;
        AttackSpeed = 1f;

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
            Vector3 target = new Vector3(nearestCastle.transform.position.x, 0, nearestCastle.transform.position.z);
            transform.Translate((target - transform.position).normalized * CurSpeed * Time.deltaTime);
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
            Debug.Log("Attack to " + nearestCastle.name + "!");
            nearestCastle.GetComponent<Castle>().Hp -= Damage;
        }
    }
}
