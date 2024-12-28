using UnityEngine;
using UnityEngine.VFX;

namespace Fishing
{
    public class MouseCollider : MonoBehaviour
    {
        [SerializeField] private Camera useCamera;
        [SerializeField] private Vector2 offset;
        [SerializeField] private int renderTextureWidth;
        [SerializeField] private int renderTextureHeight;

        private void Update()
        {
            var pos = Input.mousePosition;
            pos = useCamera.ScreenToViewportPoint(pos);
            
            pos.x *= (float)renderTextureWidth / Screen.width;
            pos.y *= (float)renderTextureHeight / Screen.height;
            
            this.transform.position = new Vector3(pos.x, pos.y, 0) + (Vector3)offset;
        }
    }
}
