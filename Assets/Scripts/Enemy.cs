using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;
public class Enemy : MonoBehaviour
{
    enum Obstacle
    {
        Player,
        Iron,
        Block
    }
    enum Action
    {
        Hit,
        Recieve,
        dodge
    }
    private float health;
    private float shield;
    private float speed;

    private float damage;
    private bool attack;
    private float attackRate;
    private int irons;

    private Random rnd;
    private Block block;
    private Transform target;
    private Player player;
    private healthbar healthbarx;
    private healthbar sheildbar;
    private Dictionary<Obstacle, string> bestAction = new Dictionary<Obstacle, string>();
    void Start()
    {
        attributeSet();
        EntitySet();

    }

    private void attributeSet()
    {
        health = 100f;
        shield = 100f;
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
        sheildbar = transform.GetChild(1).gameObject.GetComponent<healthbar>();
        healthbarx.setHealth(health, 100);
        sheildbar.setHealth(shield, 100);
        rnd = new Random();
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
        if (irons >= 5)
        {
            ironManage();
        }

    }
    private void FixedUpdate()
    {
        if (!attack)
            GetComponent<Rigidbody2D>().MovePosition(Vector3.MoveTowards(transform.position, target.position, speed));
    }
    private void ironManage()
    {
        if (irons > 5 && shield <= 50)
        {
            shield = 100f;
            irons -= 5;
        }
        else if (irons > 5)
        {
            damage += 5;
            irons -= 5;
        }
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
        }
        else if (other.gameObject.GetComponent<Iron>() != null)
        {
            irons++;
            Destroy(other.gameObject);
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
        if (rnd.Next(0, 100) < 80)
            if (shield <= 0)
            {
                health -= damageRecieved;
                healthbarx.setHealth(health, 100f);
            }
            else
            {
                shield -= damageRecieved;
                sheildbar.setHealth(shield, 100);
            }
        else
            Debug.Log("Dodged attack");


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
    private Action costSetter(Obstacles obstacle)
    {
        if (obstacle == Obstacles.Block)
        {
            if (bestAction.ContainsKey(obstacle))
            {
                return bestAction[Obstacles.Block];
            }
        }
        else if (obstacle == Obstacles.Player)
        {
            if (bestAction.ContainsKey(obstacle))
            {
                return bestAction[Obstacles.Player];
            }
        }
        else if (obstacle == Obstacles.Iron)
        {
            if (bestAction.ContainsKey(obstacle))
            {
                return bestAction[Obstacles.Iron];
            }
        }
    }
    private float costBlock(Action action)
    {
        return 0f;
    }
}
