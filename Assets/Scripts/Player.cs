using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private float health;
    //private int damage;
    private float speed;
    private int up,down,left,right;
    bool collided;
    // Start is called before the first frame update
    void Start()
    {
        speed = 0.6f;
        health = 100f;
        //damage = 5;
        up = 0;
        down = 0;
        left =0 ;
        right = 0;
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        
    }
    private void FixedUpdate() {
        Vector3 move = new Vector3((right-left)*speed,(up-down)*speed,0);
        GetComponent<Rigidbody2D>().MovePosition(transform.position + move * Time.deltaTime);
        up=0;
        down= 0;
        left=0;
        right=0;
        

    }
    private void GetInput(){
        if(Input.GetKey(KeyCode.A)){
            left++;
        }
        if(Input.GetKey(KeyCode.D)){
            right++;
        }
        if(Input.GetKey(KeyCode.W)){
            up++;
        }
        if(Input.GetKey(KeyCode.S)){
            down++;
        }
    }


    public void hurt(float damageRecieved){
        health-=damageRecieved;
        
    }
}
