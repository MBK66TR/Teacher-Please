using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RequestManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI studentNameText;
    [SerializeField] private TextMeshProUGUI requestTitleText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private Button acceptButton;
    [SerializeField] private Button rejectButton;
    
    private GameRules gameRules;
    private RequestGenerator requestGenerator;
    private StudentRequest currentRequest;

    void Start()
    {
        gameRules = FindObjectOfType<GameRules>();
        requestGenerator = FindObjectOfType<RequestGenerator>();
        
        acceptButton.onClick.AddListener(() => HandleDecision(true));
        rejectButton.onClick.AddListener(() => HandleDecision(false));
        
        GenerateNewRequest();
    }

    void GenerateNewRequest()
    {
        if (gameRules.IsGameOver()) return;
        
        currentRequest = requestGenerator.GenerateRandomRequest();
        UpdateUI();
    }

    void UpdateUI()
    {
        studentNameText.text = currentRequest.studentName;
        requestTitleText.text = currentRequest.requestTitle;
        descriptionText.text = currentRequest.description;
    }

    void HandleDecision(bool isProStudent)
    {
        gameRules.HandleRequest(isProStudent);
        GenerateNewRequest();
    }
} 