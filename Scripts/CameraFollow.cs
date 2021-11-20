using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Simple camera follow to follow the player
public class CameraFollow : MonoBehaviour
{
    public GameObject followingObject;
    public Vector3 distance;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(followingObject == null){
            return;
        }
        transform.position = followingObject.transform.position + distance;
    }
}
