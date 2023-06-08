using UnityEngine;

namespace Assets.Common.Scipts.Mutant.HelperClasses
{
    public class MutantControl
    {
        public float startMutantPosX;
        public Transform transform;
        public void RotationMutantRelativeToMovement(Vector3 velocity)
        {
            if (velocity.x < 0)
            {
                transform.rotation = GetQuaternion(transform, 180);
            }
            else
            {
                transform.rotation = GetQuaternion(transform, 0);
            }

        }
        private Quaternion GetQuaternion(Transform transform, int corner)
        {
            Quaternion rot = transform.rotation;
            rot.y = corner;
            return rot;

        }

    }
}

