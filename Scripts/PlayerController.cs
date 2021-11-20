using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float dashStrength;
    public float speed;
    public float dashCoolDown;
    Rigidbody rb;
    bool inputAllow = true;

    float dashCD;

    //When we hit an enemy, we have to die!
    void OnTriggerEnter(Collider col){
        //Debug.Log("collided");
        if(col.GetComponent<Collider>().tag == "Enemy"){
            Health health = GetComponent<Health>();
            health.OnHit(health.health);
        }
    }

    // Start is called before the first frame update
    //Makes sure we have a rigidbody and gets it
    void Start()
    {
        
        dashCD = dashCoolDown;
        rb = GetComponent<Rigidbody>();
        if(rb == null){
            Debug.LogError("No rigidbody on player");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!inputAllow){
            dashCD += Time.deltaTime;
            if(dashCD >= dashCoolDown/8.0f){
                inputAllow = true;
            }
            return;
        }
        HandleMovement();
        HandleInputs();
    }

    //If we can dash, we can let the player dash. Otherwise we add time between dashes
    void HandleInputs(){
        dashCD += Time.deltaTime;
        if(Input.GetKey(KeyCode.Q) && dashCD > dashCoolDown){
            Dash();
            dashCD = 0;
        }
    }

    //For dashing, we simply get the Direction we're moving and then teleport to there
    void Dash(){
        Vector3 movingVector = GetDirectionMoving();

        inputAllow = false;
        rb.AddForce(movingVector * dashStrength * 100.0f);
    }

    //Returns the velocity as a unit vector
    Vector3 GetDirectionMoving(){
        Vector3 velocity = rb.velocity;
        velocity = Vector3.Normalize(velocity);

        return velocity;
    }

    //Handles the player movement
    void HandleMovement(){
        Vector3 force = GetDirectionVector();

        //If we're currently not moving, we just add a small force to slow down the player so it stops
        if(force == Vector3.zero){
            if(rb.velocity != Vector3.zero){
                Vector3 backForce = rb.velocity * -1;
                
                rb.AddForce(backForce);

            }
            return;
        }
        force.y = 0;

        rb.velocity = force * speed;

        
        //Vector3 axis = Vector3.Cross(force, Vector3.down);
        //float angle = (force.magnitude * 360 / (2*Mathf.PI));
        //transform.Rotate(axis, angle * Time.deltaTime, Space.World);
    }

    //Returns the direction from our mouse position relative to the player position
    Vector3 GetDirectionVector(){

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 5));

        return GetInputMovement(mousePos);


    }

    //Based on our mouse position we get the forward vector
    Vector3 GetInputMovement(Vector3 mousePos){
        Vector3 forwardVector = Vector3.forward;

        //Vector3 vector = mousePos - transform.position;
        //forwardVector = new Vector3(vector.x/vector.magnitude, vector.y/vector.magnitude, vector.z/vector.magnitude);

        Vector3 direction = Vector3.zero;
        if(Input.GetKey(KeyCode.W)){
            direction += forwardVector;
        }
        if(Input.GetKey(KeyCode.S)){
            direction -= forwardVector;
        }
        if(Input.GetKey(KeyCode.A)){
            direction += Vector3.Cross(forwardVector, Vector3.up);
        }
        if(Input.GetKey(KeyCode.D)){
            direction -= Vector3.Cross(forwardVector, Vector3.up);
        }
        direction = Vector3.Normalize(direction);
        return direction;
    }
}

