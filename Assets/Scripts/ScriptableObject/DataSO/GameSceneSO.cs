using UnityEngine;
using UnityEngine.AddressableAssets;//调用addressableAssets实现场景转换


//此文件用于定义场景素材SO
[CreateAssetMenu(menuName = "DataSO/GameSceneSO")]
public class GameSceneSO : ScriptableObject 
{
    public SceneType sceneType; //标识场景是什么类型的
    public AssetReference sceneReference; //创建场景素材引用
    
}
