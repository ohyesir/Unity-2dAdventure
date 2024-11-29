using UnityEngine;

public class BoarPatrolState : BaseState
{
   public override void OnEnter(Enemy enemy)
    {
        currentEnemy = enemy;
        currentEnemy.currentSpeed = currentEnemy.normalSpeed;
    } 
    
    
    public override void LogicUpdate()
    {
#region 追击
       
        if(currentEnemy.FoundPlayer() || currentEnemy.isHurt)
        {
            currentEnemy.SwitchState(NPCState.Chase);
        }
#endregion

#region 走路撞墙在地面等待
        if(!currentEnemy.physicsCheck.isGround || (currentEnemy.physicsCheck.isTouchLeftWall && currentEnemy.faceDir.x < 0 
            || currentEnemy.physicsCheck.isTouchRightWall && currentEnemy.faceDir.x > 0) )//脸朝左墙撞左墙
        {
          currentEnemy.wait = true;
          currentEnemy.anim.SetBool("walk",false);
         
        }

        else
            currentEnemy.anim.SetBool("walk",true);
#endregion
    }

    public override void PhysicsUpdate()
    {
        
    }
    
    public override void OnExit()
    {
        
    }
}
