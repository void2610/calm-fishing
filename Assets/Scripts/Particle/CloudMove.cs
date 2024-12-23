using UnityEngine;
using DG.Tweening;

namespace Particle
{
    public class CloudMove : MonoBehaviour
    {
        public void Init(Vector3 pos, float lifeTime = -1f)
        {
            if(lifeTime > 0f) Destroy(this.gameObject, lifeTime);
            
            transform.DOMove(pos, 3f).SetEase(Ease.InOutSine);
        }
    }
}
