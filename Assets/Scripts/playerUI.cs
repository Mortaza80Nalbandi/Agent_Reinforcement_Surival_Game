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
    Text HealthText;
    Text attackText;
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
        HealthText = transform.GetChild(7).gameObject.GetComponent<Text>();
        attackText = transform.GetChild(9).gameObject.GetComponent<Text>();
        printedLinesNumber = 0;
        printedLines = "LearningTracker : \n";
    }

    public void updateIron(int irs)
    {
        ironText = transform.GetChild(2).gameObject.GetComponent<Text>();
        ironText.text = "" + irs;
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
    public void updateHealth(float health, float maxhealth)
    {
        HealthText = transform.GetChild(7).gameObject.GetComponent<Text>();
        HealthText.text = "" + health + " / " + maxhealth;
    }
    public void updateAttack(float damage)
    {
        attackText = transform.GetChild(9).gameObject.GetComponent<Text>();
        attackText.text = ""+damage;
    }
    public void updateScore(int score)
    {
        scoreText = transform.GetChild(1).gameObject.GetComponent<Text>();
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
