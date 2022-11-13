using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Attack : MonoBehaviour
{
    [SerializeField] protected AudioSource audioSource;
    [SerializeField] protected AudioClip sound;
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
        audioSource = this.gameObject.AddComponent<AudioSource>();
        audioSource.volume = .1f;
        audioSource.clip = sound;
        rangeClass = transform.GetChild(0).GetComponent<Range>();
        unit = GetComponent<Unit>();
        animator = unit.animator;
    }
    protected void Attackk()
    {
        unit.state = State.attack;
        if(target!= null)
        {
            if(sound != null)
                audioSource.Play();
            target.GetComponent<Health>().hp -= _damage;
            this.transform.DOLookAt(target.transform.position,.5f);
        }
        unit.animator.SetBool("Attack",true);
    }
    protected Unit FindNearestObject(List<Unit> list)
    {
        if (list.Count == 0)
            return null;
        var distance = Vector3.Distance(list[0].transform.position, this.transform.position);
        var nearestObject = list[0];
        for (int i = 0; i < list.Count; i++)
        {
            if (Vector3.Distance(this.transform.position, list[i].transform.position) < distance)
            {
                nearestObject = list[i];
                distance = Vector3.Distance(this.transform.position, list[i].transform.position);
            }
        }
        return nearestObject;
    }
}
