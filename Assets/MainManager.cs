using Unity.VisualScripting;
using UnityEngine;

public class MainManager : MonoBehaviour
{

public static MainManager Instance { get; private set;}
private int coins = 0;
public int Coins {get {return coins;}}

//
// Coins = Final score, använd den till APIn.
//

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }   
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddCoins(int amount)
    {
        coins += amount;
    }

}
