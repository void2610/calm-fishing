using UnityEngine;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Environment;
using R3;

namespace Fishing
{
    public class FishingManager : MonoBehaviour
    {
        public static FishingManager Instance { get; private set; }
        private static readonly int RatioProperty = Shader.PropertyToID("_Ratio");
        
        [SerializeField] private Player player;
        [SerializeField] private SpriteRenderer fishingGauge;
        [SerializeField] private float fishingTime;
        [SerializeField] private float fishingInterval;
        
        public void ChangeFishingInterval(float time)
        {
            fishingInterval = time;
        }
        
        private async UniTaskVoid Fishing()
        {
            var cancellationToken = this.GetCancellationTokenOnDestroy();
            
            while (true)
            {
                await UniTask.Delay(System.TimeSpan.FromSeconds(fishingInterval), cancellationToken: cancellationToken);

                fishingGauge.material.SetFloat(RatioProperty, 1);
                await fishingGauge.material.DOFloat(0, RatioProperty, fishingTime).SetEase(Ease.Linear).ToUniTask(cancellationToken: cancellationToken);
                
                InventoryManager.Instance.GetRandomItem();
                
                await UniTask.Delay(500, cancellationToken: cancellationToken);
                fishingGauge.material.SetFloat(RatioProperty, 1);
            }
        }
        
        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(this);
        }
        
        private void Start()
        {
            fishingGauge.material.SetFloat(RatioProperty, 1);
            Fishing().Forget();
        } 
    }
}
