using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
[RequireComponent(typeof(WeaponManager))]
public class PlayerShoot : NetworkBehaviour
{
    private const string PLAYER_TAG = "Player";
    [SerializeField] private Camera cam;
    private PlayerWeapon currentWeapon;
    [SerializeField] private LayerMask mask;
    private WeaponManager weaponManager;
    public PlayerWeapon GetCurrentWeapon()
    {
        return currentWeapon;
    }

    private void Start()
    {
        if(cam == null)
        {
            Debug.LogError("No camera can be referenced");
            this.enabled = false;
        }

        weaponManager = GetComponent<WeaponManager>();
     
    }
    private void Update()
    {
        currentWeapon = weaponManager.GetCurrentWeapon();
        if(currentWeapon.fireRate <= 0)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Shoot();
            }
        }
        else
        {
            if (Input.GetButtonDown("Fire1"))
            {
                InvokeRepeating("Shoot", 0, 1f/currentWeapon.fireRate);
            }else if (Input.GetButtonUp("Fire1"))
            {
                CancelInvoke("Shoot");
            }
        }
       
    }
    //Is called on the server when a player shoots
    [Command]
    private void CmdOnShoot()
    {
        RpcDoShootEffects(); 
    }
    [Command]
    private void CmdOnHit(Vector3 _pos, Vector3 _normal)
    {
        RpcDoHitEffect(_pos, _normal);
    }
    [ClientRpc]
    void RpcDoHitEffect(Vector3 _pos, Vector3 _normal)
    {
        GameObject hiteffectRef = (GameObject)Instantiate(weaponManager.GetWeaponGraphics().hitEffectPrefab, _pos, Quaternion.LookRotation(_normal));
        Destroy(hiteffectRef, 2f);
    }
    //Is called on all clients when we need to do a shoot efect
    [ClientRpc]
    void RpcDoShootEffects()
    {
        Debug.Log(weaponManager.GetWeaponGraphics().muzzleFlash.transform.position.ToString());
        weaponManager.GetWeaponGraphics().muzzleFlash.Play();
    }
    [Client]
    private void Shoot()
    {
        RaycastHit _hit;
        if (!isLocalPlayer)
        {
            return;
        }
        CmdOnShoot();
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out _hit, currentWeapon.range, mask))
        {

            
            if (_hit.collider.tag == PLAYER_TAG)
            {
                Debug.Log(_hit.collider.name);
                CmdPlayerShot(_hit.collider.name, currentWeapon.damage);
            }
            CmdOnHit(_hit.point, _hit.normal);
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
