using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PlayerShadowPool : MonoBehaviour
{
    [SerializeField] PlayerShadowSprite playerShadowPrefab;
    public ObjectPool<PlayerShadowSprite> shadowPool;

    public static PlayerShadowPool instance ;
    // Start is called before the first frame update
    private void Awake() 
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        // 初始化对象池
        shadowPool = new ObjectPool<PlayerShadowSprite>(
            GetShadow,
            (obj) => obj.gameObject.SetActive(true),
            (obj) => obj.gameObject.SetActive(false),
            (obj) => Destroy(obj.gameObject),
            true,
            10,
            10

        );
    }

    public PlayerShadowSprite GetShadow()
    {
        var shadow = Instantiate(playerShadowPrefab).GetComponent<PlayerShadowSprite>();
        shadow.shadowPool = this.shadowPool;
        shadow.transform.SetParent(transform);
        return shadow;
    }
}
