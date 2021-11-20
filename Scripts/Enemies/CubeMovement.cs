using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//A cube class that is a lot like the triangle but instead of firing a shot, instead opts to throw itself at the player
[RequireComponent(typeof(Rigidbody))]
public class CubeMovement : MonoBehaviour
{
    public float rushPause;
    public float rushCharge;
    public float rushStrength;
    public float warningRadius;

    Rigidbody rb;
    Vector3 rushPos;
    bool charging;

    float deltaTime;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if(rb == null){
            Debug.LogError("wtf");
        }
    }

    // Update is called once per frame
    void Update(){
        if(charging){
            WarnPlayer();
        }

        deltaTime += Time.deltaTime;
        if(deltaTime >= rushPause){
            Rush();
            deltaTime = 0;
            charging = false;
            Game.RemoveCubesCharging(this);
        } else 
        if(deltaTime < rushPause - rushCharge){
            RotateToPlayer();
        } else {
            SetRushPos();
        }
    }
    
    //If we're not already charging, we set the position of where the player was and set charging to true
    void SetRushPos(){
        if(!charging){
            GameObject player = GameObject.FindGameObjectsWithTag("Player")[0];
            rushPos = player.transform.position;
            charging = true;
            
        }
    }

    //Throws itself at the rush position of where the player was
    void Rush(){
        
        Vector3 playerPos = rushPos;

        Vector3 forceDir = playerPos - transform.position;
        forceDir = new Vector3(forceDir.x/forceDir.magnitude, 0, forceDir.z/forceDir.magnitude);

        rb.AddForce(forceDir * rushStrength);
    }

    //Looks at the player
    void RotateToPlayer(){
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        if(players.Length == 0){
            return;
        }
        GameObject player = players[0];
        transform.LookAt(player.transform);
    }

    //Tells the game that this cube is charging
    void WarnPlayer(){
        Game.AddCubesCharging(this);
    }

    //Checks to make sure that the player is lined up with the cube so that if it would charge the player would get hit
    public bool IsLinedUpWithPlayer(){
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        if(players.Length == 0){
            return false;
        }
        GameObject player = players[0];

        Vector3 dir = this.transform.position - player.transform.position;
        dir = Vector3.Normalize(dir);

        RaycastHit hit;

        return Physics.Raycast(this.transform.position, dir, out hit);
    }
}
