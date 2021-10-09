using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public bool gameStarted = false;
    public void OnStartAction()
    {
        if (!gameStarted)
        {   
            gameStarted = true;
            Debug.Log(PlayerManager.Instance);
            PlayerManager.Instance.StartGame();
        }
    }
}
