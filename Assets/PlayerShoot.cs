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
        Debug.Log("Just shot ");
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out _hit, weapon.range, mask))
        {
            Debug.Log(_hit.collider.tag);
            if(_hit.collider.tag == PLAYER_TAG)
            {
                Debug.Log("here");
                Debug.Log(_hit.collider.name + " has been shot");
                CmdPlayerShot(_hit.collider.name);
            }
        }
    }
    [Command]
    void CmdPlayerShot(string _ID)
    {
        Debug.Log(_ID + " has been shot");
    }
}
