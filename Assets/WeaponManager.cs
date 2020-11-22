using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class WeaponManager : NetworkBehaviour
{
    [SerializeField]
    private PlayerWeapon primaryWeapon;

    private PlayerWeapon currentWeapon;
    [SerializeField]
    private GameObject weaponGFX;
    [SerializeField]
    private string weaponLayerName = "Weapon";
    [SerializeField] private Transform weaponHolder;
    // Start is called before the first frame update
    void Start()
    {
        EquipWeapon(primaryWeapon);
    }

   void EquipWeapon(PlayerWeapon _weapon)
    {
        currentWeapon = _weapon;
        GameObject _weaponIns = Instantiate(_weapon.graphics, weaponHolder.position, weaponHolder.rotation);
        _weaponIns.transform.SetParent(weaponHolder);
        if (isLocalPlayer)
        {
            _weaponIns.layer = LayerMask.NameToLayer(weaponLayerName);
        }
    }
    public PlayerWeapon GetCurrentWeapon()
    {
        return currentWeapon;
    }
}
