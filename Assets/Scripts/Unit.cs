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
    Tank,
    Artillery
}
public class Unit : MonoBehaviour
{
    [SerializeField] GameObject enemyMarkObject;
    public AudioSource audioSource;
    public AudioClip mergeSound;
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
    void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        
    }
    void Start()
    {        
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        if(side == Side.ally)
        {
            enemyMarkObject.SetActive(false);
        }
    }
    public void Dance()
    {
        animator.SetTrigger("Dance");
    }
}