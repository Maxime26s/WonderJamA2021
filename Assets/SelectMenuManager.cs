using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SelectMenuManager : MonoBehaviour
{
    public List<Sprite> playerPreviews;
    public List<PlayerInput> playerInputs;
    public LevelLoader levelLoader;
    public List<Image> previewHolder;
    public int nbPlayers = 0;

    public void OnPlayerJoined()
    {
        /*previewHolder[nbPlayers].sprite = playerPreviews[nbPlayers];*/
        nbPlayers++;
    }
}
