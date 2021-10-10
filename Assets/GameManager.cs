using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance { get; set; }


    public List<GameObject> playerList;
    public List<GameObject> deadPlayers;
    public List<GameObject> livingPlayers;
    public List<GameObject> wonPlayers;

    public List<int> scores;

    public List<GameObject> spawnPoints;
    public List<GameObject> pachinkoZones;
    public GameObject pachinkoSpawnPoint;
    public TargetGroupManager tgm;

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
            playerList = PlayerManager.Instance.playerList;
            deadPlayers = PlayerManager.Instance.deadPlayers;
            livingPlayers = PlayerManager.Instance.livingPlayers;
            wonPlayers = PlayerManager.Instance.wonPlayers;
            InitMap();
            scores = new List<int> { 0, 0, 0, 0 };
        }
    }

    public void IsLevelEnd()
    {
        if (livingPlayers.Count == 0)// && deadPlayers.Count + wonPlayers.Count == playerList.Count
        {
            CleanUp();
            GameObject.FindGameObjectWithTag("LevelLoader").GetComponent<LevelLoader>().LoadNextLevel();
        }
    }

    public void InitMap()
    {
        spawnPoints = GameObject.FindGameObjectsWithTag("spawnpoint").ToList();
        pachinkoSpawnPoint = GameObject.FindGameObjectWithTag("deathSpawn");
        pachinkoZones = GameObject.FindGameObjectsWithTag("PachinkoZone").ToList();

        tgm = Camera.main.transform.parent.GetComponentInChildren<TargetGroupManager>();
        tgm.players.Clear();

        foreach (var go in playerList)
            go.GetComponent<CharacterController>().MoveToClimbing();

        for (int i = 0; i < livingPlayers.Count; i++)
        {
            livingPlayers[i].transform.position = spawnPoints[i].transform.position;
            livingPlayers[i].transform.localScale = new Vector3(1f, 1f, 1f);
            tgm.players.Add(livingPlayers[i]);
        }

        tgm.Setup();

        if (deadPlayers.Count == 0)
        {
            for (int i = 0; i < pachinkoZones.Count; i++)
            {
                Debug.Log(pachinkoZones.Count);
                pachinkoZones[i].transform.position += new Vector3(pachinkoZones[i].transform.position.x, pachinkoZones[i].transform.position.y, 100);
            }
        }
    }

    public void SpawnThePachinko()
    {
        Debug.Log("SPAWN PACHINI");
        for (int i = 0; i < pachinkoZones.Count; i++)
        {
            pachinkoZones[i].transform.position += new Vector3(pachinkoZones[i].transform.position.x, pachinkoZones[i].transform.position.y, -100);
        }
    }

    private void CleanUp()
    {
        livingPlayers.Clear();
        deadPlayers.Clear();
        wonPlayers.Clear();
        foreach (var go in playerList)
        {
            go.transform.parent = PlayerManager.Instance.gameObject.transform;
            livingPlayers.Add(go);
        }
    }

    public void AddScore(GameObject gameObject, bool win)
    {
        for (int i = 0; i < playerList.Count; i++)
        {
            if (gameObject == playerList[i])
            {
                if (win)
                    scores[i] += livingPlayers.Count + deadPlayers.Count;
                else
                    scores[i] += deadPlayers.Count + 1;
            }
        }
    }
}
