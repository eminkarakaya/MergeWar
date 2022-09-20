using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public enum State
{
    walk,
    attack
}
public enum Side
{
    enemy,
    ally
}
public enum UnitType{
    Swordsman,
    Archer,
    Cannon,
    Knight,
    Tank
}
public class Unit : MonoBehaviour
{
    
    public GameObject typeImageCanvas;
    public Image typeImageBackground;
    public Transform hitPos;
    public int index;
    public GameObject nextLevelUnitPrefab;
    public UnitType unitType;
    public Grid whichGrid;
    [HideInInspector] public Animator animator;
    public Side side;
    public State state;
    [HideInInspector] public NavMeshAgent agent;

    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }
    public void Dance()
    {
        animator.SetTrigger("Dance");
    }
}