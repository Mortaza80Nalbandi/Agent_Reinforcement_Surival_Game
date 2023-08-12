using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Actions;
public class PowerUp : MonoBehaviour
{
    public bool learnable = true;
    private int R_type = 2;
    private int H_type = 1;
    private bool locked;
    private int Multiplier;
    private ObjectSpawner objectSpawner;
    private int state;
    void Start()
    {
        state = 0;
        Multiplier=2;
        locked = true;
        objectSpawner =GameObject.Find("ObjectSpawner").GetComponent<ObjectSpawner>();
    }

    public void hurt()
    {
        if(locked){
            locked = false;
            H_type = -2;
            R_type = 3;
            Multiplier = 3;
        }
        else {
            Destroy(gameObject);
        }
    }
    public void unhurt()
    {
        if(!locked){
            locked = false;
            H_type = 1;
            R_type = 2;
            Multiplier = 2;
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
    public int RewardMulti(){
        return Multiplier;
    }
}
