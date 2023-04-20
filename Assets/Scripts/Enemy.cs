using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
     private float health;
    //private int damage;
    private float speed;
    private Transform target;
    private Player player;
    // Start is called before the first frame update
    void Start()
    {
        health = 100f;
        speed = 0.6f;
        player = GameObject.Find("Player").GetComponent<Player>();
         target = player.gameObject.transform;
        
    }

    // Update is called once per frame
    void Update()
    {
        target = player.gameObject.transform;
    }
    private void FixedUpdate() {
        GetComponent<Rigidbody2D>().MovePosition(Vector3.MoveTowards(transform.position,target.position,0.03f));
    }
    public void hurt(float damageRecieved){
        health-=damageRecieved;
        
    }
}
