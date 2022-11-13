using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : Attack
{
    void Update()
    {
        if(GameManager.instance.gameStage == GameStage.inGame)
        {
            if(rangeClass.enemiesInRange.Count > 0)
            {
                if(target == null)
                {
                    target = FindNearestObject(rangeClass.enemiesInRange);
                }
                if(target != null)
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
