using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using TMPro;
using DG.Tweening;
using Particle;
using R3;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ParticleManager : MonoBehaviour
{
    public static ParticleManager Instance;
    
    [SerializeField] private GameObject healParticlePrefab;
    [SerializeField] private GameObject mergeParticle;
    [SerializeField] private GameObject mergePowerParticle;
    [SerializeField] private GameObject damageTextPrefab;
    [SerializeField] private GameObject mergeTextPrefab;
    
    private Canvas PixelCanvas => GameManager.Instance.pixelCanvas;
    private Canvas UICanvas => GameManager.Instance.uiCanvas;
    
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this.gameObject);
    }
}
