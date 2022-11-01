using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Arrow : MonoBehaviour
{
    [SerializeField] LayerMask layerMask;
    [SerializeField] bool isMissile;
    [SerializeField] GameObject particle;
    [SerializeField] float jumpPower;
    public Attack rangeAttack;
    public Ease ease;
    public Unit target;
    public float distance; 
    Vector3 hitpos;
    [SerializeField] float power;
    [SerializeField] float expRange;
    void Explosion()
    {
        hitpos = target.hitPos.position;
        Collider [] colliders = Physics.OverlapSphere(hitpos,expRange,layerMask);
        foreach (var item in colliders)
        {
            if(item.TryGetComponent(out Health health))
            {
                health.expMultiplier = power;
                health.hp -= rangeAttack.damage;
            }
            Debug.Log(item + " qwe");
            if(item.TryGetComponent(out Unit unit))
            {
                // if(unit.isFlyable)
                // {
                    // var rb = item.GetComponent<Rigidbody>();
                    // if(rb != null)
                    // {
                    //     Vector3 expPos = new Vector3(hitpos.x,hitpos.y-1f,hitpos.z);
                    //     rb.AddExplosionForce(power,expPos,expRange);
                    // }
                // }
            }
            
        }
        var exp = Instantiate(particle,hitpos,Quaternion.identity);
        exp.transform.SetParent(this.transform);
        DestroyArrow();
    }
    void DestroyArrow()
    {
        if(target != null)
            target.GetComponent<Health>().hp -= rangeAttack.damage;
        Destroy(this.gameObject,1f);
    }
    public void Fly()
    {
        if(isMissile)
        {
            transform.DOJump(target.hitPos.transform.position,jumpPower,1,distance*.01f).SetEase(ease).OnComplete(()=>Explosion());
            transform.LookAt(target.transform);
        }
        transform.DOJump(target.hitPos.transform.position,jumpPower,1,distance*.01f).SetEase(ease).OnComplete(()=>DestroyArrow());
        transform.LookAt(target.transform);
    }
}
