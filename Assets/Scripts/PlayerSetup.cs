
using UnityEngine;
using Mirror;


public class PlayerSetup : NetworkBehaviour

{
    [SerializeField] Behaviour[] componentsToDisable ;
    Camera sceneCamera;
    [SerializeField] private string remoteLayerName = "Remote Player Layer";
    private void Start()
    {
        if (!isLocalPlayer)
        {
            DisableComponents();
            AssignRemoteLayer();
        }
        else
        {
            sceneCamera = Camera.main;
            if(sceneCamera != null)
            {
                sceneCamera.gameObject.SetActive(false);
            }
        }
        RegisterPlayer();
    }
    void RegisterPlayer()
    {
        string ID = "Player" + GetComponent<NetworkIdentity>().netId;
        transform.name = ID;
    }
    private void DisableComponents()
    {
        for (int i = 0; i < componentsToDisable.Length; i++)
        {
            componentsToDisable[i].enabled = false;
        }
    }
    private void AssignRemoteLayer()
    {
        gameObject.layer = LayerMask.NameToLayer(remoteLayerName);
    }

    private void OnDisable()
    {
        if (sceneCamera != null)
        {
            sceneCamera.gameObject.SetActive(true);
        }
    }
}
