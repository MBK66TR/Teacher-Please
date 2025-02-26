using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{
    [Header("Menü Panelleri")]
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject creditsPanel;
    
    [Header("Menü Butonları")]
    [SerializeField] private Button playButton;
    [SerializeField] private Button creditsButton;
    [SerializeField] private Button quitButton;
    
    [Header("Panel Butonları")]
    [SerializeField] private Button backFromCreditsButton;
    private void Start()
    {
        // Ana panel butonları
        if (playButton != null)
            playButton.onClick.AddListener(StartGame);
        if (creditsButton != null)
            creditsButton.onClick.AddListener(ShowCredits);
        if (quitButton != null)
            quitButton.onClick.AddListener(QuitGame);
            
        // Geri dönüş butonları
        if (backFromCreditsButton != null)
            backFromCreditsButton.onClick.AddListener(HideCredits);
            

        // Başlangıçta sadece ana paneli göster
        ShowMainPanel();
    }
    
    private void ShowMainPanel()
    {
        mainPanel?.SetActive(true);
        creditsPanel?.SetActive(false);
    }
    
    private void StartGame()
    {
        // Oyun sahnesine geç (Build Settings'de index 1 olarak ayarlanmalı)
        SceneManager.LoadScene(1);
    }
    
    private void ShowCredits()
    {
        mainPanel?.SetActive(false);
        creditsPanel?.SetActive(true);
    }
    
    private void HideCredits()
    {
        creditsPanel?.SetActive(false);
        mainPanel?.SetActive(true);
    }
    private void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
} 