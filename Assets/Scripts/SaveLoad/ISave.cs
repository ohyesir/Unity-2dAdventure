

public interface ISave 
{
    
    CreateID GetObjectID();
    
    void RegistSaveData() => SaveDataManager.instance.RegistSaveData(this);//通过类.实例.方法
            //通过唯一单例SaveDataManger来注册管理挂载Isave的类
    //注册与注销场景中需要保存的信息
    void UnRegistSaveData() => SaveDataManager.instance.UnRegistSaveData(this);//取消注册 效果等同于1 当函数体只有一条语句可以这么写
    void GetSaveData(SaveData saveDatas);//得到存储数据
    void LoadSaveData(SaveData saveData);//加载存储数据
}
