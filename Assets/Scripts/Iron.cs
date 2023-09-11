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
        S0,
        Null
    }
    private int R_type = 2;
    private int H_type = -2;
    private float S_type = -0.5f;
    private ObjectSpawner objectSpawner;
    private States state;
    private List<States> lastStates = new List<States>();
    private int irons = 5;
    public void destroy()
    {
        objectSpawner = GameObject.Find("ObjectSpawner").GetComponent<ObjectSpawner>();
        objectSpawner.decreaseIrons();
        Destroy(gameObject);
    }
    public void hit()
    {
        lastStates.Add(state);
        if (state == States.S0)
        {
            state = States.Bad;
        }
        stateConfigure(state);
    }
    public int Recieve()
    {
        lastStates.Add(state);
        if (state == States.S0)
        {
            state = States.Good;
        }
        stateConfigure(state);
        return irons;
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
             R_type = 2;
            H_type = -2;
            S_type = -0.5f;
        }
        else if (newState == States.Bad || newState == States.Good)
        {
            destroy();
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
