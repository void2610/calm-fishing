using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using TMPro;
using DG.Tweening;
using Environment;
using Fishing;
using R3;
using UnityEngine.Rendering;

public class UIManager : MonoBehaviour
{
    [SerializeField] private RawImage renderTexture;
        
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider seSlider;
    [SerializeField] private Image fadeImage;
    
    [SerializeField] private Volume volume;
    [SerializeField] private List<CanvasGroup> canvasGroups;
    [SerializeField] private TextMeshProUGUI coinText;

    private void EnableCanvasGroup(string canvasName, bool e)
    {
        var canvasGroup = canvasGroups.Find(c => c.name == canvasName);
        if (!canvasGroup) return;
        
        canvasGroup.interactable = e;
        canvasGroup.blocksRaycasts = e;
        
        if (e)
        {
            canvasGroup.transform.DOMoveY(-15f, 0).SetRelative(true).SetUpdate(true);
            canvasGroup.transform.DOMoveY(15f, 0.2f).SetRelative(true).SetUpdate(true).SetEase(Ease.OutBack);
            canvasGroup.DOFade(1, 0.2f).SetUpdate(true);
        }
        else
        {
            canvasGroup.DOFade(0, 0.2f).SetUpdate(true);
        }
    }
    
    public void OpenInventory()
    {
        SeManager.Instance.PlaySe("button");
        EnableCanvasGroup("Inventory", true);
    }
    
    public void CloseInventory()
    {
        SeManager.Instance.PlaySe("button");
        EnableCanvasGroup("Inventory", false);
    }
    
    public void OpenAchievement()
    {
        SeManager.Instance.PlaySe("button");
        EnableCanvasGroup("Achievement", true);
    }
    
    public void CloseAchievement()
    {
        SeManager.Instance.PlaySe("button");
        EnableCanvasGroup("Achievement", false);
    }
    
    public void OnClickPause()
    {
        SeManager.Instance.PlaySe("button");
        Time.timeScale = 0;
        EnableCanvasGroup("Pause", true);
    }

    public void OnClickSpeed()
    {
        SeManager.Instance.PlaySe("button");
        GameManager.Instance.ChangeTimeScale();
    }

    public void OnClickResume()
    {
        SeManager.Instance.PlaySe("button");
        Time.timeScale = GameManager.Instance.TimeScale;
        EnableCanvasGroup("Pause", false);
    }

    public void OnClickTitle()
    {
        SeManager.Instance.PlaySe("button");
        fadeImage.color = new Color(0, 0, 0, 0);
        // fadeImage.DOFade(1f, 1f).OnComplete(() => SceneManager.LoadScene("TitleScene")).SetUpdate(true);
    }
    
    private void Awake()
    {
        bgmSlider.value = PlayerPrefs.GetFloat("BgmVolume", 0.5f);
        seSlider.value = PlayerPrefs.GetFloat("SeVolume", 0.5f);
        
        canvasGroups.ForEach(c => EnableCanvasGroup(c.name, false));
    }

    private void Start()
    {
        bgmSlider.onValueChanged.AddListener((value) =>
        {
            BgmManager.Instance.BgmVolume = value;
        });
        seSlider.onValueChanged.AddListener((value) =>
        {
            SeManager.Instance.seVolume = value;
        });

        var trigger = seSlider.gameObject.AddComponent<EventTrigger>();
        var entry = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerUp
        };
        entry.callback.AddListener(_ => SeManager.Instance.PlaySe("button"));
        trigger.triggers.Add(entry);
        
        InventoryManager.Instance.Score.AsObservable().Subscribe(score =>
        {
            coinText.text = "score: " + score;
        }).AddTo(this);

        fadeImage.color = new Color(0, 0, 0, 1);
        fadeImage.DOFade(0, 2f).SetUpdate(true);
    }
}
