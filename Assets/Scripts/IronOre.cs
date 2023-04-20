using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IronOre : MonoBehaviour
{
    private float hardness;
    public GameObject IronPF;
    // Start is called before the first frame update
    void Start()
    {
        hardness = 100f;   
    }

    // Update is called once per frame
    public void hurt(float damage){
        hardness-= damage*4;
        if(hardness<=0){
            for (int i = 0; i <4 ; i++)
                Instantiate(IronPF, transform.position , Quaternion.identity);
        }
    }
    public float getHardness(){
        return hardness;
    }

}
