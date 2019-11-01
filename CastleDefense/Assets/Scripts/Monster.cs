using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 모든 몬스터의 부모 클래스
public abstract class Monster : MonoBehaviour
{
    // 몬스터 원소 타입을 위한 enum
    public enum ElementType { None, Fire, Water, Earth, Air };
    // 몬스터의 원소 타입
    private ElementType element;
    // 몬스터의 체력
    private float hp;
    // 몬스터의 이동속도
    private float speed;
    // 몬스터의 현재 이동속도
    private float curspeed;
    // 몬스터의 데미지
    private float damage;
    // 몬스터의 공격 사정거리
    private float attackRange;
    // 몬스터의 공격 속도
    private float attackSpeed;
    private bool isAttacking;

    // 몬스터가 공격할 castle을 받아오는 변수
    public List<GameObject> castles = new List<GameObject>();
    // 몬스터와 가장 가까운 castle을 받아오는 변수
    public GameObject nearestCastle = null;
    // 몬스터의 상태이상을 관리하는 List
    private List<CrowdControl> ccs = new List<CrowdControl>();

    // getters & setters
    public ElementType Element { get => element; set => element = value; }
    public float Hp { get => hp; set => hp = value; }
    public float Speed { get => speed; set => speed = value; }
    public float Damage { get => damage; set => damage = value; }
    public float AttackRange { get => attackRange; set => attackRange = value; }
    public float CurSpeed { get => curspeed; set => curspeed = value; }
    public float AttackSpeed { get => attackSpeed; set => attackSpeed = value; }
    public bool IsAttacking { get => isAttacking; set => isAttacking = value; }

    private void MakeCastleList()
    {
        RaycastHit[] hits;
        hits = Physics.SphereCastAll(transform.position, 100, Vector3.up, 0);
        foreach (RaycastHit hit in hits)
        {
            if (hit.transform.CompareTag("Castle"))
                castles.Add(hit.transform.gameObject);
        }
    }
    private void FindNearestWall()
    {
        float distance = Mathf.Infinity;
        foreach (GameObject c in castles)
        {
            Vector3 diff = c.transform.position - transform.position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                nearestCastle = c;
                distance = curDistance;
            }
        }
        Debug.Log(nearestCastle.name);
    }
    // 몬스터가 공격 사정거리 안에 들어오면 true 리턴, 아니면 false 리턴
    public bool CheckRange()
    {
        //Debug.Log("Distance from" + nearestCastle.name + ": " + Vector3.Distance(nearestCastle.transform.position, transform.position) + ", Attackrange: " + AttackRange);
        if (Vector3.Distance(nearestCastle.transform.position, transform.position) <= AttackRange)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public abstract IEnumerator Attack(float interval);

    // 몬스터에게 상태이상을 부여하는 함수
    // 각 마법 스크립트에서 마법이 몬스터에게 맞았을 때 이 함수를 호출해주면 됨
    public void AddCrowdControl(CrowdControl cc)
    {
        cc.InitialEffect(this);
        // 상태이상 리스트에 매개변수로 받아온 상태이상 추가
        ccs.Add(cc);
    }

    // 상태이상 지속 효과를 입히기 위한 함수
    IEnumerator CheckCrowdControl()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            // 상태이상 리스트의 모든 상태이상에 대하여
            foreach (CrowdControl cc in ccs)
            {
                // 지속 시간이 끝나면 리스트에서 지움
                if (cc.Time <= 0)
                {
                    StartCoroutine(DeleteCrowdControl(cc));
                }
            }
        }
    }

    // 상태이상 리스트 삭제를 위한 코루틴
    private IEnumerator DeleteCrowdControl(CrowdControl cc)
    {
        yield return null;
        ccs.Remove(cc);
    }
    
    public void Start()
    {
        MakeCastleList();
        // 매 초마다 상태이상 시간 체크
        StartCoroutine(CheckCrowdControl());
    }

    public void Update()
    {
        MakeCastleList();
        FindNearestWall();
        // 몬스터의 피가 0 이하로 떨어지면 죽음
        if (Hp <= 0)
        {
            Destroy(gameObject);
        }
    }
}
