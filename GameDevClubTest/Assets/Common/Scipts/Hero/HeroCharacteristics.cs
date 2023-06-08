using Assets.Common.Scipts.CommonForHeroAndMutantsScripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Common.Scipts.Hero
{
    [Serializable]
    public class HeroCharacteristics : CommonCharacteristics
    {
        public Transform HeroTransform;
        public TransformInfo transformInfo;
        private HealthBar _healthBar;
        [NonSerialized]
        private Image image;
        public HeroCharacteristics(Transform heroTransform, Image image)
        {
            HeroTransform = heroTransform;
            this.image = image;
            
        }
        public void InitHealth()
        {
            _healthBar = new HealthBar(health, maxHealth, image);
        }
        public void TakingDamage(int damage)
        {
            _healthBar.UpdateFillAmount(damage);
            health -= damage;
        }
        public TransformInfo ConvertTransformToTransformInfo()
        {
            return new TransformInfo(HeroTransform);
        }
        public void ConvertTransformInfoToTransform(TransformInfo transformInfo)
        {
            HeroTransform.position = transformInfo.pos;
            HeroTransform.rotation = transformInfo.rot;
            HeroTransform.localScale = transformInfo.scale;
        }
    }
    [Serializable]
    public class TransformInfo
    {
        public Vector3 pos;
        public Quaternion rot;
        public Vector3 scale;
        public TransformInfo(Transform transform)
        {
            pos = transform.position;
            rot = transform.rotation;
            scale = transform.localScale;
        }
    }
}
