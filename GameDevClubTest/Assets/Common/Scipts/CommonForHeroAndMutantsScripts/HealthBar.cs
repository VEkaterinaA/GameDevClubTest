using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UI;

namespace Assets.Common.Scipts.CommonForHeroAndMutantsScripts
{
    [Serializable]
    public class HealthBar
    {
        [NonSerialized]
        private float healthPercent;
        [NonSerialized]
        private Image Bar;
        [NonSerialized]
        private int health;
        private int maxHealth;
        public HealthBar(int health, int maxHealth, Image image)
        {
            Bar = image;
            this.health = health;
            this.maxHealth = maxHealth;
            healthPercent = health*100/maxHealth;
            if(healthPercent!=100)
            {
                Bar.fillAmount = healthPercent / 100;
            }
        }
        public void UpdateFillAmount(int damage)
        {
            health -= damage;
            healthPercent = health * 100 / maxHealth;
            Bar.fillAmount = healthPercent / 100;
        }
    }
}
