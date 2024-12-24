using UnityEngine;

namespace Environment
{
    public class WaterTriggerHandler : MonoBehaviour
    {
        [SerializeField] private LayerMask waterMask;
        [SerializeField] private GameObject splashParticles;

        private EdgeCollider2D _edgeColl;
        private InteractableWater _water;

        private void Awake()
        {
            _edgeColl = GetComponent<EdgeCollider2D>();
            _water = GetComponent<InteractableWater>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if ((waterMask.value & (1 << collision.gameObject.layer)) <= 0) return;
            var rb = collision.GetComponent<Rigidbody2D>();
            if (rb == null) return;
            
            var localPos = this.transform.localPosition;
            Vector2 hitObjectPos = collision.transform.position;
            var hitObjectBounds = collision.bounds;

            Vector3 spawnPos;
            if (collision.transform.position.y >= _edgeColl.points[1].y + _edgeColl.offset.y + localPos.y)
                spawnPos = hitObjectPos - new Vector2(0, hitObjectBounds.extents.y);
            else
                spawnPos = hitObjectPos + new Vector2(0, hitObjectBounds.extents.y);
                    
            Instantiate(splashParticles, spawnPos, Quaternion.identity);

            var multiplier = rb.velocity.y < 0 ? -1 : 1;

            var vel = rb.velocity.y * _water.ForceMultiplier;
            vel = Mathf.Clamp(Mathf.Abs(vel), 0, _water.MaxForce);
            vel *= multiplier;
                    
            _water.Splash(collision, vel);
        }
    }
}
