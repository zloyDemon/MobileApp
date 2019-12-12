using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FavouritesFragment : BaseFragment
{
    [SerializeField] FavoriteAdviceItem favoriteAdviceItemPrefab;
    [SerializeField] RectTransform itemParent;

    private List<Advice> currentListAdvices;
    private List<FavoriteAdviceItem> favoriteAdviceItems = new List<FavoriteAdviceItem>();

    private void Awake()
    {
        currentListAdvices = DBManager.Instance.CurrentAdvices;
        DBManager.Instance.AddedToDB += AddToDb;
        DBManager.Instance.DeletedFromDB += DeleteFromDb;
        Init();
    }

    private void OnDestroy()
    {
        DBManager.Instance.AddedToDB -= AddToDb;
        DBManager.Instance.DeletedFromDB -= DeleteFromDb;
    }

    private void AddToDb(Advice advice)
    {
        currentListAdvices = DBManager.Instance.CurrentAdvices;
        AddItemToList(advice);
    }

    private void DeleteFromDb(Advice advice)
    {
        currentListAdvices = DBManager.Instance.CurrentAdvices;
        var item = favoriteAdviceItems.Find(f => f.Advice.AdviceId == advice.AdviceId);
        if(item != null)
        {
            favoriteAdviceItems.Remove(item);
            Destroy(item.gameObject);
        }
    }

    private void Init()
    {
        foreach(var advice in currentListAdvices)
            AddItemToList(advice);
    }

    private void AddItemToList(Advice advice)
    {
        var adviceItem = Instantiate(favoriteAdviceItemPrefab, itemParent);
        adviceItem.Init(advice);
        favoriteAdviceItems.Add(adviceItem);
    }
}
