using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private float health;
    private float shield;
    private float speed;
    private Block block;
    private Transform target;
    private Player player;
    private float damage;
    private bool attack;
    private float attackRate;
    private int irons;
    healthbar healthbarx;
    void Start()
    {
        attributeSet();
        EntitySet();

    }

    private void attributeSet()
    {
        health = 100f;
        shield=100f;
        speed = 0.06f;
        damage = 5;
        attackRate = 3;
        irons = 0;
    }
    private void EntitySet()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        target = player.gameObject.transform;
        healthbarx = transform.GetChild(0).gameObject.GetComponent<healthbar>();
        healthbarx.setHealth(health, 100);
    }
    // Update is called once per frame
    void Update()
    {
        target = player.gameObject.transform;
        attackCheck();
        attackRate -= Time.deltaTime;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
        if (block != null)
        {
            block.damage(damage);
        }
        if(irons>=5){
            ironManage();
        }

    }
    private void FixedUpdate()
    {
        if (!attack)
            GetComponent<Rigidbody2D>().MovePosition(Vector3.MoveTowards(transform.position, target.position, speed));
    }
    private void ironManage(){
        if
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.GetComponent<Player>() != null)
        {
            attack = true;
        }
        else if (other.gameObject.GetComponent<Block>() != null)
        {
            block = other.gameObject.GetComponent<Block>();
        }else if (other.gameObject.GetComponent<Iron>() != null)
        {
            irons++;
        }
    }
    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.GetComponent<Player>() != null)
        {
            attack = false;
        }
        else if (other.gameObject.GetComponent<Block>() != null)
        {
            block = null;
        }
    }
    public void hurt(float damageRecieved)
    {
        health -= damageRecieved;
        healthbarx.setHealth(health, 100f);

    }
    public void bulletHit(float damage1)
    {
        hurt(damage1);
    }
    private void attackCheck()
    {
        if (attack)
        {
            if (attackRate <= 0)
            {
                player.hurt(damage);
                attackRate = 3;
            }
        }
    }
}
