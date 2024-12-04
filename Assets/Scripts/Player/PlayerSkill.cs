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
    public int chargeState; //充能状态

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
        if(currentCharge <= maxCharge) //当现在充能小于最大充能进行充能
        {
            currentCharge += chargeSpeed * Time.deltaTime;
        }

        if((int)(currentCharge / 100) >= 1 && chargeState < 3 && (int)currentCharge / 100 != chargeState) 
        // 每满100点，触发事件
        {
            
            ReSetChargeSword();//开始掉刃
            chargeState += 1;
            OnChargeSword.Invoke();
            
            
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
            if(chargeState > 0)  //时间到了状态-1
            {
                chargeState -= 1;
                currentCharge = chargeState * 100 + 1;
                OnDisChargeSword.Invoke();
                ReSetChargeSword();
            }
        }

        


    }

    public void UpdateCharge() //完美格挡时更新充能
    {
        if(chargeState < 3 )
        {
            currentCharge += 100;
            chargeState += 1;
            OnChargeSword.Invoke();
            
        }
    }
    
    public void UpdateSwordLight()
    {
        Material playerMaterial = GetComponent<Renderer>().material;
        Debug.Log("当前阶段" + chargeState);
        switch(chargeState)
        {
            case 0: case 1:
                playerMaterial.SetColor("_SwordColor",new Color(0.5f, 0.5f, 0.5f));break;
            case 2:
                playerMaterial.SetColor("_SwordColor",new Color(1, 1, 0));break;
            case 3:
                playerMaterial.SetColor("_SwordColor",new Color(1, 0, 0));break;
        }
    }

    
    
    

   
    





   
}
