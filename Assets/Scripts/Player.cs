using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Player : NetworkBehaviour
{
    [SerializeField] private int maxHealth = 100;
    [SyncVar]
    private int currentHealth;
    [SerializeField] private Behaviour[] disableOnDeath;
    private bool[] wasEnabled;
    [SyncVar]
    private bool _isDead = false;
    [SerializeField] private GameObject[] objDisableOnDeath;
    [SerializeField]
    private GameObject deathEffect;
    public bool isDead
    {
        get { return _isDead; }
        protected set { _isDead = value; }
    }
    [ClientRpc]
    public void RpcTakeDamage(int _damage)
    {
        if (isDead)
        {
            return;
        }
        currentHealth -= _damage;
        Debug.Log(transform.name + " now has health " + currentHealth);
        if(currentHealth <= 0)
        {
            Die();
        }
    }
    public void Setup()
    {
        wasEnabled = new bool[disableOnDeath.Length];
        for (int i = 0; i < wasEnabled.Length; i++)
        {
            wasEnabled[i] = disableOnDeath[i].enabled;
        }
        SetDefaults();
    }
    private void Die()
    {
        isDead = true;

        //Disable components
        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = false;
        }

        for (int i = 0; i < objDisableOnDeath.Length; i++)
        {
            objDisableOnDeath[i].SetActive(false);
        }
        Collider _col = GetComponent<Collider>();
        if (_col != null)
        {
            _col.enabled = false;
        }
        GameObject _gfxIns = (GameObject)Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(_gfxIns, 3f);
        //Log message
        Debug.Log(transform.name + " is DEAD!");

        // Respawn method
        StartCoroutine(Respawn());

        Debug.Log(transform.name + " respawned");
    }
    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(GameManager.instance.matchSettings.respawnTime);
        SetDefaults();
        Transform _spawnPoint = NetworkManager.singleton.GetStartPosition();
        transform.position = _spawnPoint.position;
        transform.rotation = _spawnPoint.rotation;
    }
    public void SetDefaults()
    {
        isDead = false;
        currentHealth = maxHealth;
        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = wasEnabled[i];
        }
        for (int i = 0; i < objDisableOnDeath.Length; i++)
        {
            objDisableOnDeath[i].SetActive(true);
        }

        //Since collider is not a behaviour
        Collider _col = GetComponent<Collider>();
        if(_col != null)
        {
            _col.enabled = true;
        }
    }
    private void Update()
    {
        if (!isLocalPlayer)
            return;
        if (Input.GetKeyDown(KeyCode.K))
        {
            RpcTakeDamage(1000);
        }
    }
}
