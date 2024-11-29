
using Unity.VisualScripting;
using UnityEngine;

public class SnailSkillState : BaseState
{
    private Vector2 oriOffset;
    private Vector2 oriSize;
    public override void OnEnter(Enemy enemy)
    {
        currentEnemy = enemy;
        currentEnemy.currentSpeed = 0;
        currentEnemy.anim.SetBool("walk",false);
        currentEnemy.anim.SetBool("hide",true);
        currentEnemy.anim.SetTrigger("foundP");

        currentEnemy.GetComponent<Character>().invulnerable = true;//将状态改为无敌
        currentEnemy.GetComponent<Character>().invulnerableCount=currentEnemy.lostTimeCounter; 

        oriOffset = currentEnemy.coll.offset;
        oriSize = currentEnemy.coll.size;
        currentEnemy.coll.offset = new Vector2(0.1f, 0.7f);
        currentEnemy.coll.size = new Vector2(1.3f, 1);

    }

    
    
    public override void LogicUpdate()
    {
        currentEnemy.GetComponent<Character>().invulnerableCount=currentEnemy.lostTimeCounter;
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
        currentEnemy.anim.SetBool("hide",false);
        currentEnemy.lostTimeCounter=currentEnemy.lostTime;
        currentEnemy.GetComponent<Character>().invulnerable = false;

        currentEnemy.coll.offset = oriOffset;
        currentEnemy.coll.size = oriSize;
    }

    
}
