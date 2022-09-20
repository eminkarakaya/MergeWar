using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Arrow : MonoBehaviour
{
    [SerializeField] float jumpPower;
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
        transform.DOJump(target.hitPos.transform.position,jumpPower,1,distance*.01f).SetEase(ease).OnComplete(()=>DestroyArrow());
        transform.LookAt(target.transform);
    }
}
