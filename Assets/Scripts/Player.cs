using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using Actions;
public class Player : MonoBehaviour
{
    enum Weapon
    {
        Bow,
        Meele,
        Block
    }
    enum States
    {
        S0,
        S1,
        S2,
        S3,
        Null
    }
    public float health;
    public float maxHealth;
    private float speed;
    private int up, down, left, right;
    public int irons;
    public GameObject blockPrefab;
    public GameObject bulletPrefab;
    private float blockCreateRate;
    private float fireRate;
    private float meeleAttackRate;
    private float unStunRate;
    private Camera MainCamera;
    private PowerUp powerUp;
    private Enemy enemy;
    public float damage;
    private Weapon weapon;
    private float R_type = 1;
    private float H_type = 0.8f;
    private float S_type = 0.8f;
    public bool Stuned = false;
    private playerUI pui;
    private healthbar healthbarx;
    private int score;
    private States state;
    private List<States> lastStates = new List<States>();
    void Start()
    {
        resetMovement();
        EntitySet();
        attributeSet();
        rateSet();

    }
    void Update()
    {
        GetInput();
        blockCreateRate -= Time.deltaTime;
        fireRate -= Time.deltaTime;
        meeleAttackRate -= Time.deltaTime;
        unStunRate -= Time.deltaTime;
        if (health <= 0)
        {
            SceneManager.LoadScene(0);
        }
        if (unStunRate <= 0)
        {
            Stuned = false;
        }
    }
    private void FixedUpdate()
    {
        Vector3 move = new Vector3((right - left) * speed, (up - down) * speed, 0);
        GetComponent<Rigidbody2D>().MovePosition(transform.position + move * Time.deltaTime);
        resetMovement();


    }
    public void updateScore(int increase)
    {
        score += increase;
        pui.updateScore(score);
    }
    private void attributeSet()
    {
        speed = 1f;
        maxHealth = 100f;
        health = maxHealth;
        irons = 10;
        damage = 5;
        pui.updateIron(irons);
        score = 0;
        pui.updateScore(score);
    }
    private void rateSet()
    {
        blockCreateRate = 0.5f;
        fireRate = 0.2f;
        meeleAttackRate = 0.25f;
    }
    private void EntitySet()
    {
        weapon = Weapon.Bow;
        MainCamera = Camera.main;
        healthbarx = transform.GetChild(1).gameObject.GetComponent<healthbar>();
        healthbarx.setHealth(health, maxHealth);
        pui = GameObject.Find("UI").gameObject.GetComponent<playerUI>();
        MainCamera.enabled = true;
        enemy = null;
        powerUp = null;
    }
    private void resetMovement()
    {
        up = 0;
        down = 0;
        left = 0;
        right = 0;
    }
    private void GetInput()
    {
        if (!Stuned)
        {
            weaponWheel();
            movementInput();
            HarvestCHeck();
            fireButton();
        }
    }
    private void HarvestCHeck()
    {
        if (Input.GetKey(KeyCode.E) && powerUp != null)
        {
            if (meeleAttackRate <= 0)
            {
                powerUp.hit();
                meeleAttackRate = 0.25f;
            }
        }
        else if (Input.GetKey(KeyCode.R) && powerUp != null)
        {
            float q = powerUp.Recieve();
            health = health * q;
            damage = damage * q;
            maxHealth = maxHealth * q;
        }
        if (powerUp != null)
        {
            if (powerUp.getDestroyed())
                powerUp = null;
        }
    }
    private void fireButton()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (weapon == Weapon.Bow && fireRate <= 0)
            {
                Vector2 positionOnScreen = MainCamera.WorldToViewportPoint(transform.position);
                Vector2 mouseOnScreen = (Vector2)MainCamera.ScreenToViewportPoint(Input.mousePosition);
                float angle = AngleBetweenTwoPoints(positionOnScreen, mouseOnScreen);
                GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.Euler(new Vector3(0f, 0f, angle)));
                bullet.GetComponent<Rigidbody2D>().AddForce((MainCamera.ScreenToWorldPoint(Input.mousePosition) - transform.position) * 300);
                bullet.GetComponent<Bullet>().setDamage(damage/2);
                fireRate = 0.2f;
            }
            else if (weapon == Weapon.Block && blockCreateRate <= 0)
            {
                Vector3 blockPos = MainCamera.ScreenToWorldPoint(Input.mousePosition);
                blockPos.z = 0f;
                makeBlock(blockPos);
                blockCreateRate = 0.5f;
            }
            else if (weapon == Weapon.Meele && meeleAttackRate <= 0 && enemy != null)
            {
                enemy.hurt(damage);
                meeleAttackRate = 0.25f;
            }
        }
    }
    private void movementInput()
    {
        if (Input.GetKey(KeyCode.A))
        {
            left++;
        }
        if (Input.GetKey(KeyCode.D))
        {
            right++;
        }
        if (Input.GetKey(KeyCode.W))
        {
            up++;
        }
        if (Input.GetKey(KeyCode.S))
        {
            down++;
        }
    }
    void weaponWheel()
    {
        if (Input.GetKey(KeyCode.Alpha1))
            weapon = Weapon.Bow;
        else if (Input.GetKey(KeyCode.Alpha2))
            weapon = Weapon.Meele;
        else if (Input.GetKey(KeyCode.Alpha3))
            weapon = Weapon.Block;
        string x = "" + weapon;
        pui.updateWeapon(x);
    }
    private float AngleBetweenTwoPoints(Vector3 a, Vector3 b)
    {
        return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
    }
    private void makeBlock(Vector3 blockPos)
    {
        if (irons >= 5)
        {
            Instantiate(blockPrefab, transform.position + (blockPos - transform.position) / Vector3.Distance(blockPos, transform.position), Quaternion.identity);
            irons -= 5;
            blockCreateRate = 0.5f;
            pui.updateIron(irons);
        }

    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.GetComponent<Iron>() != null)
        {
            irons+=other.gameObject.GetComponent<Iron>().Recieve();
            pui.updateIron(irons);
        }
        else if (other.gameObject.GetComponent<PowerUp>() != null)
        {
            powerUp = other.gameObject.GetComponent<PowerUp>();
        }
        else if (other.gameObject.GetComponent<Enemy>() != null)
        {
            enemy = other.gameObject.GetComponent<Enemy>();
        }
    }
    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.GetComponent<PowerUp>() != null)
        {
            powerUp = null;
        }
        else if (other.gameObject.GetComponent<Enemy>() != null)
        {
            enemy = null;
            state = States.S0;
            stateConfigure(state);
        }
    }
    public void hit(float damageRecieved)
    {
        lastStates.Add(state);
        if (state == States.S0 || state == States.S2 || state == States.S3)
        {
            state = States.S1;
        }
        if (Stuned)
            health -= 2 * damageRecieved;
        else
            health -= damageRecieved;
        healthbarx.setHealth(health, maxHealth);
        stateConfigure(state);
    }
    public int Recieve()
    {
        lastStates.Add(state);
        if (state == States.S0 || state == States.S2 || state == States.S3)
        {
            state = States.S3;
        }
        stateConfigure(state);
        int x = 0;
        if (irons >= 1)
        {
            irons--;
            x = 1;
            pui.updateIron(irons);
        }
        return x;
    }

    public void stun()
    {
        lastStates.Add(state);
        if (state == States.S0 || state == States.S2 || state == States.S3)
        {
            state = States.S2;
        }
        if (S_type != -2)
        {
            Stuned = true;
        }
        unStunRate = 1f;
        stateConfigure(state);
    }
    public void undone()
    {

        stateConfigure(lastStates[lastStates.Count - 1]);
        state = lastStates[lastStates.Count - 1];
        lastStates.RemoveAt(lastStates.Count - 1);
    }
    private void stateConfigure(States newState)
    {
        if (newState == States.S0)
        {
            R_type = 1;
            H_type = 0.8f;
            S_type = 0.8f;
        }
        else if (newState == States.S1)
        {
            H_type = -0.2f;
        }
        else if (newState == States.S2)
        {
            S_type = -1.8f;
            H_type = 1.4f;
        }
        else if (newState == States.S3)
        {
            R_type = -1.8f;
            S_type = 1.2f;
        }
    }
    public float costCalculator(Action action)
    {
        if (action == Action.Hit)
        {
            return H_type * 5;
        }
        else if (action == Action.Recieve)
        {
            return R_type * 5;
        }
        else if (action == Action.Stun)
        {
            return S_type * 5;
        }
        return 0;
    }
}
