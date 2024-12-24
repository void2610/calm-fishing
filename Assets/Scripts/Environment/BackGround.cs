using System;
using System.Collections.Generic;
using DG.Tweening;
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
            public List<Color> colors;
            public float height1;
            public float height2;
            public float bloomIntensity;
            public float bloomThreshold;
        }
        
        private static readonly int C1 = Shader.PropertyToID("_Color1");
        private static readonly int C2 = Shader.PropertyToID("_Color2");
        private static readonly int C3 = Shader.PropertyToID("_Color3");
        private static readonly int H1 = Shader.PropertyToID("_Height1");
        private static readonly int H2 = Shader.PropertyToID("_Height2");
        
        [SerializeField] private List<TimeColor> timeColors;
        [SerializeField] private Material material;
        [SerializeField] private Volume volume;
        [SerializeField] private float dayLength;
        
        private int _timePeriod;
        
        public void StartTween(int timePeriod){
            // グラデーションが1回しか呼ばれない
            DOTween.To(() => 0f, (t) => Gradation(timePeriod, t), 1f, dayLength / 4)
                .OnComplete(() => _timePeriod = timePeriod);
        }

        private void Gradation(int next, float t)
        {
            Debug.Log("Gradation");
            
            var color1 = Color.Lerp(timeColors[_timePeriod].colors[0], timeColors[next].colors[0], t);
            var color2 = Color.Lerp(timeColors[_timePeriod].colors[1], timeColors[next].colors[1], t);
            var color3 = Color.Lerp(timeColors[_timePeriod].colors[2], timeColors[next].colors[2], t);
            var height1 = Mathf.Lerp(timeColors[_timePeriod].height1, timeColors[next].height1, t);
            var height2 = Mathf.Lerp(timeColors[_timePeriod].height2, timeColors[next].height2, t);
            var bloomIntensity = Mathf.Lerp(timeColors[_timePeriod].bloomIntensity, timeColors[next].bloomIntensity, t);
            var bloomThreshold = Mathf.Lerp(timeColors[_timePeriod].bloomThreshold, timeColors[next].bloomThreshold, t);
            
            material.SetColor(C1, color1);
            material.SetColor(C2, color2);
            material.SetColor(C3, color3);
            material.SetFloat(H1, height1);
            material.SetFloat(H2, height2);
            
            volume.profile.TryGet<Bloom>(out var bloom);
            bloom.intensity.value = bloomIntensity;
            bloom.threshold.value = bloomThreshold;
        }
    }
}
