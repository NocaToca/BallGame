using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Triangle is a stationary enemy that shoots at the player
[RequireComponent(typeof(LineDrawer))]
public class Triangle : MonoBehaviour
{

    public float scanTime;
    public float chargeTime;

    float deltaTime;
    LineDrawer line;
    Vector3 firePos;
    // Start is called before the first frame update
    void Start(){
        line = GetComponent<LineDrawer>();
    }

    // Update is called once per frame
    //If we're scanning, keep drawing the line towards the player, otherwise keep the line stationary
    void Update(){
        deltaTime += Time.deltaTime;
        if(scanTime <= deltaTime){
            if(scanTime + chargeTime <= deltaTime){
                FireBeam();
                deltaTime = 0;
            }
        } else {
            UpdateLine();
        }
    }

    //Updates the line to follow the player
    void UpdateLine(){
        GameObject player = GameObject.FindGameObjectsWithTag("Player")[0];
        Transform[] transforms = new Transform[2];
        transforms[0] = player.transform;
        transforms[1] = this.transform;
        firePos = player.transform.position;

        line.DrawLine(transforms);
    }

    //Fires the shot in the direction of where the player was a few seconds ago, hoping to hit the player
    void FireBeam(){
        RaycastHit hit;
        GameObject player = GameObject.FindGameObjectsWithTag("Player")[0];
        Vector3 dir = firePos - this.transform.position;
        dir = Vector3.Normalize(dir);
        if(Physics.Raycast(this.transform.position, dir, out hit)){
            if(hit.transform.gameObject == player){
                Health health = player.GetComponent<Health>();
                health.OnHit(health.health);
            }
        }
    }
}
