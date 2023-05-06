using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Actions;

public class Iron : MonoBehaviour
{
    private int R_type=0;
    private int H_type=1;
    private int D_type=1;
    public float costCalculator(Action action){
        if(action == Action.Hit){
            return H_type*5;
        }else if(action == Action.Recieve){
            return R_type*5;
        }else if(action == Action.Dodge){
            return D_type*5;
        }
        return 0;
    }
    // Start is called before the first frame update
}
