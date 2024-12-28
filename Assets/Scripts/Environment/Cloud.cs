using System.Collections;
using DG.Tweening;
using Fishing;
using Particle;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.VFX.Utility;

namespace Environment
{
    public class Cloud : MonoBehaviour
    {

        [SerializeField] private RainCollider rain;

        private float _lifeTime = float.PositiveInfinity;
        private bool _isStopping = false;
        
        public void Init(Vector3 pos, InteractableWater water, float lifeTime)
        {
            rain.Init(water);
            transform.DOMove(pos, 3f).SetEase(Ease.InOutSine).SetLink(gameObject);
            _lifeTime = lifeTime;
        }
        
        public void SubtractLifeTime(float time)
        {
            _lifeTime -= time;
            if(_lifeTime <= 0)
                StartCoroutine(nameof(StopCoroutine), 1.0f);
        }
        
        private IEnumerator StopCoroutine(float time)
        {
            Debug.Log("Stop");

            _isStopping = true;
            rain.StopRain();
            GetComponent<VisualEffect>().Stop();
            yield return new WaitForSeconds(time);
            Destroy(gameObject);
        }

        private void Update()
        {
            if(_isStopping) return;
            
            if(_lifeTime > 0)
                _lifeTime -= Time.deltaTime;
            else
            {
                StartCoroutine(nameof(StopCoroutine), 1.0f);
            }
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log(other.name);
            if (other.TryGetComponent(out MouseCollider mouseCollider))
            {   
                _lifeTime -= 0.5f;
                Debug.Log("Hit");
            }
        }
    }
}
