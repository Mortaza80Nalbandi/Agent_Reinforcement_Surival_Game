using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Actions;
public class PowerUp : MonoBehaviour
{
     enum States
    {
        Good,
        Bad,
        S0 ,
        S1,
        Null
    }
    private int R_type = 2;
    private int H_type = -1;
    private float S_type = -0.5f;
    private bool locked;
    private int Multiplier;
    private ObjectSpawner objectSpawner;
    private States state;
    private States lastState;
    void Start()
    {
        state = States.S0;
        Multiplier=2;
        locked = true;
        objectSpawner =GameObject.Find("ObjectSpawner").GetComponent<ObjectSpawner>();
    }

    public void hit()
    {
        lastState= state;
        if(state == States.S0){
            state = States.S1;
        }else if(state == States.S1){
            state = States.Bad;
        }
        stateConfigure(state);
    }
    public int Recieve(){
        lastState = state
        if(state == States.S0){
            state = States.Good;
        }else if(state == States.S1){
            state = States.Good;
        }
        stateConfigure(state);
        return Multiplier;
    }
    public void stun(){
        state = state;
    }
    public void undone()
    {
        stateConfigure(lastState)
    }
    private void stateConfigure(State newState){
        if(newState == States.S1){
            H_type = -2;
            R_type = 4;
            Multiplier = 3;
            locked = false;
        }else if(newState != States.Good){
            H_type = -1;
            R_type = 2;
            Multiplier = 2;
            locked = true;
        } else if( newState == States.Bad || newState == States.Good){
            Destroy(gameObject);
        }
    }
    public float costCalculator(Action action)
    {
        if (action == Action.Hit)
        {
            return H_type * 5;
        }
        else if (action == Action.Recieve)
        {
            return R_type * 5;
        }else if (action == null)
        {
            return S_type * 5;
        }
        return 0;
    }
}
