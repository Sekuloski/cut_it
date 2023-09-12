using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using static API;

public class API : MonoBehaviour
{
    [System.Serializable]
    public class JsonData
    {
        public List<PlayerData> players;
    }

    [System.Serializable]
    public class PlayerData
    {
        public int id;
        public string name;
        public int high_score;
        public string location;

        public PlayerData(string username, int high_score, string location)
        {
            this.id = 0;
            this.name = username;
            this.high_score = high_score;
            this.location = location;
        }
    }
    private string baseUrl = "http://sekuloski.mk:25565";

    public void GetPlayerData()
    {
        string playersEndpoint = baseUrl + "/players";
        Debug.Log("Calling /players");
        StartCoroutine(SendGetRequest(playersEndpoint));
    }

    public void UpdateData(string username, int high_score, string location = "")
    {
        PlayerData playerData = new PlayerData(username, high_score, location);
        string updateEndpoint = baseUrl + "/update";
        string json = JsonUtility.ToJson(playerData);
        StartCoroutine(SendPostRequest(updateEndpoint, json));
    }

    IEnumerator SendGetRequest(string url)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError ||
                webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(webRequest.error);
            }
            else
            {
                string responseJson = webRequest.downloadHandler.text;
                List<PlayerData> playerList = JsonUtility.FromJson<JsonData>(responseJson).players;

                foreach (PlayerData playerData in playerList)
                {
                    Debug.Log($"ID: {playerData.id}, Name: {playerData.name}, High Score: {playerData.high_score}, Location: {playerData.location}");
                }
            }
        }
    }

    IEnumerator SendPostRequest(string url, string data)
    {
        byte[] postData = System.Text.Encoding.UTF8.GetBytes(data);

        using (UnityWebRequest webRequest = UnityWebRequest.Post(url, "POST"))
        {
            webRequest.uploadHandler = new UploadHandlerRaw(postData);
            webRequest.downloadHandler = new DownloadHandlerBuffer();

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError ||
                webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(webRequest.error);
            }
            else
            {
                Debug.Log("Data sent successfully.");
            }
        }
    }
}
