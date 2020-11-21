using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerShoot : NetworkBehaviour
{
    private const string PLAYER_TAG = "Player";
    [SerializeField] private Camera cam;
    public PlayerWeapon weapon;
    [SerializeField] private LayerMask mask;


    private void Start()
    {
        if(cam == null)
        {
            Debug.LogError("No camera can be referenced");
            this.enabled = false;
        }
    }
    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }
    [Client]
    private void Shoot()
    {
        RaycastHit _hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out _hit, weapon.range, mask))
        {
            Debug.Log(_hit.collider.tag);
            if(_hit.collider.tag == PLAYER_TAG)
            {
                
                CmdPlayerShot(_hit.collider.name, weapon.damage);
            }
        }
    }
    //Called only on Server
    [Command]
    void CmdPlayerShot(string _playerID, int _damage)
    {
        Debug.Log(_playerID + " has been shot");

        Player _player = GameManager.GetPlayer(_playerID);
        _player.RpcTakeDamage(_damage);
    }
}
