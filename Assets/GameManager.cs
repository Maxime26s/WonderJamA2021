using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance { get; set; }


    public List<GameObject> playerList;
    public List<GameObject> deadPlayers;
    public List<GameObject> livingPlayers;
    public List<GameObject> wonPlayers;

    public List<int> scores;

    public GameObject spawnPoint;
    public GameObject pachinkoSawnPoint;
    public TargetGroupManager tgm;

    public void Awake()
    {
        if (Instance != null)
        {

        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);
            playerList = new List<GameObject>();
        }

        playerList = PlayerManager.Instance.playerList;
        deadPlayers = PlayerManager.Instance.deadPlayers;
        livingPlayers = PlayerManager.Instance.livingPlayers;
        wonPlayers = PlayerManager.Instance.wonPlayers;

        scores = new List<int> { 0, 0, 0, 0 };

        int offset = -8;
        Debug.Log(livingPlayers.Count);
        foreach (GameObject player in livingPlayers)
        {
            player.transform.position = spawnPoint.transform.position + new Vector3(offset, 0, 0);
            player.transform.localScale = new Vector3(1f, 1f, 1f);
            tgm.players.Add(player);
            offset += 6;
        }
        foreach (GameObject player in deadPlayers)
        {
            player.transform.position = pachinkoSawnPoint.transform.position;
            player.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        }
    }

    public void IsLevelEnd()
    {
        Debug.Log("no end");
        if (livingPlayers.Count == 0)// && deadPlayers.Count + wonPlayers.Count == playerList.Count
        {
            CleanUp();
            GameObject.FindGameObjectWithTag("LevelLoader").GetComponent<LevelLoader>().LoadNextLevel();
        }
    }

    private void CleanUp()
    {
        livingPlayers.Clear();
        deadPlayers.Clear();
        wonPlayers.Clear();
        foreach (var go in playerList)
        {
            go.transform.parent = gameObject.transform;
            livingPlayers.Add(go);
        }
    }

    public void AddScore(GameObject gameObject)
    {
        for (int i = 0; i < playerList.Count; i++)
        {
            if (gameObject == playerList[i])
            {
                scores[i] += livingPlayers.Count * 50;
            }
        }
    }
}
