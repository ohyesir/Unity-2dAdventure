using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Unity.VisualScripting;

public class FadeCanvas : MonoBehaviour
{
    public FadeEventSO fadeEventSO;
    
    public Image fadeImage;


    
    private void OnEnable() 
    {

        fadeEventSO.OnEventRaised += OnFadeEvent;
    }

    private void OnDisable() 
    {
        fadeEventSO.OnEventRaised -= OnFadeEvent;
    }

    private void OnFadeEvent(Color targetColor, float fadeDuration, bool fadeIn)
    {
        fadeImage.DOBlendableColor(targetColor, fadeDuration);
    }
    
}
