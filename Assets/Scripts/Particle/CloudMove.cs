using UnityEngine;
using DG.Tweening;

namespace Particle
{
    public class CloudMove : MonoBehaviour
    {
        public void Init(Vector3 pos)
        {
            transform.DOMove(pos, 1f).SetEase(Ease.InOutSine);
        }
    }
}
