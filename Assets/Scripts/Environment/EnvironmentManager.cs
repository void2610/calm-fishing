using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Fishing;
using Particle;
using R3;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;
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
        [SerializeField] private GameObject snowPrefab;
        [SerializeField] private BackGround backGround;
        [SerializeField] private MouseCollider mouseCollider;
        [FormerlySerializedAs("GodRay")] [SerializeField] private Light2D godRay;
    
        [Header("デバッグ")]
        [SerializeField] private TextMeshProUGUI weatherText;
        [SerializeField] private TextMeshProUGUI timePeriodText;
    
        public ReadOnlyReactiveProperty<Weather> CurrentWeather => _weather.ToReadOnlyReactiveProperty();
        public ReadOnlyReactiveProperty<TimePeriod> CurrentTimePeriod => _timePeriod.ToReadOnlyReactiveProperty();
        public float DayLength => dayLength;

        private readonly ReactiveProperty<Weather> _weather = new ReactiveProperty<Weather>();
        private readonly ReactiveProperty<TimePeriod> _timePeriod = new ReactiveProperty<TimePeriod>();
        private readonly List<GameObject> _clouds = new();
        private GameObject _snow;
    
        private async UniTaskVoid ChangeWeather()
        {
            var objectCancellationToken = this.GetCancellationTokenOnDestroy();
            var weatherCancellationToken = new CancellationTokenSource();

            while (true)
            {
                var weather = (Weather)Random.Range(0, 3);
                _weather.Value = weather;
                weatherText.text = weather.ToString();
                weatherCancellationToken.Cancel();
                weatherCancellationToken = new CancellationTokenSource();

                if (_snow)
                {
                    _snow.GetComponent<ParticleSystem>().Stop();
                    Destroy(_snow, 10);
                }

                switch (weather)
                {
                    case Weather.Rainy:
                        CreateCloud(weatherCancellationToken.Token).Forget();
                        break;
                    case Weather.Snowy:
                    {
                        _snow = Instantiate(snowPrefab, new Vector3(0, 5.5f, 0), Quaternion.identity, this.transform);
                        break;
                    }
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
            _clouds.Clear();
            while (true)
            {
                var initPos = new Vector3(Random.Range(-10, 10), 4, 0);
                var cloud = Instantiate(cloudPrefab, initPos, Quaternion.identity, this.transform);
                var pos = new Vector3(Random.Range(-8, 8), 4, 0);
                var lifeTime = Random.Range((dayLength / 4) * 0.2f, (dayLength / 4) * 0.8f);
                cloud.GetComponent<Cloud>().Init(pos, water, lifeTime);
                _clouds.Add(cloud);
                
                await UniTask.Delay(TimeSpan.FromSeconds(Random.Range(1f, 3f)), cancellationToken: cancellationToken);
            }
        }
        
        private async UniTaskVoid GodRayEffect()
        {
            var cancellationToken = this.GetCancellationTokenOnDestroy();
            while (true)
            {
                var intensity = Random.Range(0.0f, 1.0f);
                await DOTween.To(() => godRay.intensity, x => godRay.intensity = x, intensity, 3.0f);
                await UniTask.Delay(TimeSpan.FromSeconds(Random.Range(30f, 180f)), cancellationToken: cancellationToken);
            }
        }
        
        private static void ChangeFishingInterval(int cloudCount)
        {
            var time = 10 + cloudCount * 2f;
            FishingManager.Instance.ChangeFishingInterval(time);
        }
    
        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(this);
        }
    
        private void Start()
        {
            _weather.Value = Weather.Sunny;
            _timePeriod.Value = TimePeriod.Dawn;
            weatherText.text = _weather.Value.ToString();
            timePeriodText.text = _timePeriod.Value.ToString();
        
            ChangeWeather().Forget();
            ChangeTimePeriod().Forget();
            GodRayEffect().Forget();
        }

        private void Update()
        {
            // 雲が削除されているか確認
            _clouds.RemoveAll(cloud => !cloud);
            ChangeFishingInterval(_clouds.Count);
        }
    }
}
