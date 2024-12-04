using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : Attack
{
    public int damageVariable;
    private Animator animator;
    
    private void Awake() 
    {
        animator = gameObject.GetComponentInParent<Animator>();
    }

    public override void OnTriggerEnter2D(Collider2D other)
    {
        other.GetComponent<Character>()?.TakeDamage(this);//如果对方有Character则执行,this传递的是当前的attack的参数
        if(other.CompareTag("Enemy") && animator.GetCurrentAnimatorStateInfo(1).IsName("blueAttack3"))
        {
            // Debug.Log("三段攻击了");
            GetComponentInParent<PlayerSkill>()?.ReSetChargeSword();
        }
    }

    public void IncreaseDamage()
    {
    
        damage += damageVariable;
        // Debug.Log("攻击力" + damage);
    }
    
    public void DecreaseDamage()
    {
        damage -= damageVariable;
        // Debug.Log("攻击力" + damage);
    }
}
