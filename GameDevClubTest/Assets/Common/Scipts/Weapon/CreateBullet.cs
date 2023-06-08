using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Common.Scipts.Weapon
{
    public class CreateBullet : MonoBehaviour
    {
        private string path = "Prefabs/Bullet";

        public void LoadBulletPrefab()
        {
            var prefab = Resources.Load(path); ;
            Instantiate(prefab,transform.position,transform.rotation);
        }
    }
}
