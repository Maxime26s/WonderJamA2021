using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    
    public List<GameObject> playerList;
    public List<GameObject> deadPlayers;
    public List<GameObject> livingPlayers;
    public static PlayerManager Instance { get; set; }
    public int nbPlayer = 0;
    public int maxPlayers = 0;
    public bool gameStarted = false;

    public void Awake()
    {
        if(Instance != null)
        {

        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);
            playerList = new List<GameObject>();
        }
    }

    public void PlayerJoined(PlayerInput newPlayer)
    {
        if (nbPlayer < maxPlayers)
        {
            newPlayer.transform.SetParent(transform);
            playerList.Add(newPlayer.gameObject);
            livingPlayers.Add(newPlayer.gameObject);
            nbPlayer++;
        }
    }

    public void StartGame()
    {
        gameStarted = true;
        gameObject.GetComponent<PlayerInputManager>().DisableJoining();
        LevelLoader levelLoader = GameObject.FindGameObjectWithTag("LevelLoader").GetComponent<LevelLoader>();
        Debug.Log(levelLoader);
        levelLoader.LoadNextLevel();
    }
}
