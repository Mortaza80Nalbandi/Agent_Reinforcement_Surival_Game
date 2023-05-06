using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;
using Actions;
public class Enemy : MonoBehaviour
{
    enum Obstacle
    {
        Player,
        Iron,
        Block,
        Bullet
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
    private Dictionary<Obstacle, Action> bestAction = new Dictionary<Obstacle, Action>();
    private Dictionary<Obstacle, Dictionary<Action,int>> actionsLearnt = new Dictionary<Obstacle,  Dictionary<Action,int>>();
    private Action[] actions;
    void Start()
    {
        attributeSet();
        EntitySet();
        actions = new Action[3];
        actions[0] = Action.Hit;
        actions[1] = Action.Dodge;
        actions[2] = Action.Recieve;

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
            //Action action = costSetter(Obstacle.Player);
            //if (action != null)
            //  callFunc(action);
            attack = true;
        }
        else if (other.gameObject.GetComponent<Block>() != null)
        {
            //Action action = costSetter(Obstacle.Block);
            //if (action != null)
            //  callFunc(action);
            block = other.gameObject.GetComponent<Block>();
        }
        else if (other.gameObject.GetComponent<Iron>() != null)
        {
            //Action action = costSetter(Obstacle.Iron);
            //if (action != null)
            //  callFunc(action);
            irons++;
            Destroy(other.gameObject);
        }
        else if (other.gameObject.GetComponent<Bullet>() != null)
        {
            //Action action = costSetter(Obstacle.Bullet);
            //if (action != null)
            //  callFunc(action);
            bulletHit(other.gameObject.GetComponent<Bullet>().getDamage());
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
    private void dodge(float damageRecieved)
    {
        if (rnd.Next(0, 100) < 80)
        {
            hurt(damageRecieved);
        }
        else
            Debug.Log("Dodged attack");
    }
    public void hurt(float damageRecieved)
    {

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
    private Action costSetter(Obstacle obstacle)
    {
        if (bestAction.ContainsKey(obstacle))
        {
            return bestAction[obstacle];
        }
        else
        {
            bestAction.Add(obstacle, costAll(obstacle));
        }
        return null;
    }
    private Action costAll(Obstacle obstacle)
    {
        float reward = 0f;
        
        return null;
    }
}
