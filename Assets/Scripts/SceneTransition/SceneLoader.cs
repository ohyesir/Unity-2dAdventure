using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.AddressableAssets; //寻找场景素材
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement; //场景加载工具
using UnityEngine.InputSystem;

//此文件挂载在loadManager上用于实现场景切换
public class SceneLoader : MonoBehaviour, ISave
{
    [Header("事件广播")]
    public VoidSO sceneLoadedDoneSO;
    public FadeEventSO fadeEventSO;
    public SceneLoadEventSO afterSceneLoad;

    [Header("事件监听")]
    public SceneLoadEventSO sceneLoadEventSO; //监听由SceneTeleport发起的事件
    public VoidSO newGameEvent;//监听新游戏按钮发起的事件
    public VoidSO backToMenuEvent;//监听返回菜单的按钮
    

    [Header("加载的场景SO")]
    public GameSceneSO firstLoadScene; //第一个游戏场景
    public GameSceneSO memuScene;
    private GameSceneSO currentScene;
    private GameSceneSO sceneToGo; //临时存储OnLoadRequestEvent传进来的变量
    private Vector3 posTogo; //可以取同名变量，用this指代这个类里的变量
    private bool fadeScreen;
    public float fadeDuration; //等候时间

    public Transform playerTrans; //通过拖拽获得player的transform
    public Vector3 playerFirstPosition;
    public Vector3 playerMenuPosition;
    private bool isLoading;


#region "Save Game"
#endregion
    private void Awake() 
    {
        sceneToGo = memuScene;
        // Addressables.LoadSceneAsync(firstLoadScene.sceneReference, LoadSceneMode.Additive);异步加载场景
        // currentScene = firstLoadScene;
        // currentScene.sceneReference.LoadSceneAsync(LoadSceneMode.Additive);
        
    }

    private void Start() 
    {
        
        sceneLoadEventSO.RaiseLoadRequestEvent(sceneToGo, playerMenuPosition, true); 
        //执行并广播,awake的执行速度太快此代码不能放在awake里，否则角色无法站立

        // NewGame();
    }

    public void Update() //执行Esc打开暂停菜单
    {
        if(Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            
        }
    }

    
    private void OnEnable() 
    {
        sceneLoadEventSO.LoadRequestEvent += OnLoadRequestEvent; //将unity动作注册为事件
        newGameEvent.OnEventRaised += NewGame;
        backToMenuEvent.OnEventRaised += BackToMenu;

        ISave Save = this;
        Save.RegistSaveData();
        
        
    }

#region "注册函数"
     private void OnLoadRequestEvent(GameSceneSO sceneToGo, Vector3 posTogo, bool fadeScreen)
    {
        if(isLoading)
            return;
        isLoading = true;

        this.sceneToGo = sceneToGo;
        this.posTogo = posTogo;
        this.fadeScreen = fadeScreen;

        if(currentScene != null)
            StartCoroutine(UnloadPreviosScene()); //当前场景不为空启用协程
        else
            LoadNewScene();//为空直接加载新场景
    }

    private IEnumerator UnloadPreviosScene() //创建协程，等待场景卸载完后加载新的场景
    {
        if(fadeScreen)
        {
            fadeEventSO.FadeIN(fadeDuration);
        }

        yield return new WaitForSeconds(fadeDuration); //场景变黑后卸载加载场景

        afterSceneLoad.RaiseLoadRequestEvent(sceneToGo, posTogo, fadeScreen);//广播

        currentScene.sceneReference.UnLoadScene();//执行卸载场景

        playerTrans.gameObject.SetActive(false); //在场景转换时关闭人物
        LoadNewScene();

    }

    
    

    private void NewGame()
    {
        sceneToGo = firstLoadScene;
        sceneLoadEventSO.RaiseLoadRequestEvent(sceneToGo, playerFirstPosition, true); //自己发起，自己监听自己
    }

    private void BackToMenu()
    {
        sceneToGo = memuScene;
        sceneLoadEventSO.RaiseLoadRequestEvent(sceneToGo, playerMenuPosition, true); 
    }
    
#endregion

    private void OnDisable() 
    {
        sceneLoadEventSO.LoadRequestEvent -= OnLoadRequestEvent;
        newGameEvent.OnEventRaised -= NewGame;
        backToMenuEvent.OnEventRaised -= BackToMenu;

        ISave Save = this;
        Save.UnRegistSaveData();
       
    }


    



   

    private void LoadNewScene()
    {
        var loadingScene = sceneToGo.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true); 
        // 与上面相比此处已经有了sceneReference不需要再写
        loadingScene.Completed += OnLoadCompleted; //函数加载完成后注册函数
        
    }

    private void OnLoadCompleted(AsyncOperationHandle<SceneInstance> handle)
    {
        currentScene = sceneToGo;
        TeleportPlayer();
        
        if(currentScene.sceneType != SceneType.Menu)
            sceneLoadedDoneSO.RaiseEvent();//通知场景已经加载完了
        
        if(fadeScreen)
            fadeEventSO.FadeOut(fadeDuration);

        
    }

    private void TeleportPlayer()
    {
        playerTrans.position = posTogo;
        playerTrans.gameObject.SetActive(true); //在转移后恢复
        isLoading = false;
    }

    public CreateID GetObjectID()
    {
        return GetComponent<CreateID>();
    }

    public void GetSaveData(SaveData saveData)
    {
        saveData.SaveGameScene(currentScene);
        
    }

    public void LoadSaveData(SaveData saveData)
    {
        var playerID = playerTrans.GetComponent<CreateID>().ID;

        if(saveData != null && saveData.characterPositonDic.ContainsKey(playerID)) //读取有playerID，恢复player位置和恢复场景
        {
            posTogo = saveData.characterPositonDic[playerID].ToVector3();
            sceneToGo = saveData.LoadGameScene();
            // Debug.Log("LoadedSceneis" + sceneToGo);
            OnLoadRequestEvent(sceneToGo, posTogo, true);
        }

        else
        {  
            NewGame();
        }
    }

    

    
}
