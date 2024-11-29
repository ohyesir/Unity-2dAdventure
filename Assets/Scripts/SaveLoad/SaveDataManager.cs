using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Newtonsoft.Json; //实现序列化
using System.IO; //实现写入


[DefaultExecutionOrder(order: -200)]
//越小越优先执行
//在Edit菜单下的project setting也可以调整

public class SaveDataManager : MonoBehaviour
{
    [Header("事件监听")]
    public VoidSO saveGameDataEvent;
    public VoidSO ReStartGameEvent; //监听restartButton发起的事件


    public static SaveDataManager instance ;//单例模式，static的作用域使所有代码，且在游戏开始到游戏关闭一直存在于内存
    //只允许有instance这一个实例

    public List<ISave> saveList = new();//创建列表用于注册的类
    public SaveData saveData;
    private string jsonFolder;//保存数据的文件夹

    private void Awake() 
    {
        if(instance == null)  //确保只有一个单例模式
            instance = this;
        else   
            Destroy(this.gameObject);

        saveData =  new SaveData();

        jsonFolder = Application.persistentDataPath + "/SaveData/"; //绝对文件夹路径
        //C:\Users\ranxing\AppData\LocalLow\Start

        

       
    }

    public void RegistSaveData(ISave save) //注册所有继承ISave接口的类 
    {
        if(!saveList.Contains(save)) //如果当前列表没有传递过来的这个save项则添加
            saveList.Add(save);
    }

    public void UnRegistSaveData(ISave save)
    {
        saveList.Remove(save); //移除save
    }

    private void OnEnable() 
    {
        saveGameDataEvent.OnEventRaised += OnSaveGame;

        ReStartGameEvent.OnEventRaised += ReStartGame;
    }

    private void ReStartGame()
    {
        
        ReadSavedData();
        
        LoadSavedGame();
    }

    public void OnSaveGame()
    {
        //当玩家按下传送点，在DataManager中收到广播在saveList中挨个执行自己类中的GetSaveData

        foreach(var save in saveList)
        {
            save.GetSaveData(saveData);  
        }

        var resultPath = jsonFolder + "saveData.sav"; //绝对文件名

        var jsonData = JsonConvert.SerializeObject(saveData); //序列化

        if(!File.Exists(resultPath))//创造路径
        {
            Directory.CreateDirectory(jsonFolder);
        }

        File.WriteAllText(resultPath, jsonData);

    }

    private void OnDisable()
    {
        saveGameDataEvent.OnEventRaised -= OnSaveGame;
        ReStartGameEvent.OnEventRaised -= ReStartGame;
    }
    

    public void LoadSavedGame()
    {
        // Debug.Log("读取存档Load");
        foreach (var save in saveList)
        {
            save.LoadSaveData(saveData);
        }
    }

    private void ReadSavedData()
    {

        var resultPath = jsonFolder + "saveData.sav"; //绝对文件名
        // Debug.Log(resultPath);
         if(File.Exists(resultPath))
         {
            var stringData = File.ReadAllText(resultPath);    
            
            var jsonData = JsonConvert.DeserializeObject<SaveData>(stringData);  //读取序列化
            // Debug.Log("读取存档");
            saveData = jsonData;
         }
        //  else
        //  {
        //     Debug.Log("没有read存档");
        //  }
    }
}

