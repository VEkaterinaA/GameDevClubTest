using Assets.Common.Scipts.CommonForHeroAndMutantsScripts;
using Assets.Common.Scipts.Hero;
using Assets.Common.Scipts.HeroInventory;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Assets.Common.Scipts
{
    public class FileOperations
    {
        private string saveInventoryPath;
        private string saveInventoryDataName = "InventoryData.json";
        private string saveHeroCharacteristicsPath;
        private string saveHeroCharacteristicsDataName = "HeroCharacteristicsData.json";

        public FileOperations()
        {
#if PLATFORM_ANDROID && !UNITY_EDITOR
            saveHeroCharacteristicsPath = Path.Combine(Application.persistentDataPath, saveHeroCharacteristicsDataName);
            saveInventoryPath = Path.Combine(Application.persistentDataPath, saveInventoryDataName);
#else
            saveHeroCharacteristicsPath = Path.Combine(Application.dataPath, saveHeroCharacteristicsDataName);
            saveInventoryPath = Path.Combine(Application.dataPath, saveInventoryDataName);
#endif
        }
        public void SaveToFile(List<InventorySlotVisualElement> slots, HeroCharacteristics heroCharacteristics)
        {
            SaveHeroCharacteristics(heroCharacteristics);
            SaveInventory(slots);

        }
        private void SaveHeroCharacteristics(HeroCharacteristics heroCharacteristics)
        {
            HeroCharacteristicsSaveStruct heroCharacteristicsSaveStruct = new HeroCharacteristicsSaveStruct
            {
                transform = heroCharacteristics.ConvertTransformToTransformInfo(),
                health = heroCharacteristics.health,
                damage = heroCharacteristics.damage,
                maxHealth = heroCharacteristics.maxHealth,
                speed = heroCharacteristics.speed,
            };


            string json = JsonUtility.ToJson(heroCharacteristicsSaveStruct, true);
            try
            {
                File.WriteAllText(saveHeroCharacteristicsPath, json);

            }
            catch (Exception e)
            {

                Debug.Log(e.Message);
            }
        }
        private void SaveInventory(List<InventorySlotVisualElement> slots)
        {
            InventorySaveStruct inventorySaveStruct = new InventorySaveStruct
            {
                slots = slots.Where(u=>u.IsEmpty==false).ToArray()
            };
            string json = JsonUtility.ToJson(inventorySaveStruct, true);
            try
            {
                File.WriteAllText(saveInventoryPath, json);
            }
            catch (Exception e)
            {

                Debug.Log(e.Message);
            }

        }
        
        public void LoadHeroCharacteristics(HeroCharacteristics heroCharacteristics)
        {
            if (!File.Exists(saveHeroCharacteristicsPath))
            {
                Debug.Log($"File {saveHeroCharacteristicsPath} not found");
                heroCharacteristics.health = 100;
                heroCharacteristics.maxHealth = 100;
                heroCharacteristics.damage = 20;
                heroCharacteristics.speed = 3f;
                return;
            }
            try
            {
                string json = File.ReadAllText(saveHeroCharacteristicsPath);

                HeroCharacteristicsSaveStruct heroCharacterJson = JsonUtility.FromJson<HeroCharacteristicsSaveStruct>(json);
                heroCharacteristics.health = heroCharacterJson.health;
                heroCharacteristics.maxHealth = heroCharacterJson.maxHealth;
                heroCharacteristics.speed = heroCharacterJson.speed;
                heroCharacteristics.damage = heroCharacterJson.damage;
                heroCharacteristics.ConvertTransformInfoToTransform(heroCharacterJson.transform);
            }
            catch (Exception e)
            {
                Debug.Log("LoadHeroCharacteristics error: " + e.Message);
            }

        }
        public void LoadInventory(List<InventorySlotVisualElement> slots)
        {
            if (!File.Exists(saveInventoryPath))
            {
                Debug.Log($"File {saveInventoryPath} not found");

                return;
            }
            try
            {
                string json = File.ReadAllText(saveInventoryPath);
                InventorySaveStruct slotsJson = JsonUtility.FromJson<InventorySaveStruct>(json);
                for (int i = 0; i < slots.Count - 1; i++)
                {
                    var slot = slots[i];
                    var slotJson = slotsJson.slots[i];
                    if(slotJson.item==null)
                    {
                        return;
                    }
                    slot.SetItem(slotJson.item, slotJson.Count);
                }
            }
            catch (Exception e)
            {
                Debug.Log("LoadInventory error: " + e.Message);
            }
        }
    }


    [Serializable]
    public struct InventorySaveStruct
    {
        public InventorySlotVisualElement[] slots;
    }
    [Serializable]
    public struct HeroCharacteristicsSaveStruct
    {
        public TransformInfo transform;
        public int health;
        public int maxHealth;
        public int damage;
        public float speed;
    }

}
