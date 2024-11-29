using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//此文件用于挂载在传送点上触发传送
public class SceneTeleport : MonoBehaviour, IInteractable
{
    public SceneLoadEventSO sceneLoadEventSO;

    public GameSceneSO sceneToGO;
    public Vector3 nextScenePosition;
    public void TrrigerAction()
    {
        sceneLoadEventSO.RaiseLoadRequestEvent(sceneToGO, nextScenePosition, true);
         //发出呼叫，在SceneLoader中进行监听
    }
}
