using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;
public class EnemySpawn : MonoBehaviour
{
    private float rate;
    private Random rnd;
    private int enemies;

    public GameObject enemyPrefab;
    // Start is called before the first frame update
    void Start()
    {
        rate = 10;
        enemies = 1;
        rnd = new Random();
    }
    public void decreaseEnemies()
    {
        enemies--;
    }
    // Update is called once per frame
    void Update()
    {
        if (rate <= 0 && enemies <= 3)
        {
            int x = rnd.Next(-24, 24);
            int y = rnd.Next(-24, 24);
            Vector3 spawnPos = new Vector3(x, y, 0);
            Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
            rate = 2;
            enemies++;
        }
        rate -= Time.deltaTime;
    }
}
