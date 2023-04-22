using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;
public class IronOreSpawn : MonoBehaviour
{
    private float rate;
    private Random rnd;

    public GameObject IOPrefab;
    // Start is called before the first frame update
    void Start()
    {
        rate = 10;
        rnd = new Random();
    }

    // Update is called once per frame
    void Update()
    {
        if(rate<=0){
            int x =rnd.Next(-24, 24);
            int y =rnd.Next(-24, 24);
             Vector3 spawnPos = new Vector3(x,y,0);
            Instantiate(IOPrefab, spawnPos, Quaternion.identity);
            rate =10;
        }
        rate -= Time.deltaTime;
    }
}
