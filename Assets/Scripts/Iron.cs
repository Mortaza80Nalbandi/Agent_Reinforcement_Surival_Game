using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Actions;

public class Iron : MonoBehaviour
{
    enum States
    {
        Good,
        Bad,
        S0 ,
        Null
    }
    private int R_type = 2;
    private int H_type = -2;
    private float S_type = -0.5f;
    private ObjectSpawner objectSpawner ;
    private States lastState;
    private int irons = 1 ;
    public void destroy()
    {
        objectSpawner = GameObject.Find("ObjectSpawner").GetComponent<ObjectSpawner>();
        learnable = false;
        Destroy(gameObject);
        objectSpawner.decreaseIrons();
        
    }
    public void hit()
    {
        lastState= state;
        if(state == States.S0){
            state = States.Bad;
        }
        stateConfigure(state);
    }
    public int Recieve(){
        lastState = state
        if(state == States.S0){
            state = States.Good;
        }
        stateConfigure(state);
        return irons;
    }
    public void stun(){
        state = state;
    }
    public void undone()
    {
        stateConfigure(lastState)
    }
    private void stateConfigure(State newState){
        if( newState == States.Bad || newState == States.Good){
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
