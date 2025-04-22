using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI coinsText;

    // Update is called once per frame
    void Update()
    {
        // Update the coins text with the current amount of coins from MainManager
        coinsText.text = "Score: " + MainManager.Instance.Coins.ToString();
    }
}
