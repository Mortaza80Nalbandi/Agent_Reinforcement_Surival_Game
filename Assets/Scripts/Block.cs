using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Actions;
public class Block : MonoBehaviour
{
    enum States
    {
        Bad,
        S0,
        S1,
        Null
    }
    private float hardness = 40;
    private float R_type = -2;
    private float H_type = 1;
    private float S_type = -0.5f;
    private States state = States.S0;
    private List<States> lastStates = new List<States>();
    public float getHardness()
    {
        return hardness;
    }
    public void hit(float damage)
    {
        lastStates.Add(state);
        if (state == States.S0 || state == States.S1)
        {
            state = States.S1;
        }
        hardness -= damage;
        if (hardness <= 0)
        {
            state = States.Bad;
            Destroy(gameObject);
        }
        stateConfigure(state);
    }
    public float Recieve()
    {
        lastStates.Add(state);
        if (state == States.S0 || state == States.S1)
        {
            state = States.Bad;
        }
        stateConfigure(state);
        float x = 0.75f;
        return x;
    }

    public void stun()
    {
        lastStates.Add(state);
    }
    public void undone()
    {
        stateConfigure(lastStates[lastStates.Count - 1]);
        state = lastStates[lastStates.Count - 1];
        lastStates.RemoveAt(lastStates.Count - 1);
    }
    private void stateConfigure(States newState)
    {
        if (newState == States.S0)
        {
            R_type = -2;
            H_type = 1;
            S_type = -0.5f;
        }
        else if (newState == States.S1)
        {
            H_type = 1;
        }
        else if (newState == States.Bad)
        {
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
        }
        else if (action == Action.Stun)
        {
            return S_type * 5;
        }
        return 0;
    }
}
