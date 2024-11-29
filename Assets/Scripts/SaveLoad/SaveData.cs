

//作为存储的数据类型
using System.Collections.Generic;
using UnityEngine;


public class SaveData 
{

    public string sceneToSave;

    public Dictionary<string, SerializeVector3> characterPositonDic = new();

    public Dictionary<string, float> floatSavedDataDic = new();
    public Dictionary<string, bool> boolSavedDataDic = new();


    public void SaveGameScene(GameSceneSO savedScene)
    {
        // Debug.Log(savedScene.sceneType);//输出存档时的场景
        sceneToSave = JsonUtility.ToJson(savedScene); //将GameSceneSO文件转为序列化的Json文件，存入到sceneToSave里
    }

    public GameSceneSO LoadGameScene()
    {
        var newScene = ScriptableObject.CreateInstance<GameSceneSO>(); //创建一个新的GameSceneSO实例
        JsonUtility.FromJsonOverwrite(sceneToSave, newScene);
        // Debug.Log(newScene.sceneType + " is loaded”");//输出读取后的场景名字
        return newScene;
    }
    
}

public class SerializeVector3
{
    public float x, y, z;

    public SerializeVector3(Vector3 pos)
    {
        this.x = pos.x;
        this.y = pos.y;
        this.z = pos.z;
    }

    public Vector3 ToVector3()
    {
        return new Vector3(x, y, z);
    }
}
