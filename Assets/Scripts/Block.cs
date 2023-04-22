using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    private float hardness=80;
    public void damage(float damage){
        hardness-= damage;
        if(hardness<=0)
                Destroy(gameObject);
    }

}
