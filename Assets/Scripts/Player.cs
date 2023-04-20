using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Player : MonoBehaviour
{
    public float health;
    //private int damage;
    private float speed;
    private int up, down, left, right;
    public int irons;
    public GameObject blockPrefab;
    public GameObject bulletPrefab;
    private float blockCreateRate;
    private float fireRate;
    private Camera MainCamera;
    IronOre ironOre;
    private float damage;
    // Start is called before the first frame update
    void Start()
    {
        speed = 0.6f;
        health = 100f;
        //damage = 5;
        up = 0;
        down = 0;
        left = 0;
        right = 0;
        irons = 10;
        damage = 5;
        blockCreateRate = 0.5f;
        fireRate = 0.5f;
        MainCamera = Camera.main;
        MainCamera.enabled = true;
        fireRate = 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        blockCreateRate -= Time.deltaTime;
        fireRate -= Time.deltaTime;
        if (health <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
    private void FixedUpdate()
    {
        Vector3 move = new Vector3((right - left) * speed, (up - down) * speed, 0);
        GetComponent<Rigidbody2D>().MovePosition(transform.position + move * Time.deltaTime);
        up = 0;
        down = 0;
        left = 0;
        right = 0;


    }
    private void GetInput()
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
        if (Input.GetButtonDown("Fire1"))
        {
            if (blockCreateRate <= 0)
            {
                Vector3 blockPos = MainCamera.ScreenToWorldPoint(Input.mousePosition);
                blockPos.z = 0f;
                makeBlock(blockPos);
            }
        }
        if (Input.GetKey(KeyCode.E) && ironOre != null)
        {
            if (blockCreateRate <= 0)
            {
                ironOre.hurt(damage);
                if (ironOre.getHardness() <= 0)
                {
                    Destroy(ironOre.gameObject);
                    ironOre = null;
                }
            }

        }
        if (Input.GetKey(KeyCode.R))
        {
            if (fireRate <= 0)
            {
                Vector2 positionOnScreen = MainCamera.WorldToViewportPoint(transform.position);
                Vector2 mouseOnScreen = (Vector2)MainCamera.ScreenToViewportPoint(Input.mousePosition);
                float angle = AngleBetweenTwoPoints(positionOnScreen, mouseOnScreen);
                GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.Euler(new Vector3(0f, 0f, angle)));
                bullet.GetComponent<Rigidbody2D>().AddForce((MainCamera.ScreenToWorldPoint(Input.mousePosition) - transform.position) * 300);
                fireRate = 0.5f;
            }
        }
    }
    private float AngleBetweenTwoPoints(Vector3 a, Vector3 b)
    {
        return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
    }
    private void makeBlock(Vector3 blockPos)
    {
        if (irons >= 5)
        {
            Vector3 move = new Vector3(1, 0, 0);
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
        if (other.gameObject.GetComponent<IronOre>() != null)
        {
            ironOre = other.gameObject.GetComponent<IronOre>();
        }
    }
    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.GetComponent<IronOre>() != null)
        {
            ironOre = null;
        }
    }
}
