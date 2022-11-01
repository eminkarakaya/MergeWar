using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class ArcherAttack : Attack
{
    [SerializeField] GameObject effect;
    [SerializeField] Arrow arrow;
    [SerializeField] Transform hand;
    float distance;
    
    void Update()
    {
        if(GameManager.instance.gameStage == GameStage.inGame)
        {
            if(rangeClass.enemiesInRange.Count > 0)
            {
                target = rangeClass.enemiesInRange[0];
                if(target != null)
                    transform.DOLookAt(target.transform.position,.5f);
                unit.state = State.attack;
                unit.animator.SetBool("Attack", true);
                
                if(TryGetComponent(out Movement movement))
                    movement.agent.speed = 0;
            }
            else
            {
                unit.state = State.walk;
                target = null;
                unit.animator.SetBool("Attack",false);
            }   
        }
    }
    void Fire()
    {
        if(target != null)
        {
            if(effect != null)
            {
                Instantiate(effect,hand.position,Quaternion.identity);
            }
            if(sound != null)
                audioSource.PlayOneShot(sound);
            distance= Vector3.Distance(transform.position,target.transform.position);
            var arrowObj = Instantiate(arrow,hand.position,Quaternion.identity);
            arrowObj.rangeAttack = this;
            arrowObj.target = target;
            arrowObj.distance = distance;
            arrowObj.Fly();
        }
    }
}
