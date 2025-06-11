using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public enum MonkeyKind
{
    Harvester,
    PickUp,
    Carry
}

// 원숭이를 관리하는 싱글톤 클래스
public class MonkeyManager : MonoBehaviour
{
    private static MonkeyManager instance;

    public static MonkeyManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameObject("MonkeyManager").AddComponent<MonkeyManager>();
            }
            return instance;
        }
    }

    public List<MonkeyParent> monkeys = new List<MonkeyParent>();
    public Dictionary<string, AnimationClip> HarvestmonkeyAnimations = new Dictionary<string, AnimationClip>();
    public Dictionary<string, AnimationClip> CarrymonkeyAnimations = new Dictionary<string, AnimationClip>();
    public Dictionary<string, AnimationClip> PickUpmonkeyAnimations = new Dictionary<string, AnimationClip>();

    public Transform HarvestMonkeyPosition;
    public Transform RollMonkeyPosition;
    public Transform LogMonkeyPosition;
    public Transform PlantMonkeyPosition;
    public Transform CloudMonkeyPosition;
    public Transform GollemMonkeyPosition;
    
    
    public Transform PickUpMonkeyPosition;
    public Transform CarryingMonkeyPosition;
    public Transform FlyMonkeyPosition;

    public GameObject HarvestMonkeyPrefab;
    public GameObject PickUpMonkeyPrefab;
    public GameObject CarryMonkeyPrefab;

    public Dictionary<string, MonkeyParent> monkeyDic = new Dictionary<string, MonkeyParent>();

    public Action OnHarvestMonkeyLevelUp;
    public Action OnHarvestMonkeyCollect;
    
    public Action OnCarryMonkeyLevelUp;
    public Action OnCarryMonkeyCollect;
    
    public List<MonkeyMaster> monkeyMasters = new List<MonkeyMaster>();
    
    public HarvestContainer harvestContainer;

    public Transform summonPos;
    
    public HarvestContainer fxContainer;
    
    
    //영구 상승 능력치
    //속도
    public float SpeedUp = 0;
    
    //운반량
    public int PowUp = 0;
    
    //수확량
    public int HarvestPowUp = 0;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {

        ShopManager.OnUnitPurchase += PurchaseCallBack;

        DataContainerSetID[] unitContainerSets =
        {
            DataContainerSetID.CarryingUnit,
            DataContainerSetID.HarvestingUnit,
            DataContainerSetID.PickUpUnit
        };
        for(int i = 0; i < unitContainerSets.Length; i++)
        {
            for (int j = 0; j < DataContainer.Instance[unitContainerSets[i]].Count; j++)
            {
                monkeyMasters.Add(CSVReader.ReadMonkeyMasterCSV((DataContainer.Instance[unitContainerSets[i]][j] as UnitContaner).code.ToString()));
            }
        }
        
        LoadAnimationClips();
        LoadMonkeyData();
    }


    private void LoadAnimationClips()
    {
        LoadAnimationClipsForPrefab(HarvestMonkeyPrefab, HarvestmonkeyAnimations);
        LoadAnimationClipsForPrefab(CarryMonkeyPrefab, CarrymonkeyAnimations);
        LoadAnimationClipsForPrefab(PickUpMonkeyPrefab, PickUpmonkeyAnimations);
    }

    private void LoadAnimationClipsForPrefab(GameObject prefab, Dictionary<string, AnimationClip> animationDict)
    {
        if (prefab == null) return;

        var clips = prefab.GetComponent<Animator>().runtimeAnimatorController.animationClips;
        foreach (var clip in clips)
        {
            if (!animationDict.ContainsKey(clip.name))
            {
                animationDict.Add(clip.name, clip);
            }
        }
    }

    private void LoadMonkeyData()
    {
        DataContainerSetID[] unitContainerSets =
        {
            DataContainerSetID.CarryingUnit,
            DataContainerSetID.HarvestingUnit,
            DataContainerSetID.PickUpUnit
        };

        foreach (var setId in unitContainerSets)
        {
            var itemSetContainer = DataContainer.Instance[setId];
            
            for (int i = 0; i < itemSetContainer.Count; i++)
            {
                int level = DataManager.GetUnitLevel(setId, itemSetContainer[i].ID);
                if (level != -1)
                {
                    CreateMonkey(setId, itemSetContainer[i].ID, level);
                }
            }
        }
    }

    public void AddMonkey(MonkeyParent monkey)
    {
        if (monkey == null) return;

        monkeys.Add(monkey);
        monkeyDic[monkey.ID] = monkey;
        
        //ChallengesManager.instance.SetChallengeLevel("Unit_Count", monkeys.Count);
    }

    public void RemoveMonkey(MonkeyParent monkey)
    {
        if (monkey == null) return;

        monkeys.Remove(monkey);
        monkeyDic.Remove(monkey.ID);
    }

    public MonkeyParent FindMonkey(string ID)
    {
        monkeyDic.TryGetValue(ID, out var monkey);
        return monkey;
    }

    public MonkeyParent CreateMonkey(DataContainerSetID setID, string unitID, int level)
    {
        MonkeyParent tempMonkey;
        GameObject prefab;
        Transform position;

                int code = int.Parse(unitID);
        switch (setID)
        {
            case DataContainerSetID.HarvestingUnit:
                position = HarvestMonkeyPosition;
                
                prefab = harvestContainer[code].prefab ?? HarvestMonkeyPrefab;
                
                if(code == 7 || code == 8 || code == 9)
                {
                    position = PlantMonkeyPosition;
                } 
                if(code == 10 || code == 11 || code == 12)
                {
                    position = CloudMonkeyPosition;
                } 
                if(code == 13 || code == 14 || code == 15)
                {
                    position = GollemMonkeyPosition;
                }
                if(code == 1 || code == 2 || code == 3)
                {
                    position = RollMonkeyPosition;
                }
                if(code == 4 || code == 5 || code == 6)
                {
                    position = LogMonkeyPosition;
                }
                
                tempMonkey = Instantiate(prefab, position.position, Quaternion.identity).GetComponent<MonkeyParent>();
                
                break;
            case DataContainerSetID.PickUpUnit:
                prefab = PickUpMonkeyPrefab;
                position = PickUpMonkeyPosition;
                tempMonkey = Instantiate(prefab, position.position, Quaternion.identity).GetComponent<PickUpMonkey>();
                
                tempMonkey.eventMark.eventID = code;
                tempMonkey.eventMark.eventSprite = (DataContainer.Instance[setID][code.ToString()] as UnitContaner)[0].Thumbnail;
                MonkeyEventManager.instance.idleEvents.Add(tempMonkey.eventMark);
                break;
            case DataContainerSetID.CarryingUnit:
                prefab = CarryMonkeyPrefab;
                position = CarryingMonkeyPosition;
                tempMonkey = Instantiate(prefab, position.position, Quaternion.identity).GetComponent<CarryingMonkey>();
                tempMonkey.eventMark.eventID = monkeyMasters.Find(x => x.Mon_Name == unitID).Mon_Code;
                tempMonkey.eventMark.eventSprite = (DataContainer.Instance[setID][code.ToString()] as UnitContaner)[0].Thumbnail;
                MonkeyEventManager.instance.idleEvents.Add(tempMonkey.eventMark);
                break;
            default:
                return null;
        }

        SetMonkeyInfo(tempMonkey, setID, unitID, level);
        
        if (setID == DataContainerSetID.HarvestingUnit)
        {
            BananaManager.instance.harvestMonkeys.Add(tempMonkey);
            BananaManager.instance.UpdateBananaPerSecond();
        }
        else if (setID == DataContainerSetID.CarryingUnit || setID == DataContainerSetID.PickUpUnit)
        {
            MonkeyDollorManager.instance.transportMonkeys.Add(tempMonkey);
            MonkeyDollorManager.instance.UpdateMonkeyDollorPerSecond();
        }
        
        AddMonkey(tempMonkey);
        
        return tempMonkey;
    }

    private void PurchaseCallBack(DataContainerSetID setID, string unitID, int level)
    {
        var monkey = FindMonkey(DataContainer.Instance[setID][unitID].ID);
        
        if (monkey == null)
        {
            MonkeyParent tmep = CreateMonkey(setID, unitID, level);
            
        }
        else
        {
            SetMonkeyInfo(monkey, setID, unitID, level);
            
        }
        
        CheckMonkeyLevel();
    }

    private void SetMonkeyInfo(MonkeyParent monkey, DataContainerSetID setID, string unitID, int level)
    {
        if (monkey == null) return;

        UnitContaner unitContainer = DataContainer.Instance[setID][unitID] as UnitContaner;
        if (unitContainer == null) return;

        monkey.gameObject.name = unitContainer[0].Name ;
        //monkey.spriteRenderer.sprite = unitContainer[0].Thumbnail ?? monkey.spriteRenderer.sprite;

        List<SecondarySpriteTexture> secondarySpriteTextures = new List<SecondarySpriteTexture>();
        monkey.spriteRenderer.sprite.GetSecondaryTextures(secondarySpriteTextures.ToArray());

        //노말맵이 있을때만 적용
        foreach (var secondarySprite in secondarySpriteTextures)
        {
            if (secondarySprite.name == "_MaskTex")
            {
                monkey.spriteRenderer.sortingLayerName = "Monkey";
            }
        }
        
        monkey.MonCode = int.Parse(unitID);
        monkey.spriteRenderer.sortingOrder = 300 + monkey.MonCode;
        monkey.ID = DataContainer.Instance[setID][unitID].ID;
        monkey.Level = level;
        
        monkey.Speed = 10;
        
        //증가치
        if(monkey.Level < 99)
            monkey.pow = (monkeyMasters.Find(x => x.Mon_Code == int.Parse(unitID)).Mon_Up100 * (level+1d));
        else if(monkey.Level < 149)
            monkey.pow = (monkeyMasters.Find(x => x.Mon_Code == int.Parse(unitID)).Mon_Up150 * (level+1d));
         else if(monkey.Level < 199)
             monkey.pow = (monkeyMasters.Find(x => x.Mon_Code == int.Parse(unitID)).Mon_Up200 * (level+1d));    
         else if(monkey.Level < 249)
             monkey.pow = (monkeyMasters.Find(x => x.Mon_Code == int.Parse(unitID)).Mon_Up250 * (level+1d)); 
         else if(monkey.Level < 299)
             monkey.pow = (monkeyMasters.Find(x => x.Mon_Code == int.Parse(unitID)).Mon_Up300 * (level+1d)); 
        
        //베이스
        monkey.basepow = monkeyMasters.Find(x => x.Mon_Code == int.Parse(unitID)).Mon_Base;
        
        monkey.output = monkey.basepow * (1 + monkey.pow / 100d);
        
        if (monkey.MonCode == 28 ||monkey.MonCode == 29||monkey.MonCode == 30)
        {
            monkey.transform.position = FlyMonkeyPosition.position;
        }
        
        if(monkey.MonCode == 10 || monkey.MonCode == 11 || monkey.MonCode == 12)
        {
            monkey.transform.position = new Vector3(monkey.transform.position.x, 0, monkey.transform.position.z);
        }
        
        if (setID == DataContainerSetID.HarvestingUnit)
        {
            for (int i = 0; i < BananaManager.instance.harvestMonkeys.Count; i++)
            {
                if (BananaManager.instance.harvestMonkeys[i].MonCode == monkey.MonCode)
                {
                    BananaManager.instance.harvestMonkeys[i] = monkey;
                }
            }
            BananaManager.instance.UpdateBananaPerSecond();
        }
        else if (setID == DataContainerSetID.CarryingUnit || setID == DataContainerSetID.PickUpUnit)
        {
            OnCarryMonkeyLevelUp?.Invoke();
            for (int i = 0; i < MonkeyDollorManager.instance.transportMonkeys.Count; i++)
            {
                if (MonkeyDollorManager.instance.transportMonkeys[i].MonCode == monkey.MonCode)
                {
                    MonkeyDollorManager.instance.transportMonkeys[i] = monkey;
                }
            }
            MonkeyDollorManager.instance.UpdateMonkeyDollorPerSecond();
        }

        SetAnimation(monkey);
    }

    public void CheckMonkeyLevel()
    {
        Dictionary<string, int> monkeyLevelCounts = new Dictionary<string, int>()
        {
            {"Unit_LV10", 0},
            {"Unit_LV20", 0},
            {"Unit_LV40", 0},
            {"Unit_LV70", 0},
            {"Unit_LV100", 0}
        };

        foreach (var monkey in monkeys)
        {
            if (monkey.Level+2 >= 10) monkeyLevelCounts["Unit_LV10"]++;
            if (monkey.Level+2 >= 20) monkeyLevelCounts["Unit_LV20"]++;
            if (monkey.Level+2 >= 40) monkeyLevelCounts["Unit_LV40"]++;
            if (monkey.Level+2 >= 70) monkeyLevelCounts["Unit_LV70"]++;
            if (monkey.Level+2 >= 100) monkeyLevelCounts["Unit_LV100"]++;
        }

        foreach (var levelCount in monkeyLevelCounts)
        {
        }
    }

    private bool HasAnimation(Animator animator, string animationName)
    {
        foreach (var clip in animator.runtimeAnimatorController.animationClips)
        {
            Debug.Log(clip.name);
            if (clip.name == animationName)
            {
                return true;
            }
        }
        
        return false;
    }
    
    public void SetAnimation(MonkeyParent monkey)
    {
        if (monkey == null) return;

        // if(HasAnimation(monkey.animator, monkey.MonCode.ToString()))
        // {
        //     Debug.Log(monkey.MonCode.ToString());
        //     monkey.animator.Play(monkey.MonCode.ToString());
        // }
        int stateID = Animator.StringToHash(monkey.MonCode.ToString());

        if (monkey.animator.HasState(0, stateID))
        {
            monkey.animator.Play(monkey.MonCode.ToString());
        }
        
    }
    
    //환생스킬로 인한 원숭이 속도 증가
    public void SpeedUpEnforce(float value)
    {
        SpeedUp = value;
    }
    
    public void PowUpEnforce(int value, MonkeyKind kind)
    {
        foreach (var monkey in monkeys)
        {
            switch (monkey.kind)
            {
                case MonkeyKind.Harvester:
                    HarvestPowUp = value;
                    break;
                case MonkeyKind.Carry:
                    PowUp = value;
                    break;
                case MonkeyKind.PickUp:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(kind), kind, null);
            }
        }
    }
    
}