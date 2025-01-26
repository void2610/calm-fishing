using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TutorialPanel : MonoBehaviour
{
    [SerializeField] private List<GameObject> pages;
    [SerializeField] private Button nextButton;
    [SerializeField] private Button previousButton;
    
    private const float OFFSET = 500;
    private int _currentPage = 0;
    private GameObject _currentActivePage;
    private RectTransform _rectTransform;
    
    public void Show()
    {
        _rectTransform.DOMoveY(-1000, 5f).SetRelative();
        CameraMove.Instance.ShakeCamera(5f, 3f);
    }

    private void NextPage()
    {
        if (_currentPage >= pages.Count - 1) return;
        ChangePage(_currentPage + 1);
    }
    
    private void PreviousPage()
    {
        if (_currentPage <= 0) return;
        ChangePage(_currentPage - 1);
    }
    
    private void ChangePage(int page)
    {
        if(_currentActivePage) Destroy(_currentActivePage);
        _currentActivePage = Instantiate(pages[page], this.transform);
        _currentPage = page;
    }

    private void Awake()
    {
        nextButton.onClick.AddListener(NextPage);
        previousButton.onClick.AddListener(PreviousPage);
        ChangePage(0);
        
        _rectTransform = GetComponent<RectTransform>();
        _rectTransform.anchoredPosition = new Vector2(0, OFFSET);
    }
}
