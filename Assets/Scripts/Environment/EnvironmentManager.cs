using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Fishing;
using Particle;
using R3;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Environment
{
    public class EnvironmentManager : MonoBehaviour
    {
        public static EnvironmentManager Instance { get; private set; }

        [Header("設定")]
        [SerializeField] private Vector2 whetherInterval;
        [SerializeField] private float dayLength;
    
        [Header("オブジェクト")]
        [SerializeField] private InteractableWater water;
        [SerializeField] private GameObject cloudPrefab;
        [SerializeField] private BackGround backGround;
        [SerializeField] private MouseCollider mouseCollider;
    
        [Header("デバッグ")]
        [SerializeField] private TextMeshProUGUI weatherText;
        [SerializeField] private TextMeshProUGUI timePeriodText;
    
        public ReadOnlyReactiveProperty<Weather> CurrentWeather => _weather.ToReadOnlyReactiveProperty();
        public ReadOnlyReactiveProperty<TimePeriod> CurrentTimePeriod => _timePeriod.ToReadOnlyReactiveProperty();

        private readonly ReactiveProperty<Weather> _weather = new ReactiveProperty<Weather>();
        private readonly ReactiveProperty<TimePeriod> _timePeriod = new ReactiveProperty<TimePeriod>();
    
        private async UniTaskVoid ChangeWeather()
        {
            var objectCancellationToken = this.GetCancellationTokenOnDestroy();
            var weatherCancellationToken = new CancellationTokenSource();

            while (true)
            {
                var weather = (Weather)Random.Range(0, 4);
                _weather.Value = weather;
                weatherText.text = weather.ToString();
                weatherCancellationToken.Cancel();
                weatherCancellationToken = new CancellationTokenSource();

                if (weather == Weather.Rainy)
                {
                    CreateCloud(weatherCancellationToken.Token).Forget();
                }
                
                await UniTask.Delay(TimeSpan.FromSeconds(Random.Range(whetherInterval.x, whetherInterval.y)), cancellationToken: objectCancellationToken);
            }
        }
    
        private async UniTaskVoid ChangeTimePeriod()
        {
            var cancellationToken = this.GetCancellationTokenOnDestroy();

            while (true)
            {
                var timePeriod = (TimePeriod)(((int)_timePeriod.Value + 1) % 4);
                _timePeriod.Value = timePeriod;
                timePeriodText.text = timePeriod.ToString();
                
                await UniTask.Delay(TimeSpan.FromSeconds(dayLength / 4), cancellationToken: cancellationToken);
            }
        }
    
        private async UniTaskVoid CreateCloud(CancellationToken cancellationToken)
        {
            while (true)
            {
                var initPos = new Vector3(Random.Range(-10, 10), 4, 0);
                var cloud = Instantiate(cloudPrefab, initPos, Quaternion.identity, this.transform);
                var pos = new Vector3(Random.Range(-8, 8), 4, 0);
                var lifeTime = Random.Range(3, 10);
                cloud.GetComponent<Cloud>().Init(pos, water, lifeTime);
            
                await UniTask.Delay(TimeSpan.FromSeconds(Random.Range(0.2f, 1)), cancellationToken: cancellationToken);
            }
        }
    
        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(this);
        }
    
        private void Start()
        {
            _weather.Value = Weather.Sunny;
            _timePeriod.Value = TimePeriod.Night;
            weatherText.text = _weather.Value.ToString();
            timePeriodText.text = _timePeriod.Value.ToString();
        
            ChangeWeather().Forget();
            ChangeTimePeriod().Forget();
        }
    }
}
