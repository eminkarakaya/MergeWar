using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

    public enum GameStage
    {
        inGame,
        hazirlik
    }
public class GameManager : MonoBehaviour,IDataPersistence
{
    [HideInInspector] public bool oyunBittimi;
    public List<Grid> allGrids;
    public List<Unit> allEnemies;
    public List<Grid> ourGrids;
    public List<Unit> allAlly;
    public List<Grid> enemiesGrids;
    public GameObject winCanvas;
    public GameObject loseCanvas;
    float distanceFactor = 100;
    float radius =200;
    public Text WinLoseText;
    public Text EarnedGoldText;
    public Color allyColor;
    public Color enemyColor;
    public GameObject buyPanel;
    public int level;
    public RectTransform goldinScene;
    public Ease ease;
    public Transform parent;
    public int earnedGold;
    public GameObject goldPrefab;
    public GameStage gameStage;
    private static GameManager _instance;
    public static GameManager instance{get =>_instance;}
    public GameObject meleePrefab;
    public GameObject rangePrefab;
    int defaultRangeCost = 45;
    int defaultMeleeCost = 30;
    int rangeBuyCount;
    int meleeBuyCount;
    public Text meleeCostText;
    public Text rangeCostText;
    public Text goldText;
    public GameObject rangeAdBtn;
    public GameObject meleeAdBtn;
    [SerializeField] private int _gold;
    public int gold{ get => _gold;
        set{
            _gold = value;
            goldText.text = _gold.ToString();
        }
    }
    void Awake()
    {
        _instance = this;
    }
    void Start()
    {
        meleeCostText.text = CalcMeleeCost().ToString();
        rangeCostText.text = CalcRangeCost().ToString();
        CheckGold();
        LoadLevel();
        for (int i = 0; i < allGrids.Count; i++)
        {
            if(allGrids[i].index > 15)
                enemiesGrids.Add(allGrids[i]);
        }
        SetAllEnemies();
    }
    IEnumerator Dance(List<Unit> winnerTeam, GameObject canvas ,  bool win)
    {
        for (int i = 0; i < winnerTeam.Count; i++)
        {
            winnerTeam[i].Dance();    
            winnerTeam[i].GetComponent<Movement>().speed = 0;        
        }
        yield return new WaitForSeconds(2f);
        WinLoseText.gameObject.SetActive(true);
        if(win)
        {
            WinLoseText.text = "WIN";
        }
        else
        {
            WinLoseText.text = "LOSE";
            earnedGold = earnedGold/2;
        }
        EarnedGoldText.gameObject.SetActive(true);
        EarnedGoldText.text = earnedGold.ToString();

        
        canvas.SetActive(true);
    }
    public void CheckRoundFinish()
    {
        if(!oyunBittimi)
        {
            if(allAlly.Count == 0)
            {
                StartCoroutine(Dance(allEnemies,loseCanvas,false));
                gameStage = GameStage.hazirlik;
                oyunBittimi = true;
            }
            else if(allEnemies.Count == 0)
            {
                StartCoroutine(Dance(allAlly,winCanvas,true));
                level++;
                gameStage = GameStage.hazirlik;
                oyunBittimi = true;
            }
        }
        
    }
    public int CalcMeleeCost()
    {
        return (defaultMeleeCost*meleeBuyCount) + defaultMeleeCost;
    }
    public int CalcRangeCost()
    {
        return (defaultRangeCost*rangeBuyCount) + defaultRangeCost;
    }
    public void LoadData(GameData data)
    {
        gold = data.gold;
        level = data.level;
        meleeBuyCount = data.meleeBuyCount;
        rangeBuyCount = data.rangeBuyCount;
    }
    public void SaveData(ref GameData data)
    {
        data.gold = gold;
        data.level = level;
        data.meleeBuyCount = meleeBuyCount;
        data.rangeBuyCount = rangeBuyCount;
    }
    Grid FindRandomEmptyGrid()
    {
        List<Grid> tempList = new List<Grid>();
        for (int i = 0; i < ourGrids.Count; i++)
        {
            if(ourGrids[i].heroOnGround == null)
            {
                tempList.Add(ourGrids[i]);
            }
        }
        if(tempList.Count == 0)
            return null;
        return tempList[Random.Range(0,tempList.Count)];
    }
    public void BuyRangeHero()
    {
        if(gameStage == GameStage.hazirlik && gold >= CalcRangeCost())
        {
            gold-= CalcRangeCost();
            rangeBuyCount++;
            rangeCostText.text = CalcRangeCost().ToString();
            var grid = FindRandomEmptyGrid();
            if(grid == null)
                return;
            var obj = Instantiate(rangePrefab,Vector3.zero,Quaternion.identity);
            SelectManager.instance.SahaIcıBosYereHeroKoy(grid,obj.transform.GetChild(0).GetComponent<Unit>());   
            obj.transform.position = grid.transform.position;
        }
        CheckGold();
    }
    public void BuyMeleeHero()
    {
        if(gameStage == GameStage.hazirlik && gold >= CalcMeleeCost())
        {
            gold -= CalcMeleeCost();
            meleeBuyCount++;
            meleeCostText.text = CalcMeleeCost().ToString();
            var grid = FindRandomEmptyGrid();
            if(grid == null)
                return;
            var obj = Instantiate(meleePrefab,Vector3.zero,Quaternion.identity);
            SelectManager.instance.SahaIcıBosYereHeroKoy(grid,obj.transform.GetChild(0).GetComponent<Unit>());   
            obj.transform.position = grid.transform.position;
        }
        CheckGold();
    }
    public void StartBattleBtn()
    {
        buyPanel.SetActive(false);
        SetAllEnemies();
        for (int i = 0; i < allGrids.Count; i++)
        {
            if(allGrids[i].heroOnGround != null)
            {
                allGrids[i].heroOnGround.GetComponent<Health>().hpSlider.gameObject.SetActive(true);
                allGrids[i].heroOnGround.typeImageCanvas.SetActive(false);
                allGrids[i].heroOnGround.agent.enabled = true;
            }
        }
        gameStage = GameStage.inGame;
    }
    void SetGold(int count)
    {
        gold += count;
        goldText.text = gold.ToString();
    }
    public void NoThanksBtn()
    {
        ClearArea();
        LoadLevel();
        StartCoroutine(EarnGoldAnim());
        WinLoseText.gameObject.SetActive(false);
        EarnedGoldText.gameObject.SetActive(false);
        oyunBittimi = false;
    }
    public void LoadLevel()
    {
        Vector3 rot;
        for (int i = 0; i < LevelManager.instance.levels[level].units.Count; i++)
        {
            Vector3 pos = Vector3.zero;
            var obj = Instantiate(LevelManager.instance.levels[level].units[i].unit,pos,Quaternion.identity);
            var unit = obj.transform.GetChild(0).GetComponent<Unit>();
            unit.GetComponent<Health>().hpSlider.gameObject.SetActive(false);
            for (int j = 0; j < allGrids.Count; j++)
            {
                if(allGrids[j].index == LevelManager.instance.levels[level].units[i].index)
                {
                    unit.whichGrid = allGrids[j];
                    allGrids[j].heroOnGround = unit;
                    pos = allGrids[j].transform.position;
                    unit.index = LevelManager.instance.levels[level].units[i].index;
                    break;
                }
            }
            if(unit.index > 15)
            {
                obj.tag = "Untagged";
                unit.gameObject.tag = "Untagged";
                unit.side = Side.enemy;
                unit.typeImageBackground.color = enemyColor;
                rot = new Vector3(0,180,0); 
            }
            else
            {
                unit.side = Side.ally;
                rot = Vector3.zero;
            }
            obj.transform.rotation = Quaternion.Euler(rot);
            obj.transform.position = pos;
        }
    }
    public void ClearArea()
    {
        for (int i = 0; i < allGrids.Count; i++)
        {
            if(allGrids[i].heroOnGround != null)
            {
                Destroy(allGrids[i].heroOnGround.gameObject);
                allGrids[i].heroOnGround = null;
            }

        }
    }
    public IEnumerator EarnGoldAnim()
    {
        var count = 15;
        var gold = earnedGold / count;
        List<GameObject> list = new List<GameObject>();
        for (int i = 0; i < count; i++)
        {
            var obj = Instantiate(goldPrefab,parent.transform.position,Quaternion.identity, parent);
            list.Add(obj);
        }
        for (int i = 0; i < count; i++)
        {
            var x = distanceFactor * Mathf.Sqrt(i) * Mathf.Cos(i*radius);
            var y = distanceFactor * Mathf.Sqrt(i) * Mathf.Sin(i*radius);
            var newPos = new Vector3(x,y,0) + parent.transform.position;
            list[i].transform.DOMove(newPos,.2f);
        }
        for (int i = 0; i < count; i++)
        {
            list[i].transform.DOMove(goldinScene.transform.position,.7f).SetEase(ease).OnComplete(()=> SetGold(gold));//.OnComplete(()=> goldFlare.Play());
            yield return new WaitForSeconds(0.15f);
        }
        yield return new WaitForSeconds(.7f);
        for (int i = 0; i < count; i++)
        {
            Destroy(list[i]);
        }
        goldText.transform.DOScale(Vector3.one *1.5f,.4f).OnComplete(()=>goldText.transform.DOScale(Vector3.one,.4f));
        yield return new WaitForSeconds(.8f);
        winCanvas.SetActive(false);    
        buyPanel.SetActive(true);       
        // var obj1 = Instantiate(goldPrefab,parent.transform.position,Quaternion.identity, parent);
        // gold = gameManager.earnedGold%100;
        // obj1.transform.DOMove(goldinScene.transform.position,.7f).SetEase(ease).OnComplete(()=> Destroy(obj1.gameObject)).OnComplete(()=> SetGold(gold));

    }
    public void SetAllEnemies()
    {
        allEnemies.Clear();
        allAlly.Clear();
        for (int i = 0; i < enemiesGrids.Count; i++)
        {
            if(enemiesGrids[i].heroOnGround != null)
            {
                allEnemies.Add(enemiesGrids[i].heroOnGround);
            }
        }
        for (int i = 0; i < ourGrids.Count; i++)
        {
            if(ourGrids[i].heroOnGround != null)
            {
                allAlly.Add(ourGrids[i].heroOnGround);
            }
        }
    }
    public void WatchAds()
    {
        StartCoroutine(EarnGoldAnim());
    }
    void CheckGold()
    {
        if(CalcMeleeCost() > gold)
        {
            meleeCostText.gameObject.transform.parent.gameObject.SetActive(false);
            meleeAdBtn.gameObject.SetActive(true);
        }
        if(CalcRangeCost() > gold)
        {
            rangeCostText.gameObject.transform.parent.gameObject.SetActive(false);
            rangeAdBtn.gameObject.SetActive(true);
        }
    }
}
