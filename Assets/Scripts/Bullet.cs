using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Actions;
public class Bullet : MonoBehaviour
{
    private float damage = 1;
    // Start is called before the first frame update
    void Start()
    {
        Camera MainCamera = Camera.main;
        Vector2 positionOnScreen = MainCamera.WorldToViewportPoint(transform.position);
        Vector2 mouseOnScreen = (Vector2)MainCamera.ScreenToViewportPoint(Input.mousePosition);
        float angle = AngleBetweenTwoPoints(positionOnScreen, mouseOnScreen);
        transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle+135));
    }

    private float AngleBetweenTwoPoints(Vector3 a, Vector3 b)
    {
        return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
    }
    public float getDamage()
    {
        return damage;
    }
    public void setDamage(float damage)
    {
        this.damage = damage;
    }
    public void destroy()
    {
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<wall>() != null || other.gameObject.GetComponent<Block>() != null)
        {
            Destroy(gameObject);
        }else if (other.gameObject.GetComponent<Enemy>() != null)
        {
            other.gameObject.GetComponent<Enemy>().hurt(damage);
            Destroy(gameObject);
        }
    }
}
