using UnityEngine;
using Cysharp.Threading.Tasks;
using DG.Tweening;

namespace Fishing
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private Vector2 moveInterval;
        [SerializeField] private Vector2 moveDistance;
        [SerializeField] private float moveSpeed;
        [SerializeField] private Vector2 moveLimit;

        private void Start()
        {
            MoveRandomly().Forget();
        }

        private async UniTaskVoid MoveRandomly()
        {
            while (true)
            {
                // 次の移動までの待機
                var waitTime = Random.Range(moveInterval.x, moveInterval.y);
                await UniTask.Delay(System.TimeSpan.FromSeconds(waitTime));

                var dis = Random.Range(moveDistance.x, moveDistance.y);
                var dir = Random.value < 0.5f ? 1 : -1;
                if (transform.position.x + dis * dir < moveLimit.x)
                    dir = 1;
                else if (transform.position.x + dis * dir > moveLimit.y)
                    dir = -1;
                var targetX = transform.position.x + dis * dir;

                // DoTweenで移動
                await transform.DOMoveX(targetX, dis / moveSpeed).SetEase(Ease.InOutQuad).ToUniTask();
            }
        }
    }
}