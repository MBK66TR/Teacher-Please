using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class DailyExpenseManager : MonoBehaviour
{
    [SerializeField] private GameObject expensePanel;
    [SerializeField] private TextMeshProUGUI remainingMoneyText;
    [SerializeField] private Button confirmButton;
    [SerializeField] private GameObject mainPanel;
    
    [Header("Expense Items")]
    [SerializeField] private ExpenseItem[] expenseItems;
    
    [SerializeField] private GameRules gameRules;
    [SerializeField] private TextMeshProUGUI dailyIncomeText;
    [SerializeField] private TextMeshProUGUI currentMoneyText;
    [SerializeField] private Slider spendingSlider;
    [SerializeField] private TextMeshProUGUI spendingAmountText;
    [SerializeField] private TextMeshProUGUI currentDayText; // Gün sayacı için UI elemanı
    
    private float currentMoney = 80f; // Başlangıç parası1
    private float remainingMoney; // Kalan para
    private float dailyIncome;
    private float spendingAmount = 0f;
    private int currentDay = 1; // Mevcut gün sayacı
    private Dictionary<ExpenseType, int> missedExpenseCounts;

    private const int MAX_MISSED_EXPENSES = 3;
    
    [System.Serializable]
    public class ExpenseItem
    {
        public ExpenseType type;
        public Toggle toggle;
        public TextMeshProUGUI costText;
        public TextMeshProUGUI descriptionText;
    }
    
    void Start()
    {
        currentDay = PlayerPrefs.GetInt("CurrentDay", 1);
        UpdateDayUI();

        if (confirmButton != null)
            confirmButton.onClick.AddListener(OnConfirmClicked);

        if (spendingSlider != null)
        {
            spendingSlider.onValueChanged.AddListener(OnSpendingChanged);
            spendingSlider.minValue = 0f;
        }

        gameRules = FindObjectOfType<GameRules>();
        
        // Sözlüğü başlat
        missedExpenseCounts = new Dictionary<ExpenseType, int>();
        foreach (ExpenseType type in System.Enum.GetValues(typeof(ExpenseType)))
        {
            missedExpenseCounts[type] = 0;
        }
        
        InitializeExpenses();
        UpdateMoneyUI();
    }
    
    void InitializeExpenses()
    {
        var costs = gameRules.GetExpenseCosts();
        
        foreach (var item in expenseItems)
        {
            item.toggle.onValueChanged.AddListener((isOn) => UpdateExpenses());
            item.costText.text = $"-{costs[item.type]:F0} TL";
            UpdateExpenseItemUI(item);
        }
        
        UpdateExpenses();
    }

    void UpdateExpenseItemUI(ExpenseItem item)
    {
        int missedCount = missedExpenseCounts[item.type];
        
        // Metin rengini güncelle
        if (missedCount >= 2)
        {
            item.descriptionText.color = Color.red;
            item.descriptionText.text = $"{GetExpenseDescription(item.type)} (Uyarı: {missedCount} gündür ödenmedi!)";
        }
        else
        {
            item.descriptionText.color = Color.black;
            item.descriptionText.text = GetExpenseDescription(item.type);
        }
    }
    
    void UpdateExpenses()
    {
        float totalExpenses = 0f;
        var costs = gameRules.GetExpenseCosts();
        
        foreach (var item in expenseItems)
        {
            if (item.toggle.isOn)
            {
                totalExpenses += costs[item.type];
            }
        }
        
        remainingMoney = gameRules.GetDailySalary() - totalExpenses;
        UpdateUI();
    }
    
    void UpdateUI()
    {
        remainingMoneyText.text = $"Kalan: {remainingMoney:F0} TL";
        
        // Eğer bütçe eksideyse kırmızı yap ve butonu devre dışı bırak
        Color textColor = remainingMoney < 0 ? Color.red : Color.black;
        remainingMoneyText.color = textColor;
        confirmButton.interactable = remainingMoney >= 0;
    }

    string GetExpenseDescription(ExpenseType type)
    {
        switch (type)
        {
            case ExpenseType.Rent: return "Ev Kirası";
            case ExpenseType.Food: return "Yemek Giderleri";
            case ExpenseType.Bills: return "Su, Elektrik, İnternet";
            case ExpenseType.RoomExpenses: return "Oda Bakım ve Temizlik";
            default: return "";
        }
    }
    
    private void UpdateMoneyUI()
    {
        dailyIncome = Random.Range(80f, 100f);
        float totalAvailable = currentMoney + dailyIncome;

        if (spendingSlider != null)
        {
            spendingSlider.maxValue = totalAvailable;
            spendingSlider.value = spendingAmount;
        }

        if (dailyIncomeText != null)
            dailyIncomeText.text = $"Günlük Gelir: +{dailyIncome:F0}₺";
        
        if (currentMoneyText != null)
            currentMoneyText.text = $"Mevcut Para: {currentMoney:F0}₺";
            
        if (remainingMoneyText != null)
            remainingMoneyText.text = $"Toplam Para: {totalAvailable:F0}₺";

        if (spendingAmountText != null)
            spendingAmountText.text = $"Harcama: {spendingAmount:F0}₺";

        remainingMoney = totalAvailable - spendingAmount;
    }

    private void OnSpendingChanged(float value)
    {
        spendingAmount = value;
        UpdateMoneyUI();
    }

    private void UpdateDayUI()
    {
        if (currentDayText != null)
        {
            currentDayText.text = $"{currentDay}. Gün";
        }
    }

    public void OnConfirmClicked()
    {
        // Harcanan miktarı düş ve kalan parayı güncelle
        float totalAvailable = currentMoney + dailyIncome;
        currentMoney = totalAvailable - spendingAmount;
        spendingAmount = 0f; // Harcama miktarını sıfırla

        // Gün sayacını artır ve kaydet
        currentDay++;
        PlayerPrefs.SetInt("CurrentDay", currentDay);
        PlayerPrefs.Save();
        UpdateDayUI();

        gameRules.ProcessDailyExpenses();
        expensePanel.SetActive(false);
        mainPanel.SetActive(true);
    }
}