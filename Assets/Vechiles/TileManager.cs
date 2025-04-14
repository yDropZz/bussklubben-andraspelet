using UnityEngine;

using System.Collections.Generic;

public class TileManager : MonoBehaviour
{

    public Transform player;
    public float spawnZ = 0f;
    public float tileLength = 320f;
    public int tilesOnScreen = 2;
    public GameObject[] tilePrefabs;

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
        if(forceDefault)
        {
            go = Instantiate(tilePrefabs[0]);
        }
        else
        {
            go = Instantiate(tilePrefabs[Random.Range(0, tilePrefabs.Length)]);
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