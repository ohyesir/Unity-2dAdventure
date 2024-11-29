using System.Collections;
using System.Collections.Generic;
using System.Drawing.Text;
using UnityEngine;

public class BeeChaseState : BaseState
{
    private Attack attack;
    private Vector3 targetPoint;
    private Vector3 moveDir;
    private Transform playerTransform;
    private bool isAttack;
    private float attackRateCounter = 0;
    private float attackDistanceX;
    private float attackDistanceY;
    public Animator anim;
    
    public override void OnEnter(Enemy enemy)
    {
        currentEnemy = enemy;
        currentEnemy.currentSpeed = currentEnemy.chaseSpeed;
        attack = enemy.GetComponent<Attack>();
        currentEnemy.lostTimeCounter = currentEnemy.lostTime;
        currentEnemy.anim.SetBool("foundP",true);
    }
    public override void LogicUpdate()
    {

        if(currentEnemy.lostTimeCounter <= 0)
        {
            currentEnemy.SwitchState(NPCState.Patrol);    
        }

        attackRateCounter -= Time.deltaTime; //攻击间隔计时器
        
        playerTransform= GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        targetPoint = new Vector3(playerTransform.position.x, playerTransform.position.y+1.5f, 0);
        //目标点=玩家+上半身身高因为要打身子

        //判断是否在攻击范围
        attackDistanceX = Mathf.Abs(targetPoint.x - currentEnemy.transform.position.x);
        attackDistanceY = Mathf.Abs(targetPoint.y - currentEnemy.transform.position.y);
        if( attackDistanceX <= attack.attackRange && attackDistanceY <= attack.attackRange)
        {
           
            isAttack = true;
            if(!currentEnemy.isHurt)
                currentEnemy.rb.velocity = Vector2.zero;//将速度停下来
            
            
            if(attackRateCounter <=0)
            {
                currentEnemy.anim.SetTrigger("attack");
                attackRateCounter = attack.attackRate;
                
            }
            
        } 
        else
        {
            isAttack = false;
        }

        moveDir = (targetPoint - currentEnemy.transform.position).normalized;
        if(moveDir.x > 0)
            currentEnemy.transform.localScale = new Vector3(-1, 1, 1);
        if(moveDir.x < 0)
            currentEnemy.transform.localScale = new Vector3(1, 1, 1);

    }
    public override void PhysicsUpdate()
    {


        if(!currentEnemy.isHurt && !currentEnemy.isDead && !isAttack)
        {
        
            currentEnemy.rb.velocity = moveDir * currentEnemy.currentSpeed * Time.deltaTime;
        } 
    }

    public override void OnExit()
    {
       currentEnemy.anim.SetBool("foundP",false);
       
    }
}
