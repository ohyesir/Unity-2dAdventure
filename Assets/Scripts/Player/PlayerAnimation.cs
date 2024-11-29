using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events; //调用事件，一键触发

public class PlayerAnimation : MonoBehaviour
{
   private Animator anim;
   private Rigidbody2D rb;
   private PhysicsCheck physicsCheck;
   private PlayerController playerController;
   private void Awake()
   {
       anim = GetComponent<Animator>();      //获得动画组件
       rb = GetComponent<Rigidbody2D>();    //获得刚体2D组件
       physicsCheck = GetComponent<PhysicsCheck>();
       playerController = GetComponent<PlayerController>();
   }

   private void Update()
   {
    SetAnimation();
   }

   public void SetAnimation()   //设置动画
   {
      anim.SetFloat("velocityX",Mathf.Abs(rb.velocity.x) ); // 将速度x的绝对值赋给velocityX
      anim.SetFloat("velocityY",rb.velocity.y );//获得y值
      anim.SetBool("isGround",physicsCheck.isGround);
      anim.SetBool("isCrouch",playerController.isCrouch);
      anim.SetBool("isAttack",playerController.isAttack);
      anim.SetInteger("combo",playerController.combo);
      anim.SetBool("isDead",playerController.isDead);
      anim.SetBool("isSlide",playerController.isSlide);
      anim.SetBool("isShield",playerController.isShield);
      anim.SetBool("isCharge",playerController.isCharge);
      anim.SetBool("onWall",physicsCheck.onWall);

   }

   public void AttackAnima()
   {
      anim.SetTrigger("attack");//使用触发器来触发动画
   }

   public void PlayHurt()
   {
      anim.SetTrigger("hurt");//使用触发器来触发动画
   }
}
