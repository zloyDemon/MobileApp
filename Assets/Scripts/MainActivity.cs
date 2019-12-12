using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Data;
using Newtonsoft;
using Newtonsoft.Json;

public class MainActivity : MonoBehaviour
{
    [SerializeField] Button randomButton;
    [SerializeField] Button favoritesAdvices;
    [SerializeField] AdviceFragment AdviceFragment;
    [SerializeField] FavouritesFragment FavouritesFragment;

    private BaseFragment currentFragment;
    private Fragment currentFragmentType;

    public enum Fragment
    {
        None,
        Advice,
        Favourites,
    }

    private void Awake()
    {
        randomButton.onClick.AddListener(RandomButtonClick);
        favoritesAdvices.onClick.AddListener(FavoritesAdvicesClick);
        ChangeFragment(Fragment.Advice);
    }

    private void Start()
    {

    }

    private void RandomButtonClick()
    {
        ChangeFragment(Fragment.Advice);
    }

    private void FavoritesAdvicesClick()
    {
        ChangeFragment(Fragment.Favourites);
    }



    private void ChangeFragment(Fragment type)
    {
        if (currentFragmentType == type)
            return;

        if (currentFragment != null)
            currentFragment.Hide();

        if (type == Fragment.Advice)
            currentFragment = AdviceFragment;

        if (type == Fragment.Favourites)
            currentFragment = FavouritesFragment;

        currentFragment.Show();
        currentFragmentType = type;
    }
}
