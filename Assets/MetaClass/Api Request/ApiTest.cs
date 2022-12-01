using System;
using System.Collections;
using System.Collections.Generic;
using OpenCover.Framework.Model;
using UnityEngine;
using UnityEngine.Networking;




public class ApiTest : MonoBehaviour
{
    
    

    public String Url;
    
    // Start is called before the first frame update
    void Start()
    {
        IEnumerator fechData = FetchData(msg => Debug.Log(msg));
        StartCoroutine(fechData);
        
    }

    // Update is called once per frame
    void Update()
    {

    }


    public IEnumerator FetchData(Action<String> action)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(Url))
        {
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                action("errore");
            }
            else
            {
                RandomCatFacts randomCatFacts = JsonUtility.FromJson<RandomCatFacts>(request.downloadHandler.text);
                action(randomCatFacts.ToString());
            }
        }
    }
}