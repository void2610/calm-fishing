using System.Linq;
using UnityEngine;

namespace Environment
{
    public class RainCollider : MonoBehaviour
    {
        private ParticleSystem _rain;
        private InteractableWater _water;
    
        public void Init(InteractableWater water)
        {
            _rain = this.GetComponent<ParticleSystem>();
            _water = water;
            
            _rain.trigger.SetCollider(0, water.GetComponent<EdgeCollider2D>());
        }
        
        public void StopRain()
        {
            _rain.Stop();
        }
    
        private void OnParticleTrigger()
        {
            // トリガー内のパーティクルを取得
            var particles = new ParticleSystem.Particle[_rain.particleCount].ToList();
            var numInside = _rain.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, particles);

            for (var i = 0; i < numInside; i++)
            {
                // Instantiate(splashParticles, spawnPos, Quaternion.identity);

                var particleWorldPos = particles[i].position;
                _water.PointSplash(particleWorldPos, 0.3f, 0.075f);
            }
        }
    }
}
