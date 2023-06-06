using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Assets.Common.Scipts.Mutant.HelperClasses
{
    public class MutantPlacementArea : MonoBehaviour
    {
        public float Radius;

        [Inject]
        void Construct(MutantPositionGeneration mutantPositionGeneration)
        {
            mutantPositionGeneration.startPoint = transform.position;
            mutantPositionGeneration.radius = Radius;
        }
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, Radius);
        }
    }
}
