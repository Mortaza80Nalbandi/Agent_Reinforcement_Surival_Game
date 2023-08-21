using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;
public class ObjectSpawner : MonoBehaviour
{
    private float rate;
    private Random rnd;

    public GameObject PUPrefab;
    public GameObject IronPrefab;
    private int xLowerband;
    private int yLowerband;
    private int xHigherband;
    private int yHigherband;
    private int irons;
    private int powerUps;
    void Start()
    {
        powerUps = 0;
        irons = 0;
        rate = 6;
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
            if (powerUps <= 3)
            {
                int x = rnd.Next(xLowerband, xHigherband);
                int y = rnd.Next(yLowerband, yHigherband);
                Vector3 spawnPos = new Vector3(x, y, 0);
                Instantiate(PUPrefab, spawnPos, Quaternion.identity);
                powerUps++;
            }
            if (irons <= 8)
            {
                int x = rnd.Next(xLowerband, xHigherband);
                int y = rnd.Next(yLowerband, yHigherband);
                Vector3 spawnPos = new Vector3(x, y, 0);
                Instantiate(IronPrefab, spawnPos, Quaternion.identity);
                irons++;
            }
            rate = 6;
        }
        rate -= Time.deltaTime;
    }
    public void decreaseIrons()
    {
        irons--;
    }
    public void decreasePowerUps()
    {
        powerUps--;
    }
}
