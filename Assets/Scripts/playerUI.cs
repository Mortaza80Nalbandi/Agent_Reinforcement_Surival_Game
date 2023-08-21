using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class playerUI : MonoBehaviour
{
    private int Irons;
    Text ironText;
    Text scoreText;
    Text learnText;
    private int score;
    Image m_Image;
    public Sprite Bow_Sprite;
    public Sprite Block_Sprite;
    public Sprite Sword_Sprite;
    private int printedLinesNumber;
    private string printedLines;
    private string lastSentence;
    // Start is called before the first frame update
    void Start()
    {
        m_Image = transform.GetChild(0).gameObject.GetComponent<Image>();
        scoreText = transform.GetChild(1).gameObject.GetComponent<Text>();
        ironText = transform.GetChild(2).gameObject.GetComponent<Text>();
        learnText = transform.GetChild(4).gameObject.GetComponent<Text>();
        printedLinesNumber = 0;
        printedLines = "LearningTracker : \n";
    }

    public void updateIron(int irs)
    {
        ironText = transform.GetChild(2).gameObject.GetComponent<Text>();
        Irons = irs;
        ironText.text = "" + Irons;
    }
    public void updateWeapon(string wpn)
    {
        if (wpn == "Bow")
        {
            m_Image.sprite = Bow_Sprite;
        }
        else if (wpn == "Meele")
        {
            m_Image.sprite = Sword_Sprite;
        }
        else if (wpn == "Block")
        {
            m_Image.sprite = Block_Sprite;
        }
    }
    public void updateScore(int score)
    {
        scoreText = transform.GetChild(1).gameObject.GetComponent<Text>();
        this.score = score;
        scoreText.text = "Score : " + score;
    }
    public void addText(string newComment)
    {
        if (printedLinesNumber > 20)
        {
            printedLines = "LearningTracker : \n";
            printedLinesNumber = 0;
        }
        if (newComment != lastSentence)
        {
            printedLines = printedLines + newComment + "\n";
            learnText.text = printedLines;
            printedLinesNumber++;
            lastSentence = newComment;
        }
    }
}
