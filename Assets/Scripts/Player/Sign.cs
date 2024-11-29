using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Sign : MonoBehaviour
{
    public PlayerInputControl playerInputControl;
    public GameObject confirmButton; //获得挂载在sign下的子游戏对象物体confirmButton
    public Transform playerTrans;
    private Animator animator;
    private IInteractable targetItem;
    public bool canPress;

    private void Awake()
    {
        animator = confirmButton.GetComponent<Animator>(); //获得animator
        playerInputControl = new PlayerInputControl();
        playerInputControl.Enable();

    }

    private void OnEnable() 
    {
        InputSystem.onActionChange += OnActionChange; //当输入设备切换
        playerInputControl.GamePlay.Confirm.started += OnConfirm; //当按下confirm键注册一个函数
    }
    private void OnDisable() 
    {
        canPress = false;
    }

    private void OnConfirm(InputAction.CallbackContext context)
    {
        if(canPress)
        {
            targetItem.TrrigerAction(); //此处调用的其实是目标的trrigerAction方法

            
        }
    }

    private void OnActionChange(object obj, InputActionChange change)
    {
       if(change == InputActionChange.ActionStarted) //当动作启动，另一个设备按按钮
       {
            var devices = ((InputAction)obj).activeControl.device; //获得当前输入设备
            switch(devices.device)
            {
                case Keyboard: animator.Play("Keyboard");break;
                case Gamepad: animator.Play("Gamepad");break;
            }
       }
    }

    private void Update() 
    {
        confirmButton.GetComponent<SpriteRenderer>().enabled = canPress;//当可按钮时启动图片显示
        confirmButton.transform.localScale = playerTrans.localScale;
        //button的localScale应该是父级的localScale*自己的localScale，1*1或-1*-1
    }

    private void OnTriggerStay2D(Collider2D other) 
    {
        if(other.CompareTag("Interactable"))
        {   
            canPress = true; 
            targetItem = other.GetComponent<IInteractable>(); //获取碰撞物体的组件，使targetItem能调用TriggerAction
        }

    }

    private void OnTriggerExit2D(Collider2D other) 
    {
        if(other.CompareTag("Interactable"))
        {   
            canPress = false; 
        }
    }

}
