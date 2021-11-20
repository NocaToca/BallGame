using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invincibility : MonoBehaviour
{
    bool started = false;
    float duration = 5.0f;
    float deltaTime;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.Space)){
            started = true;
            Health health = GetComponent<Health>();
            health.invincible = true;
        }
        if(!started){
            return;
        }
        deltaTime += Time.deltaTime;
        if(deltaTime >= duration){
            Die();
        }
    }

    void Die(){
        Health health = GetComponent<Health>();
        health.invincible = false;

        Destroy(this);
    }
}
