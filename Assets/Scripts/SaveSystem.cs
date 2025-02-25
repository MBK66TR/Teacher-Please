using UnityEngine;
using System.IO;
using System;

public class SaveSystem : MonoBehaviour
{
    [System.Serializable]
    private class GameSave
    {
        public float studentSatisfaction;
        public float administrationTrust;
        public int currentDay;
        public int requestsHandledToday;
        public bool isGameOver;
    }

    private string SavePath => Path.Combine(Application.persistentDataPath, "gamesave.json");

    public void SaveGame(GameRules gameRules)
    {
        var save = new GameSave
        {
            studentSatisfaction = gameRules.GetStudentSatisfaction(),
            administrationTrust = gameRules.GetAdministrationTrust(),
            currentDay = gameRules.GetCurrentDay(),
            requestsHandledToday = 0,
            isGameOver = gameRules.IsGameOver()
        };

        string json = JsonUtility.ToJson(save);
        File.WriteAllText(SavePath, json);
        
        Debug.Log($"Oyun kaydedildi: {SavePath}");
    }

    public bool LoadGame(GameRules gameRules)
    {
        try
        {
            if (File.Exists(SavePath))
            {
                string json = File.ReadAllText(SavePath);
                var save = JsonUtility.FromJson<GameSave>(json);

                // GameRules'a yeni bir LoadGame metodu eklememiz gerekiyor
                gameRules.LoadGame(
                    save.studentSatisfaction,
                    save.administrationTrust,
                    save.currentDay,
                    save.isGameOver
                );

                Debug.Log("Oyun yüklendi!");
                return true;
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Yükleme hatası: {e.Message}");
        }
        return false;
    }
} 