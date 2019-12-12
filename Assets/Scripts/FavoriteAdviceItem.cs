using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FavoriteAdviceItem : MonoBehaviour
{
    [SerializeField] Text adviceText;
    [SerializeField] Button deleteButton;

    private Advice currentAdvice;

    public Advice Advice => currentAdvice;

    private void Awake()
    {
        deleteButton.onClick.AddListener(OnDeleteClick);
    }

    public void Init(Advice advice)
    {
        currentAdvice = advice;
        adviceText.text = advice.AdviceText;
    }

    private void OnDeleteClick()
    {
        if (currentAdvice != null)
            DBManager.Instance.DeleteAdviceById(currentAdvice.AdviceId);
    }
}
