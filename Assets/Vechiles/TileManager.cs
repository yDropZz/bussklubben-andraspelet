using UnityEngine;

using System.Collections.Generic;

public class TileManager : MonoBehaviour
{

    public Transform player;
    public float spawnZ = 0f;
    public float tileLength = 320f;
    public int tilesOnScreen = 2;
    public GameObject[] tilePrefabs;
    public int tileSpawned = 0;

    [SerializeField] private float safeZone = 800f;
    [SerializeField] private float deleteZone = 200f;
    private List<GameObject> activeTiles = new List<GameObject>();

    void Start()
    { 
        for(int i = 0; i < tilesOnScreen; i++)
        {
            SpawnTile(i == 0);
        } 

    }

    void Update()
    {
        if(player.position.z + safeZone > spawnZ)
        {
            SpawnTile();

        }   

        if(activeTiles.Count > 0 && player.position.z - deleteZone > activeTiles[0].transform.position.z)
        {
            DeleteTile();
        }

    }

    void SpawnTile(bool forceDefault = false)
    {
        GameObject go;
        int randomIndex = Random.Range(0, tilePrefabs.Length);
        int attempts = 0;
        // Ensure we don't spawn the same tile twice in a row  
        while(randomIndex == tileSpawned && attempts < 10)
        {
            randomIndex = Random.Range(0, tilePrefabs.Length);
            attempts++;
        }
//
        if(forceDefault)
        {
            go = Instantiate(tilePrefabs[randomIndex]);
            tileSpawned = randomIndex;
        }
        else
        {
            go = Instantiate(tilePrefabs[randomIndex]);
            tileSpawned = randomIndex;
        }

        go.transform.position = new Vector3(0, 0, spawnZ);

        activeTiles.Add(go);
        spawnZ += tileLength;

    }

    void DeleteTile()
    {
        Destroy(activeTiles[0]);
        activeTiles.RemoveAt(0);
    }


}