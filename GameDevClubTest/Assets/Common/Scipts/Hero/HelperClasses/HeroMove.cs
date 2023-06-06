using UnityEngine;

namespace Assets.Common.Scipts.Hero.HelperClasses
{
    public class HeroMove
    {
        public void JoystickCoordinateUpdate(Joystick joystick, float speed, out float dirX, out float dirY)
        {
            dirX = joystick.Horizontal * speed;
            dirY = joystick.Vertical * speed;
        }
        public Vector2 MotionVector(float dirX, float dirY)
        {
            return new Vector2(dirX, dirY);
        }
    }
}
