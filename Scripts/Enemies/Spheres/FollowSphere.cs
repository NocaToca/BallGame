using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//A simple follow sphere that follows the player
[RequireComponent(typeof(Rigidbody))]
public class FollowSphere : MonoBehaviour
{
    public float speed;
    public bool stunned;
    
    float stun;
    GameObject player;
    Rigidbody rb;

    // Start is called before the first frame update
    void Start(){
        player = GameObject.FindGameObjectsWithTag("Player")[0];

        rb = GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update(){

        //We have to make sure the sphere can move and is not temparily stunned
        if(stunned){
            stun += Time.deltaTime;
            stunned = stun <= 1.0f;
            return;
        } else {
            stun = 0.0f;
        }
        MoveTowardsPlayer();
    }

    //Simply just finds the player position and follows it
    void MoveTowardsPlayer(){
        Vector3 velocityDirection = FindVelocityVector();

        rb.velocity = velocityDirection * speed;
    }

    //returns the unit vector for the direction between the player and this sphere
    Vector3 FindVelocityVector(){
        Vector3 playerPos = player.transform.position;
        Vector3 spherePos = this.transform.position;

        Vector3 vel = playerPos - spherePos;

        return Vector3.Normalize(vel);

    }
}
