using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    float bombStrength = 500.0f;
    public float cooldown = 5.0f;
    public GameObject bombPrefab;
    float deltaTime;
    GameObject boom;

    float boomTime;
    float currentRadius;
    public float growSpeed;
    bool playingEffects;

    // Start is called before the first frame update
    void Start()
    {
        deltaTime = cooldown;
    }


    // Update is called once per frame
    void Update()
    {
        if(playingEffects){
            UpdateBoomEffects();
        }
        deltaTime += Time.deltaTime;
        if(deltaTime >= cooldown){
            if(Input.GetKey(KeyCode.Space)){
                Boom();
                //Debug.Log("Boom");
                deltaTime = 0.0f;
            }
            
        }
    }

    //While the bomb effect is playing, we tell it to grow and then fade off before destroying it
    void UpdateBoomEffects(){
        currentRadius += growSpeed * Time.deltaTime;
        if(currentRadius >= 1.0f){
            playingEffects = false;
            Destroy(boom);
            return;
        }
        if(currentRadius >= .5f){
            float fade = -1.95f * (currentRadius-.5f) + 1.0f;
            Material mat = boom.GetComponent<MeshRenderer>().material;
            mat.SetFloat("Opacity", fade);
        }

        boom.transform.localScale = new Vector3(currentRadius, 1, currentRadius);
    }

    //Explodes, and hits every gameobject withen a radius that isnt the player. We also have to make sure to stun certain objects
    void Boom(){
        RaycastHit[] hits;
        hits = Physics.SphereCastAll(this.transform.position, 4.0f, Vector3.up, .05f);
        foreach(RaycastHit hit in hits){
            GameObject obj = hit.transform.gameObject;
            if(obj != this.transform.gameObject){
                Health health = obj.GetComponent<Health>();
                if(health != null){
                    health.OnHit(500);
                    Rigidbody rb = obj.GetComponent<Rigidbody>();

                    FollowSphere fs = obj.GetComponent<FollowSphere>();
                    RandomSphere rs = obj.GetComponent<RandomSphere>();
                    MovingSphere ms = obj.GetComponent<MovingSphere>();
                    if(fs != null){
                        fs.stunned = true;
                    } else
                    if(rs != null){
                        rs.stunned = true;
                    } else
                    if(ms != null){
                        ms.stunned = true;
                    }

                    if(rb != null){
                        rb.AddForce(CalculateForceDirection(this.transform.position, hit.transform.position) * -bombStrength);
                    }
                    
                }
            }
        }
        PlayBoomEffects();
        //Destroy(this);
    }

    //Plays the explosion effects
    void PlayBoomEffects(){
        boom = Instantiate(bombPrefab);
        boom.transform.position = new Vector3(this.transform.position.x, 0, this.transform.position.z);
        boom.transform.localScale = new Vector3(0, 1, 0);

        playingEffects = true;

        boomTime = 0.0f;
        currentRadius = 0.0f;
    }

    //Calculates knockback direction if the game object does not die
    Vector3 CalculateForceDirection(Vector3 origin, Vector3 obj){
        Vector3 vel = origin - obj;
        vel = Vector3.Normalize(vel);

        return vel;
    }
}
