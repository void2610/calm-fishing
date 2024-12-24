using System.Linq;
using UnityEngine;
using DG.Tweening;

namespace Particle
{
    public class Cloud : MonoBehaviour
    {

        [SerializeField] private RainCollider rain;
        private InteractableWater _water;
        
        public void Init(Vector3 pos, InteractableWater water, float lifeTime = -1f)
        {
            if(lifeTime > 0f) Destroy(this.gameObject, lifeTime);
            
            rain.Init(water);
            transform.DOMove(pos, 3f).SetEase(Ease.InOutSine);
        }
    }
}
