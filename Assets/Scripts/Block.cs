using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Actions;
public class Block : MonoBehaviour
{
    private float hardness = 40;
    private int R_type = 0;
    private int H_type = 2;
    private int D_type = 1;
    public bool learnable = true;
    public void damage(float damage)
    {
        hardness -= damage;
        if (hardness <= 0)
        {
            learnable = false;
            Destroy(gameObject);
        }
    }
    public float getHardness()
    {
        return hardness;
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
        else if (action == Action.Dodge)
        {
            return D_type * 5;
        }
        return 0;
    }
}
