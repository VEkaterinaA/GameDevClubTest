using Assets.Common.Scipts.CommonForHeroAndMutantsScripts;
using System;
using UnityEngine.UI;

namespace Assets.Common.Scipts.Mutant.HelperClasses
{
    public class MutantHealthManagement
    {
        private MutantCharacteristics _characteristics;
        private HealthBar _healthBar;
        public event Action OnMutantDie;
        public MutantHealthManagement(MutantCharacteristics mutantCharacteristics, Image barImage)
        {
            _characteristics = mutantCharacteristics;
            _healthBar = new HealthBar(mutantCharacteristics.health, mutantCharacteristics.health, barImage);
        }
        public void TakeDamage(int damage)
        {
            _characteristics.health -= damage;
            _healthBar.UpdateFillAmount(damage);
            if (_characteristics.health <= 0)
            {
                OnMutantDie?.Invoke();
            }
        }
    }
}
