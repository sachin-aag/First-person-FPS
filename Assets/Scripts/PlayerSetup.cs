
using UnityEngine;
using Mirror;

[RequireComponent(typeof(Player))]
[RequireComponent(typeof(PlayerController))]
public class PlayerSetup : NetworkBehaviour

{
    [SerializeField] Behaviour[] componentsToDisable ;
    [SerializeField] private string remoteLayerName = "Remote Player Layer";
    [SerializeField] private string dontdrawlayer = "DontDraw";
    [SerializeField] GameObject playerGraphics;
    [SerializeField] GameObject playerUIPrefab;
    [HideInInspector]
    public GameObject playerUIinstance;
    private void Start()
    {
        if (!isLocalPlayer)
        {
            DisableComponents();
            AssignRemoteLayer();
        }
        else
        {
            

            //Disable player graphics for local player
            SetLayerRecursively(playerGraphics, LayerMask.NameToLayer(dontdrawlayer));

            playerUIinstance = Instantiate(playerUIPrefab);
            playerUIinstance.name = playerUIPrefab.name;

            //Configure playerUI
            PlayerUI ui = playerUIinstance.GetComponent<PlayerUI>();
            if(ui== null)
            {
                Debug.LogError("No player UI on playerUI prefab");
            }
            ui.SetPlayerController(GetComponent<PlayerController>());
            GetComponent<Player>().SetupPlayer();


        }
    }
    private void SetLayerRecursively(GameObject obj, int newLayer)
    {
        obj.layer = newLayer;
        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }
    public override void OnStartClient()
    {
        base.OnStartClient();
        string _netID = GetComponent<NetworkIdentity>().netId.ToString();
        Player _player = GetComponent<Player>();
        GameManager.RegisterPlayer(_netID, _player);
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
        if (isLocalPlayer)
        {
            GameManager.instance.SetSceneCameraActive(true);
            GameManager.DeregisterPlayer(transform.name);
        }
        Destroy(playerUIinstance);
    }
}
