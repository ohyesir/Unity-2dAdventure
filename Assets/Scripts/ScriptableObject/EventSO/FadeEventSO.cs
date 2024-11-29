using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Event/FadeEventSO")]
public class FadeEventSO : ScriptableObject 
{
    public UnityAction<Color, float, bool> OnEventRaised; 

    public void FadeIN(float fadeDuration) //逐渐变黑
    {
        RaiseEvent(Color.black, fadeDuration, true);
    }

    public void FadeOut(float fadeDuration)//逐渐恢复
    {
        RaiseEvent(Color.clear, fadeDuration, false); //clear变透明
    }

    public void RaiseEvent(Color target, float fadeDuration, bool fadeIn)
    {
        OnEventRaised?.Invoke(target, fadeDuration, fadeIn);
    }

}
