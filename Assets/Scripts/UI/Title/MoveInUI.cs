using Cysharp.Threading.Tasks;
using UnityEngine;
using DG.Tweening;

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

    private Vector2 _moveVector;
    private RectTransform _rectTransform;

    public void StartMove()
    {
        _rectTransform.DOAnchorPos(_moveVector, time).SetEase(Ease.Unset).SetRelative();
    }

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();

        var dir = direction switch
        {
            Direction.Up => Vector2.up,
            Direction.Down => Vector2.down,
            Direction.Left => Vector2.left,
            Direction.Right => Vector2.right,
            _ => Vector2.zero
        };

        _rectTransform.anchoredPosition -= dir * distance;
        _moveVector = dir * distance;
    }
}