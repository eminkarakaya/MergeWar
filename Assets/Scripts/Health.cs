using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField] protected float deathTime;
    public float expMultiplier = 1;
    public Vector3 dir;
    Animator animator;
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
                Death(deathTime);
                // isDeath = true;
            }
        } 
    }    
    void OnDisable()
    {
        DisableChar();
    }
    
    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
        hpSlider.maxValue = maxHp;
        hpSlider.value = maxHp;
        hp = maxHp;
    }
    void DisableChar()
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
    protected virtual void Death(float time)
    {
        hpSlider.gameObject.SetActive(false);
        animator.enabled = false;
        DisableChar();
        Destroy(this.gameObject,time);
    }

}
