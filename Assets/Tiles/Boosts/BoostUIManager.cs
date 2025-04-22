using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoostUIManager : MonoBehaviour
{
    [SerializeField] private GameObject boostUIPrefab;
    [SerializeField] private Transform boostPanel;

    private Dictionary<string, GameObject> activeBoosts = new();

    public void ShowBoost(string boostId, Sprite icon, float duration)
    {
        if (activeBoosts.ContainsKey(boostId))
        {
            Destroy(activeBoosts[boostId]);
            activeBoosts.Remove(boostId);
        }

        GameObject ui = Instantiate(boostUIPrefab, boostPanel);
        ui.transform.Find("Icon").GetComponent<Image>().sprite = icon;

        activeBoosts[boostId] = ui;
        StartCoroutine(UpdateBoostProgress(boostId, ui, duration));
    }

    private IEnumerator UpdateBoostProgress(string boostId, GameObject ui, float duration)
    {
        Image progressBar = ui.transform.Find("ProgressBar").GetComponent<Image>();
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            progressBar.fillAmount = 1f - (elapsed / duration);
            yield return null;
        }

        Destroy(ui);
        activeBoosts.Remove(boostId);
    }
}
