using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class playerUI : MonoBehaviour
{
    public Text text;
    public Vector3 Offset;
    private string weapon;
    private int Irons;
    private int score;

    // Start is called before the first frame update
    void Start()
    {
        weapon = "Bow";
    }

    void Update()
    {
        text.transform.position = Camera.main.WorldToScreenPoint(transform.parent.position + Offset);
    }
    public void updateIron(int irs)
    {
        Irons = irs;
        updateText();
    }
    public void updateWeapon(string wpn)
    {
        weapon = wpn;
        updateText();
    }
    public void updateScore(int score)
    {
        this.score = score;
        updateText();
    }
    private void updateText()
    {
        text.text = "Weapon is " + weapon + "\n Irons = " + Irons + "\nScore : " + score;
    }
}
