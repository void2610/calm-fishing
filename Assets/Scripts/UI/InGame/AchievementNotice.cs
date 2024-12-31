using Cysharp.Threading.Tasks;
using DG.Tweening;
using ScriptableObject;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AchievementNotice : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private Image gauge;
    [SerializeField] private float showDuration = 0.5f;
    [SerializeField] private float waitDuration = 3f;
    [SerializeField] private float closeDuration = 1f;
    [SerializeField] private float rightMoveDistance = 100f;
    [SerializeField] private float upMoveDistance = 100f;

    private CanvasGroup _canvasGroup;
    private RectTransform _rectTransform;
    private UniTaskCompletionSource _currentTask;
    
    public async UniTaskVoid Notice(AchievementData achievement)
    {
        // 現在の動作が完了するまで待機
        if (_currentTask != null)
        {
            await _currentTask.Task;
        }

        // 新しいタスクの開始
        _currentTask = new UniTaskCompletionSource();
        await NoticeAsync(achievement);
        _currentTask.TrySetResult();
    }
    
    private async UniTask NoticeAsync(AchievementData achievement)
    {
        // なぜかエラーが出るが大丈夫
        icon.sprite = achievement.icon;
        title.text = achievement.title;
        gauge.fillAmount = 0;

        await _rectTransform.DOMoveX(rightMoveDistance, showDuration).SetEase(Ease.OutSine).SetRelative().ToUniTask();
        gauge.DOFillAmount(1, waitDuration).SetEase(Ease.Linear).ToUniTask().Forget();
        await UniTask.Delay((int)(waitDuration * 1000));
        
        _rectTransform.DOMoveY(upMoveDistance, closeDuration).SetEase(Ease.InSine).SetRelative().ToUniTask().Forget();
        await _canvasGroup.DOFade(0, closeDuration).SetEase(Ease.InSine).ToUniTask();

        // Reset
        await _rectTransform.DOMoveX(-rightMoveDistance, 0).SetRelative();
        await _rectTransform.DOMoveY(-upMoveDistance, 0).SetRelative();
        _canvasGroup.alpha = 1;
        gauge.fillAmount = 0;
    }

    private void Awake()
    {
        _canvasGroup = this.GetComponent<CanvasGroup>();
        _rectTransform = this.GetComponent<RectTransform>();
        
        _rectTransform.DOMoveX(-rightMoveDistance, 0).SetRelative();
        gauge.fillAmount = 0;
    }
}
