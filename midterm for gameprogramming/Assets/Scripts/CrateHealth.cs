using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateHealth : MonoBehaviour, IDamageable
{
    public float health;

    public void Damage(float damageAmount)
    {
        health -= damageAmount;
    }

    private void Update()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
