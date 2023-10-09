using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Actions;
public class PowerUp : MonoBehaviour
{
    enum States
    {
        Goal,
        Fail,
        Initial,
        BrokenLock,
        Null
    }
    private int R_type = 2;
    private int H_type = -1;
    private float S_type = -0.5f;
    private bool locked;
    private float Multiplier;
    private ObjectSpawner objectSpawner;
    private States state;
    private List<States> lastStates = new List<States>();
    void Start()
    {
        state = States.Initial;
        Multiplier = 1.2f;
        locked = true;
        objectSpawner = GameObject.Find("ObjectSpawner").GetComponent<ObjectSpawner>();
    }
    public bool getDestroyed()
    {
        return (state == States.Fail || state == States.Goal);
    }
    public void hit()
    {
        lastStates.Add(state);
        if (state == States.Initial && locked == true)
        {
            state = States.BrokenLock;
        }
        else if (state == States.BrokenLock)
        {
            state = States.Fail;
        }
        stateConfigure(state);
    }
    public float Recieve()
    {
        lastStates.Add(state);
        if (state == States.Initial)
        {
            state = States.Good;
        }
        else if (state == States.BrokenLock)
        {
            state = States.Good;
        }
        stateConfigure(state);
        return Multiplier;
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
        if (newState == States.Initial)
        {
            H_type = -1;
            R_type = 2;
            Multiplier = 1.2f;
            locked = true;
        }
        else if (newState == States.BrokenLock)
        {
            H_type = -2;
            R_type = 4;
            Multiplier = 1.5f;
            locked = false;
        }
        else if (newState == States.Fail)
        {
            Multiplier = 1;
            Destroy(gameObject);
        }
        else if (newState == States.Goal)
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
