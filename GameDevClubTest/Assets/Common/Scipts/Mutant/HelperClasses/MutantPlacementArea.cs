using UnityEngine;
using Zenject;

namespace Assets.Common.Scipts.Mutant.HelperClasses
{
    public class MutantPlacementArea : MonoBehaviour
    {
        public float Radius;

        [Inject]
        void Construct(MutantGenerationService mutantPositionGeneration)
        {
            mutantPositionGeneration.SetBaseParameters(transform.position, Radius);
        }
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, Radius);
        }
    }
}
