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
        Block,
        Bullet
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
    private Dictionary<Obstacle, Action> bestAction = new Dictionary<Obstacle, Action>();
    private Dictionary<Obstacle, Dictionary<Action,int>> actionsLearnt = new Dictionary<Obstacle,  Dictionary<Action,int>>();
    private Action[] actions;
    void Start()
    {
        attributeSet();
        EntitySet();
        actions = new Action[3];
        actions[0] = Action.Hit;
        actions[1] = Action.dodge;
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
    private Action costSetter(Obstacles obstacle)
    {
        if (bestAction.ContainsKey(obstacle))
        {
            return bestAction[obstacle];
        }
        else
        {
            bestAction.Add(obstacle, costAll(obstacle));
        }
    }
    private float costAll(Obstacle obstacle)
    {
        float reward = 0f;
        if (obstacle == Obstacle.Block)
        {
            reward = costBlockAll();
        }
        else if (obstacle == Obstacle.Iron)
        {
            reward = costIronAll();
        }
        else if (obstacle == Obstacle.Player)
        {
            reward = costPlayerAll();
        }
        return reward;
    }
    private float costBlockAll()
    {
        int reward = 0f;
        foreach (Action action in actions)
        {
            if (actionsLearnt.TryGetValue(Obstacle.Block, out result))
            {
                if (!result.Contains(action))
                {
                    int x = costBlock(action);
                    if (x > reward)
                        reward = x;
                    result.Add(action)
                }
            }
        }

        return reward;
    }
    private float costBlock(Action action)
    {
        float reward = 0f;
        if (action == Action.Hit)
        {
            reward = 5f;
        }
        else if (action == Action.Recieve)
        {
            reward = -5f;
        }
        else if (action == Action.dodge)
        {
            reward = 0f;
        } // jump 10f
        return reward;
    }

    private float costIronAll()
    {
        int reward = 0f;
        foreach (Action action in actions)
        {
            int x = costIron(action);
            if (x > reward)
                reward = x;
        }

        return reward;
    }
    private float costIron(Action action)
    {
        float reward = 0f;
        if (action == Action.Hit)
        {
            reward = -5f;
        }
        else if (action == Action.Recieve)
        {
            reward = 10f;
        }
        else if (action == Action.dodge)
        {
            reward = 0f;
        } // jump 5f
        return reward;
    }

    private float costPlayerAll()
    {
        int reward = 0f;
        foreach (Action action in actions)
        {
            int x = costPlayer(action);
            if (x > reward)
                reward = x;
        }

        return reward;
    }
    private float costPlayer(Action action)
    {
        float reward = 0f;
        if (action == Action.Hit)
        {
            reward = 10f;
        }
        else if (action == Action.Recieve)
        {
            reward = -5f;
        }
        else if (action == Action.dodge)
        {
            reward = 0f;
        } // jump -5f
        return reward;
    }
    private int HitCost(Obstacle obstacle)
    {
        return 0;
    }
    private
}
