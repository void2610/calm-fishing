using UnityEngine;
using Cysharp.Threading.Tasks;
using DG.Tweening;

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
        
        private async UniTaskVoid Fishing()
        {
            while (true)
            {
                await UniTask.Delay(System.TimeSpan.FromSeconds(fishingInterval));

                fishingGauge.material.SetFloat(RatioProperty, 1);
                await fishingGauge.material.DOFloat(0, RatioProperty, fishingTime).SetEase(Ease.Linear).ToUniTask();
                
                Debug.Log("釣り終了");
                InventoryManager.Instance.AddRandomItem();
                
                await UniTask.Delay(500);
                fishingGauge.material.SetFloat(RatioProperty, 1);
            }
        }
        
        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(this);
        }
        
        private void Start()
        {
            fishingGauge.material.SetFloat(RatioProperty, 1);
            Fishing().Forget();
        } 
    }
}
