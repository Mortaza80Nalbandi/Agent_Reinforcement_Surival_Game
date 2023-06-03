using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class healthbar : MonoBehaviour
{
    public Slider slider;
    public Color low;
    public Color high;
    public Vector3 Offset;

    void Update()
    {
        slider.transform.position = Camera.main.WorldToScreenPoint(transform.parent.position + Offset);
    }
    public void setHealth(float health, float maxHealth)
    {
        slider.gameObject.SetActive(health < maxHealth && health > 0);
        slider.value = health;
        slider.maxValue = maxHealth;
        slider.fillRect.GetComponentInChildren<Image>().color = high;
    }
}
