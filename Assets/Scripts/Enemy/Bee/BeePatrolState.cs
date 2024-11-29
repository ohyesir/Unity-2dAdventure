using UnityEngine;

public class BeePatrolState : BaseState
{
    private Vector3 targetPoint;//目标点
    private Vector3 moveDir;
    public override void OnEnter(Enemy enemy)
    {
        currentEnemy = enemy;
        currentEnemy.currentSpeed = currentEnemy.normalSpeed;
        targetPoint=enemy.GetNewPoint(); //目标位置
    }
    public override void LogicUpdate()
    {
        if(currentEnemy.FoundPlayer())
        {
            currentEnemy.SwitchState(NPCState.Chase);
        }

        if(Mathf.Abs(targetPoint.x - currentEnemy.transform.position.x) < 0.1f  //判断是否到达目标位置。目标位置减现在位置
            && Mathf.Abs(targetPoint.y - currentEnemy.transform.position.y) < 0.1f)

        {
            currentEnemy.wait = true;
            targetPoint = currentEnemy.GetNewPoint();
        }

        moveDir = (targetPoint - currentEnemy.transform.position).normalized;
        if(moveDir.x > 0)
            currentEnemy.transform.localScale = new Vector3(-1, 1, 1);
        if(moveDir.x < 0)
            currentEnemy.transform.localScale = new Vector3(1, 1, 1);
    }

    public override void PhysicsUpdate()
    {
        if(!currentEnemy.wait && !currentEnemy.isHurt && !currentEnemy.isDead)
        {
            currentEnemy.rb.velocity = moveDir * currentEnemy.currentSpeed * Time.deltaTime;
        } 
        else
        {
             currentEnemy.rb.velocity = Vector2.zero;
        }
    }

    public override void OnExit()
    {
        
    }

    
}
