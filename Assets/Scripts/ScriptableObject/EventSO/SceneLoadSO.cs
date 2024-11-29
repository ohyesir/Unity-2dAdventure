using UnityEngine;
using UnityEngine.Events;



//此文件为事件SO用来启动切换场景
[CreateAssetMenu(menuName = "Event/SceneLoadEventSO")]
public class SceneLoadEventSO : ScriptableObject 
{
    public UnityAction<GameSceneSO, Vector3, bool> LoadRequestEvent; //unity的事件action是在监听者用注册函数实现的
    //需要下一个场景的so，下个场景的目标点，一个是否需要渐进渐出的效果

    public void RaiseLoadRequestEvent(GameSceneSO nextSceneSO, Vector3 nextScenePosition, bool fadeScreen)
    {
        LoadRequestEvent?.Invoke(nextSceneSO, nextScenePosition, fadeScreen);
    }
}
