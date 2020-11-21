using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Player : NetworkBehaviour
{
    [SerializeField] private int maxHealth = 100;
    [SyncVar]
    private int currentHealth; 
    public void TakeDamage(int _damage)
    {
        currentHealth -= _damage;
        Debug.Log(transform.name + " now has health " + currentHealth);
    }
    private void Awake()
    {
        SetDefaults();
    }
    public void SetDefaults()
    {
        currentHealth = maxHealth;
    }
}
