using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "DataSO/ItemSO")]
public class ItemSO : ScriptableObject
{
    public string itemName;
    public Sprite itemIcon;
    public string itemDescription;
}
