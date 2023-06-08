using Assets.Common.Scipts.Weapon;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Zenject;
using Button = UnityEngine.UI.Button;

namespace Assets.Common.Scipts
{
    public class UserInterfaceController : MonoBehaviour
    {
        private FileOperations _data;
        private HeroController _heroController;
        public InventoryController _inventoryController;

        private bool IsHeroAttack = false;
        private bool IsHeroKilled = false;
        private bool IsPaused = false;

        private string FileInventoryPath;
        private string FileInventoryDataName = "InventoryData.json";
        private string FileHeroCharacteristicsPath;
        private string FileHeroCharacteristicsDataName = "HeroCharacteristicsData.json";


        public Button inventoryButton;
        public GameObject shootButton;
        public Text textBulletCount;
        public GameObject GameOverWindow;
        public Button RestartButton;

        [Inject]
        private void Contruct(HeroController heroController, FileOperations data)
        {
            _heroController = heroController;
            _data = data;
        }
        private void Awake()
        {
            Subscribe();

            shootButton.SetActive(IsHeroAttack);
            var ShootButton = shootButton.GetComponentInChildren<Button>();
            ShootButton.onClick.AddListener(ShootButtonOnClick);

            _inventoryController.InventoryVisualRoot.style.display = DisplayStyle.None;
            inventoryButton.onClick.AddListener(UseInventoryOnClick);

        }
        private void Subscribe()
        {
            _heroController.OnCollisionHeroFieldWithEnemy += AttackButtonStateChange;
            _heroController.OnHeroDeath += OpenWindowGameOver;
            _inventoryController._heroWeapon.OnChangeBulletCount += UpdateCountBulletLabel;
        }

        private void UpdateCountBulletLabel()
        {
            textBulletCount.text = _inventoryController._heroWeapon.CountBullet.ToString();
        }

        private void AttackButtonStateChange()
        {
            IsHeroAttack = !IsHeroAttack;
            shootButton.SetActive(IsHeroAttack);
        }

        void UseInventoryOnClick()
        {
            _inventoryController.InventoryVisualRoot.style.display = DisplayStyle.Flex;
        }
        void ShootButtonOnClick()
        {
            _heroController.Shoot();
        }
        void OpenWindowGameOver()
        {
            PauseGame(true, 0);
            IsHeroKilled = true;
            GameOverWindow.SetActive(true);
            RestartButton.onClick.AddListener(RestartOnClick);
        }
        private void RestartOnClick()
        {
            DeleteFiles();
            SceneManager.LoadScene(0);
            PauseGame(false,1);
                }
        private void DeleteFiles()
        {
#if PLATFORM_ANDROID && !UNITY_EDITOR
            FileHeroCharacteristicsPath = Path.Combine(Application.persistentDataPath, FileHeroCharacteristicsDataName);
            FileInventoryPath = Path.Combine(Application.persistentDataPath, FileInventoryDataName);
#else
            FileHeroCharacteristicsPath = Path.Combine(Application.dataPath, FileHeroCharacteristicsDataName);
            FileInventoryPath = Path.Combine(Application.dataPath, FileInventoryDataName);
#endif
            FileUtil.DeleteFileOrDirectory(FileHeroCharacteristicsPath);
            FileUtil.DeleteFileOrDirectory(FileInventoryPath);

        }
        private void OnApplicationQuit()
        {
            if(IsHeroKilled)
            {
                DeleteFiles();
                return;
            }
            _data.SaveToFile(_inventoryController.slots, _heroController._heroCharacteristics);
        }
        private void OnApplicationPause(bool pause)
        {
            if(pause)
            {
                PauseGame(true, 0);
            }
            if (!pause)
            {
                PauseGame(false,1);
            }
        }

        private void PauseGame(bool pause, float timeScale)
        {
            if(timeScale!=0 && timeScale!=1)
            {
                return;
            }
            IsPaused = pause;
            Time.timeScale = timeScale;
        }
    }
}
