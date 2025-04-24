using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoostUIManager : MonoBehaviour
{
    
    [Header("Boots")]
    [SerializeField] private GameObject bootsBoostObject;
    [SerializeField] private Image bootsProgressBar;

    [Header("Bus")]
    [SerializeField] private GameObject busBoostObject;
    [SerializeField] private Image busProgressBar;

    [Header("Magnet")]
    [SerializeField] private GameObject magnetBoostObject;
    [SerializeField] private Image magnetProgressBar;

    [Header("TwoXMultiplier")]
    [SerializeField] private GameObject twoXMultiplierBoostObject;
    [SerializeField] private Image twoXMultiplierProgressBar;

    void Update()
    {

        // increase progress on filled image
        if (bootsBoostObject.activeSelf)
        {
            if (bootsProgressBar.fillAmount < 1f)
            {
                bootsProgressBar.fillAmount += Time.deltaTime / 5f; // 5 seconds to fill
            }
            else
            {
                bootsBoostObject.SetActive(false);
                bootsProgressBar.fillAmount = 0f;
            }
        }

    }

    public void StartBoost(string boostType, float duration)
    {
        switch(boostType)
        {
            case "Boots":
                bootsProgressBar.fillAmount = 0f;
                StartCoroutine(FillProgressBar("Boots", duration));
                break;
            case "Bus":
                busProgressBar.fillAmount = 0f;
                StartCoroutine(FillProgressBar("Bus", duration));
                break;
            case "Magnet":
                magnetProgressBar.fillAmount = 0f;
                StartCoroutine(FillProgressBar("Magnet", duration));
                break;
            case "TwoXMultiplier":
                twoXMultiplierProgressBar.fillAmount = 0f;
                StartCoroutine(FillProgressBar("TwoXMultiplier", duration));
                break;
            default:
                Debug.LogError("Invalid boost type: " + boostType);
                break;
        }
    }

    IEnumerator FillProgressBar(string boostType, float duration)
    {
        GameObject boostObject = null;
        Image progressBar = null;
        float time = 0f;
    
    if(boostType == "Boots")
    {
        progressBar = bootsProgressBar;
        boostObject = bootsBoostObject;
    }
    else if(boostType == "Bus")
    {
        progressBar = busProgressBar;
        boostObject = busBoostObject;
    }
    else if(boostType == "Magnet")
    {
        progressBar = magnetProgressBar;
        boostObject = magnetBoostObject;
    }
    else if(boostType == "TwoXMultiplier")
    {
        progressBar = twoXMultiplierProgressBar;
        boostObject = twoXMultiplierBoostObject;
    }

        // Set boost object active
        boostObject.SetActive(true);

        // Reset the progress bar
        progressBar.fillAmount = 0f;

        // Fill the progress bar over the duration
        while (time < duration)
        {
            time += Time.deltaTime;
            progressBar.fillAmount = time / duration;
            yield return null;
        }

        boostObject.SetActive(false);

    }

    

}
