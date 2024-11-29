using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Runtime.InteropServices.ComTypes;
using TMPro;
using UnityEditor.ProjectWindowCallback;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour,ISave
{
#region 参数

    [Header("事件监听")]
    public SceneLoadEventSO sceneLoadEventSO; //f2集体改名
    public VoidSO sceneLoadedDoneSO;
    // public VoidSO RestartGameEvent;
    public VoidSO RestartGameEvent;
   

    
    public  PlayerInputControl inputControl;//通过类创建对象
    public  Vector2 inputDirection;
    private Rigidbody2D rb; // 若是public 可以再unity中拖拽执行
    private PlayerAnimation playerAnimation;
    private PhysicsCheck physicsCheck;
    private CapsuleCollider2D coll;
    private Character playerCharacter;
    private Vector2 originalOffset;
    private Vector2 originalSize;


    [Header("基本参数")]
    public  float speed;
    private float runSpeed;
    private float walkSpeed =>  speed / 2.5f; //每次调用都会执行
    public float slideDistance;
    public float slideSpeed;
    public float jumpForce;
    public float wallJumpForce;
    public float hurtForce;
    public int combo;  //动作长时用来变招

    [Header("状态")]
    public bool isCrouch;
    public bool isAttack;
    public bool isHurt;
    public bool wallJump;
    public bool isSlide;
    public bool isShield;
    public bool isCharge;
    public bool isDead ;

    [Header("物理材质")]
    public PhysicsMaterial2D normal;
    public PhysicsMaterial2D wall;
    public PhysicsMaterial2D slide;

#endregion

    private void Awake() 
    {

        rb = GetComponent<Rigidbody2D>();  //获取该人物的组件

        physicsCheck = GetComponent<PhysicsCheck>(); //获得跳跃检测
        playerAnimation = GetComponent<PlayerAnimation>();
        playerCharacter = GetComponent<Character>();

        coll = GetComponent<CapsuleCollider2D>(); // 改变碰撞体大小
        
        originalOffset = coll.offset; //记录初始值
        originalSize = coll.size;
    //GetComponent是获取自身的组件，new是获取非自身的
        inputControl = new PlayerInputControl(); // 获得输入创建新实例
        inputControl.GamePlay.Jump.started += Jump;  //+=注册事件函数，可以再写一行添加别的事件函数触发一键多功能
                                                    //.started是当案件按下时注册这个函数
        inputControl.GamePlay.Attack.started += PlayerAttack;
        inputControl.GamePlay.Slide.started += Slide;
        inputControl.GamePlay.Shield.started += Shield;
        inputControl.GamePlay.Shield.canceled += Shield; //取消防御
        inputControl.GamePlay.Charge.started += Charge;
        inputControl.GamePlay.Charge.canceled += Charge;
        

        inputControl.Enable();

#region 强制走路
        runSpeed = speed; // 让跑动速度等于初始速度
       
        bool isRun = false;    
                                                // callbackcontest 是unity中自带的回调函数，使用lambda表达式
        inputControl.GamePlay.WalkButton.started  += ctx =>   //通过shift切换走路跑步，+=注册了一个ctx回调函数 =>
        {
            if(physicsCheck.isGround && isRun == true)
            {
                speed = runSpeed;
                isRun = false;
            
            }
            else
            {
                speed = walkSpeed;
                isRun = true;
               
            }
        };

        
        
#endregion
    }
    
    
    private void OnEnable()     //当人物player启动时启动，再unity中勾选启动或者关闭
    {
        
        sceneLoadEventSO.LoadRequestEvent += OnSceneLoadEventSO;
        sceneLoadedDoneSO.OnEventRaised += OnSceneLoadedDoneSO;
        // RestartGameEvent.OnEventRaised += RestartGame;
        RestartGameEvent.OnEventRaised += RestartGame;
        

        ISave save = this;
        save.RegistSaveData();
        
    }

#region "注册函数"
    private void RestartGame()
    {

        
        isDead = false;
        
    }


    private void OnSceneLoadEventSO(GameSceneSO arg0, Vector3 arg1, bool arg2)
    {

        inputControl.GamePlay.Disable();
    }

    private void OnSceneLoadedDoneSO()
    {
        inputControl.GamePlay.Enable();
        
    }

#endregion

    private void OnDisable()
    {
        inputControl.Disable(); //当人物player关闭时关闭，
        sceneLoadEventSO.LoadRequestEvent -= OnSceneLoadEventSO;
        sceneLoadedDoneSO.OnEventRaised -= OnSceneLoadedDoneSO;
        // RestartGameEvent.OnEventRaised -= RestartGame;
        RestartGameEvent.OnEventRaised -= RestartGame;

        ISave save = this;
        save.UnRegistSaveData();
       
    }
    
    private void Update()
    {
        inputDirection = inputControl.GamePlay.Move.ReadValue<Vector2>(); //读取自己创建的ganmeplay的move里的数值

        CheckState();
    }

    private void FixedUpdate()
    {
        if(!isHurt && !isAttack && !isSlide)
            Move();
    }


    

    public void Move()
    {
#region 人物移动及翻转    
        if(!isCrouch && !wallJump && !isShield && !isCharge)
        {
            rb.velocity = new Vector2(inputDirection.x * speed * Time.deltaTime ,rb.velocity.y); 
                   //人物移动，Time.deltatime是一个时间的修正，帮助我们在不同的电脑配置下获得相同效果
            
            int faceDir = (int)transform.localScale.x;  //人物翻转
            if(inputDirection.x > 0)
                faceDir = 1;
            if(inputDirection.x < 0)
                faceDir = -1;
            transform.localScale = new Vector3(faceDir,1,1);
        }
            
        
        
#endregion

#region 人物下蹲
        isCrouch = inputDirection.y < -0.5f && physicsCheck.isGround;

        if(isCrouch)
        {
            //修改碰撞体
            coll.size = new Vector2(1.07f,1.65f);
            coll.offset = new Vector2(-0.0899f,0.81f);
        }
        else
        {
            //还原之前参数
            coll.size = originalSize;
            coll.offset = originalOffset;

        }
#endregion

    }

    private void Jump(InputAction.CallbackContext obj)
    {
       if(playerCharacter.currentStamina >= 20)
       {
        if(physicsCheck.isGround )
            {
            rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);//transform.up是添加向上的力，号后面是力的模式此处为瞬时力。
            GetComponent<AudioDefinition>().PlayAudioClip();
            }
        if(physicsCheck.onWall )
        {
            rb.AddForce(new Vector2(-inputDirection.x, 2.5f) * wallJumpForce, ForceMode2D.Impulse);
            wallJump = true;
        }
            playerCharacter.currentStamina -= 20;
            playerCharacter.TriggerWaitStaminaRecover();
            // playerCharacter.SetStamina();
       }
       
    }

    private void Shield(InputAction.CallbackContext obj)
    {
        if(!obj.canceled && physicsCheck.isGround)
            {
                isShield = true;
                // playerCharacter.SetStamina();
            }
        if(obj.canceled || playerCharacter.currentStamina < 1)
            {
                isShield = false;
                playerCharacter.TriggerWaitStaminaRecover();
            
                // playerCharacter.isWaitStaminaRecover = true;
            }
    }
    private void Charge(InputAction.CallbackContext obj)
    {

        if(!obj.canceled && physicsCheck.isGround)
            {
                isCharge = true;
                // playerCharacter.SetStamina();
            }
        if(obj.canceled || playerCharacter.currentStamina < 1)
            {
                isCharge = false;
                playerCharacter.TriggerWaitStaminaRecover();
            }
    }
    
    
    
    private void Slide(InputAction.CallbackContext obj)
    {
        if(physicsCheck.isGround && !isSlide && (playerCharacter.currentStamina >= 20))//在地面且不在滑行状态下才的到新的点
        {
            isSlide = true;
            playerCharacter.currentStamina -= 20;
            playerCharacter.TriggerWaitStaminaRecover();
            
            // playerCharacter.SetStamina();
            var targetPoint = new Vector3(transform.position.x + slideDistance * transform.localScale.x
                                            , transform.position.y);//现在的坐标加上滑铲距离*方向，现在的y值
            
            
            StartCoroutine(TriggerSlide(targetPoint));
        }
       
    }

    private IEnumerator TriggerSlide(Vector3 targetPoint) //当携程返回null时就会不断执行
    {
        do
        {
            yield return null;

            if(physicsCheck.isTouchLeftWall || physicsCheck.isTouchRightWall)
                break;
            
            rb.MovePosition(new Vector2(transform.position.x + slideSpeed * transform.localScale.x, transform.position.y));
            
        }while(Mathf.Abs(targetPoint.x - transform.position.x) > 0.1f);

        
    }


    private void PlayerAttack(InputAction.CallbackContext obj)
    {
        playerAnimation.AttackAnima();
        isAttack = true;
        /* combo++;
        if(combo>3)
            combo = 0; */
    }

    public void GetHurt(Transform attacker)
    {
        isHurt = true;
        rb.velocity = Vector2.zero;  //将速度等于0取消惯性

        //计算受伤方向
        Vector2 dir = new Vector2((transform.position.x -attacker.position.x) , transform.position.y -attacker.position.y).normalized;
        // 人物坐标x-攻击者坐标x,如果人物在左边就会得到负数,人物在右边就会得到正数
        //normalized的作用是当玩家x与目标x过大时归一化处理，返回一个固定值1

        rb.AddForce(dir * hurtForce , ForceMode2D.Impulse);//添加瞬时力进行反弹



    }

    public void Dead()
    {
        isDead = true;
        inputControl.GamePlay.Disable();//将游玩按键禁止
        
    }

    private void CheckState() //检查状态来判断用哪种材质
    {
        coll.sharedMaterial = physicsCheck.isGround ? normal : wall;



        /* if(physicsCheck.onWall)   改变材质的话判定会变，回退出onwall状态
        {
            coll.sharedMaterial = slide;
        } */

        if(physicsCheck.onWall)//改速度就不会发生此情况
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y/2);
        else
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y);

        if(rb.velocity.y < 0f)
            wallJump = false;


        if(isDead || isSlide)
        {
            gameObject.layer = LayerMask.NameToLayer("Enemy"); //将检测层变为敌人，不在触发trigger
        }
        else
        {
            gameObject.layer = LayerMask.NameToLayer("Player");
        }
    }

    public CreateID GetObjectID()
    {
        return GetComponent<CreateID>();
    }


    public void GetSaveData(SaveData saveData)
    {
        if(saveData.characterPositonDic.ContainsKey(GetObjectID().ID))
        {
            saveData.boolSavedDataDic[GetObjectID().ID + "IsDead"] = isDead;
        }

        else
        {
            saveData.boolSavedDataDic.Add(GetObjectID().ID + "IsDead", isDead);
        }
    }

    public void LoadSaveData(SaveData saveData)
    {
        if(saveData.characterPositonDic.ContainsKey(GetObjectID().ID))
        {
            isDead = saveData.boolSavedDataDic[GetObjectID().ID + "IsDead"];
           

        }
    }

}

