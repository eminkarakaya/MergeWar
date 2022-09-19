using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class DataPersistanceManager : MonoBehaviour
{
    [Header("File Stroage Config")]
    [SerializeField] private string fileName;
    private FileDataHandler dataHandler;
    GameData gameData;
    public static DataPersistanceManager instance {get; private set;}

    private List<IDataPersistence> dataPersistenceObjects;
    void Awake()
    {
        instance = this;
        this.dataHandler = new FileDataHandler(Application.persistentDataPath,fileName);
        this.dataPersistenceObjects = FindAllIDataPersistenceObjects();
        Debug.Log("load");
        LoadGame();
    
    }
    public void NewGame()
    {
        this.gameData = new GameData();
    }
    public void LoadGame()
    {
        this.gameData = dataHandler.Load();
        if(this.gameData == null)
        {
            Debug.Log("Data bulunamadı");
            NewGame();
        }

        foreach (IDataPersistence dataPersistanceObject in dataPersistenceObjects)
        {
            dataPersistanceObject.LoadData(gameData);
        }
    }
    public void SaveGame()
    {
        foreach (IDataPersistence dataPersistanceObject in dataPersistenceObjects)
        {
            dataPersistanceObject.SaveData(ref gameData);
        }
        Debug.Log("saved");
        dataHandler.Save(gameData);
    }
    void OnApplicationQuit()
    {
        SaveGame();
    }
    List<IDataPersistence> FindAllIDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();
        return new List<IDataPersistence>(dataPersistenceObjects);
    }
}
