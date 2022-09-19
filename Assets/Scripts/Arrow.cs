using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using DG.Tweening.Core;

public class Arrow : MonoBehaviour
{

    public Attack rangeAttack;
    public Ease ease;
    public Unit target;
    public float distance; 
    void DestroyArrow()
    {
        if(target != null)
            target.GetComponent<Health>().hp -= rangeAttack.damage;
        Destroy(this.gameObject,1f);
    }
    public void Fly()
    {
        transform.DOMove(target.hitPos.transform.position,distance*.01f).SetEase(ease).OnComplete(()=>DestroyArrow());
        transform.LookAt(target.transform);
    }
}
