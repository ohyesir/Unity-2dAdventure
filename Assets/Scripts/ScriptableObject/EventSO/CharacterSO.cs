using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/* 此文件作为中间件来保存数据
   在character中呼叫调用，在uimanager中监听再传到palyerState
   数据流向：character -> characterSO -> UIManager -> playerState */
[CreateAssetMenu(menuName = "Event/CharacterSO")]//创建了event 右键能选中
public class CharacterSO : ScriptableObject
{
   public UnityAction<Character> OnEventRaised;//<脚本名> <事件名> 传递character出去，广播


   public void RaiseEvent(Character character)//谁想启动就把自己的Character传进去
   {
        OnEventRaised?.Invoke(character);
   }
}
