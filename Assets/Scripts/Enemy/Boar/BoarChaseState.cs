using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoarChaseState : BaseState
{
    public override void OnEnter(Enemy enemy)
    {
        currentEnemy = enemy;
        currentEnemy.currentSpeed = currentEnemy.chaseSpeed;
        currentEnemy.anim.SetBool("run",true);
    }
    public override void LogicUpdate()
    {
        if(!currentEnemy.physicsCheck.isGround || (currentEnemy.physicsCheck.isTouchLeftWall && currentEnemy.faceDir.x < 0 
            || currentEnemy.physicsCheck.isTouchRightWall && currentEnemy.faceDir.x > 0) )
        {
          currentEnemy.transform.localScale = new Vector3(currentEnemy.faceDir.x, 1, 1);//猪不等待直接翻转撞墙
        }

        if(currentEnemy.lostTimeCounter <= 0)
        {
            currentEnemy.SwitchState(NPCState.Patrol);    
        }
    }

    public override void PhysicsUpdate()
    {
       
    }

    public override void OnExit()
    {
      currentEnemy.anim.SetBool("run",false);
      currentEnemy.lostTimeCounter=currentEnemy.lostTime;
    }

    
}
