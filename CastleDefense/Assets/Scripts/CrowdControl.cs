using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// CC기(상태이상) 추상 클래스
public abstract class CrowdControl
{
    private float time;

    public float Time { get => time; set => time = value; }

    // 상태이상에 처음 걸렸을 때 실행될 함수
    public abstract void InitialEffect(Monster g);
    // 상태이상 지속 효과를 구현하는 함수
    public abstract void PersistingEffect(Monster g);
    // 상태이상이 끝날 때 실행될 함수
    public abstract void FinalEffect(Monster g);
    public IEnumerator Apply(Monster g, float interval)
    {
        while (true)
        {
            yield return new WaitForSeconds(interval);
            PersistingEffect(g);
            Time -= interval;
            if (Time <= 0)
            {
                FinalEffect(g);
                break;
            }
        }
    }
}
