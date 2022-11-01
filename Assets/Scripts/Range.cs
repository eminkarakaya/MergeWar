using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Range : MonoBehaviour
{
    Attack parentAttack;
    public List<Unit> enemiesInRange = new List<Unit>();
    [SerializeField] SphereCollider rangeCollider;
    [SerializeField] bool isColliderActive;
    void Awake()
    {
        parentAttack = GetComponentInParent<Attack>();
        rangeCollider = GetComponent<SphereCollider>();
        rangeCollider.radius = parentAttack.range;
    }
    void Update()
    {
        if(isColliderActive)
            return;
        if(GameManager.instance.gameStage == GameStage.inGame)
        {
            GetComponent<Collider>().enabled = true;
            isColliderActive = true;
        }
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
