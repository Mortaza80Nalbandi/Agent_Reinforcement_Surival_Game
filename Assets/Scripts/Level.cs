using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Level : MonoBehaviour
{
    private EnemySpawn enemySpawn;
    int level;
    // Start is called before the first frame update
    void Start()
    {
        enemySpawn = GameObject.Find("EnemySpawn").GetComponent<EnemySpawn>();
        level = SceneManager.GetActiveScene().buildIndex;
        string objective="Objective : ";
        if(level == 1){
            objective = "Objective : Let the Enemy Learn complete About Blocks and then kill it";
        }else if(level ==2){
             objective = "Objective : Let the Enemy Learn complete About PowerUps shile it gets interrupted and then kill it";
        }else if( level ==3){
             objective = "Objective : You can Test difierent methods , when you are done , Kill the Enemy";
        }else if(level ==4){
             objective = "Objective : Get The Highest score Possible";
        }
        GameObject.Find("UI").transform.GetChild(5).gameObject.GetComponent<Text>().text = objective;
    }
    void Update()
    {
        if(level == 1||level == 2||level == 3){
            if(enemySpawn.getEnemies() == 0){
                if( GameObject.Find("Enemy").GetComponent<Enemy>().LevelValidator(level)){
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
                }else {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                }
            }   
        }         
    }
    // Update is called once per frame
    public int getSpawnInformation(){
        level = SceneManager.GetActiveScene().buildIndex;
        if(level == 1){
            return 0 ;
        }else if(level ==2){
            return 0 ;
        }else if( level ==3){
            return 0 ;
        }else if(level ==4){
            return 5 ;
        }
        return 1;
    }
}
