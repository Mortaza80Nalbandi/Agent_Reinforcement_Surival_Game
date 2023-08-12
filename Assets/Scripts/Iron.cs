using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Actions;

public class Iron : MonoBehaviour
{
    private int R_type = 2;
    private int H_type = -2;
    public bool learnable = true;
    private ObjectSpawner objectSpawner ;
    public void destroy()
    {
        objectSpawner = GameObject.Find("ObjectSpawner").GetComponent<ObjectSpawner>();
        learnable = false;
        Destroy(gameObject);
        objectSpawner.decreaseIrons();
        
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
            destroy();
            return R_type * 5;
        }
        return 0;
    }
}
