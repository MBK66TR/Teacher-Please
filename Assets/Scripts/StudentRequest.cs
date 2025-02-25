using UnityEngine;

[System.Serializable]
public class StudentRequest
{
    public string studentName;
    public string requestTitle;
    public string description;
    public RequestType type;
    public float studentImpact;
    public float administrationImpact;

    public StudentRequest(string name, string title, string desc, RequestType type, 
        float studentImp, float adminImp)
    {
        studentName = name;
        requestTitle = title;
        description = desc;
        this.type = type;
        studentImpact = studentImp;
        administrationImpact = adminImp;
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