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

        requestTemplates.Add(RequestType.ComplexRequest, new RequestTemplate(
            "Karmaşık İstek",
            "{0}",
            8f, 12f, 6f, 10f
        ));

        requestTemplates.Add(RequestType.GroupPetition, new RequestTemplate(
            "Toplu Dilekçe",
            "Bir grup öğrenci {0} konusunda dilekçe verdi. Nasıl yanıtlamak istersiniz?",
            10f, 15f, 8f, 12f
        ));

        requestTemplates.Add(RequestType.UnclearSituation, new RequestTemplate(
            "Belirsiz Durum",
            "{0}",
            7f, 13f, 5f, 11f
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
            
            case RequestType.ComplexRequest:
                string[] complexRequests = {
                    "Bir öğrenci grubu, final sınavlarının stres seviyesini düşürmek için alternatif değerlendirme yöntemleri öneriyor. Kabul edilirse diğer bölümlerde de emsal oluşturabilir.",
                    "Engelli bir öğrenci için özel düzenlemeler talep ediliyor, ancak bu düzenlemeler diğer öğrencilerin de faydalanmak isteyebileceği imkanlar içeriyor.",
                    "Öğrenci konseyi, kampüsteki yemek şirketinin değiştirilmesini talep ediyor. Mevcut şirketle uzun vadeli sözleşme var, ama şikayetler artıyor.",
                    "Bir grup öğrenci, derslerin %30'unun online yapılmasını öneriyor. Bu değişiklik bazı öğrenciler için avantajlı olacakken, bazıları için dezavantaj oluşturabilir.",
                    "Uluslararası bir öğrenci değişim programı için kontenjan artırımı isteniyor, ancak bu mevcut öğrencilerin staj imkanlarını etkileyebilir.",
                    "Öğrenciler, bir hocanın ders anlatım tarzının değişmesi için imza topluyor. Hoca deneyimli ve saygın biri, ama modern öğretim tekniklerine uzak.",
                    "Bir araştırma projesinde etik ihlal şüphesi var. Projeyi durdurmak prestij kaybına neden olabilir, devam ettirmek ise riskli olabilir.",
                    "Öğrenci kulüpleri için yeni bir bütçe sistemi öneriliyor, ama bu bazı kulüplerin bütçesinin azalması anlamına geliyor."
                };
                return complexRequests[Random.Range(0, complexRequests.Length)];

            case RequestType.GroupPetition:
                string[] petitionTopics = {
                    "sınav tarihlerinin değiştirilmesi",
                    "yeni bir laboratuvar açılması",
                    "kantindeki fiyatların düşürülmesi",
                    "kütüphane çalışma saatlerinin uzatılması",
                    "uzaktan eğitim seçeneği sunulması"
                };
                return petitionTopics[Random.Range(0, petitionTopics.Length)];

            case RequestType.UnclearSituation:
                string[] unclearSituations = {
                    "Bir grup öğrenci, başka bir öğrencinin sosyal medyada yaptığı paylaşımların rahatsız edici olduğunu söylüyor, ancak ifade özgürlüğü tartışması var.",
                    "Bir öğrenci, derste verilen projenin telif haklı bir içerik kullandığını iddia ediyor, ama durum belirsiz.",
                    "Bazı öğrenciler, bir hocanın sınavda bazı öğrencilere ayrıcalık tanıdığını iddia ediyor, ancak somut kanıt yok.",
                    "Öğrenci kulübünün düzenlediği etkinlikte yaşanan bir tartışma, fakülte içinde gerginlik yaratıyor.",
                    "Bir öğrenci, mental sağlık sorunları nedeniyle özel muamele talep ediyor, ancak resmi bir rapor henüz yok.",
                    "Yabancı öğrenciler dil bariyeri nedeniyle bazı derslerde zorlandıklarını söylüyor, ama çözüm karmaşık.",
                    "Bir grup öğrenci, ders içeriğinin güncel teknolojileri kapsamadığını iddia ediyor, ancak müfredat değişikliği zor.",
                    "Öğrenciler arasında politik bir tartışma akademik ortamı etkiliyor, müdahale edilmeli mi tartışmalı."
                };
                return unclearSituations[Random.Range(0, unclearSituations.Length)];

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