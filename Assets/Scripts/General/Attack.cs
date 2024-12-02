using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public int damage;
    public float attackRange;
    public float attackRate;
    
    public virtual void OnTriggerEnter2D(Collider2D other)//当触碰到其他碰撞体触发trigger，此为持续碰撞
    {
        bool isShield = false;
        if(other.TryGetComponent(out PlayerController playerController))
        {
            isShield = playerController.isShield;
        }
        if(isShield)
        {
            // Debug.Log("格挡");
            other.GetComponent<Character>()?.GuardDamage(this);
        }
        else
        {
            other.GetComponent<Character>()?.TakeDamage(this);//如果对方有Character则执行,this传递的是当前的attack的参数
        }
            
            
    }
    
    
    

    
   
}
