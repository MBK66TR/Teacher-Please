using UnityEngine;
using System.Collections.Generic;

public class RequestGenerator : MonoBehaviour
{
    private string[] turkishNames = { "Ahmet", "Mehmet", "Ayşe", "Fatma", "Can", "Deniz", "Ece", "Burak" };
    
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

    public StudentRequest GenerateRandomRequest()
    {
        RequestType randomType = (RequestType)Random.Range(0, System.Enum.GetValues(typeof(RequestType)).Length);
        string randomName = turkishNames[Random.Range(0, turkishNames.Length)];
        
        var template = requestTemplates[randomType];
        string description = string.Format(template.descriptionTemplate, 
            GetRandomContextForType(randomType));

        return new StudentRequest(
            randomName,
            template.title,
            description,
            randomType,
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