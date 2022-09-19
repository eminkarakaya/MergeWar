using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    void OnDisable()
    {
        if(GameManager.instance.gameStage == GameStage.inGame)
        {
                
            if(GetComponent<Unit>().side == Side.ally)
            {
                GameManager.instance.allAlly.Remove(GetComponent<Unit>());
                for (int i = 0; i < GameManager.instance.allEnemies.Count; i++)
                {
                    if(GameManager.instance.allEnemies[i].transform.GetChild(0).GetComponent<Range>().enemiesInRange.Contains(GetComponent<Unit>()))
                    {
                        GameManager.instance.allEnemies[i].transform.GetChild(0).GetComponent<Range>().enemiesInRange.Remove(GetComponent<Unit>());
                    }
                }
            }
            else
            {
                GameManager.instance.allEnemies.Remove(GetComponent<Unit>());
                for (int i = 0; i < GameManager.instance.allAlly.Count; i++)
                {
                    if(GameManager.instance.allAlly[i].transform.GetChild(0).GetComponent<Range>().enemiesInRange.Contains(GetComponent<Unit>()))
                    {
                        GameManager.instance.allAlly[i].transform.GetChild(0).GetComponent<Range>().enemiesInRange.Remove(GetComponent<Unit>());
                    }
                }
            }
            GameManager.instance.CheckRoundFinish();
        }
    }
    public int maxHp;
    // public bool isDeath;
    public Slider hpSlider;
    [SerializeField] private int _hp;
    public int hp{
        get => _hp;
        set{
            _hp = value;
            hpSlider.value = value;
            if(_hp <= 0 )//&& !isDeath)
            {
                Death();
                // isDeath = true;
            }
        } 
    }    
    void Start()
    {
        maxHp = 100;
        hpSlider.maxValue = maxHp;
        hpSlider.value = maxHp;
        hp = maxHp;
    }
    
    // public void Death(Unit unit)
    // {
    //     if(unit.TryGetComponent(out Hero hero))
    //     {
    //         isDeath = true;
    //         GameManager.instance.livingHeros.Remove(unit);
    //         this.gameObject.SetActive(false);
    //     }
    //     else if(unit.TryGetComponent(out Minyonlar minyonlar))
    //     {
    //         GameManager.instance.minionsOnGround.Remove(unit);
    //         isDeath = true;
    //         this.gameObject.SetActive(false);
    //     }
    //     OnDeath?.Invoke();
    // }
    public void Death()
    {
        Destroy(this.gameObject);
    }

}
