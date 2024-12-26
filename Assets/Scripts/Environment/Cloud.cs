using DG.Tweening;
using Fishing;
using Particle;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.VFX.Utility;

namespace Environment
{
    public class Cloud : MonoBehaviour
    {

        [SerializeField] private RainCollider rain;
        private InteractableWater _water;
        private VisualEffect _vfx;
        private MouseCollider _mouse;
        
        public void Init(Vector3 pos, InteractableWater water,　MouseCollider mouse, float lifeTime = -1f)
        {
            if(lifeTime > 0f) Destroy(this.gameObject, lifeTime);
            
            _water = water;
            _mouse = mouse;
            _vfx = GetComponent<VisualEffect>();
            rain.Init(water);
            transform.DOMove(pos, 3f).SetEase(Ease.InOutSine);
        }

        private void Update()
        {
            if(!_mouse || !_vfx) return;
            
            // _vfx.SetVector3("MousePosition", _mouse.transform.position);
        }
    }
}
