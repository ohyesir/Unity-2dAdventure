using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Chest : MonoBehaviour, IInteractable//只能继承一个父类，但能继承多个接口
{
    private SpriteRenderer spriteRenderer;
    public Sprite chestOpen;
    public Sprite chestClose;
    public bool isOpen;
   
    public void TrrigerAction()
    {

        OpenChest();
    }

    private void Awake() 
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OpenChest()
    {
        if(!isOpen)
        {
            spriteRenderer.sprite = chestOpen;
            isOpen = true;
            GetComponent<AudioDefinition>()?.PlayAudioClip(); //播放音乐代码
        }
        else
        {
            spriteRenderer.sprite = chestClose;
            isOpen = false;
        }
        

    }

   

   
}
