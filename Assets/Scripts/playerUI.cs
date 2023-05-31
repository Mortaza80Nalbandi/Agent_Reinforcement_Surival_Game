using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class playerUI : MonoBehaviour
{
    private int Irons;
    Text ironText;
    Text scoreText;
    private int score;
    Image m_Image;
    public Sprite Bow_Sprite;
    public Sprite Block_Sprite;
    public Sprite Sword_Sprite;

    // Start is called before the first frame update
    void Start()
    {
        m_Image = transform.GetChild(0).gameObject.GetComponent<Image>();
        scoreText = transform.GetChild(1).gameObject.GetComponent<Text>();
        ironText = transform.GetChild(2).gameObject.GetComponent<Text>();
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
}
