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
    
    [Header("UI Panelleri")]
    [SerializeField] private GameObject requestPanel; // İstek açıklamasının olduğu panel
    [SerializeField] private GameObject namePanel;    // İsim paneli

    private RequestGenerator requestGenerator;
    private StudentRequest currentRequest;
    private bool isWaitingForStudentToLeave = false;

    void Start()
    {
        requestGenerator = GetComponent<RequestGenerator>();
        
        if (acceptButton != null)
            acceptButton.onClick.AddListener(HandleAccept);
        if (rejectButton != null)
            rejectButton.onClick.AddListener(HandleReject);
            
        // Başlangıçta panelleri gizle
        HideUI();
        
        GenerateNewRequest();
    }

    private void HideUI()
    {
        if (requestPanel != null)
            requestPanel.SetActive(false);
        if (namePanel != null)
            namePanel.SetActive(false);
        SetButtonsInteractable(false);
    }

    private void ShowUI()
    {
        if (requestPanel != null)
            requestPanel.SetActive(true);
        if (namePanel != null)
            namePanel.SetActive(true);
        SetButtonsInteractable(true);
    }

    public void GenerateNewRequest()
    {
        if (gameRules.IsGameOver() || isWaitingForStudentToLeave) return;
        
        // UI'ı gizle
        HideUI();
        
        currentRequest = requestGenerator.GenerateRandomRequest();
        
        if (studentManager != null)
        {
            studentManager.SpawnNewStudent(currentRequest.studentType, () => {
                UpdateUI(currentRequest);
                ShowUI(); // Öğrenci yerine geldiğinde UI'ı göster
            });
        }
    }

    private void UpdateUI(StudentRequest request)
    {
        if (requestText != null)
        {
            requestText.text = request.requestDescription;
            requestText.color = Color.black;
        }
        
        if (studentNameText != null)
            studentNameText.text = request.studentName;
    }

    private void SetButtonsInteractable(bool interactable)
    {
        if (acceptButton != null)
            acceptButton.interactable = interactable;
        if (rejectButton != null)
            rejectButton.interactable = interactable;
    }

    public void HandleAccept()
    {
        HandleDecision(true);
    }

    public void HandleReject()
    {
        HandleDecision(false);
    }

    private string GetRequestTypeName(RequestType type)
    {
        switch (type)
        {
            case RequestType.GradeAppeal: return "Not İtirazı";
            case RequestType.ExtensionRequest: return "Süre Uzatma";
            case RequestType.MakeupExam: return "Telafi Sınavı";
            case RequestType.SpecialPermission: return "Özel İzin";
            case RequestType.CourseOverload: return "Fazla Ders";
            case RequestType.LateSubmission: return "Geç Teslim";
            case RequestType.ComplexRequest: return "Karmaşık İstek";
            case RequestType.GroupPetition: return "Toplu Dilekçe";
            case RequestType.UnclearSituation: return "Belirsiz Durum";
            default: return "Bilinmeyen";
        }
    }

    private void HandleDecision(bool isApproved)
    {
        if (gameRules != null)
        {
            gameRules.SetCurrentRequest(currentRequest);
            gameRules.HandleRequest(isApproved);
        }

        isWaitingForStudentToLeave = true;
        SetButtonsInteractable(false);

        if (studentManager != null)
        {
            studentManager.DismissCurrentStudent(() => {
                isWaitingForStudentToLeave = false;
                GenerateNewRequest();
            });
        }
    }


    

}