using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class DestroyableObject : MonoBehaviour
{
    public UnityEvent onDestroy;

    [SerializeField]
    private GameMaterialData materialData;

    private int currentDurability;

    public void Damage(int damage)
    {
        currentDurability -= damage;
        if(currentDurability <= 0)
        {
            DestroyObject();
        }
    }

    public void DestroyObject()
    {
        onDestroy?.Invoke();
        Destroy(gameObject);
    }

    public void Reset()
    {
        if(materialData)
        {
            currentDurability = materialData.Durability;
        }
    }

    private void Awake()
    {
        
    }
}

