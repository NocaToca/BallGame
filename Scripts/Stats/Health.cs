using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{

    public float health;
    public float score;
    public bool invincible;

    public AudioSource al;
    // Start is called before the first frame update
    void Start()
    {
    }

    public bool OnHit(float damage){
        if(invincible){
            return true;
        }
        health -= damage;
        if(health <= 0){
            OnDeath();
        }
        return health <= 0;
    }

    void OnDeath(){
        al.Play();
        GameObject go = transform.gameObject;
        if(go.tag == "Player"){
            go.SetActive(false);
            return;
        }
        Game.score += score;
        Destroy(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
