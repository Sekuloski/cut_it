using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using static API;

public class AxeSelector : MonoBehaviour
{
    Axe axe;
    SpriteRenderer axeSpriteRenderer;
    AxeSO selectedAxe;
    [SerializeField] AxeSO[] axes;
    [SerializeField] GameObject axeGridElementPrefab;
    [SerializeField] GameObject playerPrefab;
    [SerializeField] TextMeshProUGUI locationText;
    [SerializeField] TextMeshProUGUI usernameText;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        axe = FindObjectOfType<Axe>();
        selectedAxe = axes[0];
        axeSpriteRenderer = axe.GetComponent<SpriteRenderer>();
        axeSpriteRenderer.sprite = selectedAxe.axeTexture;
        UpdateGrid();
    }

    public void SelectDifferentAxe(int axeIndex)
    {
        Debug.Log(axeIndex);
        selectedAxe = axes[axeIndex];
        axeSpriteRenderer.sprite = selectedAxe.axeTexture;
    }

    void UpdateGrid()
    {
        GridLayoutGroup layoutGroup = FindObjectsOfType<GridLayoutGroup>()[1];
        for (int i = 0; i < axes.Length; i++)
        {
            GameObject newAxe = Instantiate(axeGridElementPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            var i1 = i;
            newAxe.GetComponent<Button>().onClick.AddListener(() => SelectDifferentAxe(i1));
            newAxe.transform.SetParent(layoutGroup.gameObject.transform, true);
            newAxe.transform.localScale = Vector3.one;
            newAxe.transform.GetChild(0).GetComponent<Image>().sprite = axes[i].axeTexture;
            newAxe.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = axes[i].name;
        }
    }

    public void UpdateLeaderboard(List<PlayerData> players, string location, string username)
    {
        locationText.text = location;
        usernameText.text = username;
        GridLayoutGroup layoutGroup = FindObjectsOfType<GridLayoutGroup>()[0];
        for (int i = 0; i < players.Count; i++)
        {
            GameObject newPlayer = Instantiate(playerPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            newPlayer.transform.SetParent(layoutGroup.gameObject.transform, true);
            newPlayer.transform.localScale = Vector3.one;
            newPlayer.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = players[i].name;
            newPlayer.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = players[i].location;
            newPlayer.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = players[i].high_score.ToString();
        }
    }
}
