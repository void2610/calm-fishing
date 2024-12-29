using System;
using UnityEngine;
using R3;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DOTween.SetTweensCapacity(tweenersCapacity: 800, sequencesCapacity: 800);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    
    [Header("オブジェクト")]
    [SerializeField] private GameObject playerObj;
    [SerializeField] public Canvas pixelCanvas;
    [SerializeField] public Canvas uiCanvas;

    public float TimeScale { get; private set; } = 1.0f;
    public UIManager UIManager => this.GetComponent<UIManager>();

    public bool IsPaused { get; private set; }

    public void ChangeTimeScale()
    {
        if (PlayerPrefs.GetInt("IsDoubleSpeed", 0) == 0)
        {
            TimeScale = 3.0f;
            PlayerPrefs.SetInt("IsDoubleSpeed", 1);
        }
        else
        {
            TimeScale = 1.0f;
            PlayerPrefs.SetInt("IsDoubleSpeed", 0);
        }
        Time.timeScale = TimeScale;
    }

    public void Start()
    {
        if (PlayerPrefs.GetInt("IsDoubleSpeed", 0) == 1)
        {
            TimeScale = 3.0f;
            Time.timeScale = TimeScale;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (IsPaused)
            {
                IsPaused = false;
                UIManager.OnClickResume();
            }
            else
            {
                IsPaused = true;
                UIManager.OnClickPause();
            }
        }
    }
}
