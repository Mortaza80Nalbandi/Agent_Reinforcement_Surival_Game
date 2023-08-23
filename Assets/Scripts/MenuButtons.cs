using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuButtons : MonoBehaviour
{
    // Start is called before the first frame update
    public void PlayPress(){  
         SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }  
    public void ExitPress(){  
         Application.Quit();
    }  
}
