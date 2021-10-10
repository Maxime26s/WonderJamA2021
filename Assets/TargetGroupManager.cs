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
    
    float timeElapsed;
    float lerpDuration = 2f;
    int old_index;
    IEnumerator currentCoroutine;

    void Start()
    {
    } 

    // Update is called once per frame
    void Update()
    {
        float maxHeight = Mathf.NegativeInfinity;
        int index = -1;
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].transform.position.y > maxHeight && targetGroup.m_Targets[i].weight != 0f)
            {
                maxHeight = players[i].transform.position.y;
                index = i;
            }
        }

        if (old_index != index)
        {
            old_index = index;
            List<float> startValues = new List<float>();
            for (int i = 0; i < players.Count; i++)
                startValues.Add(targetGroup.m_Targets[i].weight);
            if (currentCoroutine != null)
                StopCoroutine(currentCoroutine);
            currentCoroutine = LerpWeight(index, lerpDuration, startValues);
            StartCoroutine(currentCoroutine);
        }
    }

    IEnumerator LerpWeight(float main, float duration, List<float> startValues)
    {
        float timeElapsed = 0;

        while (timeElapsed < duration)
        {
            for (int i = 0; i < players.Count; i++)
            {
                if (targetGroup.m_Targets[i].weight != 0f)
                {
                    if (main == i)
                    {
                        targetGroup.m_Targets[i].weight = Mathf.Lerp(startValues[i], players.Count * 1f, timeElapsed / duration);
                    }

                    else
                    {
                        targetGroup.m_Targets[i].weight = Mathf.Lerp(startValues[i], 1f, timeElapsed / duration);
                    }
                    
                }
            }
            timeElapsed += Time.deltaTime;
            yield return null;
        }
    }

    public void Setup()
    {
        targetGroup.m_Targets = null;
        foreach (var player in players)
        {
            targetGroup.AddMember(player.transform, 1, 0);
        }
    }
}
