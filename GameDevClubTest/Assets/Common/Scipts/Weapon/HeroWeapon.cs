using Assets.Common.Scipts.HeroInventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Common.Scipts.Weapon
{
    public class HeroWeapon
    {
        public event Action OnChangeBulletCount;
        private InventorySlotVisualElement _bulletSlot;
        private int countBullet;
        public int CountBullet
        {
            get
            {
                return countBullet;
            }
            set
            {
                countBullet = value;
                OnChangeBulletCount?.Invoke();
            }
        }

        public void InitBullet(InventorySlotVisualElement bulletSlot)
        {
            _bulletSlot = bulletSlot;
            CountBullet = _bulletSlot.Count;
        }
        public void SubstractBullet()
        {
            _bulletSlot.SubstractCount(1);;
            CountBullet = _bulletSlot.Count;
        }
        public void AddBullet(int count)
        {
            _bulletSlot.SetCount(count);
            CountBullet = _bulletSlot.Count;
        }
    }
}
