using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snail : Enemy
{
   protected override void Awake()
    {
        base.Awake();
        patrolState  = new SnailPatrolState();
        skillState = new SnailSkillState();
    }

    public override void Move()
    {
        if(!anim.GetCurrentAnimatorStateInfo(0).IsName("PreMove") && !anim.GetCurrentAnimatorStateInfo(0).IsName("TurnBack"))
        {
        
            rb.velocity = new Vector2(currentSpeed * faceDir.x * Time.deltaTime, rb.velocity.y );
        }
    }
}
