using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LearnUI : MonoBehaviour
{
    public Text text;
    public Vector3 Offset;
    private int printedLinesNumber;
    private string printedLines;
    private string lastSentence;

    void Start()
    {
        printedLinesNumber = 0;
        printedLines = "";
    }

    void Update()
    {
        text.transform.position = Camera.main.WorldToScreenPoint(transform.parent.position + Offset);
    }
    public void addText(string newComment)
    {
        if (printedLinesNumber > 5)
        {
            printedLines = "";
            printedLinesNumber = 0;
        }
        if (newComment != lastSentence)
        {
            printedLines = printedLines + newComment + "\n";
            text.text = printedLines;
            printedLinesNumber++;
            lastSentence = newComment;
        }


    }
}
