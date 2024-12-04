using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Pool;

public class PlayerShadowSprite : MonoBehaviour
{
    private SpriteRenderer shadowSpriteRenderer; //自身的SpriteRenderer
    private SpriteRenderer playerSprite;//玩家的SpriteRenderer
    private Transform playerTransform;//获取玩家的Transform
    
    [Header("时间信息")]
    [SerializeField] float shadowDuration; //阴影持续时间
    [SerializeField] float shadowStartTime; //阴影出现时间
    [Header("阴影参数")]
    [SerializeField] float colorAset; //阴影透明度
    [SerializeField] float Multipler;//阴影缩放倍数
    private float colorA;//阴影透明度初始值
    public ObjectPool<PlayerShadowSprite> shadowPool; //阴影对象池当Duration时间结束之后会自动回收

    private void OnEnable() 
    {
        //获取参数
        shadowSpriteRenderer = GetComponent<SpriteRenderer>();
        playerTransform = GameObject.FindWithTag("Player").transform;
        //transform 是每个 GameObject 自带的一个属性，因此你不需要使用 GetComponent<Transform>() 来获取它。
        playerSprite = playerTransform.GetComponent<SpriteRenderer>();
        

        //初始化
        colorA = colorAset;
        shadowSpriteRenderer.sprite = playerSprite.sprite;
        this.transform.position = playerTransform.position;
        this.transform.localScale = playerTransform.localScale;

        //记录初始当前时间
        shadowStartTime = Time.time;
    }

    private void Update()
    {
        if (object.ReferenceEquals(shadowPool, null))
        {
            Debug.Log("shadowPool is null");
            return;
        }
        else
        {
            //如果当前时间 >= 阴影出现时间 + 阴影持续时间则返回对象池
            if(Time.time >= shadowStartTime + shadowDuration) 
            {
                shadowPool.Release(this);
            }
            else
            {
                colorA *= Multipler;
                shadowSpriteRenderer.color = new Color(1, 1, 1, colorA);
            }
        }
    }

}
