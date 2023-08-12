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

    public float health;
    private float shield;
    private float speed;

    public float damage;
    private bool attack;
    private bool attackBlock;
    private float attackRate;
    private int irons;
    private int deathScore;
    private int Threshhold=10;

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
        List<Action> actionss = null;
        Obstacle obstacle = Obstacle.Null;
        if (other.gameObject.GetComponent<Player>() != null)
        {
            obstacle = Obstacle.Player;
            actionss = costSetter(Obstacle.Player, other.gameObject);
        }
        else if (other.gameObject.GetComponent<Block>() != null)
        {
            obstacle = Obstacle.Block;
            actionss = costSetter(Obstacle.Block, other.gameObject);
        }
        else if (other.gameObject.GetComponent<Iron>() != null)
        {
            obstacle = Obstacle.Iron;
            actionss = costSetter(Obstacle.Iron, other.gameObject);
        }else if (other.gameObject.GetComponent<PowerUp>() != null)
        {
            obstacle = Obstacle.PowerUp;
            actionss = costSetter(Obstacle.PowerUp, other.gameObject);
            
        }
        actionManagerArray(obstacle,other.gameObject,actionss);

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


    private List<Action> costSetter(Obstacle obstacle, GameObject go)
    {
        foreach (var action in bestAction) 
            {
                print(action.Key);
            }
        if (bestAction.ContainsKey(obstacle))
        {
            string y = "";
            foreach (Action action in bestAction[obstacle]) 
            {
                y+=action;
                y+="->";
            }
            learnUI.addText("bast Action about" + obstacle + "Action List :" + y);
            return bestAction[obstacle];
        }
        else
        {
            return costAll(obstacle, go);
        }
    }

    private  List<Action>  costAll(Obstacle obstacle, GameObject go)
    {
        if (!actionsLearnt.ContainsKey(obstacle))
        {
            Dictionary<List<Action>, int> x = new Dictionary<List<Action>, int>();
            actionsLearnt.Add(obstacle, x);
        }
         
        List<Action> actionArray = new List<Action>();
        if(RecursiveLearning(actionArray,go,obstacle,0)){
            List<Action> Best = new List<Action>();
            int bestReward=0;
            foreach(var a in actionsLearnt[obstacle]) {
                if(a.Value > bestReward)
                    Best = a.Key;
                    bestReward = a.Value;
            }
            bestAction.Add(obstacle,Best);
            actionManagerArray(obstacle,go,Best);
            print("ddddddd");
        }
            
       string f = "1\n";
        foreach (var action in actionsLearnt[obstacle]) 
            {
                List<Action> z = action.Key;
                foreach(Action a in z){
                    f+=a;
                    f+="->";
                }
                f=f+"r: "+action.Value+ "      ";  
            }
            print(f);
        List<Action> y = new List<Action>();
        return y;
    }
    private bool RecursiveLearning(List<Action> actionArray,GameObject go,Obstacle obstacle,int reward){
        if(actionArray.Count==3){
            return true;
        }
        foreach (Action action in actions)
        {
            if(checkEquals(actionArray,obstacle))
                if(checkReward(actionArray,obstacle))
                    return true;
            actionArray.Add(action);
            int cost = 0;
            if (!checkEquals(actionArray,obstacle))
            {
                cost = Learn(obstacle, go, actionArray,reward);
                actionManager(obstacle,go,actionArray[actionArray.Count-1]);
                if (cost <=-1*Threshhold){
                    actionsLearnt[obstacle].Add(actionArray,-10);
                    return false;
                }
                else if(cost >=Threshhold){
                    actionsLearnt[obstacle].Add(actionArray,cost+reward);
                    return false;
                }
                if(!RecursiveLearning(actionArray,go,obstacle,cost+reward))
                return false;
            unactionManager(obstacle,go,actionArray[actionArray.Count-1]);
            }
            actionArray.RemoveAt(actionArray.Count-1);
        }
        return true;
    }
    private bool checkEquals(List<Action> actionArray,Obstacle obstacle){
        foreach (var action in actionsLearnt[obstacle]){
            List<Action> z = action.Key;
            int i=0;
            if(z.Count == actionArray.Count)
                foreach(Action a in z){
                    if(actionArray[i] != a )
                        break;
                    i+=1;
                }
                if(i== actionArray.Count&& i!=0)
                    return true;       
        } 
        return false;
    }
    private bool checkReward(List<Action> actionArray,Obstacle obstacle){
        foreach (var action in actionsLearnt[obstacle]){
            List<Action> z = action.Key;
            int i=0;
            if(z.Count == actionArray.Count)
                foreach(Action a in z){
                    if(actionArray[i] != a )
                        break;
                    i+=1;
                }
                if(i== actionArray.Count&& i!=0)
                    if(action.Value<=-1*Threshhold || action.Value>=Threshhold)   
                        return true;    
        } 
        return false;
    }
    private int Learn(Obstacle obstacle, GameObject go, List<Action> actions, int reward)
    {
        int cost = 0;
        if (obstacle == Obstacle.Block)
        {
            cost = go.GetComponent<Block>().costCalculator(actions[0]);
            learnUI.addText("Learning about " + obstacle + " ,Action List :" + actions + "Result = " + (cost+reward));
            return cost;

        }
        else if (obstacle == Obstacle.Player)
        {
            cost = go.GetComponent<Player>().costCalculator(actions[0]);
            learnUI.addText("Learning about " + obstacle + " ,Action List :" + actions + "Result = " + (cost+reward));
            return cost;
        }
        else if (obstacle == Obstacle.Iron)
        {
            cost = go.GetComponent<Iron>().costCalculator(actions[0]);
            learnUI.addText("Learning about " + obstacle + " ,Action List :" + actions + "Result = " + (cost+reward));
            return cost;
        }else if (obstacle == Obstacle.PowerUp)
        {
            string y = "";
            foreach (Action action in actions) 
            {
                y+=action;
                y+="->";
            }
            cost = go.GetComponent<PowerUp>().costCalculator(actions[actions.Count-1]);
            learnUI.addText("Learning about " + obstacle + " ,Action List :" + y + "Result = " + (cost+reward));
            return cost;
        }
        return cost;
    }
    private void actionManagerArray(Obstacle obstacle, GameObject go, List<Action> actionss){
        foreach(Action action in actionss){
            actionManager(obstacle,go,action);
        }

    }
    private void actionManager(Obstacle obstacle, GameObject go, Action action)
    {
        if (action == Action.Hit)
        {
            hitManager(obstacle, go);
        }
        else if (action == Action.Recieve)
        {
            RecieveManager(obstacle, go);
        }
    }
    private void hitManager(Obstacle obstacle, GameObject go)
    {
        if (obstacle == Obstacle.Player)
        {
            attack = true;
        }
        else if (obstacle == Obstacle.Block)
        {
            attackBlock = true;
            block = go.GetComponent<Block>();
            block.damage(damage);
        }
        else if (obstacle == Obstacle.Iron)
        {
            go.GetComponent<Iron>().destroy();
        }
        else if (obstacle == Obstacle.Bullet)
        {
            //NOTHING IS DONE here
        }else if (obstacle == Obstacle.PowerUp)
        {
            powerUp = go.GetComponent<PowerUp>(); 
            powerUp.hurt();
        }
    }
    private void RecieveManager(Obstacle obstacle, GameObject go)
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
            go.GetComponent<Iron>().destroy();
        }
        else if (obstacle == Obstacle.PowerUp)
        {
            //aaaaaaaaaaaaaaaaaaaaaaaaa
            int q = go.GetComponent<PowerUp>().RewardMulti();
            health= health*q;
            damage = damage*q;
            Destroy(go);
        }
    }
    private void unactionManager(Obstacle obstacle, GameObject go, Action action)
    {
        if (action == Action.Hit)
        {
            if (obstacle == Obstacle.PowerUp)
        {
            powerUp = go.GetComponent<PowerUp>(); 
            powerUp.unhurt();
        }
        }
        else if (action == Action.Recieve)
        {
            //sssssssssssssssssss
        }
    }
}
