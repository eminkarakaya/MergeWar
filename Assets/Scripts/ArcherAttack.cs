using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class ArcherAttack : RangeAttack
{
    protected override void Start()
    {
        base.Start();
    }
    void Update()
    {
        if(GameManager.instance.gameStage == GameStage.inGame)
        {
            if(rangeClass.enemiesInRange.Count > 0)
            {
                target = rangeClass.enemiesInRange[0];
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
}
