using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RequestManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI requestText;
    [SerializeField] private TextMeshProUGUI studentNameText;
    [SerializeField] private StudentManager studentManager;
    [SerializeField] private GameRules gameRules;
    [SerializeField] private Button acceptButton;
    [SerializeField] private Button rejectButton;
    
    private RequestGenerator requestGenerator;
    private StudentRequest currentRequest;

    void Start()
    {
        requestGenerator = GetComponent<RequestGenerator>();
        
        // Butonlara click event'lerini ekle
        if (acceptButton != null)
            acceptButton.onClick.AddListener(HandleAccept);
        if (rejectButton != null)
            rejectButton.onClick.AddListener(HandleReject);
            
        GenerateNewRequest();
    }

    public void GenerateNewRequest()
    {
        if (gameRules.IsGameOver()) return;
        
        currentRequest = requestGenerator.GenerateRandomRequest();
        
        if (studentManager != null)
        {
            // UI'ı öğrenci vardığında güncelle
            studentManager.SpawnNewStudent(currentRequest.studentType, () => {
                UpdateUI(currentRequest);
            });
        }
    }

    private void UpdateUI(StudentRequest request)
    {
        if (requestText != null)
            requestText.text = request.requestDescription;
        if (studentNameText != null)
            studentNameText.text = request.studentName;
    }

    public void HandleAccept()
    {
        HandleDecision(true);
    }

    public void HandleReject()
    {
        HandleDecision(false);
    }

    public void HandleDecision(bool isProStudent)
    {
        if (currentRequest == null || gameRules.IsGameOver()) return;

        gameRules.HandleRequest(isProStudent);
        GenerateNewRequest();
    }
} 