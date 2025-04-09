using UnityEngine;

[CreateAssetMenu(menuName = "TileData", fileName = "New Tile Data")]
public class TileData : ScriptableObject
{
    
    public GameObject[] carPrefabs;
    public Vector3[] carPosition;
    public float[] carSpeeds;


}
