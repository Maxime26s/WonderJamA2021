using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SceneTitle : MonoBehaviour
{
    public string levelName;
    public Image panel;
    public TextMeshProUGUI text;
    public float wait, timer, stop;
    public float textAlpha, panelAlpha;
    // Start is called before the first frame update
    void Start()
    {
        textAlpha = text.color.a;
        panelAlpha = panel.color.a;
        text.text = levelName;
        text.color = new Color(text.color.r, text.color.g, text.color.b, 0);
        panel.color = new Color(panel.color.r, panel.color.g, panel.color.b, 0);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer < wait)
        {
            text.color += new Color(0, 0, 0, textAlpha / wait * Time.deltaTime);
            panel.color += new Color(0, 0, 0, panelAlpha / wait * Time.deltaTime);
            if (text.color.a > textAlpha)
                text.color = new Color(text.color.r, text.color.g, text.color.b, textAlpha);
            if (panel.color.a > panelAlpha)
                panel.color = new Color(panel.color.r, panel.color.g, panel.color.b, panelAlpha);
        }
        else if (timer < stop + wait)
        {

        }
        else if (timer < stop + wait * 2)
        {
            text.color -= new Color(0, 0, 0, textAlpha / wait * Time.deltaTime);
            panel.color -= new Color(0, 0, 0, panelAlpha / wait * Time.deltaTime);
            if (text.color.a < 0)
                text.color = new Color(text.color.r, text.color.g, text.color.b, 0);
            if (panel.color.a < 0)
                panel.color = new Color(panel.color.r, panel.color.g, panel.color.b, 0);
        }
        else
            Destroy(gameObject);

    }
}
