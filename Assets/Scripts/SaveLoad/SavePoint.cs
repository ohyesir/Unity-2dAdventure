using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : MonoBehaviour, IInteractable
{
    private SpriteRenderer signSpriteRenderer;
    public GameObject signLight;
    public Sprite darkSprite;
    public Sprite lightSprite;

    [Header("冷却计时器")]
    public float waitTime;
    public float waitTimeCounter;
    public bool isWait;

    //存储场景所需
    [Header("广播")]
    public VoidSO saveGameDataEvent; //在SaveDataManager中监听


    private void Awake() 
    {
        Transform childTransform = transform.GetChild(0);
        signSpriteRenderer = childTransform.GetComponent<SpriteRenderer>();
    }

    private void Update() 
    {
        WaitTimeCounter();
    }

    private void WaitTimeCounter()
    {
        if(isWait)
        {
            
            waitTimeCounter -= Time.deltaTime;
            signLight.SetActive(isWait); //开启灯光
            
            if(waitTimeCounter <= 0)
            {
                signSpriteRenderer.sprite = darkSprite; //当计时器结束变黑可继续保存
                isWait = false;
                
                signLight.SetActive(isWait);
            }
        }
    }

    public void TrrigerAction()
    {
        SaveGame();
    }

    public void SaveGame()
    {
        if(!isWait)
        {
            signSpriteRenderer.sprite = lightSprite;
            GetComponent<AudioDefinition>()?.PlayAudioClip(); //播放音乐代码
        
            waitTimeCounter = waitTime;
            
            isWait = true;
            
            saveGameDataEvent.RaiseEvent();


            
        }
        

    }
}


