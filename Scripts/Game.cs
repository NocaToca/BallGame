using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//The game logic of the game
[RequireComponent(typeof(SpawnZone))]
public class Game : MonoBehaviour
{
    public GameObject player;
    public static float score;

    public GameObject[] enemyPrefabs;
    SpawnZone sz;

    public float spawn;

    int difficulty;
    bool gameover = false;

    float timePassed;
    float lastSpawn;

    public Text scoreTex;
    public Canvas ui;

    public Text warningPrefab;

    List<Text> warningList = new List<Text>();

    static List<CubeMovement> cubes = new List<CubeMovement>();

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectsWithTag("Player")[0];
        sz = GetComponent<SpawnZone>();
    }

    // Update is called once per frame
    //In update, we have to make sure to update our hud, controls and enemy spawn based on how the game progress is going
    void Update()
    {
        UpdateCubeWarnings();

        if(gameover){
            if(Input.GetKey(KeyCode.R)){
                Restart();
            }
            if(Input.GetKey(KeyCode.Escape)){
                Application.Quit();
                
				#if UNITY_EDITOR
				UnityEditor.EditorApplication.isPlaying = false; 
				#endif
            }
            return;
        }
        if(player.activeSelf == false){
            DestroyAllEnemies();
            EndGame();
            gameover = true;
            return;
        }

        timePassed += Time.deltaTime;
        lastSpawn += Time.deltaTime;
        if(spawn <= lastSpawn){
            SpawnEnemy();
            lastSpawn = 0.0f;
        }
        scoreTex.text = "Score " + score;
    }

    //Destroys all enemies in the field, simply by destroying their game objects
    void DestroyAllEnemies(){
        cubes.Clear();
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach(GameObject enemy in enemies){
            Destroy(enemy);
        }
    }
    
    //Ends the current game
    void EndGame(){
        gameover = true;
    }

    //Spawns a random enemy based on our current choices 
    void SpawnEnemy(){
        int index = Random.Range(0, enemyPrefabs.Length);
        sz.Spawn(enemyPrefabs[index]);
    }

    //Restarts the game, resetting the score
    void Restart(){
        gameover = false;
        player.SetActive(true);
        player.transform.position = new Vector3(0, 1, 0);
        score = 0;
    }

    //Adds cubes to the list of cubes charging so we can update the HUD
    public static void AddCubesCharging(CubeMovement cube){
        cubes.Add(cube);
    }
    public static void RemoveCubesCharging(CubeMovement cube){
        if(cubes.Contains(cube)){
            cubes.Remove(cube);
        }
    }

    //Foreach cube that we added to cube charging, we simply just calculate if the cube would hit the player and then put a big '!' in line with the cube and the player, so the player can dodge
    void UpdateCubeWarnings(){

        foreach(Text warning in warningList){
            GameObject.Destroy(warning.gameObject);
        }

        warningList.Clear();

        foreach(CubeMovement cube in cubes){
            if(cube.IsLinedUpWithPlayer()){
                Vector3 playerPos = player.transform.position;
                Vector3 cubePos = cube.transform.position;

                Vector3 dir = cubePos - playerPos;
                dir = Vector3.Normalize(dir);

                Vector3 warningPosition = (dir * cube.warningRadius) + player.transform.position;

                Vector3 screenPos = Camera.main.WorldToScreenPoint(warningPosition);

                Text tex = GameObject.Instantiate(warningPrefab);
                tex.transform.SetParent(ui.transform);

                tex.rectTransform.position = screenPos;
                tex.gameObject.SetActive(true);

                warningList.Add(tex);
            }
            
        }
        
        //We need to clear the cubes otherwise it will continuously add them
        cubes.Clear();
    }
}
