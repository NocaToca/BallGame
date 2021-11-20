using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//A line drawer class to help the triangle class
public class LineDrawer : MonoBehaviour
{
    public float width;
    public Material mat;
    LineRenderer lr;

    // Start is called before the first frame update
    void Start(){
        GameObject go = new GameObject();
        go.transform.parent = this.transform;

        go.AddComponent<LineRenderer>();
        lr = go.GetComponent<LineRenderer>();

        lr.startWidth = width;
        lr.material = mat;
    }

    // Update is called once per frame
    void Update(){
        
    }

    //Just draws the line based off of the given transforms
    public void DrawLine(Transform[] transforms){

        Vector3[] vectors = new Vector3[transforms.Length];
        for(int i = 0; i < transforms.Length; i++){
            vectors[i] = transforms[i].position;
        }

        lr.SetPositions(vectors);
    }
}
