using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Common.Scipts.Weapon
{    
    public class BulletMove : MonoBehaviour
    {
        private Rigidbody2D RigidbodyButton;
        private float speed = 50f;

        private void Start()
        {
            RigidbodyButton = GetComponent<Rigidbody2D>();
            RigidbodyButton.velocity = transform.right * speed;

        }
    }
}
