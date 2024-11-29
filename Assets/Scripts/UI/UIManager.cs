using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public PlayerState playerState;
    public FadeCanvas  fadeCanvas;
    public GameSceneSO currentScene;
    public bool isMune;


    [Header("事件监听")]
    public CharacterSO healthEvernt;
    public SceneLoadEventSO afterSceneLoad;
    public VoidSO RestartGameEvent;
    public VoidSO GameOverEvent;
    public VoidSO BackToMenuEventSO;
    public FloatSO SyncVolumeEvent;

    [Header("事件广播")]
    public VoidSO PauseEvent;
   

    [Header("组件")]
    public GameObject GameOverPanel;
    public GameObject ReStartButton;
    public GameObject MobileControl;
    public Button PauseButton;
    public GameObject PausePannel;
    public Slider VolumeSlider;

    private void Awake() 
    {
#if UNITY_STANDALONE    // 判断是否为PC端,为PC端就隐藏触控
        MobileControl.SetActive(false);
#endif

    PauseButton.onClick.AddListener(() =>
    {
        if (PausePannel.activeInHierarchy) // 判断面板是否在游戏界面显示
        {
            Time.timeScale = 1;  //游戏正常运行
            PausePannel.SetActive(false);
        }
        else
        {
            
            PausePannel.SetActive(true);
            PauseEvent.RaiseEvent();
            Time.timeScale = 0;  //游戏暂停
        }
    });
    }
    
    private void OnEnable() {
        healthEvernt.OnEventRaised += OnHealthEvent;//将OnHealthEvent函数加入订阅
        afterSceneLoad.LoadRequestEvent += OnAfterSceneLoad;
        RestartGameEvent.OnEventRaised += RestartGame;
        BackToMenuEventSO.OnEventRaised += RestartGame; //在主界面菜单取消GameOverGanel

        GameOverEvent.OnEventRaised += GameOver;

        SyncVolumeEvent.OnEventRaised += SyncVolume;
    }

    

    #region "注册函数"

    private void SyncVolume(float volume)
    {
       volume = (volume + 80 ) / 100;
       VolumeSlider.value = volume;
    }

    private void GameOver()
    {
        GameOverPanel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(ReStartButton);//设置默认选项
    }

    private void RestartGame()
    {
        GameOverPanel.SetActive(false);
    }

    private void OnHealthEvent(Character character)
    {
        var healthPercent = character.currentHealth / character.maxHealth;
        playerState.OnHealthChange(healthPercent);
        playerState.OnStaminaChange(character.currentStamina / character.maxStamina);
    
    }

    private void OnAfterSceneLoad(GameSceneSO currentScene, Vector3 arg1, bool arg2)
    {
        bool isMune = currentScene.sceneType == SceneType.Menu;
        playerState.gameObject.SetActive(!isMune);
    }

#endregion  
    
    private void OnDisable() {
        healthEvernt.OnEventRaised -= OnHealthEvent;
        afterSceneLoad.LoadRequestEvent -= OnAfterSceneLoad;
        RestartGameEvent.OnEventRaised -= RestartGame;
        BackToMenuEventSO.OnEventRaised -= RestartGame;
        GameOverEvent.OnEventRaised -= GameOver;
        SyncVolumeEvent.OnEventRaised -= SyncVolume;

    }
}
