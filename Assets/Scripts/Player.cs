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

    public float health;
    private float speed;
    private int up, down, left, right;
    public int irons;
    public GameObject blockPrefab;
    public GameObject bulletPrefab;
    private float blockCreateRate;
    private float fireRate;
    private float meeleAttackRate;
    private Camera MainCamera;
    private PowerUp powerUp;
    private Enemy enemy;
    private float damage;
    private Weapon weapon;
    private int R_type = 0;
    private int H_type = 2;
    public bool learnable = true;
    private playerUI pui;
    private healthbar healthbarx;
    private int score;
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
        if (health <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
        health = 20f;
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
        healthbarx.setHealth(health, 20);
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
        weaponWheel();
        movementInput();
        HarvestCHeck();
        fireButton();
    }
    private void HarvestCHeck()
    {
        if (Input.GetKey(KeyCode.E) && powerUp != null)
        {
            if (meeleAttackRate <= 0)
            {
                powerUp.hurt(damage);
                meeleAttackRate = 0.25f;
            }
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
                bullet.GetComponent<Bullet>().setDamage(2);
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
    public void hurt(float damageRecieved)
    {
        health -= damageRecieved;
        healthbarx.setHealth(health, 20);

    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.GetComponent<Iron>() != null)
        {
            irons++;
            Destroy(other.gameObject);
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
        }
    }

    public int costCalculator(Action action)
    {
        if (action == Action.Hit)
        {
            return H_type * 5;
        }
        else if (action == Action.Recieve)
        {
            return R_type * 5;
        }
        return 0;
    }
}
