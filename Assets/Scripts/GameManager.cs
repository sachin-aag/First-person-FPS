using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Mirror;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public MatchSettings matchSettings;
    [SerializeField]
    private GameObject sceneCamera;
    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("More than one game manager in scene");
        }
        else
        {
            instance = this;
        }
    }
    public void SetSceneCameraActive(bool isActive)
    {
        if(sceneCamera == null)
        {
            return;

        }
        sceneCamera.SetActive(isActive);
    }

    #region Player tracking
    private const string PLAYER_ID_PREFIX = "Player";
    //Create a dictionary to store id to player mapping
    private static Dictionary<string, Player> players = new Dictionary<string, Player>();
    public static void RegisterPlayer(string _netID, Player _player)
    {
        string _playerID = PLAYER_ID_PREFIX + _netID;
        players.Add(_playerID, _player);
        _player.transform.name = _playerID;
    }
    public static void DeregisterPlayer(string _playerID)
    {
        players.Remove(_playerID);
    }
    public static Player GetPlayer(string _playerID)
    {
        return players[_playerID];
    }
    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(200f, 200f, 200f, 500f));
        GUILayout.BeginVertical();
        foreach(string _playerID in players.Keys)
        {
            GUILayout.Label(_playerID + " - " + players[_playerID].transform.name);
        }
        GUILayout.EndVertical();
        GUILayout.EndArea();
    }
    #endregion


}
