using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;
using Actions;
public class Enemy : MonoBehaviour
{
    enum Obstacle
    {
        Player,
        Iron,
        Block,
        Bullet,
        Null
    }

    private float health;
    private float shield;
    private float speed;

    private float damage;
    private bool attack;
    private bool attackBlock;
    private float attackRate;
    private int irons;

    private Random rnd;
    private Block block;
    private Transform target;
    private Player player;
    private healthbar healthbarx;
    private healthbar sheildbar;
    private LearnUI learnUI;
    private Dictionary<Obstacle, Action> bestAction = new Dictionary<Obstacle, Action>();
    private Dictionary<Obstacle, Dictionary<Action, int>> actionsLearnt = new Dictionary<Obstacle, Dictionary<Action, int>>();
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
        speed = 0.02f;
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
        learnUI = transform.GetChild(2).gameObject.GetComponent<LearnUI>();
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
        Action action = Action.Null;
        Obstacle obstacle = Obstacle.Null;
        if (other.gameObject.GetComponent<Player>() != null)
        {
            obstacle = Obstacle.Player;
            action = costSetter(Obstacle.Player, other.gameObject);
        }
        else if (other.gameObject.GetComponent<Block>() != null)
        {
            obstacle = Obstacle.Block;
            action = costSetter(Obstacle.Block, other.gameObject);
        }
        else if (other.gameObject.GetComponent<Iron>() != null)
        {
            obstacle = Obstacle.Iron;
            action = costSetter(Obstacle.Iron, other.gameObject);
        }
        actionManager(obstacle, other.gameObject, action);

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<Bullet>() != null)
        {
            Action action = costSetter(Obstacle.Bullet, other.gameObject);
            actionManager(Obstacle.Bullet, other.gameObject, action);
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
            attackBlock = false;
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
        } else if(attackBlock){
            if (attackRate <= 0 && block !=null)
            {
                if(block.getHardness()<=damage){
                    block.damage(damage);
                    block=null;
                }else{
                    block.damage(damage);
                }
                
                attackRate = 1;
                print("block attack" );
                
            }
        }
    }
    private Action costSetter(Obstacle obstacle, GameObject gameObject)
    {
        if (bestAction.ContainsKey(obstacle))
        {
            print(obstacle + " : " + bestAction[obstacle]);
            return bestAction[obstacle];
        }
        else
        {
            return costAll(obstacle, gameObject);
        }
    }

    private Action costAll(Obstacle obstacle, GameObject gameObject)
    {
        if (!actionsLearnt.ContainsKey(obstacle))
        {
           
            Dictionary<Action, int> x = new Dictionary<Action, int>();
            actionsLearnt.Add(obstacle, x);
        }
        foreach (Action action in actions)
        {
            if (!actionsLearnt[obstacle].ContainsKey(action))
            {
                 learnUI.setText("Learning about" + obstacle + "Action :" +action );
                if (!Learn(obstacle, gameObject, action))
                    break;
                actionManager(obstacle, gameObject, action);
            }
        }
        Action best = Action.Dodge;
        int y = -10;
        int i = 0;
        foreach (Action action in actions)
        {
            if (actionsLearnt[obstacle].ContainsKey(action))
            {
                if (actionsLearnt[obstacle][action] > y)
                    best = action;
                y = actionsLearnt[obstacle][action];
                i++;
                print(obstacle + " " + action + " " + actionsLearnt[obstacle][action]);
            }
        }
        if (i == 3)
        {
            bestAction.Add(obstacle, best);
        }
        return best;
    }
    private bool Learn(Obstacle obstacle, GameObject gameObject, Action action)
    {
        if (obstacle == Obstacle.Block)
        {
            if (gameObject.GetComponent<Block>().learnable)
            {
                
                Dictionary<Action, int> x = actionsLearnt[obstacle];
                x.Add(action, gameObject.GetComponent<Block>().costCalculator(action));
            }
            else
            {
                return false;
            }

        }
        else if (obstacle == Obstacle.Player)
        {
            if (gameObject.GetComponent<Player>().learnable)
            {
                Dictionary<Action, int> x = actionsLearnt[obstacle];
                x.Add(action, gameObject.GetComponent<Player>().costCalculator(action));
            }
            else
            {
                return false;
            }
        }
        else if (obstacle == Obstacle.Iron)
        {
            if (gameObject.GetComponent<Iron>().learnable)
            {
                Dictionary<Action, int> x = actionsLearnt[obstacle];
                x.Add(action, gameObject.GetComponent<Iron>().costCalculator(action));
            }
            else
            {
                return false;
            }
        }
        else if (obstacle == Obstacle.Bullet)
        {
            if (gameObject.GetComponent<Bullet>().learnable)
            {
                Dictionary<Action, int> x = actionsLearnt[obstacle];
                x.Add(action, gameObject.GetComponent<Bullet>().costCalculator(action));
            }
            else
            {
                return false;
            }
        }
        return true;
    }
    private void actionManager(Obstacle obstacle, GameObject gameObject, Action action)
    {
        if (action == Action.Hit)
        {
            hitManager(obstacle, gameObject);
        }
        else if (action == Action.Dodge)
        {
            dodgeManager(obstacle, gameObject);
        }
        else if (action == Action.Recieve)
        {
            RecieveManager(obstacle, gameObject);
        }
    }
    private void hitManager(Obstacle obstacle, GameObject gameObject)
    {
        if (obstacle == Obstacle.Player)
        {
            attack = true;
        }
        else if (obstacle == Obstacle.Block)
        {
            attackBlock=true;
            block = gameObject.GetComponent<Block>();
            block.damage(damage);
        }
        else if (obstacle == Obstacle.Iron)
        {
            gameObject.GetComponent<Iron>().destroy();
        }
        else if (obstacle == Obstacle.Bullet)
        {
            //NOTHING IS DONE here
        }
    }
    private void dodgeManager(Obstacle obstacle, GameObject gameObject)
    {
        if (obstacle == Obstacle.Player)
        {
            //NOTHING IS DONE here
        }
        else if (obstacle == Obstacle.Block)
        {
            //NOTHING IS DONE here
        }
        else if (obstacle == Obstacle.Iron)
        {
            //NOTHING IS DONE here
        }
        else if (obstacle == Obstacle.Bullet)
        {
            dodge(gameObject.GetComponent<Bullet>().getDamage());
            gameObject.GetComponent<Bullet>().learnable = false;
            Destroy(gameObject);
        }
    }
    private void RecieveManager(Obstacle obstacle, GameObject gameObject)
    {
        if (obstacle == Obstacle.Player)
        {
            //NOTHING IS DONE here
        }
        else if (obstacle == Obstacle.Block)
        {
            //NOTHING IS DONE here
        }
        else if (obstacle == Obstacle.Iron)
        {
            irons++;
            gameObject.GetComponent<Iron>().destroy();
        }
        else if (obstacle == Obstacle.Bullet)
        {
            hurt(gameObject.GetComponent<Bullet>().getDamage() * 3);
            gameObject.GetComponent<Bullet>().destroy();
        }
    }
}
