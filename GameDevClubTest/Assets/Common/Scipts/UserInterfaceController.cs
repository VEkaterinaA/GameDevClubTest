using Assets.Common.Scipts.Weapon;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Zenject;
using Button = UnityEngine.UI.Button;

namespace Assets.Common.Scipts
{
    public class UserInterfaceController : MonoBehaviour
    {
        public Button inventoryButton;
        public GameObject shootButton;
        public Text textBulletCount;
        private FileOperations _data;
        public InventoryController _inventoryController;
        private HeroController _heroController;

        private bool IsHeroAttack = false;
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

        }
        private void OnApplicationQuit()
        {
            _data.SaveToFile(_inventoryController.slots, _heroController._heroCharacteristics);
        }

    }
}
