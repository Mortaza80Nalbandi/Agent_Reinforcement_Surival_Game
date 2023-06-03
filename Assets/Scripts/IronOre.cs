using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IronOre : MonoBehaviour
{
    private float hardness;
    public GameObject IronPF;
    private int ironsTpSpawn;
    void Start()
    {
        hardness = 100f;
        ironsTpSpawn = 4;
    }

    public void hurt(float damage)
    {
        hardness -= damage * 4;
        if (hardness <= 0)
        {
            for (int i = 0; i < ironsTpSpawn; i++)
                Instantiate(IronPF, transform.position, Quaternion.identity);
        }
    }
    public float getHardness()
    {
        return hardness;
    }

}
