using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BalanceSlider : MonoBehaviour
{
    [SerializeField] private Slider balanceSlider;
    [SerializeField] private TextMeshProUGUI studentValueText;
    [SerializeField] private TextMeshProUGUI adminValueText;
    [SerializeField] private Image dangerZoneLeft;
    [SerializeField] private Image dangerZoneRight;
    
    private GameRules gameRules;
    private float dangerThreshold = 0.2f; // Tehlike bölgesi eşiği (0-1 arası)
    
    void Start()
    {
        gameRules = FindObjectOfType<GameRules>();
        UpdateUI();
    }

    void Update()
    {
        float studentValue = gameRules.GetStudentSatisfaction();
        float adminValue = gameRules.GetAdministrationTrust();
        
        // Slider değerini 0-1 arasına normalize et
        float normalizedBalance = (studentValue - adminValue + 100f) / 200f;
        balanceSlider.value = normalizedBalance;
        
        // Tehlike bölgelerini kontrol et
        dangerZoneLeft.color = new Color(1, 0, 0, 
            normalizedBalance < dangerThreshold ? 0.5f : 0f);
        dangerZoneRight.color = new Color(1, 0, 0, 
            normalizedBalance > (1 - dangerThreshold) ? 0.5f : 0f);
        
        UpdateUI();
    }
    
    private void UpdateUI()
    {
        studentValueText.text = $"Öğrenci: {gameRules.GetStudentSatisfaction():F0}";
        adminValueText.text = $"Yönetim: {gameRules.GetAdministrationTrust():F0}";
    }
} 