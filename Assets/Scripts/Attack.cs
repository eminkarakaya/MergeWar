using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Attack : MonoBehaviour
{
    public bool attack;
    Animator animator;
    [SerializeField] protected Range rangeClass;
    protected Unit unit;
    [SerializeField] protected int _damage;
    public int damage{
        get => _damage;
        set{
            _damage = value;
        }
    }
    public float range = 1;
    [SerializeField] protected Unit target;
    protected virtual void Start()
    {
        rangeClass = transform.GetChild(0).GetComponent<Range>();
        unit = GetComponent<Unit>();
        animator = unit.animator;
    }
    protected void Attackk()
    {
        unit.state = State.attack;
        target.GetComponent<Health>().hp -= _damage;
        this.transform.DOLookAt(target.transform.position,.5f);
        unit.animator.SetBool("Attack",true);
    }
}
