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
            
        // Geri dönüş butonu
        if (backFromCreditsButton != null)
        {
            backFromCreditsButton.onClick.RemoveAllListeners(); // Önceki dinleyicileri temizle
            backFromCreditsButton.onClick.AddListener(HideCredits);
        }
        else
        {
            Debug.LogError("Geri dönüş butonu atanmamış!");
        }

        // Başlangıçta sadece ana paneli göster
        ShowMainPanel();
    }
    
    private void ShowMainPanel()
    {
        if (mainPanel != null)
        {
            mainPanel.SetActive(true);
        }
        else
        {
            Debug.LogError("Main Panel atanmamış!");
        }

        if (creditsPanel != null)
        {
            creditsPanel.SetActive(false);
        }
        else
        {
            Debug.LogError("Credits Panel atanmamış!");
        }
    }
    
    private void StartGame()
    {
        // Oyun sahnesine geç (Build Settings'de index 1 olarak ayarlanmalı)
        SceneManager.LoadScene(1);
    }
    
    private void ShowCredits()
    {
        if (mainPanel != null && creditsPanel != null)
        {
            mainPanel.SetActive(false);
            creditsPanel.SetActive(true);
        }
        else
        {
            Debug.LogError("Panel referansları eksik!");
        }
    }
    
    private void HideCredits()
    {
        if (mainPanel != null && creditsPanel != null)
        {
            mainPanel.SetActive(true);
            creditsPanel.SetActive(false);
        }
        else
        {
            Debug.LogError("Panel referansları eksik!");
        }
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