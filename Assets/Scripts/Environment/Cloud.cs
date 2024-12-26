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
        
        public void Init(Vector3 pos, InteractableWater water, float lifeTime = -1)
        {
            rain.Init(water);
            transform.DOMove(pos, 3f).SetEase(Ease.InOutSine).SetLink(gameObject);
            
            if (lifeTime > 0)
            {
                StartCoroutine(StopCoroutine(lifeTime));
            }
        }
        
        private IEnumerator StopCoroutine(float time)
        {
            yield return new WaitForSeconds(time);
            rain.StopRain();
            GetComponent<VisualEffect>().Stop();
            yield return new WaitForSeconds(5.0f);
            Destroy(gameObject);
        }
    }
}
