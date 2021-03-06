using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    public List<Color32> colors;
    public List<string> scenes;
    public List<GameObject> playerList;
    public List<GameObject> deadPlayers;
    public List<GameObject> livingPlayers;
    public List<GameObject> wonPlayers;

    public GameObject displayPlayer;
    public List<GameObject> spawners;
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
            DontDestroyOnLoad(gameObject);
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
            GameObject newDisplayPlayer = Instantiate(displayPlayer, new Vector3(spawners[nbPlayer].transform.position.x, spawners[nbPlayer].transform.position.y, spawners[nbPlayer].transform.position.z - 5f), transform.rotation);
            newDisplayPlayer.GetComponentInChildren<MeshRenderer>().material.color = colors[nbPlayer];
            newPlayer.transform.position = new Vector3(999f, 0, 0);
            newPlayer.gameObject.GetComponent<CharacterController>().SetColor(nbPlayer, colors[nbPlayer]);

            nbPlayer++;
        }
    }

    public void StartGame()
    {
        if (!gameStarted)
        {
            gameStarted = true;
            gameObject.GetComponent<PlayerInputManager>().DisableJoining();
            LevelLoader levelLoader = GameObject.FindGameObjectWithTag("LevelLoader").GetComponent<LevelLoader>();
            Debug.Log(levelLoader);
            int index = Random.Range(0, scenes.Count);
            string name = scenes[index];
            scenes.Remove(scenes[index]);

            levelLoader.LoadNextLevel(name);
        }
    }
}
