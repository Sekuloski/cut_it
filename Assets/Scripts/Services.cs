using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Services : MonoBehaviour
{
    private string openCageApiKey = "79353961e8fe4ed88156cab72d549914";
    public string location;
    public string country;
    private RawImage rawImage;
    private WebCamTexture webCamTexture;

    [System.Serializable]
    public class OpenCageApiResponse
    {
        public Result[] results;
    }

    [System.Serializable]
    public class Result
    {
        public Components components;
    }

    [System.Serializable]
    public class Components
    {
        public string country_code;
    }

    private void Awake()
    {
        StartCamera();
            StartCoroutine(GetLocation());
        }

    public void StartCamera()
    {
        rawImage = GetComponent<RawImage>(); // Reference to the RawImage component

        //StartCoroutine(GetLocation());
        if (WebCamTexture.devices.Length > 0)
        {
            // Access the first available camera
            webCamTexture = new WebCamTexture();

            rawImage.texture = webCamTexture;
            Debug.LogError(webCamTexture);
            webCamTexture.Play();
        }
        else
        {
            Debug.LogError("No camera found on the device.");
        }
    }

    private IEnumerator GetLocation()
    {
        if (Input.location.isEnabledByUser)
        {
            Input.location.Start();
            int maxWait = 5;

            while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
            {
                yield return new WaitForSeconds(1);
                maxWait--;
            }

            if (Input.location.status == LocationServiceStatus.Running)
            {
                float latitude = Input.location.lastData.latitude;
                float longitude = Input.location.lastData.longitude;
                location = $"{latitude},{longitude}";

                string url = $"https://api.opencagedata.com/geocode/v1/json?q={latitude}+{longitude}&key={openCageApiKey}";

                using (UnityWebRequest www = UnityWebRequest.Get(url))
                {
                    yield return www.SendWebRequest();

                    if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
                    {
                        Debug.LogError($"Geocoding API request error: {www.error}");
                    }
                    else
                    {
                        string jsonResult = www.downloadHandler.text;
                        OpenCageApiResponse response = JsonUtility.FromJson<OpenCageApiResponse>(jsonResult);

                        if (response.results.Length > 0)
                        {
                            country = response.results[0].components.country_code.ToUpper();
                        }
                        else
                        {
                            Debug.LogError(jsonResult);
                        }

                        yield return null;
                    }
                }
            }
        }
        else
        {
            Debug.Log("Location disabled");
            Permission.RequestUserPermission(Permission.FineLocation);
        }
    }
}
