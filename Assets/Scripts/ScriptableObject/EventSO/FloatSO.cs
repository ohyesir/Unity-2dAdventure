using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Event/FloatSO")]
public class FloatSO : ScriptableObject
{
    public UnityAction<float> OnEventRaised;

    public void RaiseEvent(float volumeValue)
   {
        OnEventRaised?.Invoke(volumeValue);
   }
}
