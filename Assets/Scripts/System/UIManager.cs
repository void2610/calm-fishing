using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using TMPro;
using DG.Tweening;
using R3;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class UIManager : MonoBehaviour
{
    [SerializeField] private RawImage renderTexture;
        
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider seSlider;
    [SerializeField] private Image fadeImage;
    
    [SerializeField] private Volume volume;
    [SerializeField] private List<CanvasGroup> canvasGroups;
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private TextMeshProUGUI stageText;
    [SerializeField] private TextMeshProUGUI expText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] public Slider hpSlider;
    [SerializeField] public TextMeshProUGUI hpText;

    public int remainingLevelUps;

    public void EnableCanvasGroup(string canvasName, bool e)
    {
        var canvasGroup = canvasGroups.Find(c => c.name == canvasName);
        if (!canvasGroup) return;
        
        canvasGroup.alpha = e ? 1 : 0;
        canvasGroup.interactable = e;
        canvasGroup.blocksRaycasts = e;
    }
    
    private void UpdateCoinText(int amount)
    {
        coinText.text = "coin: " + amount.ToString();
    }

    private void UpdateExpText(int now, int max)
    {
        expText.text = "exp: " + now + "/" + max;
    }

    private void UpdateLevelText(int level)
    {
        if (!levelText) return;
        levelText.text = "level: " + level;
    }

    private void UpdateStageText(int stage)
    {
        int s = Mathf.Max(1, stage + 1);
        stageText.text = "stage: " + s;
    }
    
    public void OnClickSkippRestButton()
    {
        SeManager.Instance.PlaySe("button");
        GameManager.Instance.ChangeState(GameManager.GameState.MapSelect);
        EnableCanvasGroup("Rest", false);
    }

    public void OnClickShopExit()
    {
        SeManager.Instance.PlaySe("button");
        EnableCanvasGroup("Shop", false);
        GameManager.Instance.ChangeState(GameManager.GameState.MapSelect);
    }
    
    public void OnClickTreasureExit()
    {
        SeManager.Instance.PlaySe("button");
        EnableCanvasGroup("Treasure", false);
        GameManager.Instance.ChangeState(GameManager.GameState.MapSelect);
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
        Time.timeScale = GameManager.Instance.timeScale;
        EnableCanvasGroup("Pause", false);
    }

    public void OnClickTitle()
    {
        SeManager.Instance.PlaySe("button");
        fadeImage.color = new Color(0, 0, 0, 0);
        fadeImage.DOFade(1f, 1f).OnComplete(() => SceneManager.LoadScene("TitleScene")).SetUpdate(true);
    }

    public void OnClickRetry()
    {
        SeManager.Instance.PlaySe("button");
        fadeImage.color = new Color(0, 0, 0, 0);
        fadeImage.DOFade(1f, 1f).OnComplete(() => SceneManager.LoadScene("MainScene")).SetUpdate(true);
    }

    public void OpenMap()
    {
        SeManager.Instance.PlaySe("button");
        EnableCanvasGroup("Map", true);
    }
    
    public void CloseMap()
    {
        SeManager.Instance.PlaySe("button");
        EnableCanvasGroup("Map", false);
    }
    
    private void SetVignette(float value)
    {
        if(!volume.profile.TryGet(out Vignette vignette)) return;
        vignette.intensity.value = value;
    }
    
    private void Awake()
    {
        bgmSlider.value = PlayerPrefs.GetFloat("BgmVolume", 1.0f);
        seSlider.value = PlayerPrefs.GetFloat("SeVolume", 1.0f);
        
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

        fadeImage.color = new Color(0, 0, 0, 1);
        fadeImage.DOFade(0, 2f).SetUpdate(true);
    }
}
