using UnityEngine;
using Cysharp.Threading.Tasks;
using DG.Tweening;

namespace Fishing
{
    public class Player : MonoBehaviour
    {
        public enum AnimationType
        {
            Stand,
            Fishing
        }
        
        [SerializeField] private RuntimeAnimatorController standAnimator;
        [SerializeField] private RuntimeAnimatorController fishingAnimator;
        
        [SerializeField] private Vector2 moveInterval;
        [SerializeField] private Vector2 moveDistance;
        [SerializeField] private float moveSpeed;
        [SerializeField] private Vector2 moveLimit;
        
        private Animator _animator;
        
        public void PlayAnimation(AnimationType animationType)
        {
            if(!_animator) return;
            
            _animator.runtimeAnimatorController = animationType switch
            {
                AnimationType.Stand => standAnimator,
                AnimationType.Fishing => fishingAnimator,
                _ => _animator.runtimeAnimatorController
            };
        }

        private void Awake()
        {
            _animator = this.transform.GetComponentInChildren<Animator>();
            _animator.runtimeAnimatorController = standAnimator;
        }

        private void Start()
        {
            this.transform.DOMoveY(-0.15f, 2.0f).SetEase(Ease.InOutQuad).SetLoops(-1, LoopType.Yoyo).SetRelative();
            
            MoveRandomly().Forget();
        }

        private async UniTaskVoid MoveRandomly()
        {
            var cancellationToken = this.GetCancellationTokenOnDestroy();
            
            while (true)
            {
                // 次の移動までの待機
                var waitTime = Random.Range(moveInterval.x, moveInterval.y);
                await UniTask.Delay(System.TimeSpan.FromSeconds(waitTime), cancellationToken: cancellationToken);

                var dis = Random.Range(moveDistance.x, moveDistance.y);
                var dir = Random.value < 0.5f ? 1 : -1;
                if (transform.position.x + dis * dir < moveLimit.x)
                    dir = 1;
                else if (transform.position.x + dis * dir > moveLimit.y)
                    dir = -1;
                var targetX = transform.position.x + dis * dir;

                // DoTweenで移動
                await transform.DOMoveX(targetX, dis / moveSpeed).SetEase(Ease.InOutQuad).ToUniTask(cancellationToken: cancellationToken);
            }
        }
    }
}