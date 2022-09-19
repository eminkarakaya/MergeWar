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
}
