using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialPanel : MonoBehaviour
{
    [SerializeField] private List<GameObject> pages;
    [SerializeField] private Button nextButton;
    [SerializeField] private Button previousButton;
    
    private int _currentPage = 0;
    private GameObject _currentActivePage;
    
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
    }
}
