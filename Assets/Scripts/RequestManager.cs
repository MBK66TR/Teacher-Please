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
    private bool isWaitingForStudentToLeave = false;

    void Start()
    {
        requestGenerator = GetComponent<RequestGenerator>();
        
        if (acceptButton != null)
            acceptButton.onClick.AddListener(HandleAccept);
        if (rejectButton != null)
            rejectButton.onClick.AddListener(HandleReject);
            
        GenerateNewRequest();
    }

    public void GenerateNewRequest()
    {
        if (gameRules.IsGameOver() || isWaitingForStudentToLeave) return;
        
        currentRequest = requestGenerator.GenerateRandomRequest();
        
        if (studentManager != null)
        {
            studentManager.SpawnNewStudent(currentRequest.studentType, () => {
                UpdateUI(currentRequest);
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
            
        SetButtonsInteractable(true);
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
            default: return "Bilinmeyen";
        }
    }

    public void HandleDecision(bool isProStudent)
    {
        if (currentRequest == null || gameRules.IsGameOver() || isWaitingForStudentToLeave) return;

        gameRules.HandleRequest(isProStudent);
        
        SetButtonsInteractable(false);
        
        isWaitingForStudentToLeave = true;
        studentManager.DismissCurrentStudent(() => {
            isWaitingForStudentToLeave = false;
            GenerateNewRequest();
        });
    }
}