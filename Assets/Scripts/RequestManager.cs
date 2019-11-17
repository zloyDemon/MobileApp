using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

public class RequestManager : MonoBehaviour
{
    private const string MainUrl = "https://api.adviceslip.com/advice";

    private Coroutine corCurrentRequest;
    
    public enum ResponseStatus
    {
        None,
        Success,
        Error,
        ParseError,
    }
    
    public class Response
    {
        [JsonProperty("slip")]
        public Slip slip;
        
        public class Slip
        {
            [JsonProperty("advice")]
            public string advice;
            [JsonProperty("slip_id")]
            public string slip_id;
        }
    }
    
    public static RequestManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void OnDestroy()
    {
        Instance = null;
    }

    public void RequestAdvice(Action<ResponseStatus, Advice> onComplete)
    {
        if (corCurrentRequest != null)
        {
            StopCoroutine(corCurrentRequest);
            corCurrentRequest = null;
        }

        corCurrentRequest = StartCoroutine(CorRequest(onComplete));
    }
    
    private IEnumerator CorRequest(Action<ResponseStatus, Advice> onComplete)
    {
        Advice advice = null;
        ResponseStatus status = ResponseStatus.None;
        
        var request = UnityWebRequest.Get(MainUrl);
        
        yield return request.SendWebRequest();
        
        if (!request.isHttpError && !request.isNetworkError)
        {
            var downloadText = request.downloadHandler.text;
            var response = JsonConvert.DeserializeObject<Response>(downloadText);
            if (response != null)
            {
                var id = response.slip.slip_id;
                if (int.TryParse(id, out int intId))
                {
                    advice = new Advice(intId, response.slip.advice);
                    status = ResponseStatus.Success;
                }
                else
                {
                    Debug.LogError("RequestManager: Integer id parse error");
                    status = ResponseStatus.ParseError;
                }
            }
        }
        else
        {
            Debug.LogError("RequestManager: Http request has errors");
            status = ResponseStatus.Error;
        }

        onComplete?.Invoke(status, advice);
        request.Dispose();
    }
}


