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
    [SerializeField] private TextMeshProUGUI spendingAmountText; // Gün sayacı için UI elemanı
    
    private float currentMoney = 80f; // Başlangıç parası1
    private float remainingMoney; // Kalan para
    private float dailyIncome;
    private float spendingAmount = 0f;
    private Dictionary<ExpenseType, int> missedExpenseCounts;

    private const int MAX_MISSED_EXPENSES = 2;
    
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
        if (confirmButton != null)
            confirmButton.onClick.AddListener(OnConfirmClicked);

        if (spendingSlider != null)
        {
            spendingSlider.onValueChanged.AddListener(OnSpendingChanged);
            spendingSlider.minValue = 0f;
        }

        gameRules = FindObjectOfType<GameRules>();
        
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
        if (missedCount >= 1) // İlk günden sonra uyarı
        {
            item.descriptionText.color = Color.red;
            item.descriptionText.text = $"{GetExpenseDescription(item.type)} (Uyarı: {missedCount} gündür ödenmedi!)";
            
            // MAX_MISSED_EXPENSES kontrolü
            if (missedCount >= MAX_MISSED_EXPENSES)
            {
                if (gameRules != null)
                {
                    gameRules.GameOver($"{GetExpenseDescription(item.type)} {MAX_MISSED_EXPENSES} gün ödenmedi!");
                }
            }
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
            dailyIncomeText.text = $"Günlük Maaş: {gameRules.GetDailySalary():F0} TL";
        
        if (currentMoneyText != null)
            currentMoneyText.text = $"Mevcut Para: {gameRules.GetCurrentMoney():F0} TL";
            
        if (remainingMoneyText != null)
            remainingMoneyText.text = $"Kalan Para: {remainingMoney:F0} TL";

        if (spendingAmountText != null)
            spendingAmountText.text = $"{spendingAmount:F0} TL";

        remainingMoney = totalAvailable - spendingAmount;
    }

    private void OnSpendingChanged(float value)
    {
        spendingAmount = value;
        UpdateMoneyUI();
    }


    public void OnConfirmClicked()
    {
        var costs = gameRules.GetExpenseCosts();
        
        // Her expense için kontrol et
        foreach (var item in expenseItems)
        {
            if (!item.toggle.isOn) // Eğer ödeme yapılmadıysa
            {
                missedExpenseCounts[item.type]++;
              // Eğer MAX_MISSED_EXPENSES'e ulaştıysa oyunu bitir
                if (missedExpenseCounts[item.type] >= MAX_MISSED_EXPENSES)
                {
                    gameRules.GameOver($"{GetExpenseDescription(item.type)} {MAX_MISSED_EXPENSES} gün ödenmedi!");
                    return;
                }
            }
            else
            {
                missedExpenseCounts[item.type] = 0;
            }
            
            UpdateExpenseItemUI(item);
        }
        
        // Oyun bitmemişse normal akışa devam et
        gameRules.ProcessDailyExpenses();
    }
}