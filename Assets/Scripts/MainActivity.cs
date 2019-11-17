using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft;
using Newtonsoft.Json;

public class MainActivity : MonoBehaviour
{
    [SerializeField] private Button randomButton;
    [SerializeField] private Button favoritesAdvices;

    private void Awake()
    {
        randomButton.onClick.AddListener(RandomButtonClick);
        
    }

    private void Start()
    {
        RequestManager.Instance.RequestAdvice((status, advice) =>
        {
            Debug.Log("MainActivity: " + status + " " + advice.AdviceText);
        });
    }

    private void RandomButtonClick()
    {

    }

    private void FavoritesAdvicesClick()
    {
        
    }
}
