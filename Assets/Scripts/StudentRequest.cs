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
    GradeAppeal,      // Not İtirazı
    ExtensionRequest, // Süre Uzatma
    MakeupExam,       // Telafi Sınavı
    SpecialPermission,// Özel İzin
    CourseOverload,   // Fazla Ders
    LateSubmission,   // Geç Teslim
    ComplexRequest,   // Karmaşık İstek - yeni eklenen
    GroupPetition,    // Toplu Dilekçe - yeni eklenen
    UnclearSituation  // Belirsiz Durum - yeni eklenen
}
