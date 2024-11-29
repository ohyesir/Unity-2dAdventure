using UnityEngine;

public class Bee : Enemy
{
    [Header("移动范围")]
    public float patrolRadius;

    protected override void Awake()
    {
        base.Awake();  //基于父级，没有完全重写
        patrolState  = new BeePatrolState();
        chaseState = new BeeChaseState();
        
    }

    public override bool FoundPlayer()
    {
                //        画圆                  人物初始坐标         检测距离         目标层
        var obj = Physics2D.OverlapCircle(transform.position, checkDistance, targetLayer);
        if(obj)
        {
            attaker=obj.transform;
        }
        return obj;
    }

    public override void OnDrawGizmos()
    {
        Gizmos.color=Color.red;
        Gizmos.DrawWireSphere(transform.position, checkDistance);
        Gizmos.color=Color.blue;
        Gizmos.DrawWireSphere(transform.position, patrolRadius);
    }

    public override Vector3 GetNewPoint()
    {
        var targetX = Random.Range(-patrolRadius, patrolRadius);
        var targetY = Random.Range(-patrolRadius, patrolRadius);

        return spwanPoint + new Vector3(targetX, targetY);
    }

    public override void Move()
    {

    }
}