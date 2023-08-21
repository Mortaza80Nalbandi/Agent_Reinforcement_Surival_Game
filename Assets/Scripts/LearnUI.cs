using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LearnUI : MonoBehaviour
{
    public Text text;
    public Vector3 Offset;

    void Update()
    {
        text.transform.position = Camera.main.WorldToScreenPoint(transform.parent.position + Offset);
    }
    public void addText(int id)
    {
        text.text = "Enemy "+id;
    }
}
