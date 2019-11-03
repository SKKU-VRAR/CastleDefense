using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 게임의 상황을 Manage해주는 script
public class GameManager : MonoBehaviour
{
    /* 다른 스크립트에서 GameManager.instance를 써서 이 스크립트의 변수 및 메소드에 접근 가능 */
    public static GameManager instance;

    void Awake()
    {
        instance = this;
        castles.Add(mainCastle);
    }
    /* ================================================================================= */

    // 메인 castle이 들어갈 변수
    public GameObject mainCastle;
    // 흙마법(벽)과 같은 몬스터가 공격 가능한 다른 벽들을 담는 list
    public List<GameObject> castles;

    // 몬스터가 공격 가능한 벽이 만들어질 때 호출되는 함수
    // 해당 벽 스크립트의 Start()에서 GameManager.instance.AddCastle(gameObject) 해주면 됨
    public void AddCastle(GameObject g)
    {
        castles.Add(g);
    }
    // 몬스터가 벽을 부쉈을 때 호출되는 함수
    // 해당 벽 스크립트에서 Destroy 되기 전 GameManager.instance.DestroyCastle(gameObject) 해주면 됨
    public void DestroyCastle(GameObject g)
    {
        castles.Remove(g);
    }
}
