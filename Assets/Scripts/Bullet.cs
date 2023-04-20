using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float damage;
    // Start is called before the first frame update
    void Start()
    {
        damage = 5;   
    }
    

    public float getDamage(){
        return damage;
    }
    void OnTriggerEnter2D(Collider2D other) {
       if(other.gameObject.GetComponent<Block>()!=null){
           Destroy(gameObject);
       }else if(other.gameObject.GetComponent<IronOre>()!=null){
           Destroy(gameObject);
       }else if(other.gameObject.GetComponent<Enemy>()!=null){
           other.gameObject.GetComponent<Enemy>().bulletHit(damage);
           Destroy(gameObject);
       }
       Debug.Log("a");
    }
}
