using System;
using UnityEngine;

namespace Particle
{
    public class WaterTriggerHandler : MonoBehaviour
    {
        [SerializeField] private LayerMask _waterMask;
        [SerializeField] private GameObject _splashParticles;

        private EdgeCollider2D _edgeColl;
        private InteractableWater _water;

        private void Awake()
        {
            _edgeColl = GetComponent<EdgeCollider2D>();
            _water = GetComponent<InteractableWater>();
        }

        private void OnTriggerEnter2D(Collider2D collision){
            if ((_waterMask.value & (1 << collision.gameObject.layer)) > 0)
            {
                var rb = collision.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    var localPos = this.transform.localPosition;
                    Vector2 hitObjectPos = collision.transform.position;
                    var hitObjectBounds = collision.bounds;

                    var spawnPos = Vector3.zero;
                    if (collision.transform.position.y >= _edgeColl.points[1].y + _edgeColl.offset.y + localPos.y)
                    {
                        spawnPos = hitObjectPos - new Vector2(0, hitObjectBounds.extents.y);
                    }
                    else
                    {
                        spawnPos = hitObjectPos + new Vector2(0, hitObjectBounds.extents.y);
                    }
                    
                    Instantiate(_splashParticles, spawnPos, Quaternion.identity);

                    var multiplier = rb.velocity.y < 0 ? -1 : 1;

                    var vel = rb.velocity.y * _water.ForceMultiplier;
                    vel = Mathf.Clamp(Mathf.Abs(vel), 0, _water.MaxForce);
                    vel *= multiplier;
                    
                    _water.Splash(collision, vel);
                }
            }
            
        }
    }
}
