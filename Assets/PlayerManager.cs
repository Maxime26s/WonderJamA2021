using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    public List<Color32> colors;
    public List<SceneAsset> scenes;
    public List<GameObject> playerList;
    public List<GameObject> deadPlayers;
    public List<GameObject> livingPlayers;
    public List<GameObject> wonPlayers;
    public static PlayerManager Instance { get; set; }
    public int nbPlayer = 0;
    public int maxPlayers = 0;
    public bool gameStarted = false;

    public void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
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
            newPlayer.gameObject.GetComponentInChildren<MeshRenderer>().material.color = colors[nbPlayer];
            newPlayer.gameObject.GetComponent<LineRenderer>().materials[0].color = colors[nbPlayer];
            nbPlayer++;
        }
    }

    public void StartGame()
    {
        gameStarted = true;
        gameObject.GetComponent<PlayerInputManager>().DisableJoining();
        LevelLoader levelLoader = GameObject.FindGameObjectWithTag("LevelLoader").GetComponent<LevelLoader>();
        Debug.Log(levelLoader);

        int index = Random.Range(0, scenes.Count);
        string name = scenes[index].name;
        scenes.Remove(scenes[index]);

        levelLoader.LoadNextLevel(name);
    }
}
