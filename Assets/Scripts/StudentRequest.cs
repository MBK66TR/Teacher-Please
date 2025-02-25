using UnityEngine;

[System.Serializable]
public class StudentRequest
{
    public string studentName;
    public string requestTitle;
    public string requestDescription;
    public RequestType requestType;
    public StudentType studentType;
    public float studentImpact;
    public float adminImpact;

    public StudentRequest(string name, string title, string description, RequestType type, StudentType sType, float sImpact, float aImpact)
    {
        studentName = name;
        requestTitle = title;
        requestDescription = description;
        requestType = type;
        studentType = sType;
        studentImpact = sImpact;
        adminImpact = aImpact;
    }
}

public enum RequestType
{
    GradeAppeal,         // Not itirazı
    ExtensionRequest,    // Süre uzatma talebi
    MakeupExam,         // Telafi sınavı
    SpecialPermission,   // Özel izin
    CourseOverload,      // Fazla ders alma
    LateSubmission      // Geç teslim
} 