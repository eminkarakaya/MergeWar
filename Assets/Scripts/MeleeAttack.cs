using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : Attack
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
                transform.LookAt(target.transform);
                unit.state = State.attack;
                unit.animator.SetBool("Attack",true);
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