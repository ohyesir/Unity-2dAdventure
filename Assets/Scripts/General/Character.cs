using System.Collections;
using System.Collections.Generic;
using Microsoft.SqlServer.Server;
using UnityEngine;
using UnityEngine.Events; //调用事件，一键触发多个代码

public class Character : MonoBehaviour, ISave
{
    [Header("事件监听")]
    public VoidSO newGameEvent;

    [Header("基本属性")]
    public float maxHealth;
    public float currentHealth;
    public float maxStamina;
    public float currentStamina;
    public float staminaReduceSpeed;
    public float staminaRecoverSpeed;
    public bool isWaitStaminaRecover;
    public float waitStaminaRecoverDuration;
    public float waitStaminaRecoverCount;

    [Header("受伤无敌")]
    public float invulnerableDuration; //无敌invulnerable 持续时间
    public float invulnerableCount; //无敌倒计时
    public bool invulnerable; //是否处于无敌

    [Header("事件广播")]
    public UnityEvent<Character> OnHealthChange; //注册事件列表
    public UnityEvent<Transform> OnTakeDamage;  //创建OnTakeDamage事件函数，参数是Transform
    public UnityEvent OnDie;


    // private PlayerController playerController;
    private PhysicsCheck physicsCheck;
    
    private void OnEnable() 
    {
        physicsCheck = GetComponent<PhysicsCheck>();
        
        // playerController = GetComponent<PlayerController>();
        newGameEvent.OnEventRaised += NewGame;
        ISave isave = this;
        isave.RegistSaveData();
    }
    void NewGame()
    {
        currentHealth = maxHealth; //将当前血量等于最大血量
        currentStamina = maxStamina;
        OnHealthChange?.Invoke(this);
    }

    private void OnDisable() 
    {
        newGameEvent.OnEventRaised -= NewGame;
        ISave isave = this;
        isave.UnRegistSaveData();
    }

    private void Update()
    {
        if(invulnerable)
        {
            invulnerableCount -= Time.deltaTime; //修正时间，无敌时间倒计时
            if(invulnerableCount <= 0)
            {
                invulnerable = false;    //当无敌时间小于0退出无敌
            }
        }
        
        SetStamina();
        
    }

    private void OnTriggerStay2D(Collider2D other) 
    {
        if(other.CompareTag("Obstacles")) //对比是否碰到标签
        {
            if(currentHealth > 0) //只有在生命值大于0时会执行否则死了之后也会一直执行
            {
                currentHealth = 0;
                OnHealthChange?.Invoke(this);
                OnDie?.Invoke();
            }
            
        }
    }

    public void TakeDamage(Attack attacker) //受到攻击，参数是攻击者
    {
        if(invulnerable)
            return;     //如果处于无敌则不执行下面的扣血代码
        
        if(currentHealth - attacker.damage > 0)
        {
            currentHealth -= attacker.damage;//如果不处于无敌则扣血并进入无敌
            TriggerInvulnerable();
            //执行受伤
            OnTakeDamage?.Invoke(attacker.transform);  //判断是否有函数加入这个事件，如果有就传入attacker的transform
        }
        else
        {
            currentHealth = 0;
             OnDie?.Invoke();

        }
       
       OnHealthChange?.Invoke(this);
    }
    
    public void SetStamina()
    {
        if(physicsCheck.isPlayer && currentStamina > 0.1f)
        {
            if((physicsCheck.playerController.isShield || physicsCheck.playerController.isCharge) && currentStamina >= 1)
            {
                currentStamina -= staminaReduceSpeed * Time.deltaTime; //耐力消耗
                
            }
            else if(currentStamina < maxStamina)
            {
                if(!isWaitStaminaRecover)
                    currentStamina += staminaRecoverSpeed * Time.deltaTime; //耐力恢复
                else
                {
                    waitStaminaRecoverCount -= Time.deltaTime;
                    if(waitStaminaRecoverCount <= 0)
                    {
                        isWaitStaminaRecover = false;//行动后耐力隔一段时间后继续恢复
                    }
                }
                
            }
            
            OnHealthChange?.Invoke(this);
        }
    }

    public void TriggerWaitStaminaRecover()
    {
        if(!isWaitStaminaRecover)
        {
            isWaitStaminaRecover = true;
            waitStaminaRecoverCount = waitStaminaRecoverDuration;
        }
    }

    private void TriggerInvulnerable() //无敌开关
    {
        if(!invulnerable)
        {
            invulnerable = true;
            invulnerableCount = invulnerableDuration;//将无敌倒计时= 无敌持续时间
        }
    }

    public CreateID GetObjectID()
    {
        return GetComponent<CreateID>();
    }

    public void GetSaveData(SaveData saveData)
    {
        if(saveData.characterPositonDic.ContainsKey(GetObjectID().ID)) //查找是否有同ID
        {
            saveData.characterPositonDic[GetObjectID().ID] = new SerializeVector3(transform.position); //有就覆盖
            saveData.floatSavedDataDic[GetObjectID().ID + "Health"] = maxHealth;
            saveData.floatSavedDataDic[GetObjectID().ID + "Stamina"] = maxStamina;
        }
        else
        {
            saveData.characterPositonDic.Add(GetObjectID().ID, new SerializeVector3(transform.position));
            saveData.floatSavedDataDic.Add(GetObjectID().ID + "Health", maxHealth);//添加到字典中
            saveData.floatSavedDataDic[GetObjectID().ID + "Stamina"] = maxStamina;
        }


    }

    public void LoadSaveData(SaveData saveData)
    {

        if(saveData.characterPositonDic.ContainsKey(GetObjectID().ID))
        {
            currentHealth = saveData.floatSavedDataDic[GetObjectID().ID + "Health"];
            // Debug.Log("读取生命" + currentHealth);
            currentStamina = saveData.floatSavedDataDic[GetObjectID().ID + "Stamina"];
            
            OnHealthChange?.Invoke(this);

        }
        
    }

    
}
