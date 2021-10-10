using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreboardManager : MonoBehaviour
{
    public List<TextMeshProUGUI> texts;
    public GameObject displayPlayer;
    public List<GameObject> spawners;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < texts.Count; i++)
        {
            texts[i].text = GameManager.Instance.scores[i].ToString();
            if(i < GameManager.Instance.playerList.Count)
            {
                GameObject newDisplayPlayer = Instantiate(displayPlayer, new Vector3(spawners[i].transform.position.x, spawners[i].transform.position.y, spawners[i].transform.position.z - 5f), transform.rotation);
                newDisplayPlayer.GetComponentInChildren<MeshRenderer>().material.color = PlayerManager.Instance.colors[i];

                GameManager.Instance.playerList[i].transform.position = new Vector3(999, 0, 0);
            }
        }
    }
}
