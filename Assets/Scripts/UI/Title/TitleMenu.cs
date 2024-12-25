using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TitleMenu : MonoBehaviour
{
    [SerializeField] private CanvasGroup inGameUI;
    [SerializeField] private CanvasGroup credit;
    [SerializeField] private CanvasGroup license;
    
    [SerializeField] private List<MoveUpUI> moveUpUIs;
    [SerializeField] private List<MoveInUI> moveInUIs;
    
    private CanvasGroup _titleCanvasGroup;
    
    public void StartGame()
    {
        StartGameAsync().Forget();
    }

    private async UniTaskVoid StartGameAsync()
    {
        SeManager.Instance.PlaySe("button");
        this.GetComponent<CanvasGroup>().interactable = false;
        
        // タイトルUIを非表示
        foreach (var ui in moveUpUIs)
        {
            ui.StartMoveUp();
            await UniTask.Delay(50);
        }
        await UniTask.Delay(1500);
        
        // ゲーム内UIを表示
        foreach (var ui in moveInUIs)
        {
            ui.StartMove();
            // await UniTask.Delay(50);
        }
        await UniTask.Delay(1500);
        
        inGameUI.interactable = true;
        inGameUI.blocksRaycasts = true;
    }

    public void ShowCredit()
    {
        PlayButtonSe();
        credit.alpha = 1.0f;
        credit.interactable = true;
        credit.blocksRaycasts = true;
    }

    public void HideCredit()
    {
        PlayButtonSe();
        credit.alpha = 0.0f;
        credit.interactable = false;
        credit.blocksRaycasts = false;
    }

    public void ShowLicense()
    {
        PlayButtonSe();
        license.alpha = 1.0f;
        license.interactable = true;
        license.blocksRaycasts = true;
    }

    public void HideLicense()
    {
        PlayButtonSe();
        license.alpha = 0.0f;
        license.interactable = false;
        license.blocksRaycasts = false;
    }

    public static void PlayButtonSe()
    {
        if (Time.time > 0.5f)
            SeManager.Instance.PlaySe("button");
    }

    private static void InitPlayerPrefs()
    {
        PlayerPrefs.SetFloat("BgmVolume", 1.0f);
        PlayerPrefs.SetFloat("SeVolume", 1.0f);
    }

    public void ResetSetting()
    {
        PlayerPrefs.SetFloat("BgmVolume", 1.0f);
        PlayerPrefs.SetFloat("SeVolume", 1.0f);
    }
    
    private void EnableCanvasGroup(CanvasGroup canvasGroup, bool e)
    {
        canvasGroup.alpha = e ? 1.0f : 0.0f;
        canvasGroup.interactable = e;
        canvasGroup.blocksRaycasts = e;
    }

    private void Awake()
    {
        if (!PlayerPrefs.HasKey("BgmVolume")) InitPlayerPrefs();
        _titleCanvasGroup = this.GetComponent<CanvasGroup>();

        Time.timeScale = 1.0f;
        HideCredit();
        HideLicense();
        
        // EnableCanvasGroup(inGameUI, false);
        EnableCanvasGroup(_titleCanvasGroup, false);
        
        _titleCanvasGroup.DOFade(1f, 3f).OnComplete(
            () => EnableCanvasGroup(_titleCanvasGroup, true)
        );
        
        Debug.Log("TitleMenu.cs is loaded.");
    }
}
