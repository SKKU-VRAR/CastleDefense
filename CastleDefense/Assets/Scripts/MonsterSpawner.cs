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

    IEnumerator SpawnMonster(float interval)
    {
        while (true)
        {
            Instantiate(mon_walker, transform.position + new Vector3(0, 1, 0), Quaternion.Euler(0, 90, 0));
            yield return new WaitForSeconds(interval);
        }
    }
}
