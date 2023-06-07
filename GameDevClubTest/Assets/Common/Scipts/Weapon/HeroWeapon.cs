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
        public int CountBullet
        {
            get
            {
                return _bulletSlot.Count;
            }
            set
            {
                _bulletSlot.Count = value;
                OnChangeBulletCount?.Invoke();
            }
        }

        public void InitBullet(InventorySlotVisualElement bulletSlot)
        {
            _bulletSlot = bulletSlot;
        }
        public void SubstractBullet()
        {
            _bulletSlot.SubstractCount(1);;
        }
        public void AddBullet(int count)
        {
            _bulletSlot.SetCount(count);
        }
    }
}
