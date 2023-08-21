using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;
public class EnemySpawn : MonoBehaviour
{
    private float rate;
    private Random rnd;
    private int enemies;
    private int killedEnemies;
    private int maxEnemies;
    private int xLowerband;
    private int yLowerband;
    private int xHigherband;
    private int yHigherband;

    public GameObject enemyPrefab;
    // Start is called before the first frame update
    void Start()
    {
        rate =5 ;
        enemies = 1;
        xLowerband = -24;
        xHigherband = 24;
        yLowerband = -24;
        yHigherband = 24;
        maxEnemies = -5;
        killedEnemies = 0;
;        rnd = new Random();
    }
    public void decreaseEnemies()
    {
        enemies--;
        killedEnemies++;
        if(killedEnemies>=5){
            killedEnemies = 0;
            maxEnemies++;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (rate <= 0 && enemies <= maxEnemies)
        {
            GameObject clone;
            int x = rnd.Next(xLowerband, xHigherband);
            int y = rnd.Next(yLowerband, yHigherband);
            Vector3 spawnPos = new Vector3(x, y, 0);
            clone=  Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
            clone.GetComponent<Enemy>().setID(enemies);
            rate = 2;
            enemies++;
        }
        rate -= Time.deltaTime;
    }
}
