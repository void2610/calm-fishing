using System;
using System.Collections.Generic;
using DG.Tweening;
using R3;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;

namespace Environment
{
    public class BackGround : MonoBehaviour
    {
        [Serializable]
        private class TimeColor
        {
            public Color color1;
            public Color color2;
            public Color color3;
            public float height1;
            public float height2;
            public float bloomThreshold;
            public float bloomIntensity;
            public float lightIntensity;
            public Color lightColor;
            public float sunLightIntensity;
        }
        
        private static readonly int C1 = Shader.PropertyToID("_Color1");
        private static readonly int C2 = Shader.PropertyToID("_Color2");
        private static readonly int C3 = Shader.PropertyToID("_Color3");
        private static readonly int H1 = Shader.PropertyToID("_Height1");
        private static readonly int H2 = Shader.PropertyToID("_Height2");
        
        [SerializeField] private List<TimeColor> timeColors;
        [SerializeField] private Material material;
        [SerializeField] private Volume volume;
        [SerializeField] private Light2D globalLight;
        [SerializeField] private Light2D sunLight;
        private float _dayLength;
        
        private int _timePeriod;
        
        private void StartTween(TimePeriod timePeriod){
            var next = (int)timePeriod;
            DOTween.To(() => 0f, (t) => Gradation(next, t), 1f, _dayLength / 8)
                .OnComplete(() => _timePeriod = next);
        }

        private void Gradation(int next, float t)
        {
            var color1 = Color.Lerp(timeColors[_timePeriod].color1, timeColors[next].color1, t);
            var color2 = Color.Lerp(timeColors[_timePeriod].color2, timeColors[next].color2, t);
            var color3 = Color.Lerp(timeColors[_timePeriod].color3, timeColors[next].color3, t);
            var height1 = Mathf.Lerp(timeColors[_timePeriod].height1, timeColors[next].height1, t);
            var height2 = Mathf.Lerp(timeColors[_timePeriod].height2, timeColors[next].height2, t);
            var bloomIntensity = Mathf.Lerp(timeColors[_timePeriod].bloomIntensity, timeColors[next].bloomIntensity, t);
            var bloomThreshold = Mathf.Lerp(timeColors[_timePeriod].bloomThreshold, timeColors[next].bloomThreshold, t);
            var lightIntensity = Mathf.Lerp(timeColors[_timePeriod].lightIntensity, timeColors[next].lightIntensity, t);
            var lightColor = Color.Lerp(timeColors[_timePeriod].lightColor, timeColors[next].lightColor, t);
            var sunLightIntensity = Mathf.Lerp(timeColors[_timePeriod].sunLightIntensity, timeColors[next].sunLightIntensity, t);
            
            material.SetColor(C1, color1);
            material.SetColor(C2, color2);
            material.SetColor(C3, color3);
            material.SetFloat(H1, height1);
            material.SetFloat(H2, height2);
            
            volume.profile.TryGet<Bloom>(out var bloom);
            bloom.intensity.value = bloomIntensity;
            bloom.threshold.value = bloomThreshold;
            
            globalLight.intensity = lightIntensity;
            globalLight.color = lightColor;
            sunLight.intensity = sunLightIntensity;
        }

        public void Awake()
        {
            _timePeriod = 0;
            material.SetColor(C1, timeColors[0].color1);
            material.SetColor(C2, timeColors[0].color2);
            material.SetColor(C3, timeColors[0].color3);
            material.SetFloat(H1, timeColors[0].height1);
            material.SetFloat(H2, timeColors[0].height2);
            
            volume.profile.TryGet<Bloom>(out var bloom);
            bloom.intensity.value = timeColors[0].bloomIntensity;
            bloom.threshold.value = timeColors[0].bloomThreshold;
            
            globalLight.intensity = timeColors[0].lightIntensity;
            globalLight.color = timeColors[0].lightColor;
            sunLight.intensity = timeColors[0].sunLightIntensity;
        }

        public void Start()
        {
            _dayLength = EnvironmentManager.Instance.DayLength;
            EnvironmentManager.Instance.CurrentTimePeriod.AsObservable().Subscribe(StartTween).AddTo(this);
        }
    }
}
