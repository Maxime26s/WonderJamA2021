using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Mianmenu : MonoBehaviour
{
    public Button play;
    public Button quit;
    public Button inst;
    public EventSystem es;

    private void Update()
    {
        if (es.currentSelectedGameObject == play.gameObject)
        {
            play.GetComponent<Image>().color = new Color(play.GetComponent<Image>().color.r, play.GetComponent<Image>().color.g, play.GetComponent<Image>().color.b, 0.7f);
        }
        else
        {
            play.GetComponent<Image>().color = new Color(play.GetComponent<Image>().color.r, play.GetComponent<Image>().color.g, play.GetComponent<Image>().color.b, 1f);
        }
        if (es.currentSelectedGameObject == quit.gameObject)
        {
            quit.GetComponent<Image>().color = new Color(quit.GetComponent<Image>().color.r, quit.GetComponent<Image>().color.g, quit.GetComponent<Image>().color.b, 0.7f);
        }
        else
        {
            quit.GetComponent<Image>().color = new Color(quit.GetComponent<Image>().color.r, quit.GetComponent<Image>().color.g, quit.GetComponent<Image>().color.b, 1f);
        }
        if (es.currentSelectedGameObject == inst.gameObject)
        {
            inst.GetComponent<Image>().color = new Color(inst.GetComponent<Image>().color.r, inst.GetComponent<Image>().color.g, inst.GetComponent<Image>().color.b, 0.7f);
        }
        else
        {
            inst.GetComponent<Image>().color = new Color(inst.GetComponent<Image>().color.r, inst.GetComponent<Image>().color.g, inst.GetComponent<Image>().color.b, 1f);
        }
    }

    
}
