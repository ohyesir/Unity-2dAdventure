using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class PhysicsCheck : MonoBehaviour
{
    private CapsuleCollider2D coll;
    private Rigidbody2D rb;

    [Header("检测参数")]
    public bool isPlayer;
    public bool manual; //手动
    public Vector2 bottomOffset;//监测点与人物中心的位移差值
    public Vector2 leftOffset;
    public Vector2 rightOffset;



    public float checkRaduis;  //检测范围
    public LayerMask groundLayer;
    public PlayerController playerController;   

    [Header("状态")]
    public bool isGround;
    public bool isTouchLeftWall;
    public bool isTouchRightWall;
    public bool onWall;

    void Awake()
    {
        coll = GetComponent<CapsuleCollider2D>();

        if(!manual)
        {
            rightOffset = new Vector2((coll.bounds.size.x + coll.offset.x) /2, coll.bounds.size.y /2);
            leftOffset = new Vector2 (-rightOffset.x , rightOffset.y);
        }

        if(isPlayer)
        {
            playerController = GetComponent<PlayerController>();
            rb = GetComponent<Rigidbody2D>();
        }

    }

    private void Update()
    {
        Check();
    }
    public void Check()
    {
       //检测地面，ocerlabCircle就是圆形检测，返回的是bool值
       isGround = Physics2D.OverlapCircle((Vector2)transform.position + 
       new Vector2(bottomOffset.x * transform.localScale.x,bottomOffset.y) //使判断点能随面朝方向改变
       , checkRaduis, groundLayer);

       //检测左边墙体
       isTouchLeftWall = Physics2D.OverlapCircle((Vector2)transform.position + leftOffset, checkRaduis, groundLayer);

       isTouchRightWall = Physics2D.OverlapCircle((Vector2)transform.position + rightOffset, checkRaduis, groundLayer);

        if(isPlayer)
            onWall = rb.velocity.y < 0f &&  //当人物下落时，onwall为true播放下落动画
            (isTouchLeftWall &&  playerController.inputDirection.x < 0f || isTouchRightWall && playerController.inputDirection.x > 0f);
            //当输入方向与左墙一致时 onwall=true
    }

    private void OnDrawGizmosSelected()  //绘制出圆形检测范围
    {
        Gizmos.DrawWireSphere((Vector2)transform.position + 
        new Vector2(bottomOffset.x * transform.localScale.x,bottomOffset.y), checkRaduis);//前者是检测内容后者是检测范围
        Gizmos.DrawWireSphere((Vector2)transform.position + leftOffset, checkRaduis);
        Gizmos.DrawWireSphere((Vector2)transform.position + rightOffset, checkRaduis);
    }
}
