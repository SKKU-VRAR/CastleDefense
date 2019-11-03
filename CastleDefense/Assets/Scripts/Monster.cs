using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// 모든 몬스터의 부모 클래스
public abstract class Monster : Entity
{
    // 몬스터 원소 타입을 위한 enum
    public enum ElementType { None, Fire, Water, Earth, Air };
    // 몬스터의 원소 타입
    private ElementType element;
    // 몬스터의 이동속도
    private float speed;
    // 몬스터의 데미지
    private float damage;
    // 몬스터의 공격 사정거리
    private float attackRange;
    // 몬스터의 공격 속도
    private float attackSpeed;
    private bool isAttacking;

    // 몬스터와 가장 가까운 castle을 저장하는 변수
    private GameObject nearestCastle;
    // 몬스터와 가장 가까운 castle의 point를 저장하는 변수
    private Vector3 targetPoint;
    // 몬스터의 상태이상을 관리하는 List
    private List<CrowdControl> ccs = new List<CrowdControl>();
    // NavMeshAgent 속성
    private NavMeshAgent agent;

    #region getter & setter
    // getters & setters
    public ElementType Element { get => element; set => element = value; }
    public float Speed { get => speed; set => speed = value; }
    public float Damage { get => damage; set => damage = value; }
    public float AttackRange { get => attackRange; set => attackRange = value; }
    public float AttackSpeed { get => attackSpeed; set => attackSpeed = value; }
    public bool IsAttacking { get => isAttacking; set => isAttacking = value; }
    public NavMeshAgent Agent { get => agent; set => agent = value; }
    public GameObject NearestCastle { get => nearestCastle; set => nearestCastle = value; }
    public List<CrowdControl> Ccs { get => ccs; set => ccs = value; }
    public Vector3 TargetPoint { get => targetPoint; set => targetPoint = value; }
    #endregion

    // GameManager Script에 등록되어있는 모든 castle에 대해서 가장 가까운 castle을 찾음
    // 결과: nearestCastle 변수에 가장 가까운 castle의 GameObject가 들어감
    private void FindNearestWall()
    {
        float distance = Mathf.Infinity;
        foreach (GameObject c in GameManager.instance.castles)
        {
            Vector3 castlePos = new Vector3(c.transform.position.x, transform.position.y, c.transform.position.z);
            Vector3 diff = castlePos - transform.position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                nearestCastle = c;
                distance = curDistance;
            }
        }
        SetTargetPoint();
    }
    // 몬스터의 target point를 정하는 함수
    private void SetTargetPoint()
    {
        float x = Mathf.Clamp(transform.position.x,
            nearestCastle.transform.position.x - nearestCastle.transform.localScale.x / 2,
            nearestCastle.transform.position.x + nearestCastle.transform.localScale.x / 2);
        float z = Mathf.Clamp(transform.position.z,
            nearestCastle.transform.position.z - nearestCastle.transform.localScale.z / 2,
            nearestCastle.transform.position.z + nearestCastle.transform.localScale.z / 2);

        targetPoint = new Vector3(x, 0, z);
        agent.destination = targetPoint;
    }
    // 몬스터가 공격 사정거리 안에 들어오면 true 리턴, 아니면 false 리턴
    public bool CheckRange()
    {
        return Vector3.Distance(targetPoint, transform.position) <= attackRange;
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

    // 목표물을 바라보는 함수
    private void FaceTarget()
    {
        Vector3 direction = (targetPoint - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }
    
    public void Start()
    {
        Agent = GetComponent<NavMeshAgent>();
        nearestCastle = GameManager.instance.mainCastle;
        agent.destination = nearestCastle.transform.position;
        // 매 초마다 상태이상 시간 체크
        StartCoroutine(CheckCrowdControl());
    }

    public void Update()
    {
        // 디버깅
        Debug.DrawLine(transform.position, targetPoint, Color.red);
        // NavMeshAgent 속성이 활성화 되어있을 때에만 실행
        if (agent.enabled)
        {
            FindNearestWall();
            FaceTarget();
        }
        // 몬스터의 피가 0 이하로 떨어지면 죽음
        if (Hp <= 0)
        {
            Destroy(gameObject);
        }
    }

    
}
