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
        PowerUp,
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
    private int deathScore;
    private int Threshhold=20;

    private Random rnd;
    private Block block;
    private PowerUp powerUp;
    private Transform target;
    private Player player;
    private healthbar healthbarx;
    private healthbar sheildbar;
    private EnemySpawn enemySpawn;
    private LearnUI learnUI;
    private Dictionary<Obstacle,List<Action>> bestAction = new Dictionary<Obstacle,List<Action>>();
    private Dictionary<Obstacle, Dictionary<List<Action>, int>> actionsLearnt = new Dictionary<Obstacle, Dictionary<List<Action>, int>>();
    private Action[] actions;
    void Start()
    {
        attributeSet();
        EntitySet();
        actions = new Action[2];
        actions[0] = Action.Hit;
        actions[1] = Action.Recieve;

    }

    private void attributeSet()
    {
        health = 10f;
        shield = 10f;
        speed = 0.02f;
        damage = 5;
        attackRate = 3;
        irons = 0;
        deathScore = 5;
    }
    private void EntitySet()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        enemySpawn = GameObject.Find("EnemySpawn").GetComponent<EnemySpawn>();
        target = player.gameObject.transform;
        healthbarx = transform.GetChild(0).gameObject.GetComponent<healthbar>();
        sheildbar = transform.GetChild(1).gameObject.GetComponent<healthbar>();
        learnUI = transform.GetChild(2).gameObject.GetComponent<LearnUI>();
        healthbarx.setHealth(health, 10);
        sheildbar.setHealth(shield, 10);
        rnd = new Random();
        powerUp = null;
    }
    // Update is called once per frame
    void Update()
    {
        target = player.gameObject.transform;
        attackCheck();
        attackRate -= Time.deltaTime;
        if (health <= 0)
        {
            player.updateScore(deathScore);
            enemySpawn.decreaseEnemies();
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
            sheildbar.setHealth(shield, 100);
            irons -= 5;
        }
        else if (irons > 5)
        {
            damage += 5;
            irons -= 5;
        }
    }
        public void hurt(float damageRecieved)
    {
        if (rnd.Next(0, 100) < 80)
        {
            if (shield <= 0)
            {
            health -= damageRecieved;
            healthbarx.setHealth(health, 10f);
            }
            else
            {
            shield -= damageRecieved;
            sheildbar.setHealth(shield, 10f);
            }
        }
        else
            learnUI.addText("Dodged attack");
        
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
        else if (attackBlock)
        {
            if (attackRate <= 0 && block != null)
            {
                if (block.getHardness() <= damage)
                {
                    block.damage(damage);
                    block = null;
                }
                else
                {
                    block.damage(damage);
                }

                attackRate = 1;
                learnUI.addText("block attack");
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        List<Action> actions;
        Obstacle obstacle = Obstacle.Null;
        if (other.gameObject.GetComponent<Player>() != null)
        {
            obstacle = Obstacle.Player;
            actions = costSetter(Obstacle.Player, other.gameObject);
        }
        else if (other.gameObject.GetComponent<Block>() != null)
        {
            obstacle = Obstacle.Block;
            actions = costSetter(Obstacle.Block, other.gameObject);
        }
        else if (other.gameObject.GetComponent<Iron>() != null)
        {
            obstacle = Obstacle.Iron;
            actions = costSetter(Obstacle.Iron, other.gameObject);
        }else if (other.gameObject.GetComponent<PowerUp>() != null)
        {
            obstacle = Obstacle.PowerUp;
            actions = costSetter(Obstacle.PowerUp, other.gameObject);
            
        }
        //actionManager(obstacle, other.gameObject, actions);

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
        }else if (other.gameObject.GetComponent<PowerUp>() != null)
        {
            powerUp = null;
        }
    }


    private List<Action> costSetter(Obstacle obstacle, GameObject gameObject)
    {
        if (bestAction.ContainsKey(obstacle))
        {
            learnUI.addText("bast Action about" + obstacle + "Action :" + bestAction[obstacle]);
            return bestAction[obstacle];
        }
        else
        {
            return costAll(obstacle, gameObject);
        }
    }

    private  List<Action>  costAll(Obstacle obstacle, GameObject gameObject)
    {
        Dictionary<List<Action>, int> x ;
        if (!actionsLearnt.ContainsKey(obstacle))
        {
            x = new Dictionary<List<Action>, int>();
            actionsLearnt.Add(obstacle, x);
        } else {
            x = actionsLearnt[obstacle];
        }
        List<Action> actionArray = new List<Action>();
        RecursiveLearning(actionArray,obstacle,0);
        List<Action> y = new List<Action>();
        return y;
    }
    private bool RecursiveLearning(List<Action> actionArray,Obstacle obstacle,int reward){
        foreach (Action action in actions)
        {
            actionArray.Add(action);
            int cost = 0;
            if (!actionsLearnt[obstacle].ContainsKey(actionArray))
            {
                cost = Learn(obstacle, gameObject, actionArray,reward);
                if (cost <=-10){
                    Dictionary<List<Action>, int> x = actionsLearnt[obstacle];
                    x.Add(actionArray,-10);
                    return false;
                }
                else if(cost >=10){
                    Dictionary<List<Action>, int> x = actionsLearnt[obstacle];
                    x.Add(actionArray,cost+reward);
                }
            }
            RecursiveLearning(actionArray,obstacle,cost+reward);
            actionArray.RemoveAt(actionArray.Count-1);
        }
        return true;
    }
    private int Learn(Obstacle obstacle, GameObject gameObject, List<Action> actions, int reward)
    {
        int cost = 0;
        if (obstacle == Obstacle.Block)
        {
            cost = gameObject.GetComponent<Block>().costCalculator(actions[0]);
            learnUI.addText("Learning about " + obstacle + " ,Action List :" + actions + "Result = " + (cost+reward));
            return cost;

        }
        else if (obstacle == Obstacle.Player)
        {
            cost = gameObject.GetComponent<Player>().costCalculator(actions[0]);
            learnUI.addText("Learning about " + obstacle + " ,Action List :" + actions + "Result = " + (cost+reward));
            return cost;
        }
        else if (obstacle == Obstacle.Iron)
        {
            cost = gameObject.GetComponent<Iron>().costCalculator(actions[0]);
            learnUI.addText("Learning about " + obstacle + " ,Action List :" + actions + "Result = " + (cost+reward));
            return cost;
        }else if (obstacle == Obstacle.PowerUp)
        {
             cost = gameObject.GetComponent<PowerUp>().costCalculator(actions[0]);
            learnUI.addText("Learning about " + obstacle + " ,Action List :" + actions + "Result = " + (cost+reward));
            return cost;
        }
        return cost;
    }
    private void actionManager(Obstacle obstacle, GameObject gameObject, Action action)
    {
        if (action == Action.Hit)
        {
            hitManager(obstacle, gameObject);
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
            attackBlock = true;
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
        }else if (obstacle == Obstacle.PowerUp)
        {
            powerUp = gameObject.GetComponent<PowerUp>(); 
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
        else if (obstacle == Obstacle.PowerUp)
        {
            //aaaaaaaaaaaaaaaaaaaaaaaaa
        }
    }
}
