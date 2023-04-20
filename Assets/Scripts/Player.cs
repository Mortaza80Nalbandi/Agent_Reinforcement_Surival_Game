using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float health;
    //private int damage;
    private float speed;
    private int up,down,left,right;
    public int irons;
    public GameObject blockPrefab;
    private float blockCreateRate;
    private Camera MainCamera;
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
        irons = 1000;
        blockCreateRate=0.5f;
        MainCamera = Camera.main;
        MainCamera.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        blockCreateRate-=Time.deltaTime;
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
        if (Input.GetButtonDown("Fire1")){
            if(blockCreateRate<=0){
                Vector3 blockPos = MainCamera.ScreenToWorldPoint(Input.mousePosition);
                blockPos.z=0f;
                makeBlock(blockPos);
            }
        }
    }
    private void makeBlock( Vector3 blockPos ){
        if(irons>=5){
            Vector3 move = new Vector3(1,0,0);
            GameObject block = Instantiate(blockPrefab, transform.position  + (blockPos-transform.position)/ Vector3.Distance(blockPos,transform.position), Quaternion.identity);
            irons-=5;
            blockCreateRate = 0.5f;
        }
        
    }
    public void hurt(float damageRecieved){
        health-=damageRecieved;
        
    }
     private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.GetComponent<Iron>()!=null ){
            irons++;
            Destroy(other.gameObject);
        }
    }
}
