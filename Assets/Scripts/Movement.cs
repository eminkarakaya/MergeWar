using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Movement : MonoBehaviour
{
    Animator animator;
    public List<Unit> allEnemies;
    Unit unit;
    public NavMeshAgent agent;
    public Unit target;
    public float speed = 2;
    void Start()
    {
        unit = GetComponent<Unit>();
        agent = GetComponent<NavMeshAgent>();
        animator = unit.animator;
        
    }
    public void Move()
    {
        agent.SetDestination(target.gameObject.transform.position);
    }
    void Update()
    {
        if(GameManager.instance.gameStage == GameStage.inGame)
        {
            if(unit.state == State.walk)
            {
                if(unit.animator!= null)
                    unit.animator.SetBool("Move",true);
                agent.speed = speed;
                NearestEnemy();
                if(target == null)
                {
                    return;
                }
               // animator.SetBool("Move" , true);
                speed = 10;
                Move();
            }
            else
            {
                if(unit.animator!= null)
                    unit.animator.SetBool("Move",false);
                speed = 0;
               // animator.SetBool("Move" , false);
            }
            agent.speed = speed;
        }
    }
    public void NearestEnemy()
    {
        List<Unit> allEnemies;
        if(unit.side == Side.ally)
            allEnemies = GameManager.instance.allEnemies;
        else
            allEnemies = GameManager.instance.allAlly;

        this.allEnemies = allEnemies;
        if(allEnemies.Count == 0)
        {
            return;
        }
        var nearestEnemy = allEnemies[0];
        var uzaklik = Vector3.Distance(this.transform.position , allEnemies[0].transform.position);
        for (int i = 1; i < allEnemies.Count; i++)
        {
            if(Vector3.Distance(this.transform.position , allEnemies[i].transform.position) < uzaklik)
            {
                uzaklik = Vector3.Distance(this.transform.position , allEnemies[i].transform.position);
                nearestEnemy = allEnemies[i];
            }
        }
        target = nearestEnemy;
    }
}
