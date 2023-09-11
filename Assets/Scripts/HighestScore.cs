using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighestScore : MonoBehaviour
{
    public int highestScore;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
    public void sethighestScore(int score){
        highestScore = score;
    }
    public int gethighestScore(){
        return highestScore ;
    }
    // Update is called once per frame

}
