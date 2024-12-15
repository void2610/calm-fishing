using System.Collections.Generic;
using UnityEngine;

public class GunParticle : MonoBehaviour
{
    [SerializeField] private GameObject explosionPrefab;

    private ParticleSystem gun;
    private readonly List<ParticleCollisionEvent> collisionEvents = new();
    
    private void Start()
    {
        gun = this.GetComponent<ParticleSystem>();
        gun.Play();
    }

    private void OnParticleCollision(GameObject other)
    {
        gun.GetCollisionEvents(other, collisionEvents);
        var intersection = collisionEvents[0].intersection;
        // NaNを含んでいたら無視
        if (float.IsNaN(intersection.x) || float.IsNaN(intersection.y)) return;
        var explosion = Instantiate(explosionPrefab, intersection, Quaternion.identity);

        var p = explosion.GetComponent<ParticleSystem>();
        p.Play();

        if (other.GetComponent<Rigidbody2D>() != null)
            other.GetComponent<Rigidbody2D>().AddForceAtPosition(intersection * 30 - transform.position, intersection + Vector3.up);
        
    }
}
