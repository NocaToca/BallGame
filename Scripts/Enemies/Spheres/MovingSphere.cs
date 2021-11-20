using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//A sphere that moves randomly but is much smoother than Random sphere
[RequireComponent(typeof(Rigidbody))]
public class MovingSphere : MonoBehaviour
{

    public float speed;
    public float rotateTime;
    
    public bool stunned;
    
    float stun;

    Rigidbody rb;

    float deltaTime;

    bool rotating;
    Vector3 newVel;

    // Start is called before the first frame update
    void Start(){
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

        //If we're not moving, we can just choose a direction to go in
        if(rb.velocity == Vector3.zero){
            ChooseStartVelocity();
        }

        if(rotating){
           rotating = !RotateTowardsVector();
        }

        //After a certain amount of time, we choose a new random vector to circle around to
        if(deltaTime >= rotateTime){
            FindVectorToRotateTo();
            deltaTime = 0.0f;
        }
        
    }

    //Rotates towards vector slowly
    bool RotateTowardsVector(){
        float x = newVel.x - rb.velocity.x;
        x = x/ Mathf.Abs(x);

        float z = newVel.z - rb.velocity.z;
        z = z/ Mathf.Abs(z);

        rb.velocity += new Vector3(x, 0, z);
        return Mathf.Abs(rb.velocity.x - newVel.x) < .05f &&  Mathf.Abs(rb.velocity.z - newVel.z) < .05f;
    }

    //Finds a random velocity
    Vector3 FindRandomVel(){
        float angle = Random.Range(0, 360);
        float x = Mathf.Cos(angle * Mathf.Deg2Rad);
        float z = Mathf.Sin(angle * Mathf.Deg2Rad);

        return new Vector3(x, 0, z);
    }

    //Chooses a random start velocity
    void ChooseStartVelocity(){
        Vector3 vel = FindRandomVel();
        rb.velocity = vel * speed;
    }

    //Chooses a random vector to rotate to
    void FindVectorToRotateTo(){
        newVel = FindRandomVel(); 
        rotating = true;
    }
}
