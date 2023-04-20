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
    private float damage;
    private bool attack;
    private float attackRate=3;
    // Start is called before the first frame update
    void Start()
    {
        health = 100f;
        speed = 0.06f;
        damage=5;
        player = GameObject.Find("Player").GetComponent<Player>();
         target = player.gameObject.transform;
        
    }

    // Update is called once per frame
    void Update()
    {
        target = player.gameObject.transform;
        attackCheck();
        attackRate-=Time.deltaTime;
        
    }
    private void FixedUpdate() {
        if(!attack)
            GetComponent<Rigidbody2D>().MovePosition(Vector3.MoveTowards(transform.position,target.position,speed));
    }
    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.GetComponent<Player>()!=null ){
            attack = true;
        }
    }
    private void OnCollisionExit2D(Collision2D other) {
        if(other.gameObject.GetComponent<Player>()!=null){
            attack = false;
        }
    }
    public void hurt(float damageRecieved){
        health-=damageRecieved;
        
    }
    private void attackCheck(){
        if(attack){
            if(attackRate<=0){
                player.hurt(damage);
                attackRate = 3;
            }
        }
    }
}