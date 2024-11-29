using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CreateID : MonoBehaviour
{
    public PersistenType persistenType;
    public string ID;
    public string currentObjiectName;

   
    private void OnValidate() 
    {
        if (persistenType == PersistenType.Unchangeable)
        {
            if (ID == string.Empty)
                ID = System.Guid.NewGuid().ToString(); //生成string类的Guid
        }
        else
        {
            ID = string.Empty;
        }
        
        currentObjiectName = gameObject.name;

        // Debug.Log("GameObject " + "'" + gameObject.name +  "'"  + "     ID: " + ID);
    }
}
