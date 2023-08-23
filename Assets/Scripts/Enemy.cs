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
        PowerUp,
        Null
    }

    public float health;
    public float maxHealth;
    private float shield;
    public float speed;

    public float damage;
    private bool attack;
    private bool attackBlock;
    private float attackRate;
    private float blockSlowdownRate;
    private int irons;
    private int deathScore;
    private int Threshhold = 10;
    private int ID = 0;
    private playerUI pui;
    private Random rnd;
    private Block block;
    private PowerUp powerUp;
    private Transform target;
    private Player player;
    private healthbar healthbarx;
    private healthbar sheildbar;
    private EnemySpawn enemySpawn;
    private Dictionary<Obstacle, List<Action>> bestAction = new Dictionary<Obstacle, List<Action>>();
    private Dictionary<Obstacle, Dictionary<List<Action>, float>> actionsLearnt = new Dictionary<Obstacle, Dictionary<List<Action>, float>>();
    private Action[] actions;
    void Start()
    {
        attributeSet();
        EntitySet();
        actions = new Action[3];
        actions[0] = Action.Hit;
        actions[1] = Action.Recieve;
        actions[2] = Action.Stun;
        transform.GetChild(2).gameObject.GetComponent<LearnUI>().UpdateID(ID);
    }
    public void setID(int id)
    {
        ID = id;
        transform.GetChild(2).gameObject.GetComponent<LearnUI>().UpdateID(ID);
    }
    private void attributeSet()
    {
        health = 10f;
        maxHealth = 10f;
        shield = 10f;
        speed = 0.02f;
        damage = 1;
        attackRate = 3;
        blockSlowdownRate = -2;
        irons = 0;
        deathScore = 5;
    }
    private void EntitySet()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        enemySpawn = GameObject.Find("EnemySpawn").GetComponent<EnemySpawn>();
        pui = GameObject.Find("UI").gameObject.GetComponent<playerUI>();
        target = player.gameObject.transform;
        healthbarx = transform.GetChild(0).gameObject.GetComponent<healthbar>();
        sheildbar = transform.GetChild(1).gameObject.GetComponent<healthbar>();
        healthbarx.setHealth(health, maxHealth);
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
        blockSlowdownRate -= Time.deltaTime;
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
        if (blockSlowdownRate <= 0)
        {
            speed = 0.02f;
        }

    }
    private void FixedUpdate()
    {
        if (!attack)
            GetComponent<Rigidbody2D>().MovePosition(Vector3.MoveTowards(transform.position, target.position, speed));
    }
    private void ironManage()
    {
        if (irons > 5 && shield <= 5)
        {
            shield = 100f;
            sheildbar.setHealth(shield, 10);
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
                healthbarx.setHealth(health, maxHealth);
            }
            else
            {
                shield -= damageRecieved;
                sheildbar.setHealth(shield, 10f);
            }
        }

    }
    private void attackCheck()
    {
        if (attack)
        {
            if (attackRate <= 0)
            {
                player.hit(damage);
                attackRate = 3;
            }
        }
        else if (attackBlock)
        {
            if (attackRate <= 0 && block != null)
            {
                if (block.getHardness() <= damage)
                {
                    block.hit(damage);
                    block = null;
                }
                else
                {
                    block.hit(damage);
                }

                attackRate = 1;
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
        }
        else if (other.gameObject.GetComponent<PowerUp>() != null)
        {
            obstacle = Obstacle.PowerUp;
            actionss = costSetter(Obstacle.PowerUp, other.gameObject);

        }
        if (other.gameObject.GetComponent<Enemy>() == null)
        {
            actionManagerArray(obstacle, other.gameObject, actionss);

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
        else if (other.gameObject.GetComponent<PowerUp>() != null)
        {
            powerUp = null;
        }
    }


    private List<Action> costSetter(Obstacle obstacle, GameObject go)
    {

        if (bestAction.ContainsKey(obstacle))
        {
            string y = "";
            foreach (Action action in bestAction[obstacle])
            {
                y += action;
                y += "->";
            }
            pui.addText("Enemy " + ID + " => bast Action about" + obstacle + "Action List :" + y);
            return bestAction[obstacle];
        }
        else
        {
            return costAll(obstacle, go);
        }
    }

    private List<Action> costAll(Obstacle obstacle, GameObject go)
    {
        if (!actionsLearnt.ContainsKey(obstacle))
        {
            Dictionary<List<Action>, float> x = new Dictionary<List<Action>, float>();
            actionsLearnt.Add(obstacle, x);
        }

        List<Action> actionArray = new List<Action>();
        if (RecursiveLearning(actionArray, go, obstacle, 0))
        {
            List<Action> Best = new List<Action>();
            float bestReward = 0;
            foreach (var a in actionsLearnt[obstacle])
            {
                if (a.Value > bestReward)
                {
                    Best = a.Key;
                    bestReward = a.Value;
                }
            }
            string y = "";
            foreach (Action action in Best)
            {
                y += action;
                y += "->";
            }
            pui.addText("Enemy " + ID + " =>Learn Complete bast Action about" + obstacle + "Action List :" + y);
            bestAction.Add(obstacle, Best);
            actionManagerArray(obstacle, go, Best);
            if(obstacle == Obstacle.Player)
                damage*=3;
        }

        string f = "1\n";
        foreach (var action in actionsLearnt[obstacle])
        {
            List<Action> z = action.Key;
            foreach (Action a in z)
            {
                f += a;
                f += "->";
            }
            f = f + "r: " + action.Value + "      ";
        }
        print(f);
        List<Action> zzz = new List<Action>();
        return zzz;
    }
    private bool RecursiveLearning(List<Action> actionArray, GameObject go, Obstacle obstacle, float reward)
    {

        foreach (Action action in actions)
        {

            actionArray.Add(action);
            float cost = Learn(obstacle, go, actionArray, reward);
            string f = "22 \n";
            foreach (Action a in actionArray)
            {
                f += a;
                f += "->";
            }
            f = f + "r: " + (cost + reward) + "      ";
            print(f);

            if (!checkEquals(actionArray, obstacle))
            {
                if (!checkReward(actionArray, obstacle, cost + reward))
                {
                    actionManager(obstacle, go, actionArray[actionArray.Count - 1]);
                    if (cost <= -1 * Threshhold)
                    {
                        actionsLearnt[obstacle].Add(actionArray, -10);
                        printUIofLearning(actionArray, obstacle, cost + reward);
                        return false;
                    }
                    else if (cost >= Threshhold)
                    {
                        actionsLearnt[obstacle].Add(actionArray, cost + reward);
                        printUIofLearning(actionArray, obstacle, cost + reward);
                        return false;
                    }
                    else if (actionArray.Count == 3)
                    {
                        List<Action> actionArrayTemp = new List<Action>();
                        foreach (Action temp in actionArray)
                        {
                            actionArrayTemp.Add(temp);
                        }
                        printUIofLearning(actionArray, obstacle, cost + reward);
                        actionsLearnt[obstacle].Add(actionArrayTemp, cost + reward);
                    }
                    if (actionArray.Count <= 2)
                        if (!RecursiveLearning(actionArray, go, obstacle, cost + reward))
                            return false;
                    unactionManager(obstacle, go);

                }
            }

            actionArray.RemoveAt(actionArray.Count - 1);
        }
        return true;
    }
    private bool checkEquals(List<Action> actionArray, Obstacle obstacle)
    {
        foreach (var action in actionsLearnt[obstacle])
        {
            List<Action> z = action.Key;
            int i = 0;
            if (z.Count == actionArray.Count)
                foreach (Action a in z)
                {
                    if (actionArray[i] != a)
                        break;
                    i += 1;
                }

            if (i == actionArray.Count && i != 0)
                return true;
        }
        return false;
    }
    private void printUIofLearning(List<Action> actionss, Obstacle obstacle, float cost)
    {
        string y = "";
        foreach (Action action in actionss)
        {
            y += action;
            y += "->";
        }
        pui.addText("Enemy " + ID + " => Learning about " + obstacle + " ,Action List :" + y + "Result = " + cost);
    }
    private bool checkReward(List<Action> actionArray, Obstacle obstacle, float x)
    {
        foreach (var action in actionsLearnt[obstacle])
        {
            List<Action> z = action.Key;
            int i = 0;
            if (z.Count == actionArray.Count)
                foreach (Action a in z)
                {
                    if (actionArray[i] != a)
                        break;
                    i += 1;
                }
            if (i == actionArray.Count && i != 0)
                if (action.Value == x)
                    return true;
        }
        return false;
    }
    private float Learn(Obstacle obstacle, GameObject go, List<Action> actions, float reward)
    {
        float cost = 0;
        if (obstacle == Obstacle.Block)
        {

            cost = go.GetComponent<Block>().costCalculator(actions[actions.Count - 1]);
            return cost;

        }
        else if (obstacle == Obstacle.Player)
        {
            cost = go.GetComponent<Player>().costCalculator(actions[actions.Count - 1]);
            return cost;
        }
        else if (obstacle == Obstacle.Iron)
        {
            cost = go.GetComponent<Iron>().costCalculator(actions[actions.Count - 1]);
            return cost;
        }
        else if (obstacle == Obstacle.PowerUp)
        {
            cost = go.GetComponent<PowerUp>().costCalculator(actions[actions.Count - 1]);
            return cost;
        }
        return cost;
    }
    private void actionManagerArray(Obstacle obstacle, GameObject go, List<Action> actionss)
    {

        Action temp = Action.Null;
        if (actionss.Count >= 1)
            temp = actionss[0];
        bool flag = true;
        foreach (Action action in actionss)
        {
            if (action != temp)
            {
                flag = false;
                break;
            }

        }
        if (flag)
        {
            actionManager(obstacle, go, temp);
        }
        foreach (Action action in actionss)
        {
            actionManager(obstacle, go, action);
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
        else if (action == Action.Stun)
        {
            stunManager(obstacle, go);
        }
    }
    private void hitManager(Obstacle obstacle, GameObject go)
    {
        if (obstacle == Obstacle.Player)
        {
            attack = true;
            player.hit(damage);
        }
        else if (obstacle == Obstacle.Block)
        {
            attackBlock = true;
            block = go.GetComponent<Block>();
            block.hit(5);
        }
        else if (obstacle == Obstacle.Iron)
        {
            go.GetComponent<Iron>().hit();
        }
        else if (obstacle == Obstacle.PowerUp)
        {
            powerUp = go.GetComponent<PowerUp>();
            powerUp.hit();
        }
    }
    private void RecieveManager(Obstacle obstacle, GameObject go)
    {
        if (obstacle == Obstacle.Player)
        {
            int x = player.Recieve();
            irons += x;
        }
        else if (obstacle == Obstacle.Block)
        {
            float y = go.GetComponent<Block>().Recieve();
            speed = speed * y;
            blockSlowdownRate = 3;
        }
        else if (obstacle == Obstacle.Iron)
        {
            irons++;
            go.GetComponent<Iron>().destroy();
        }
        else if (obstacle == Obstacle.PowerUp)
        {
            float q = go.GetComponent<PowerUp>().Recieve();
            health = health * q;
            damage = damage * q;
            maxHealth = maxHealth * q;
        }
    }
    private void stunManager(Obstacle obstacle, GameObject go)
    {
        if (obstacle == Obstacle.Player)
        {
            player.stun();
        }
        else if (obstacle == Obstacle.Block)
        {
            block = go.GetComponent<Block>();
            block.stun();
        }
        else if (obstacle == Obstacle.Iron)
        {
            go.GetComponent<Iron>().stun();
        }
        else if (obstacle == Obstacle.PowerUp)
        {
            powerUp = go.GetComponent<PowerUp>();
            powerUp.stun();
        }
    }
    private void unactionManager(Obstacle obstacle, GameObject go)
    {

        if (obstacle == Obstacle.PowerUp)
        {
            go.GetComponent<PowerUp>().undone();
        }
        else if (obstacle == Obstacle.Block)
        {
            go.GetComponent<Block>().undone();
        }
        else if (obstacle == Obstacle.Player)
        {
            go.GetComponent<Player>().undone();
        }
        else if (obstacle == Obstacle.Iron)
        {
            go.GetComponent<Iron>().undone();
        }
    }
    public bool LevelValidator(int level){
            if(level ==1){
                if(bestAction.ContainsKey(Obstacle.Block))
                return true;
            }else if(level ==2){
                if(bestAction.ContainsKey(Obstacle.PowerUp) && actionsLearnt.ContainsKey(Obstacle.Iron))
                    return true;
            }if(level ==3){
                return true;
            }
            return false;
    }
}
