using UnityEngine;

namespace Assets.Common.Scipts.Mutant
{
    public class MutantTurn
    {
        public void RotationMutantRelativeToMovement(Vector3 velocity,Transform transform, Vector3 walkPoint)
        {
            if (velocity.x < 0)
            {
                transform.rotation = GetQuaternion(transform,180);
            }
            else
            {
                transform.rotation = GetQuaternion(transform,0);
            }

        }
        private Quaternion GetQuaternion(Transform transform,int corner)
        {
            Quaternion rot = transform.rotation;
            rot.y = corner;
            return rot;

        }

    }
}

