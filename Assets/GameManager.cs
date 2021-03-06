using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance { get; set; }

    public List<GameObject> playerList;
    public List<GameObject> deadPlayers;
    public List<GameObject> livingPlayers;
    public List<GameObject> wonPlayers;

    public List<int> scores;
    public List<string> shuffledList;

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
            playerList = PlayerManager.Instance.playerList;
            Debug.Log(playerList.Count);
            deadPlayers = PlayerManager.Instance.deadPlayers;
            livingPlayers = PlayerManager.Instance.livingPlayers;
            wonPlayers = PlayerManager.Instance.wonPlayers;
            InitMap();
            scores = new List<int> { 0, 0, 0, 0 };
            shuffledList = PlayerManager.Instance.scenes.OrderBy(x => UnityEngine.Random.value).ToList();
        }
    }

    public void IsLevelEnd()
    {
        if (livingPlayers.Count == 0)// && deadPlayers.Count + wonPlayers.Count == playerList.Count
        {
            CleanUp();
            if(shuffledList.Count > 0)
            {
                GameObject.FindGameObjectWithTag("LevelLoader").GetComponent<LevelLoader>().LoadNextLevel(shuffledList[0]);
                shuffledList.Remove(shuffledList[0]);
            }
            else
            {
                foreach (var player in playerList)
                    player.GetComponent<UIController>().onScoreboard = true;
                GameObject.FindGameObjectWithTag("LevelLoader").GetComponent<LevelLoader>().LoadScoreboard();
            }
        }
    }

    public void InitMap()
    {
        spawnPoints = GameObject.FindGameObjectsWithTag("spawnpoint").ToList();
        pachinkoSpawnPoint = Camera.main.gameObject.GetComponent<ObjectHolder>().sp;
        pachinkoZones = Camera.main.gameObject.GetComponent<ObjectHolder>().GOs;

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
                pachinkoZones[i].SetActive(false);
                //pachinkoZones[i].gameObject.SetActive(false);
                //pachinkoZones[i].transform.position += new Vector3(pachinkoZones[i].transform.position.x, pachinkoZones[i].transform.position.y, 100);
            }
        }
        Debug.Log("end"+ pachinkoZones.Count);
    }

    public void SpawnThePachinko()
    {
        for (int i = 0; i < pachinkoZones.Count; i++)
        {
            pachinkoZones[i].SetActive(true);
            //pachinkoZones[i].transform.position += new Vector3(pachinkoZones[i].transform.position.x, pachinkoZones[i].transform.position.y, -100);
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

        foreach (var go in playerList)
        {
            go.GetComponent<CharacterController>().MoveToClimbing();
            go.transform.position = new Vector3(999f, 0, 0);
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
