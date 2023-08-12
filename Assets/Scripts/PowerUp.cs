using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Actions;
public class PowerUp : MonoBehaviour
{
    public bool learnable = true;
    private int R_type = 0;
    private int H_type = 2;
    private bool locked;
    private ObjectSpawner objectSpawner;
    void Start()
    {
        locked = true;
        objectSpawner =GameObject.Find("ObjectSpawner").GetComponent<ObjectSpawner>();
    }

    public void hurt(float damage)
    {
        if(locked){
            locked = false;
        }
        else {
            Destroy(gameObject);
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
