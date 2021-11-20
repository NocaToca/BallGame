using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    #region Initializing fields

    // Controls the main functions of the gun when attached to the player
    // Base class. I'm making it with the idea in mind that we're going to have different gun types

    [Header("Gun Settings")]
    public int Ammo = 100;
    [SerializeField] private float damage = 50.0f;

    [Header("Firing Settings")]
    [Tooltip("Fire rate is bullets/minute (120 will mean 2 bullets fire a second)")]
    [SerializeField] private float FireRate = 120.0f;
    [SerializeField] private float Velocity = 100.0f;
    private float TimeSinceLastFired = 0.0f;
    [HideInInspector] public bool shooting = false;

    [Header("Raytrace Settings")]
    [Tooltip("How far the bullet will travel before we consider it \"dead\"")]
    [SerializeField] private float TravelDistance = 25.0f;
    [SerializeField] private int Increments = 100;
    [Tooltip("Shows bullet lines")]
    [SerializeField] private bool ShowDebug = false;

    public Material lrMat;

    LineRenderer lr;

    #endregion

    private void Start(){
        GameObject obj = new GameObject();

        obj.transform.parent = this.transform;

        obj.gameObject.AddComponent<LineRenderer>();
        lr = obj.gameObject.GetComponent<LineRenderer>();
        lr.startWidth = .05f;
        lr.material = lrMat;
    }

    private void Update()
    {
        if (Input.GetMouseButton(0) && (TimeSinceLastFired >= (60.0f / FireRate)) && Ammo > 0)
        {
            shooting = true;
            Fire();
            TimeSinceLastFired = 0.0f;
        }
        else
        {
            TimeSinceLastFired += Time.deltaTime;
            //lr.gameObject.SetActive(false);
        }

        if(!Input.GetMouseButton(0)){
            lr.gameObject.SetActive(false);
        }

    }


    #region Firing

    //Main fire function. Each call fires an individual bullet
    public virtual void Fire(){

        //Play fire animination here once implementable 

        Vector3 playerPos = transform.position;
        Vector3 endPos = new Vector3(playerPos.x + TravelDistance, playerPos.y, playerPos.z);
        Vector3 FiringDirection = FindFiringAngle();

        //If we had clicked on our character we will just not fire, mostly because the direction of firing is unclear
        if(FiringDirection == new Vector3(0, 0, 0)){
            return;
        }

        //Since our vector found from FiringDirection is a unit vector, we just have to make the magnitude of it our travel distance
        Vector3 increment = new Vector3(FiringDirection.x * TravelDistance, FiringDirection.y * TravelDistance, FiringDirection.z * TravelDistance);

        //And then divide each by our increment to split it up
        increment = new Vector3(increment.x/Increments, increment.y/Increments, increment.z/Increments);

        RaycastHit hit;

        if(Physics.Raycast(new Vector3(playerPos.x, playerPos.y, playerPos.z), FiringDirection, out hit)){

            if(hit.rigidbody != this.GetComponent<Rigidbody>()){
                if(ShowDebug){
                    Debug.DrawRay(transform.position, FiringDirection, Color.green, 2.0f, true);
                    //Debug.Log(hit.point);
                    //Debug.Log(FiringDirection);

                }
                if(hit.distance > TravelDistance){
                    DrawLine(hit.point, this.transform.position);
                    return;
                }
                //Apply hit effects here. If the bullet hit an object we don't need to calculate it anymore

                Health enemyHealth = hit.transform.GetComponent<Health>();
                if(enemyHealth != null){
                    enemyHealth.OnHit(damage);
                }
            }
            DrawLine(hit.point, this.transform.position);

        } else {
            
        }

    }

    //Draws the line of our bullet/shot
    void DrawLine(Vector3 endPos, Vector3 startPos){
        lr.gameObject.SetActive(true);
        Vector3[] vects = new Vector3[2];
        vects[1] = endPos;
        vects[0] = startPos;
        lr.SetPositions(vects);

    }

    //Finds the firing angle based off of the transform of the player and position of the mouse
    public Vector3 FindFiringAngle(){
        //Getting our cursor position relative to world to make a vector
        Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 5));
        if(transform.position == cursorPosition){
            return new Vector3(0, 0, 0);
        }

        cursorPosition.y = transform.position.y;

        //Making a triangle
        float x = cursorPosition.x - transform.position.x;
        float y = cursorPosition.y - transform.position.y;
        float z = cursorPosition.z - transform.position.z;
        float h = Mathf.Sqrt((x*x) + (y*y) + (z*z));

        //With the triangle, make a unit vector
        return new Vector3(x/h, y/h, z/h);
    }



    #endregion
}
