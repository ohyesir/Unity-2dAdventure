using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public int damage;
    public float attackRange;
    public float attackRate;
    




    
    public virtual void OnTriggerStay2D(Collider2D other)//当触碰到其他碰撞体触发trigger，此为持续碰撞
     /* void OnTriggerEnter(Collider other)    当碰撞体刚碰撞
    private void OnTriggerExit(Collider other)  当碰撞体离开     */  
    {
        other.GetComponent<Character>()?.TakeDamage(this);//如果对方有Character则执行,this传递的是当前的attack的参数
        
            
            
    }
    
    
    

    
   
}
