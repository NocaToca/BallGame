using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Simple class that controls enemy spawning. 
public class SpawnZone : MonoBehaviour
{
    public float radius;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Takes a game object and then spawns it at a random location
    public bool Spawn(GameObject gameObject){

        //int here to prevent crashing and freezes during logic
        int crashPrevention = 0;

        bool spawn = false;

        //We simply just choose a random spot in the circle and check to make sure theres no surrounding enemies or players (objects with the health component)
        while(crashPrevention < 10){
            float angle = Random.Range(0, 360);
            float dist = Random.Range(0.0f, radius);

            Vector3 vector = new Vector3(Mathf.Cos(angle*Mathf.Deg2Rad) * dist, 0, Mathf.Sin(angle*Mathf.Deg2Rad) * dist);

            RaycastHit[] hits = Physics.SphereCastAll(new Vector3(vector.x, 10.0f, vector.z), 1.0f, Vector3.down, 10.5f);
            spawn = true;
            foreach(RaycastHit hit in hits){
                
                Health health = hit.transform.gameObject.GetComponent<Health>();
                if(health != null){
                    spawn = false;
                    break;
                    
                } 
        
            }

            //if we found no player objects in our circle, we will spawn the object
            if(spawn){
                Spawn(new Vector3(vector.x, vector.y + 1.0f, vector.z), gameObject);
                return true;
            }
            

            crashPrevention++;
        }

        return false; 
    }

    //If we have a location, we can just spawn it at that position
    public void Spawn(Vector3 position, GameObject gameObject){
        GameObject go = Instantiate(gameObject, position, gameObject.transform.rotation);
        go.SetActive(true);
    }
}
