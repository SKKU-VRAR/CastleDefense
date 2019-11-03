using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public GameObject mon_walker;
    
    void Start()
    {
        StartCoroutine(SpawnMonster(2.0f));
    }

    // 몬스터 스폰 코루틴
    IEnumerator SpawnMonster(float interval)
    {
        while (true)
        {
            for (int i = 0; i < 3; i++)
            {
                Instantiate(mon_walker,
                    new Vector3(Random.Range(-10, 10), 1, transform.position.z),
                    Quaternion.Euler(0, -180, 0));
            }
            yield return new WaitForSeconds(interval);
        }
    }
}
