using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "GameMaterialData", menuName = "Data/Game Material Data", order = 0)]
public class GameMaterialData : ScriptableObject
{
    [SerializeField]
    private string id;

    [SerializeField]
    private int durability = 100;

    public string ID => id;
    public int Durability => durability;
}
