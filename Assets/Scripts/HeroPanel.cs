using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroPanel : MonoBehaviour
{
    public static HeroPanel Instance{ get; private set;}
    
    public static event System.Action BuyHeroClick; 
    void Awake()
    {
        Instance = this;
    }
    // void OnEnable()
    // {
    //     BuyHeroClick += BuyHero;
    // }
    // void OnDisable()
    // {
    //     BuyHeroClick -= BuyHero;
    // }
    // public void BuyHero()
    // {
    //     BuyHeroClick?.Invoke();
    // }
    public void BuyRangeHero()
    {
        
    }
    public void BuyMeleeHero()
    {

    }
}
