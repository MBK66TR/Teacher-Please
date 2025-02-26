using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour
{
    [Header("Müzik Dosyaları")]
    [SerializeField] private AudioClip music1;
    [SerializeField] private AudioClip music2;
    
    [Header("Ayarlar")]
    [SerializeField] private float fadeTime = 2f; // Geçiş süresi
    [SerializeField] [Range(0f, 1f)] private float musicVolume = 0.5f;
    
    private AudioSource audioSource1;
    private AudioSource audioSource2;
    private int currentTrack = 1; // 1 veya 2
    
    private void Awake()
    {
        // Singleton pattern - sadece bir tane MusicManager olmasını sağlar
        if (FindObjectsOfType<MusicManager>().Length > 1)
        {
            Destroy(gameObject);
            return;
        }
        
        // Sahneler arası geçişte yok olmamasını sağla
        DontDestroyOnLoad(gameObject);
        
        // AudioSource'ları oluştur
        audioSource1 = gameObject.AddComponent<AudioSource>();
        audioSource2 = gameObject.AddComponent<AudioSource>();
        
        // AudioSource ayarlarını yap
        SetupAudioSource(audioSource1, music1);
        SetupAudioSource(audioSource2, music2);
        
        // İlk müziği başlat
        StartCoroutine(PlayMusicRoutine());
    }
    
    private void SetupAudioSource(AudioSource source, AudioClip clip)
    {
        source.clip = clip;
        source.volume = 0f;
        source.loop = false;
        source.playOnAwake = false;
    }
    
    private IEnumerator PlayMusicRoutine()
    {
        while (true)
        {
            AudioSource currentSource = (currentTrack == 1) ? audioSource1 : audioSource2;
            AudioSource nextSource = (currentTrack == 1) ? audioSource2 : audioSource1;
            
            // Mevcut müziği başlat ve fade in yap
            currentSource.Play();
            yield return StartCoroutine(FadeAudioSource(currentSource, 0f, musicVolume, fadeTime));
            
            // Müzik bitene kadar bekle (son 2 saniye kala geçiş başlasın)
            yield return new WaitForSeconds(currentSource.clip.length - (fadeTime * 2));
            
            // Sonraki müziği başlat
            nextSource.Play();
            
            // Cross-fade yap
            StartCoroutine(FadeAudioSource(currentSource, musicVolume, 0f, fadeTime));
            yield return StartCoroutine(FadeAudioSource(nextSource, 0f, musicVolume, fadeTime));
            
            // Mevcut müziği durdur
            currentSource.Stop();
            
            // Sıradaki müziğe geç
            currentTrack = (currentTrack == 1) ? 2 : 1;
        }
    }
    
    private IEnumerator FadeAudioSource(AudioSource source, float startVolume, float endVolume, float duration)
    {
        float elapsed = 0f;
        
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            source.volume = Mathf.Lerp(startVolume, endVolume, elapsed / duration);
            yield return null;
        }
        
        source.volume = endVolume;
    }
    
    // Müzik sesini ayarlamak için public metod
    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);
        audioSource1.volume = musicVolume;
        audioSource2.volume = musicVolume;
    }
} 