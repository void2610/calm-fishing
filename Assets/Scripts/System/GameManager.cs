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
            if (PlayerPrefs.GetString("SeedText", "") == "")
            {
                _seed = (int)DateTime.Now.Ticks;
                // Debug.Log("random seed: " + seed);
            }
            else
            {
                _seed = PlayerPrefs.GetInt("Seed", _seed);
                // Debug.Log("fixed seed: " + seed);
            }
            _random = new System.Random(_seed);
            DOTween.SetTweensCapacity(tweenersCapacity: 800, sequencesCapacity: 800);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    
    [Header("オブジェクト")]
    [SerializeField] private GameObject playerObj;
    [SerializeField] private GameObject cloudPrefab;
    [SerializeField] public Canvas pixelCanvas;
    [SerializeField] public Canvas uiCanvas;

    public float TimeScale { get; private set; } = 1.0f;
    public UIManager UIManager => this.GetComponent<UIManager>();

    private int _seed = 42;
    private bool _isPaused;
    private System.Random _random;

    public float RandomRange(float min, float max)
    {
        var randomValue = (float)(this._random.NextDouble() * (max - min) + min);
        return randomValue;
    }

    public int RandomRange(int min, int max)
    {
        var randomValue = this._random.Next(min, max);
        return randomValue;
    }

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
            if (_isPaused)
            {
                _isPaused = false;
                UIManager.OnClickResume();
            }
            else
            {
                _isPaused = true;
                UIManager.OnClickPause();
            }
        }
    }
}
