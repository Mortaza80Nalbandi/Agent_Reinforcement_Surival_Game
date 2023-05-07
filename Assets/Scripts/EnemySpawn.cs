using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;
public class EnemySpawn : MonoBehaviour
{
    private float rate;
    private Random rnd;

    public GameObject enemyPrefab;
    // Start is called before the first frame update
    void Start()
    {
        rate = 100;
        rnd = new Random();
    }

    // Update is called once per frame
    void Update()
    {
        if (rate <= 0)
        {
            int x = rnd.Next(-24, 24);
            int y = rnd.Next(-24, 24);
            Vector3 spawnPos = new Vector3(x, y, 0);
            Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
            rate = 5;
        }
        rate -= Time.deltaTime;
    }
}
