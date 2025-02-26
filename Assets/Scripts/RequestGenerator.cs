using UnityEngine;
using System.Collections.Generic;

public class RequestGenerator : MonoBehaviour
{
    private string[] maleNames = { 
        "Ahmet", "Mehmet", "Can", "Burak", "Ali", "Mustafa", "Emre", "Yusuf", 
        "Eren", "Murat", "Kemal", "Serkan", "Hakan", "Ozan", "Cem" 
    };
    
    private string[] femaleNames = { 
        "Ayşe", "Fatma", "Ece", "Zeynep", "Elif", "Merve", "Selin", "Deniz", 
        "Melis", "Ceren", "Yağmur", "Esra", "Büşra", "Gizem", "Aslı" 
    };
    
    private string[] unisexNames = {
        "Deniz", "Ege", "Özgür", "Barış", "Kaya", "Yağız", "Toprak"
    };
    
    private Dictionary<RequestType, RequestTemplate> requestTemplates = new Dictionary<RequestType, RequestTemplate>();

    void Awake()
    {
        InitializeTemplates();
    }

    private void InitializeTemplates()
    {
        requestTemplates.Add(RequestType.GradeAppeal, new RequestTemplate(
            "Not İtirazı",
            "Hocam finalden aldığım {0} notu hakkında itirazım var. Kağıdımın tekrar değerlendirilmesini talep ediyorum.",
            5f, 8f, 3f, 7f
        ));

        requestTemplates.Add(RequestType.ExtensionRequest, new RequestTemplate(
            "Süre Uzatma",
            "Hocam projeyi teslim etmem için 2 gün ek süre alabilir miyim? {0}",
            4f, 7f, 2f, 5f
        ));

        requestTemplates.Add(RequestType.MakeupExam, new RequestTemplate(
            "Telafi Sınavı",
            "Hocam {0} tarihindeki sınava sağlık sorunları nedeniyle katılamadım. Telafi sınavına girebilir miyim?",
            6f, 9f, 4f, 8f
        ));

        requestTemplates.Add(RequestType.SpecialPermission, new RequestTemplate(
            "Özel İzin",
            "Hocam {0} nedeniyle dersten erken çıkmam gerekiyor. İzin verebilir misiniz?",
            3f, 6f, 2f, 4f
        ));

        requestTemplates.Add(RequestType.CourseOverload, new RequestTemplate(
            "Fazla Ders",
            "Hocam bu dönem kredi limitinin üzerinde {0} kredilik ders almak istiyorum. Onaylayabilir misiniz?",
            7f, 9f, 5f, 8f
        ));

        requestTemplates.Add(RequestType.LateSubmission, new RequestTemplate(
            "Geç Teslim",
            "Hocam ödevi {0} gün geç teslim etmem mümkün mü? {1}",
            4f, 7f, 3f, 6f
        ));
    }

    private string GetRandomNameForGender(bool isMale)
    {
        float randomValue = Random.value;
        
        // %10 şansla unisex isim seç
        if (randomValue < 0.1f)
        {
            return unisexNames[Random.Range(0, unisexNames.Length)];
        }
        
        // %90 şansla cinsiyete uygun isim seç
        if (isMale)
        {
            return maleNames[Random.Range(0, maleNames.Length)];
        }
        else
        {
            return femaleNames[Random.Range(0, femaleNames.Length)];
        }
    }

    private StudentType GetRandomStudentTypeForGender(bool isMale)
    {
        // Öğrenci tiplerini cinsiyete göre grupla
        StudentType[] types = isMale ? 
            new StudentType[] { 
                StudentType.MaleRegular, 
                StudentType.MaleNerd, 
                StudentType.MaleAthlete, 
                StudentType.MaleRebel, 
                StudentType.MaleRich 
            } :
            new StudentType[] { 
                StudentType.FemaleRegular, 
                StudentType.FemaleNerd, 
                StudentType.FemaleAthlete, 
                StudentType.FemaleRebel, 
                StudentType.FemaleRich 
            };
            
        return types[Random.Range(0, types.Length)];
    }

    public StudentRequest GenerateRandomRequest()
    {
        RequestType randomType = (RequestType)Random.Range(0, System.Enum.GetValues(typeof(RequestType)).Length);
        bool isMale = Random.value > 0.5f; // %50 şansla erkek/kadın
        string randomName = GetRandomNameForGender(isMale);
        StudentType studentType = GetRandomStudentTypeForGender(isMale);
        
        var template = requestTemplates[randomType];
        string description;
        
        // LateSubmission için özel işlem
        if (randomType == RequestType.LateSubmission)
        {
            string[] contextValues = GetRandomContextForType(randomType).Split('|');
            description = string.Format(template.descriptionTemplate, contextValues[0], contextValues[1]);
        }
        else
        {
            description = string.Format(template.descriptionTemplate, GetRandomContextForType(randomType));
        }

        return new StudentRequest(
            randomName,
            template.title,
            description,
            randomType,
            studentType,
            Random.Range(template.minStudentImpact, template.maxStudentImpact),
            Random.Range(template.minAdminImpact, template.maxAdminImpact)
        );
    }

    private string GetRandomContextForType(RequestType type)
    {
        switch (type)
        {
            case RequestType.GradeAppeal:
                return Random.Range(30, 65).ToString();
            
            case RequestType.ExtensionRequest:
                return "Ailemdeki sağlık sorunları nedeniyle vakit bulamadım.";
            
            case RequestType.MakeupExam:
                return System.DateTime.Now.AddDays(-Random.Range(1, 10)).ToString("dd/MM/yyyy");
            
            case RequestType.SpecialPermission:
                string[] reasons = {
                    "ailemden birinin doktor randevusu",
                    "acil diş tedavisi",
                    "önemli bir iş görüşmesi",
                    "aile ziyareti"
                };
                return reasons[Random.Range(0, reasons.Length)];
            
            case RequestType.CourseOverload:
                return Random.Range(3, 9).ToString();
            
            case RequestType.LateSubmission:
                string[] excuses = {
                    "Bilgisayarım bozuldu.",
                    "İnternette sorun yaşadım.",
                    "Hastalık nedeniyle geciktim.",
                    "Dosyalarım kayboldu."
                };
                return string.Format("{0}|{1}", 
                    Random.Range(1, 5).ToString(),
                    excuses[Random.Range(0, excuses.Length)]);
            
            default:
                return "";
        }
    }

    private class RequestTemplate
    {
        public string title;
        public string descriptionTemplate;
        public float minStudentImpact;
        public float maxStudentImpact;
        public float minAdminImpact;
        public float maxAdminImpact;

        public RequestTemplate(string title, string descTemplate, 
            float minStudent, float maxStudent, 
            float minAdmin, float maxAdmin)
        {
            this.title = title;
            this.descriptionTemplate = descTemplate;
            this.minStudentImpact = minStudent;
            this.maxStudentImpact = maxStudent;
            this.minAdminImpact = minAdmin;
            this.maxAdminImpact = maxAdmin;
        }
    }
} 