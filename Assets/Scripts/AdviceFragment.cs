using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdviceFragment : BaseFragment
{
    [SerializeField] Text adviceText;
    [SerializeField] Button addAdviceBtn;
    [SerializeField] Button nextAdviceBtn;
    [SerializeField] Image loader;

    private Advice currentAdvice;

    private void Awake()
    {
        addAdviceBtn.onClick.AddListener(AddAdviceToFavourite);
        nextAdviceBtn.onClick.AddListener(NextAdvice);
        DBManager.Instance.DatabaseUpdated += DatabaseUpdated;
        GetNewAdvice();
    }

    private void OnDestroy()
    {
        DBManager.Instance.DatabaseUpdated -= DatabaseUpdated;
    }

    private void AddAdviceToFavourite()
    {
        if(currentAdvice != null)
        {
            DBManager.Instance.AddNewAdvice(currentAdvice);
        }
            
    }

    private void NextAdvice()
    {
        GetNewAdvice();
    }

    private void Update()
    {
        loader.transform.Rotate(0, 0, -2);
    }

    private void GetNewAdvice()
    {
        SetLoaderVisible(true);
        RequestManager.Instance.RequestAdvice(Response);
    }

    private void Response(RequestManager.ResponseStatus status, Advice advice)
    {
        SetLoaderVisible(false);
        if (status == RequestManager.ResponseStatus.Success)
        {
            currentAdvice = advice;
            adviceText.text = currentAdvice.AdviceText;
            addAdviceBtn.gameObject.SetActive(!CheckExistsAdviceInDb(currentAdvice));
        }
        else
        {
            adviceText.color = Color.red;
            adviceText.text = "Error";
        }
    }

    private bool CheckExistsAdviceInDb(Advice advice)
    {
        var list = DBManager.Instance.CurrentAdvices;
        return list.Find(a => a.AdviceId == advice.AdviceId) != null;
    }

    private void SetLoaderVisible(bool isVisible)
    {
        loader.gameObject.SetActive(isVisible);
        adviceText.gameObject.SetActive(!isVisible);
        addAdviceBtn.gameObject.SetActive(!isVisible);
        nextAdviceBtn.gameObject.SetActive(!isVisible);
    }

    private void DatabaseUpdated()
    {
        addAdviceBtn.gameObject.SetActive(!CheckExistsAdviceInDb(currentAdvice));
    }
}
