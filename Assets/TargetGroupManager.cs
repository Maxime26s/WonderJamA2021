using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using static Cinemachine.CinemachineTargetGroup;

public class TargetGroupManager : MonoBehaviour
{
    public List<GameObject> players;
    public CinemachineTargetGroup targetGroup;
    // Start is called before the first frame update
    void Start()
    {
        //players = GetPlayers
        Target[] targets = new Target[players.Count];
        for (int i = 0; i < players.Count; i++)
        {
            targetGroup.AddMember(players[i].transform, 1, 0);
        }
    }


    // Update is called once per frame
    void Update()
    {
        float maxHeight = Mathf.NegativeInfinity;
        int index = -1;
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i] == null)
            {
                players.RemoveAt(i);
                break;
            }
            if (players[i].transform.position.y > maxHeight)
            {
                maxHeight = players[i].transform.position.y;
                index = i;
            }
        }

        for (int i = 0; i < players.Count; i++)
        {
            if (targetGroup.m_Targets[i].weight != 0f)
            {
                if (index == i)
                    targetGroup.m_Targets[i].weight = players.Count * 1.5f;
                else
                    targetGroup.m_Targets[i].weight = 1f;
            }
        }
    }
}
