using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerSkill : MonoBehaviour
{
    [Header("充能")]
    public float maxCharge;
    public float currentCharge;
    public float chargeSpeed;
    public int chargeState;

    [Header("气刃")]
    public float timeDuration;
    public float timeCount;
    public bool disCharge; 

    

    
    private PlayerController playerController;
    
    public UnityEvent OnChargeSword;
    public UnityEvent OnDisChargeSword;

    

    private void Awake() 
    {
        playerController = GetComponent<PlayerController>();
        
    }

    private void Update() 
    {
        if (playerController.isCharge) 
        {  
            ChargeSword();
        }
        
        if(disCharge)
        {
            
            DisChargeSword();
                
        }
        
        
    }
    public void ChargeSword()
    {
        if(currentCharge <= maxCharge)
        {
            currentCharge += chargeSpeed * Time.deltaTime;
        }

        if((int)(currentCharge / 100) >= 1 && chargeState < 3 && (int)currentCharge / 100 != chargeState) // 每满100点，触发事件
        {

            OnChargeSword.Invoke();
            ReSetChargeSword();//开始计时
            chargeState += 1;
            
            
        }    
    }
    
    public void ReSetChargeSword()
    {
        timeCount = timeDuration;
        disCharge = true;
    }
    
    private void DisChargeSword()
    {
        if(timeCount > 0)
        {
            timeCount -= Time.deltaTime;
            if(chargeState == 0)
                disCharge = false;
        }
        else
        {
            if(chargeState > 0)
            {
                chargeState -= 1;
                currentCharge = chargeState * 100 + 1;
                OnDisChargeSword.Invoke();
                ReSetChargeSword();
            }
        }
        


    }

    

    
    
    

   
    





   
}
