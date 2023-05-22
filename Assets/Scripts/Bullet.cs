using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Actions;
public class Bullet : MonoBehaviour
{
    private float damage;
    private int R_type = 0;
    private int H_type = 0;
    private int D_type = 2;
    public bool learnable = true;
    // Start is called before the first frame update
    void Start()
    {
        damage = 5;
    }


    public float getDamage()
    {
        return damage;
    }
    public void destroy()
    {
        learnable = false;
        Destroy(gameObject);
    }

    public int costCalculator(Action action)
    {
        if (action == Action.Hit)
        {
            destroy();
            return H_type * 5;
        }
        else if (action == Action.Recieve)
        {
            return R_type * 5;
        }
        else if (action == Action.Dodge)
        {
            return D_type * 5;
        }
        return 0;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<wall>() != null ||other.gameObject.GetComponent<Block>() != null )
        {
           Destroy(gameObject);
        }
    }
}
