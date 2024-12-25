using Cysharp.Threading.Tasks;
using UnityEngine;
using DG.Tweening;

public class MoveUpUI : MonoBehaviour
{
    [SerializeField] private float upDistance;
    [SerializeField] private float upTime;
    [SerializeField] private float downDistance;
    [SerializeField] private float downTime;
    
    public void StartMoveUp()
    {
        MoveUp().Forget();
    }

    private async UniTaskVoid MoveUp()
    {
        await transform.DOLocalMoveY(-downDistance, downTime).SetEase(Ease.OutSine).SetRelative().ToUniTask();
        await transform.DOLocalMoveY(upDistance, upTime).SetEase(Ease.InSine).SetRelative().ToUniTask();
    }
}
