
using UnityEngine;

[System.Serializable]
public class PlayerWeapon : MonoBehaviour
{
    [SerializeField] public string _name = "Glock";
    [SerializeField] public int damage = 10;
    [SerializeField] public float range = 100f;
    public GameObject graphics;
    public float fireRate = 0f;
        
    
}
