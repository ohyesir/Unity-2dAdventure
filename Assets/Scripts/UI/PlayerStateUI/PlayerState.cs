using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerState : MonoBehaviour
{
    public Image healthImage;
    public Image healthDecreaseImage;
    public Image StaminaImage;

    private void Start() 
    {
        healthDecreaseImage.fillAmount = healthImage.fillAmount;
    }

    public void OnHealthChange(float healthPercent)
    {
        
        healthImage.fillAmount = healthPercent;
    }

    public void OnStaminaChange(float StaminaPercent)
    {
        StaminaImage.fillAmount = StaminaPercent;
    }

    public void Update()
    {
        if(healthDecreaseImage.fillAmount > healthImage.fillAmount)
        {
            healthDecreaseImage.fillAmount -= Time.deltaTime * 0.5f;
        }
        
    }
}
