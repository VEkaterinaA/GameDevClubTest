using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public InventoryController _inventoryController;
        private HeroController _heroController;

        private bool IsHeroAttack = false;
        [Inject]
        private void Contruct(HeroController heroController)
        {
            _heroController = heroController;
        }
        private void Awake()
        {
            Subscribe();

            shootButton.SetActive(IsHeroAttack);
            var ShootButton = shootButton.GetComponentInChildren<Button>();
            ShootButton.onClick.AddListener(ShootButtonOnClick);

            _inventoryController.m_Root.style.display = DisplayStyle.None;
            inventoryButton.onClick.AddListener(UseInventoryOnClick);
        }
        private void Subscribe()
        {
            _heroController.OnCollisionHeroFieldWithEnemy += AttackButtonStateChange;
            _heroController.OnHeroDeath += OpenWindowGameOver;
            _inventoryController.OnDropBullet += (int countBullet) => UpdateCountBulletLabel(countBullet);
        }

        private void UpdateCountBulletLabel(int countBullet)
        {
            textBulletCount.text = countBullet.ToString();
        }

        private void AttackButtonStateChange()
        {
            IsHeroAttack = !IsHeroAttack;
            shootButton.SetActive(IsHeroAttack);
        }

        void UseInventoryOnClick()
        {
            _inventoryController.m_Root.style.display = DisplayStyle.Flex;
        }
        void ShootButtonOnClick()
        {
            _heroController.ClickShootButton();
        }
        void OpenWindowGameOver()
        {

        }

    }
}
