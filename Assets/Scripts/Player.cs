using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Player : MonoBehaviour
{
    enum Weapon
    {
        Gun,
        Meele,
        Block
    }

    public float health;
    //private int damage;
    private float speed;
    private int up, down, left, right;
    public int irons;
    public GameObject blockPrefab;
    public GameObject bulletPrefab;
    private float blockCreateRate;
    private float fireRate;
    private float meeleAttackRate;
    private Camera MainCamera;
    private IronOre ironOre;
    private Enemy enemy;
    private float damage;
    private Weapon weapon;

    void Start()
    {
        resetMovement();
        attributeSet();
        rateSet();
        EntitySet();
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
    private void attributeSet()
    {
        speed = 0.6f;
        health = 100f;
        irons = 10;
        damage = 5;
    }
    private void rateSet()
    {
        blockCreateRate = 0.5f;
        fireRate = 0.2f;
        meeleAttackRate = 0.25f;
    }
    private void EntitySet()
    {
        weapon = Weapon.Gun;
        MainCamera = Camera.main;
        MainCamera.enabled = true;
        enemy = null;
        ironOre = null;
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
        if (Input.GetKey(KeyCode.E) && ironOre != null)
        {
            if (meeleAttackRate <= 0)
            {
                ironOre.hurt(damage);
                if (ironOre.getHardness() <= 0)
                {
                    Destroy(ironOre.gameObject);
                    ironOre = null;
                }
                meeleAttackRate = 0.25f;
            }
        }
    }
    private void fireButton()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (weapon == Weapon.Gun && fireRate <= 0)
            {
                Vector2 positionOnScreen = MainCamera.WorldToViewportPoint(transform.position);
                Vector2 mouseOnScreen = (Vector2)MainCamera.ScreenToViewportPoint(Input.mousePosition);
                float angle = AngleBetweenTwoPoints(positionOnScreen, mouseOnScreen);
                GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.Euler(new Vector3(0f, 0f, angle)));
                bullet.GetComponent<Rigidbody2D>().AddForce((MainCamera.ScreenToWorldPoint(Input.mousePosition) - transform.position) * 300);
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
            weapon = Weapon.Gun;
        else if (Input.GetKey(KeyCode.Alpha2))
            weapon = Weapon.Meele;
        else if (Input.GetKey(KeyCode.Alpha3))
            weapon = Weapon.Block;
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
        }

    }
    public void hurt(float damageRecieved)
    {
        health -= damageRecieved;

    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.GetComponent<Iron>() != null)
        {
            irons++;
            Destroy(other.gameObject);
        }
        else if (other.gameObject.GetComponent<IronOre>() != null)
        {
            ironOre = other.gameObject.GetComponent<IronOre>();
        }
        else if (other.gameObject.GetComponent<Enemy>() != null)
        {
            enemy = other.gameObject.GetComponent<Enemy>();
        }
    }
    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.GetComponent<IronOre>() != null)
        {
            ironOre = null;
        }
        else if (other.gameObject.GetComponent<Enemy>() != null)
        {
            enemy = null;
        }
    }
}
