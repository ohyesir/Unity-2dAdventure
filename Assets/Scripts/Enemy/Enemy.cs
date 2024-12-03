using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using JetBrains.Annotations;
using UnityEditor.ShaderGraph;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator), typeof(PhysicsCheck))]//添加必要组件最多三个
[RequireComponent(typeof(Character), typeof(Attack), typeof(CapsuleCollider2D))]//但可以分开写
public class Enemy : MonoBehaviour
{
#region 变量
    [HideInInspector]public Rigidbody2D rb; //protected只有子类和自己能进行访问
    [HideInInspector]public Animator anim;//在unity界面不显示
    [HideInInspector]public CapsuleCollider2D coll; 
    [HideInInspector]public PhysicsCheck physicsCheck;

    [Header("状态")]
    public bool isHurt;
    public bool isDead;
    protected BaseState patrolState; //巡逻状态
    protected BaseState chaseState; //追逐状态
    protected BaseState currentState;//现在状态
    protected BaseState skillState; 

   [Header("基本参数")]
   public float normalSpeed;
   public float chaseSpeed;
   public float currentSpeed;
   public float hurtForce;
   public Vector3 faceDir;
   public Transform attaker;
   public Vector3 spwanPoint; //原始出生点  

   [Header("计时器")]
   public float waitTime;
   public float waitTimeCounter;
   public bool wait;
   public float lostTime;
   public float lostTimeCounter;

   [Header("检测玩家")]
   public Vector2 centerOffset; //偏移量
   public Vector2 checkSize; //检查范围大小
   public float checkDistance; //检查距离
   public LayerMask targetLayer; //目标层

#endregion

#region unity函数
   protected virtual void Awake()
   {
          rb = GetComponent<Rigidbody2D>();
          anim = GetComponent<Animator>();
          physicsCheck = GetComponent<PhysicsCheck>();
          coll = GetComponent<CapsuleCollider2D>();
          currentSpeed = normalSpeed;

          //waitTimeCounter = waitTime;
          
          spwanPoint = transform.position;
   }

   private void OnEnable() 
   {
          currentState = patrolState; //将现在状态初始为巡逻
          currentState.OnEnter(this);//执行一进入的函数,传入当前挂载的enemy
         
   }

   void Update()
   {
        faceDir = new Vector3(-transform.lossyScale.x, 0, 0);

        currentState.LogicUpdate();//在update里不断执行对当前状态的判断
        TimeCounter();
   }

   void FixedUpdate()
   {
     if(!isHurt && !isDead && !wait)   
        Move();

     currentState.PhysicsUpdate();//在update里不断执行对当前状态的判断
   }

   private void OnDisable() 
   {
    
     currentState.OnExit();
   }

   public virtual void Move()
   {  
     
     
     rb.velocity = new Vector2(currentSpeed * faceDir.x * Time.deltaTime, rb.velocity.y );
     
   }
#endregion

#region 各类自定义函数
   public void TimeCounter()
   {
     if(wait)
     {
          waitTimeCounter -= Time.deltaTime;
          if(waitTimeCounter <= 0)
          {
               wait = false;
               waitTimeCounter = waitTime;
               transform.localScale = new Vector3(faceDir.x, 1, 1);
          }

          
     }

     if(!FoundPlayer())
     {
          lostTimeCounter -= Time.deltaTime;
     }


   }

   public virtual bool FoundPlayer()
   {
     return Physics2D.BoxCast((Vector2)transform.position+centerOffset,checkSize,0,faceDir,checkDistance,targetLayer);
     //BoxCast可以返回命中单位的基本数值，也可以是bool是否命中

     
   }

   public virtual void OnDrawGizmos() 
   {
     Gizmos.color=Color.red;
     Gizmos.DrawWireSphere(transform.position+(Vector3)centerOffset + 
     new Vector3(checkDistance*-transform.localScale.x,0),0.25f); //绘制boxCast的范围
   }

   public void SwitchState(NPCState state)
   {
     var newState = state switch //进行变量切换 赋予newState值
     {
          NPCState.Patrol => patrolState,   
          NPCState.Chase => chaseState,
          NPCState.Skill => skillState,
          _ => null                        //其余默认是null
     };

     currentState.OnExit();      //退出当前状态
     currentState = newState;   //进入新状态
     currentState.OnEnter(this);  //启用新状态
   }

   public virtual Vector3 GetNewPoint()  //得到新的巡逻点位
   {

     return transform.position;  //默认是返回当前的位置，在蜜蜂里复写
   }



#endregion

#region 各类事件函数
   public void OnTakeDamage(Transform attackTrans)
   {
     attaker = attackTrans; //玩家的位置
     //转身
     if(attackTrans.position.x - transform.position.x > 0) //玩家位置-怪物位置，此为怪物在角色左边，向左
          transform.localScale = new Vector3(-1,1,1);
     if(attackTrans.position.x - transform.position.x < 0)//此为怪物在角色右边，向右
          transform.localScale = new Vector3(1,1,1);

     //受伤被击退
     isHurt = true;
     // Debug.Log(anim.name);
     anim.SetTrigger("hurt"); //执行animator里的trigger hurt
     anim.SetBool("isHurt", isHurt);
     Vector2 dir = new Vector2(transform.position.x - attackTrans.position.x , 0).normalized;
     rb.velocity = new Vector2(0,rb.velocity.y); //将受伤时速度改为0实现怪物被击退

     StartCoroutine( OnHurt(dir) ); //开启协程


     }

     private IEnumerator OnHurt(Vector2 dir) //协程， 迭代器，
     {
          rb.AddForce(dir * hurtForce, ForceMode2D.Impulse);//先执行
          yield return new WaitForSeconds(1f); //等待时间
          isHurt = false ; //然后执行
          anim.SetBool("isHurt", isHurt);

     }

     public void OnDie()
     {
          gameObject.layer=9;
          anim.SetBool("dead",true);
          isDead = true;
     }

     public void DestroyAnim()
     {
          Destroy(this.gameObject);
     }

#endregion
}
