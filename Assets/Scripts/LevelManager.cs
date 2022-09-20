using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour //, IDataPersistence
{
    private static LevelManager _instance;

    public static LevelManager instance{get =>_instance;}
    [System.Serializable] public struct Levels
    {
        public List<Units> units;
    }
    [System.Serializable] public struct Units{
        public int index;
        public GameObject unit;
    }
    public List<Levels> levels;
    void Awake()
    {
        _instance = this;
    }
}
