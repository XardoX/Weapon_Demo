using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class DestroyableObject : MonoBehaviour
{
    public UnityEvent onDestroy;

    [SerializeField]
    private MaterialType materialType;

    [SerializeField]
    private float durability;

    [SerializeField]
    private ParticleSystem destroyParticle;

    public void Damage(float damage, MaterialType damageType)
    {
        if (damageType != materialType) return;
        durability -= damage;
        if(durability <= 0)
        {
            DestroyObject();
        }
    }

    public void DestroyObject()
    {
        destroyParticle.transform.parent = null;
        destroyParticle.Play();
        onDestroy?.Invoke();
        Destroy(gameObject);
    }
}

public enum MaterialType
{
    Default,
    Wood,
    Metal,
    Concrete
}