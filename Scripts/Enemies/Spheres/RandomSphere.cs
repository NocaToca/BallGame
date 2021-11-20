using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//A sphere that moves eratically 
[RequireComponent(typeof(Rigidbody))]
public class RandomSphere : MonoBehaviour
{
    public float randomStart;
    public float randomEnd;
    public float speed;
    
    public bool stunned;
    
    float stun;

    Rigidbody rb;

    float deltaTime;
    float chosenTime = -1.0f;

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

        //If no time is chosen, or we need to choose another time, we choose a new time
        if(chosenTime < 0){
            ChooseNewTime();
            return;
        }

        deltaTime += Time.deltaTime;

        //If we're not moving or if the time is up we change direction
        if(deltaTime >= chosenTime || rb.velocity == Vector3.zero){

            ChooseNewVelocity();
            deltaTime = 0;
            chosenTime = -1.0f;

        }
    }

    //Chooses a random time for the sphere to move in the specified direction
    void ChooseNewTime(){
        chosenTime = Random.Range(randomStart, randomEnd);
    }

    //Chooses a new random direction to move in
    void ChooseNewVelocity(){
        float angle = Random.Range(0, 360);
        float x = Mathf.Cos(angle * Mathf.Deg2Rad);
        float z = Mathf.Sin(angle * Mathf.Deg2Rad);

        Vector3 vel = new Vector3(x, 0, z);
        rb.velocity = vel * speed;
    }
}
