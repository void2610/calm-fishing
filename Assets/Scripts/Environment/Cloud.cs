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
                StartCoroutine(nameof(StopCoroutine));
        }
        
        private IEnumerator StopCoroutine()
        {
            _isStopping = true;
            rain.StopRain();
            GetComponent<VisualEffect>().Stop();
            yield return new WaitForSeconds(5);
            Destroy(gameObject);
        }

        private void Update()
        {
            if(_isStopping) return;
            
            if(_lifeTime > 0)
                _lifeTime -= Time.deltaTime;
            else
            {
                StartCoroutine(nameof(StopCoroutine));
            }
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if(GameManager.Instance.IsPaused) return;
            
            if (other.TryGetComponent(out MouseCollider mouseCollider))
            {   
                _lifeTime -= 5.0f;
            }
        }
    }
}
