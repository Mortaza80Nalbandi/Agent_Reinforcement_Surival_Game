using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MenuButtons : MonoBehaviour
{
    // Start is called before the first frame update
    void Start(){
     if( GameObject.Find("HighestScore") != null)
          GameObject.Find("Canvas").transform.GetChild(2).gameObject.GetComponent<Text>().text = "Highest Score : " + GameObject.Find("HighestScore").gameObject.GetComponent<HighestScore>().gethighestScore();
    }
    public void PlayPress(){  
         SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }  
    public void ExitPress(){  
         Application.Quit();
    }  
}
