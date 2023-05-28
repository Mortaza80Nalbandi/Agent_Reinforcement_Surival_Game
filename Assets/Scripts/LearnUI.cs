using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LearnUI : MonoBehaviour
{
    public Text text;
    public Vector3 Offset;
    private int i;
    private string y;
    private string lastWord;

    // Start is called before the first frame update
    void Start()
    {
        i = 0;
        y = "";
    }

    // Update is called once per frame
    void Update()
    {
        text.transform.position = Camera.main.WorldToScreenPoint(transform.parent.position + Offset);
    }
    public void addText(string x)
    {
        if (x != lastWord)
        {
            y = y + x + "\n";
            text.text = y;
            i++;
            lastWord = x;
        }

        if (i >= 5)
        {
            y = "";
            i = 0;
        }
    }
}
