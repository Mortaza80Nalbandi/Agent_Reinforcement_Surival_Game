using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;
public class IronOreSpawn : MonoBehaviour
{
    private float rate;
    private Random rnd;

    public GameObject IOPrefab;
    private int xLowerband;
    private int yLowerband;
    private int xHigherband;
    private int yHigherband;
    void Start()
    {
        rate = 10;
        xLowerband = -24;
        xHigherband = 24;
        yLowerband = -24;
        yHigherband = 24;
        rnd = new Random();
    }
    void Update()
    {
        if (rate <= 0)
        {
            int x = rnd.Next(xLowerband, xHigherband);
            int y = rnd.Next(yLowerband, yHigherband);
            Vector3 spawnPos = new Vector3(x, y, 0);
            Instantiate(IOPrefab, spawnPos, Quaternion.identity);
            rate = 10;
        }
        rate -= Time.deltaTime;
    }
}
