using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Mime;
using System.Text;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class CSVReader
{

    public static List<MissionData> ReadMissionCSV()
    {
        List<MissionData> MissionDataList = new List<MissionData>();

        string filePath = "CSV/Mission/Mission";

        TextAsset data = Resources.Load<TextAsset>(filePath);

        string[] lines = data.text.Split('\n');

        for (int i = 1; i < lines.Length; i++)
        {
            if (!string.IsNullOrEmpty(lines[i]))
            {
                string[] row = lines[i].Split(',');

                MissionData MissionData = new MissionData
                {
                    ID = (row[0]),
                    Level = int.Parse(row[1]),
                    missionName = (row[2]),
                    goalType = (MissionType)Enum.Parse(typeof(MissionType), row[3]),
                    goal = int.Parse(row[4]),
                    reward = int.Parse(row[5])
                };

                MissionDataList.Add(MissionData);
            }
        }

        return MissionDataList;
    }

    public static List<SkillData> ReadSkillCSV()
    {
        List<SkillData> skillDataList = new List<SkillData>();

        string filePath = "CSV/Skill/Skill";

        TextAsset data = Resources.Load<TextAsset>(filePath);

        string[] lines = data.text.Split('\n');

        for (int i = 1; i < lines.Length; i++)
        {
            if (!string.IsNullOrEmpty(lines[i]))
            {
                string[] row = lines[i].Split(',');

                SkillData skillData = new SkillData
                {
                    ID = (row[0]),
                    needLevel = int.Parse((row[1])),
                    skillName = (row[2]),
                    skillDesc = (row[3]),
                    skillLevel = int.Parse(row[4]),
                    skillType = (SkillType)Enum.Parse(typeof(SkillType), row[5]),
                    skillValue = int.Parse(row[6]),
                    levelUpCost = int.Parse(row[7])
                };

                skillDataList.Add(skillData);
            }
        }

        return skillDataList;
    }

    //모든 원숭이 마스터 정보 읽어오기(ID -> code)
    public static MonkeyMaster ReadMonkeyMasterCSV(string UnitID)
    {
        //기본 정보
        string filePath = "CSV/Monkey/MonkeyMaster";
        
        TextAsset data = Resources.Load<TextAsset>(filePath);

        string modifiedText = data.text.Replace(".\n", "<br>");
        string[] lines = modifiedText.Split('\n');

        for (int i = 1; i < lines.Length; i++)
        {
            if (!string.IsNullOrEmpty(lines[i]))
            {
                string[] row = lines[i].Split(',');

                if(!row[0].Contains(UnitID)) continue;
                
                MonkeyMaster mMasterData = new MonkeyMaster
                {
                    Mon_Code = int.Parse(row[0]),
                    Mon_Name = row[1],
                    Mon_Type = int.Parse(row[2]),
                    Mon_Mtype = int.Parse(row[3]),
                    Mon_M_UP = int.Parse(row[4]),
                    Mon_Base = int.Parse(row[5]),
                    Mon_Up100 = int.Parse(row[6]),
                    Mon_Up150 = int.Parse(row[7]),
                    Mon_Up200 = int.Parse(row[8]),
                    Mon_Up250 = int.Parse(row[9]),
                    Mon_Up300 = int.Parse(row[10]),
                    Mon_Tier100 = int.Parse(row[11]),
                    Mon_Tier150 = int.Parse(row[12]),
                    Mon_Tier200 = int.Parse(row[13]),
                    Mon_Tier250 = int.Parse(row[14]),
                    Mon_Tier300 = int.Parse(row[15])
                };
                //레벨별 가격 정보
                mMasterData.Mon_M_Price = ReadMonkeyPriceCSV(mMasterData.Mon_Code);
                return mMasterData;
            }
        }
        return null;
    }
    
    //가격 정보만 읽어오기
    public static List<double> ReadMonkeyPriceCSV(int code)
    {
        List<double> priceDataList = new List<double>();

        string filePath = "CSV/Monkey/MonkeyUp";

        TextAsset data = Resources.Load<TextAsset>(filePath);
        
        string[] lines = data.text.Split('\n');
        
        //해당 이름의 가격 정보가 없을 경우
        if(code < 0)
        {
            Debug.LogError("해당 이름의 가격 정보가 없습니다.");
            return null;
        }

        for (int i = 1; i < lines.Length; i++)
        {
            if (!string.IsNullOrEmpty(lines[i]))
            {
                string[] row = lines[i].Split(',');

                if (string.IsNullOrEmpty(row[code]))
                {
                    continue;
                }
                
                //해당 이름의 가격 정보
                if (double.TryParse(row[code], out double priceData))
                {
                    priceDataList.Add(priceData);
                }
                else
                {
                    Debug.LogError($"가격 정보를 파싱하는 중 오류 발생: {lines[1].Split(',')[code]}, {code}, {i}");
                    priceDataList.Add(0); // 기본값 설정
                }
            }
        }
        return priceDataList;
    }
    
    public static BuildMaster ReadBuildMasterCSV(int BuildID)
    {
        string filePath = "CSV/Building/BuildingMaster";

        TextAsset data = Resources.Load<TextAsset>(filePath);

        string[] lines = data.text.Split('\n');

        for (int i = 1; i < lines.Length; i++)
        {
            if (!string.IsNullOrEmpty(lines[i]))
            {
                string[] row = lines[i].Split(',');

                if (BuildID != int.Parse(row[0]))
                {
                    continue;
                }
                
                BuildMaster bMasterData = new BuildMaster
                {
                    Build_Code = int.Parse(row[0]),
                    Build_Name = row[1],
                    Build_Type = int.Parse(row[2]),
                    Build_UpType = int.Parse(row[3]),
                    Build_Value = int.Parse(row[4]),
                    Build_Time = int.Parse(row[5]),
                    Build_Buff1 = int.Parse(row[6]),
                    Build_Buff2 = int.Parse(row[7]),
                    Build_Buff3 = int.Parse(row[8]),
                    Build_Buff4 = int.Parse(row[9]),
                    Build_Buff5 = int.Parse(row[10]),
                    Build_Buff6 = int.Parse(row[11]),
                    Build_Buff7 = int.Parse(row[12]),
                    Build_Buff8 = int.Parse(row[13]),
                    Build_Buff9 = int.Parse(row[14]),
                    Build_Buff10 = int.Parse(row[15])
                };
                bMasterData.Build_Price = ReadBuildingPriceCSV(bMasterData);

                return bMasterData;
            }
        }
        return null;
    }
    
    public static List<double> ReadBuildingPriceCSV(BuildMaster name)
    {
        List<double> priceDataList = new List<double>();

        string filePath = "CSV/Building/BuildUp";

        TextAsset data = Resources.Load<TextAsset>(filePath);
        
        if (data == null)
        {
            Debug.LogError($"파일을 찾을 수 없습니다: {filePath}");
            return null;
        }
        
        using (StreamReader reader = new StreamReader(new MemoryStream(data.bytes), Encoding.UTF8))
        {
            string[] lines = reader.ReadToEnd().Split('\n');

            if (lines.Length == 0)
            {
                Debug.LogError("CSV 파일이 비어 있습니다.");
                return null;
            }
            
            //이름으로 가격 찾으려는 첫번째 라인
            string[] priceLines = lines[0].Split(',');

            int priceIndex = -1;

            for (int i = 1; i < priceLines.Length; i++)
            {
                if (priceLines[i].Contains(name.Build_Name))
                {
                    //이름 인덱스
                    priceIndex = i;
                    break;
                }
            }

            //해당 이름의 가격 정보가 없을 경우
            if (priceIndex < 0)
            {
                Debug.LogError("해당 이름의 가격 정보가 없습니다.");
                
                priceDataList.Add(0);
                return priceDataList;
            }

            for (int i = 1; i < lines.Length; i++)
            {
                if (!string.IsNullOrEmpty(lines[i]))
                {
                    string[] row = lines[i].Split(',');

                    //해당 이름의 가격 정보
                    if (double.TryParse(row[priceIndex], out double priceData))
                    {
                        priceDataList.Add(priceData);
                    }
                    else
                    {
                        Debug.LogError($"가격 정보를 파싱하는 중 오류 발생: {row[priceIndex]}, {priceIndex}");
                        priceDataList.Add(0); // 기본값 설정
                    }
                }
            }
        }
        return priceDataList;
    }

    public static List<int> GetMonkeyHarvestOrder(int monkeyType)
    {
        List<int> orderDataList = new List<int>();

        string filePath;
        if (monkeyType == 1)
        {
            filePath = "CSV/Monkey/MonkeyHarvestOrder";
        }
        else
        {
            filePath = "CSV/Monkey/CarryMonkeyOrder";
        }
        
        TextAsset data = Resources.Load<TextAsset>(filePath);
        
        if (data == null)
        {
            Debug.LogError($"파일을 찾을 수 없습니다: {filePath}");
            return null;
        }
        
        using (StreamReader reader = new StreamReader(new MemoryStream(data.bytes), Encoding.UTF8))
        {
            string[] lines = reader.ReadToEnd().Split('\n');

            if (lines.Length == 0)
            {
                Debug.LogError("CSV 파일이 비어 있습니다.");
                return null;
            }
            
            string[] priceLines = lines[0].Split(',');

            for (int i = 0; i < priceLines.Length; i++)
            {
                orderDataList.Add(int.Parse(priceLines[i]));
            }
        }
        return orderDataList;
    }
    
    public static List<DecoData> GetDecoData()
    {
        List<DecoData> DecoDataList = new List<DecoData>();

        string filePath;
        filePath = "CSV/Deco/DecoData";
        
        TextAsset data = Resources.Load<TextAsset>(filePath);
        
        if (data == null)
        {
            Debug.LogError($"파일을 찾을 수 없습니다: {filePath}");
            return null;
        }

        using (StreamReader reader = new StreamReader(new MemoryStream(data.bytes), Encoding.UTF8))
        {
            string[] lines = reader.ReadToEnd().Split('\n');

            if (lines.Length == 0)
            {
                Debug.LogError("CSV 파일이 비어 있습니다.");
                return null;
            }

            for (int i = 1; i < lines.Length; i++)
            {
                string[] row = lines[i].Split(',');

                DecoData decoData = new DecoData()
                {
                    ID = row[0],
                    Deco_Location = row[1],
                    Deco_Name = (row[2]),
                    Desc = row[3],
                    CombineCard = int.Parse(row[4]),
                    AbilityType = int.Parse(row[5]),
                    AbilityValue = int.Parse(row[7]),
                    AbilityLevelValue = int.Parse(row[9]),
                    CardLevelValue = int.Parse(row[10]),
                    MaxLevel = int.Parse(row[11]),
                    CombinePrice = int.Parse(row[12]),
                    CardPrice = int.Parse(row[13])
                };
                DecoDataList.Add(decoData);
            }
        }
        return DecoDataList;
    }
    
    static public List<WindowMaster> GetWindowMaster(EventActionType type)
    {
        List<WindowMaster> stringMasterList = new List<WindowMaster>();

        string filePath = "CSV/Event/StringMaster";

        int[] indexes = {4, 5, 6, 8, 9};
        
        switch (type)
        {
            case EventActionType.Environment:
                filePath = "CSV/Event/Environment";
                break;
            case EventActionType.Post:
                filePath = "CSV/Event/Post/PostData";
                indexes = new[] {3, 4, 5, 7, 8};
                break;
            case EventActionType.MonkeyTalk:
                filePath = "CSV/Event/MonkeySudden/ChatMaster";
                indexes = new[] {4, 5, 6, 8, 9};
                break;
        }

        TextAsset data = Resources.Load<TextAsset>(filePath);

        string[] lines = data.text.Split('\n');

        for (int i = 1; i < lines.Length; i++)
        {
            if (!string.IsNullOrEmpty(lines[i]))
            {
                string[] row = lines[i].Split(',');

                WindowMaster stringMaster = new WindowMaster
                {
                    ChatCode = row[indexes[0]],
                    ChatText = row[indexes[1]],
                    ChatNext = row[indexes[2]],
                    ChatSound = row[indexes[3]],
                };
                
                if (int.TryParse(row[indexes[4]], out int reward))
                {
                    stringMaster.Reward = reward;
                }
                else
                {
                    stringMaster.Reward = 0;
                }

                stringMasterList.Add(stringMaster);
            }
        }

        return stringMasterList;
    }
    
    public static List<StringMaster> GetStringMaster(EventActionType type)
    {
        List<StringMaster> stringMasterList = new List<StringMaster>();

        string filePath = "CSV/Event/StringMaster";

        switch (type)
        {
            case EventActionType.Environment:
                filePath = "CSV/Event/Environment/EnvironmentString";
                break;
            case EventActionType.Post:
                filePath = "CSV/Event/Post/PostString";
                break;
            case EventActionType.MonkeyTalk:
                filePath = "CSV/Event/MonkeySudden/StringMaster";
                break;
        }
        
        TextAsset data = Resources.Load<TextAsset>(filePath);

        if (data == null)
        {
            Debug.LogError($"파일을 찾을 수 없습니다: {filePath}");
            return null;
        }
        
        string[] lines;
        
        if(type == EventActionType.Environment)
        {
            string temp = data.text.Replace("\r\n", "\n").Replace(".\n", ".").Replace("@\n", "").Replace("\r", " ").Replace("\n\r", " ");
            lines = temp.Split('\n');
        }
        else
        {
            lines = data.text.Split('\n');
        }
            

        for (int i = 1; i < lines.Length; i++)
        {
            if (!string.IsNullOrEmpty(lines[i]))
            {
                string[] row = lines[i].Split(',');

                StringMaster stringMaster = new StringMaster
                {
                    StringCode = row[3],
                    Desc = row[4]
                };

                stringMasterList.Add(stringMaster);
            }
        }

        return stringMasterList;
    }

    static public List<EnvironmentMaster> GetEnvironmentMasters()
    {
        List<EnvironmentMaster> environmentMasters = new List<EnvironmentMaster>();

        string filePath = "CSV/Event/Environment/EnvironmentWindow1";

        int[] indexes = {3, 4, 5, 7, 8, 9, 10, 11};
            
        TextAsset data = Resources.Load<TextAsset>(filePath);
    
        string modifiedText = data.text.Replace("\r\n", "\n").Replace(".\n", "");
    
        string[] lines = modifiedText.Split('\n');
        for (int i = 1; i < lines.Length; i++)
        {
            if (!string.IsNullOrEmpty(lines[i]))
            {
                string[] row = lines[i].Split(',');
                
                EnvironmentMaster stringMaster = new EnvironmentMaster
                {
                    ChatCode = row[indexes[0]],
                    ChatType = row[indexes[1]],
                    ChatText = row[indexes[2]],
                    ChatSuccessProbability = int.Parse(row[indexes[3]]),
                    ChatNextSeccess = row[indexes[4]],
                    ChatNextFailed = row[indexes[5]],
                    Sound = row[indexes[6]],
                    Reward = int.Parse(row[indexes[7]]),
                };
                    
                environmentMasters.Add(stringMaster);
            }
        }

        return environmentMasters;
    }
    
    public static List<Dialogue> GetDialogueData()
    {
        List<Dialogue> dialogueList = new List<Dialogue>();

        string filePath = "CSV/Event/MonkeySudden/Dialogue";

        TextAsset data = Resources.Load<TextAsset>(filePath);

        string[] lines = data.text.Split('\n');

        for (int i = 1; i < lines.Length; i++)
        {
            if (!string.IsNullOrEmpty(lines[i]))
            {
                string[] row = lines[i].Split(',');

                Dialogue dialogue = new Dialogue
                {
                    DialogueCode = row[0],
                    Result = row[1],
                    Rate = int.Parse(row[2])
                };

                dialogueList.Add(dialogue);
            }
        }

        return dialogueList;
    }    
    
    public static List<RewardMaster> GetRewardData(EventActionType type)
    {
        List<RewardMaster> RewardList = new List<RewardMaster>();

        string filename = "Chat_Reward";
        
        switch (type)
        {
            case EventActionType.Environment:
                filename = "Env_Reward";
                break;
            case EventActionType.MonkeyTalk:
                filename = "Chat_Reward";
                break;
        }
        
        string filePath = "CSV/Event/Environment/Reward/" + filename;

        TextAsset data = Resources.Load<TextAsset>(filePath);

        string[] lines = data.text.Split('\n');

        for (int i = 1; i < lines.Length; i++)
        {
            if (!string.IsNullOrEmpty(lines[i]))
            {
                string[] row = lines[i].Split(',');

                RewardMaster dialogue = new RewardMaster
                {
                    Code = int.Parse(row[0]),
                    Name = row[1],
                    Type = int.Parse(row[2]),
                    Value = int.Parse(row[3])
                };

                RewardList.Add(dialogue);
            }
        }

        return RewardList;
    }    
    
    public static List<Archivement> GetArchivementData()
    {
        List<Archivement> RewardList = new List<Archivement>();

        string filePath = "CSV/archivement/achievement";

        TextAsset data = Resources.Load<TextAsset>(filePath);

        string[] lines = data.text.Split('\n');

        for (int i = 1; i < lines.Length; i++)
        {
            if (!string.IsNullOrEmpty(lines[i]))
            {
                string[] row = lines[i].Split(',');

                Archivement dialogue = new Archivement
                {
                    ID = row[0],
                    Name = row[1],
                    Level = int.Parse(row[2]),
                    IconCode = row[3],
                    GoalType = int.Parse(row[4]),
                    GoalQuantity = int.Parse(row[5]),
                    RewardType = int.Parse(row[6]),
                    Reward = int.Parse(row[7])
                };

                RewardList.Add(dialogue);
            }
        }
        return RewardList;
    }    
    
    public static List<BuffMaster> GetBuffMaster()
    {
        List<BuffMaster> BuffList = new List<BuffMaster>();

        string filePath = "CSV/Buff/BuffMaster";

        TextAsset data = Resources.Load<TextAsset>(filePath);

        string[] lines = data.text.Split('\n');

        for (int i = 1; i < lines.Length; i++)
        {
            if (!string.IsNullOrEmpty(lines[i]))
            {
                string[] row = lines[i].Split(',');

                BuffMaster dialogue = new BuffMaster
                {
                    BuffCode = int.Parse(row[0]),
                    BuffName = row[1],
                    BuffType = int.Parse(row[2]),
                    BuffValueType = int.Parse(row[3]),
                    BuffUp = int.Parse(row[4])
                };

                BuffList.Add(dialogue);
            }
        }
        return BuffList;
    }
    
    public static List<CompanyValue> GetCompanyValueData()
    {
        List<CompanyValue> RewardList = new List<CompanyValue>();

        string filePath = "CSV/CompanyValue/companyValue";

        TextAsset data = Resources.Load<TextAsset>(filePath);

        string[] lines = data.text.Split('\n');

        for (int i = 1; i < lines.Length; i++)
        {
            if (!string.IsNullOrEmpty(lines[i]))
            {
                string[] row = lines[i].Split(',');

                CompanyValue dialogue = new CompanyValue
                {
                    ID = int.Parse(row[0]),
                    Title = row[1],
                    IconCode = row[2],
                };

                RewardList.Add(dialogue);
            }
        }
        return RewardList;
    }
    
}



//원숭이 CSV 마스터
public class MonkeyMaster
{
    //인덱스로 이용
    public int Mon_Code{ get; set; }
    public string Mon_Name{ get; set; }
    
    //1수확, 2운반
    public int Mon_Type{ get; set; }
    
    //재화 타입 1바나나, 2골드(몽달라)
    public int Mon_Mtype{ get; set; }
 
    //강화 배율
    public int Mon_M_UP{ get; set; }
    
    //레벨별 가격
    public List<double> Mon_M_Price{ get; set; }
    
    public int Mon_Base { get; set; }
    
    public int Mon_Up100 { get; set; }
    public int Mon_Up150 { get; set; }
    public int Mon_Up200 { get; set; }
    public int Mon_Up250 { get; set; }
    public int Mon_Up300 { get; set; }
    
    //승급
    public int Mon_Tier100 { get; set; }
    public int Mon_Tier150 { get; set; }
    public int Mon_Tier200 { get; set; }
    public int Mon_Tier250 { get; set; }
    public int Mon_Tier300 { get; set; }
}

public class BuildMaster
{
    public int Build_Code{ get; set; }
    public string Build_Name{ get; set; }
    public int Build_Type{ get; set; }
    
    public int Build_UpType{ get; set; }
    
    public int Build_Value{ get; set; }
    
    public int Build_Time{ get; set; }
    
    //버프 코드
    public int Build_Buff1{ get; set; }
    public int Build_Buff2{ get; set; }
    public int Build_Buff3{ get; set; }
    public int Build_Buff4{ get; set; }
    public int Build_Buff5{ get; set; }
    public int Build_Buff6{ get; set; }
    public int Build_Buff7{ get; set; }
    public int Build_Buff8{ get; set; }
    public int Build_Buff9{ get; set; }
    public int Build_Buff10{ get; set; }
    
    
    public List<double> Build_Price{ get; set; }

}

    public class StorageData
    {
        public int Level { get; set; }
        public int Max { get; set; }
        public float Price { get; set; }
    }

    public interface IBuildingData
    {
        public int Level { get; set; }
        public float Price { get; set; }
    }

    public class ProductorData : IBuildingData
    {
        public int Level { get; set; }

        public float monkeyDollarperSecond { get; set; }
        public float Price { get; set; }
        public uint BananaDecrease { get; set; }
        public float SellValue { get; set; }


    }

    public class MarketData : IBuildingData
    {
        public int Level { get; set; }
        public int Max { get; set; }
        public float Price { get; set; }
    }

    public class BuildingData : IBuildingData
    {
        public int Level { get; set; }
        public int MonkeyPerSecond { get; set; }
        public float Price { get; set; }
    }

    public class MissionData
    {
        public string ID { get; set; }
        public int Level { get; set; }
        public string missionName { get; set; }
        public MissionType goalType { get; set; }
        public int goal { get; set; }
        public int reward { get; set; }
    }

    public class SkillData
    {
        public string ID { get; set; }
        
        public int needLevel { get; set; }
        public string skillName { get; set; }
        public string skillDesc { get; set; }
        public int skillLevel { get; set; }
        public SkillType skillType { get; set; }
        public int skillValue { get; set; }
        public int levelUpCost { get; set; }
    }

    public class DecoData
    {
        public string ID { get; set; }
        public string Deco_Location { get; set; }
        public string Deco_Name { get; set; }
        public string Desc { get; set; }
        public int CombineCard { get; set; }
        public int AbilityType { get; set; }
        public int AbilityValue { get; set; }
        public int AbilityLevelValue { get; set; }
        public int CardLevelValue { get; set; }
        public int MaxLevel { get; set; }
        public int CombinePrice { get; set; }
        public int CardPrice { get; set; }
    }

    public class StringMaster
    {
        public string StringCode { get; set; }
        public string Desc { get; set; }
    }

    public class WindowMaster
    {
        //Chat_Code	Chat_Text	Chat_Next	스트링 내용 표시 ( 데이터로 사용x)	Sound	Reward
        public string ChatCode { get; set; }
        
        public string ChatText { get; set; }
        
        public string ChatNext { get; set; }
        
        public string ChatSound { get; set; }
        
        public int  Reward { get; set; }
    }

    public class EnvironmentMaster
    {
        public string ChatCode { get; set; }
        
        public string ChatType { get; set; }
        
        public string ChatText { get; set; }
        
        //다음 대화문 성공확률 챗타입이 2일때만 사용
        public int ChatSuccessProbability { get; set; }
        
        //성공이 나왔을 경우 이어지는 대화문
        public string ChatNextSeccess { get; set; }
        
        //실패가 나왔을 경우 이어지는 대화문
        public string ChatNextFailed { get; set; }
        
        public string Sound  { get; set; }
        
        public int  Reward { get; set; }
    } 

    public class Dialogue
    {
        public string DialogueCode { get; set; }
        
        public string Result { get; set; }
        
        public int Rate { get; set; }
    }

    public class RewardMaster
    {
        public int Code { get; set; }
        public string Name { get; set; }
        public int Type { get; set; }
        public int Value { get; set; }
    }

    public class MonkeyInfo
    {
        public int Code { get; set; }
        public string Task { get; set; }//역할 1수확 2 운반
        public int Type { get; set; }//등급 1일반 2스페셜
        public int Name { get; set; }//이름
        public int CostType { get; set; }//가격 타입 1바나나 2다이아
        public int Cost { get; set; }//가격
        public int GetType { get; set; }//생산재화 1바나나 2골드
        public int HoldEffectType { get; set; }//유닛 보유 효과
        public int HoldEffectValue { get; set; }//유닛 보유 효과 값
        public int MonkeyMaxLv { get; set; }//최대 레벨
    }
    public class Archivement
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public int Level { get; set; }
        public string IconCode { get; set; }
        public int GoalType { get; set; }
        public int GoalQuantity { get; set; }
        public int RewardType { get; set; }
        public int Reward { get; set; }
    }

    public class CompanyValue
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string IconCode { get; set; }
    }
    
    public class BuffMaster
    {
        public int BuffCode { get; set; }
        public string BuffName { get; set; }
        public int BuffType { get; set; }
        public int BuffValueType { get; set; }
        public int BuffUp { get; set; }
    }