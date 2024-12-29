using DG.Tweening;
using R3;
using UnityEngine;

namespace Environment
{
    public class BrightnessBg : MonoBehaviour
    {
        [SerializeField] private float sunnyBrightness;
        [SerializeField] private float rainyBrightness;
        [SerializeField] private float snowyBrightness;
        [SerializeField] private float cloudyBrightness;
        
        private float _dayLength;
        private SpriteRenderer _brightnessBg;

        
        private void StartGradation(Weather weather)
        {
            var brightness = weather switch
            {
                Weather.Sunny => sunnyBrightness,
                Weather.Rainy => rainyBrightness,
                Weather.Snowy => snowyBrightness,
                Weather.Cloudy => cloudyBrightness,
                _ => 0f
            };
            _brightnessBg.DOColor(new Color(0,0,0,1 - brightness), _dayLength / 8);
        }

        private void Start()
        {
            _brightnessBg = GetComponent<SpriteRenderer>();
            _dayLength = EnvironmentManager.Instance.DayLength;
            EnvironmentManager.Instance.CurrentWeather.AsObservable().Subscribe(StartGradation).AddTo(this);
        }
    }
}
