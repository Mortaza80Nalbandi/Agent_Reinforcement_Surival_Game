using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class imgChanger : MonoBehaviour
{
    Image m_Image;
    public Sprite Bow_Sprite;
    public Sprite Block_Sprite;
    public Sprite Sword_Sprite;
    public void updateImage(string wpn){
        m_Image = GetComponent<Image>();
        if(wpn =="Bow"){
            m_Image.sprite = Bow_Sprite;
        }else if(wpn == "Meele"){
            m_Image.sprite = Sword_Sprite;
        }else if(wpn == "Block"){
            m_Image.sprite = Block_Sprite;
        }
    }
    
}
