using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeAttack : Attack
{
    
    public Arrow arrow;
    public Transform hand;
    float distance;
    
    protected void Fire()
    {
        if(target != null)
        {
            distance= Vector3.Distance(transform.position,target.transform.position);
            var arrowObj = Instantiate(arrow,hand.position,Quaternion.identity);
            arrowObj.rangeAttack = this;
            arrowObj.target = target;
            arrowObj.distance = distance;
            arrowObj.Fly();
        }
    }
    void Update()
    {
        if(GameManager.instance.gameStage == GameStage.inGame)
        {
            if(rangeClass.enemiesInRange.Count > 0)
            {
                if(attack)
                {
                    target = rangeClass.enemiesInRange[0];
                    transform.LookAt(target.transform);
                    unit.state = State.attack;
                    Fire();
                    if(TryGetComponent(out Movement movement))
                        movement.agent.speed = 0;
                }
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
