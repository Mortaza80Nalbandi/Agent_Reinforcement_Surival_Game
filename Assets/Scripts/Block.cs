using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Actions;
public class Block : MonoBehaviour
{
    enum States
    {
        Bad,
        S0 ,
        S1,
        Null
    }
    private float hardness = 40;
    private int R_type = -1;
    private float H_type = 1.8f;
    private float S_type = -0.5f;
    private States state;
    private States lastState;
    public float getHardness()
    {
        return hardness;
    }
    public void hit(float damage)
    {
        lastState= state;
        if(state == States.S0){
            state = States.Good;
        }else if(state == States.Good){
            state = States.Good;
        }
        hardness -= damage;
        if (hardness <= 0)
        {
            st
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
