using UnityEngine;

public class TileController : MonoBehaviour
{

    public TileData tileData;

    void Start()
    {
        for(int i = 0; i < tileData.carPrefabs.Length; i++)
        {
            GameObject car = Instantiate(tileData.carPrefabs[i], transform.position + tileData.carPosition[i], Quaternion.identity);
            car.GetComponent<CarMover>().SetSpeed(tileData.carSpeeds[i]);
        }   
    }

}
