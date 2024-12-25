using Cysharp.Threading.Tasks;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UIElements;

public class MoveInUI : MonoBehaviour
{
    private enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }
    
    [SerializeField] private Direction direction;
    [SerializeField] private float distance;
    [SerializeField] private float time;
    
    private Vector3 _moveVector;

    public void StartMove()
    {
        this.transform.DOMove(_moveVector, time).SetEase(Ease.Unset).SetRelative();
    }

    private void Awake()
    {
        var dir = direction switch
        {
            Direction.Up => Vector2.up,
            Direction.Down => Vector2.down,
            Direction.Left => Vector2.left,
            Direction.Right => Vector2.right,
            _ => Vector2.zero
        };
        
        transform.localPosition -= (Vector3)dir * distance;
        _moveVector = (Vector3)dir * distance;
    }
}
