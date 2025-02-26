# Teacher Please - Proje Raporu

## 1. Proje Özeti
Teacher Please, Unity oyun motoru kullanılarak geliştirilmiş bir akademik yönetim simülasyon oyunudur. Oyuncular bir öğretim görevlisi rolünde, öğrenci talepleri ve yönetim beklentileri arasında denge kurmaya çalışırken aynı zamanda günlük masraflarını yönetmeye çalışırlar.

## 2. Teknik Altyapı

### 2.1 Temel Sistemler

#### GameRules Sistemi
- Oyunun ana mantık motoru
- Öğrenci memnuniyeti ve yönetim güveni mekaniklerinin yönetimi
- Ekonomi sistemi kontrolü
- Gün döngüsü yönetimi

Referans:
```csharp:Assets/Scripts/GameRules.cs
startLine: 8
endLine: 93
```

#### Request Sistemi
- Rastgele talep üretimi
- 9 farklı talep tipi (Not İtirazı, Süre Uzatma, Telafi Sınavı vb.)
- Karmaşık talep senaryoları
- Grup dilekçesi mekanizması

Referans:
```csharp:Assets/Scripts/RequestManager.cs
startLine: 103
endLine: 118
```

#### Ekonomi Sistemi
- Günlük maaş sistemi (100 TL)
- 4 farklı masraf kalemi:
  - Ev Kirası (45 TL)
  - Yemek Giderleri (35 TL)
  - Faturalar (20 TL)
  - Oda Giderleri (20 TL)
- Masraf takip ve ceza mekanizması

### 2.2 Kayıt Sistemi
- JSON tabanlı veri saklama
- Oyun durumu persistance
- Hata yönetimi

## 3. Oynanış Mekanikleri

### 3.1 Karar Verme Sistemi
- Her karar öğrenci ve yönetim barlarını etkiler
- Kararların parasal etkileri
- Özel durum senaryoları:
  - Karmaşık İstekler
  - Toplu Dilekçeler
  - Belirsiz Durumlar

### 3.2 Denge Sistemi
- Bar değerleri 0-100 arasında
- Kritik eşikler (10 ve 90)
- Her kararın çift yönlü etkisi

### 3.3 Ekonomi Yönetimi
- Günlük sabit masraflar
- 2 gün ödeme yapılmadığında oyun sonu
- Karar bazlı gelir sistemi:
  - Öğrenci yanlısı: +10 TL
  - Yönetim yanlısı: +20 TL

## 4. Kullanıcı Arayüzü
- Ana menü sistemi
- Dinamik bar göstergeleri
- Masraf yönetim paneli
- Oyun sonu ekranı

## 5. Geliştirme Sürecinde Karşılaşılan Zorluklar
1. Gün döngüsü ve masraf sisteminin senkronizasyonu
2. Karar mekaniklerinin dengelenmesi
3. UI state yönetimi
4. Veri persistance implementasyonu

## 6. Gelecek Geliştirmeler
1. Yeni talep tipleri
2. Karakter gelişim sistemi
3. Başarı sistemi
4. Detaylı istatistik ekranı
5. Zorluk seviyeleri
6. Ses efektleri ve müzik sistemi

## 7. Sonuç
Proje, temel hedeflerine ulaşmış ve çalışan bir simülasyon oyunu ortaya çıkarmıştır. Oyun mekanikleri başarıyla implemente edilmiş ve dengeli bir oynanış sunulmaktadır. İleride yapılacak güncellemelerle içerik zenginleştirilecek ve oynanış deneyimi geliştirilecektir.