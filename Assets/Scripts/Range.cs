using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Range : MonoBehaviour
{
    public Attack parentAttack;
    public List<Unit> enemiesInRange = new List<Unit>();
    SphereCollider rangeCollider;
    void Awake()
    {
        parentAttack = GetComponentInParent<Attack>();
        rangeCollider = GetComponent<SphereCollider>();
        rangeCollider.radius = parentAttack.range;
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out Unit unit))
        {
            if(unit.side != parentAttack.GetComponent<Unit>().side)
                enemiesInRange.Add(unit);
        }
    }
    void OnTriggerExit(Collider other)
    {
        if(other.gameObject.TryGetComponent(out Unit unit))
        {
            if(unit.side != parentAttack.GetComponent<Unit>().side)
                enemiesInRange.Remove(unit);
        }
    }
}
