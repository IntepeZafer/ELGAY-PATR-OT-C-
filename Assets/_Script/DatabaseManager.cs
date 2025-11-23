using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class Secenekler
{
    public string A;
    public string B;
    public string C;
    public string D;
}

[System.Serializable]
public class Soru
{
    public string soruMetni;
    public string cevap;
    public bool cozuldumu;
    public Secenekler secenekler;
    public string DogruSecenek; 
}

[System.Serializable]
public class QuizRoot
{
    public List<Dictionary<string, List<Soru>>> kategoriler;
}

public class DatabaseManager : MonoBehaviour
{
    public QuizRoot loadedQuizData;

    private const string JSON_FILE_NAME = "sorular.json";

    void Start()
    {
        LoadDataFromJSON();
    }

    public void LoadDataFromJSON()
    {
        string path = Path.Combine(Application.streamingAssetsPath, JSON_FILE_NAME);

        if (!File.Exists(path))
        {
            Debug.LogError("❌ JSON dosyası bulunamadı: " + path);
            return;
        }

        try
        {
            string jsonString = File.ReadAllText(path);

            loadedQuizData = JsonUtility.FromJson<QuizRoot>(jsonString); // Bu satırı kullanın

            if (loadedQuizData == null || loadedQuizData.kategoriler == null || loadedQuizData.kategoriler.Count == 0)
            {
                Debug.LogError("❌ JSON ayrıştırıldı fakat içerik boş veya hatalı.");
                return;
            }

            Debug.Log("✅ JSON başarıyla yüklendi!");

            // İlk kategori dictionary
            var firstCategoryDict = loadedQuizData.kategoriler[0];

            // Kategori adı
            string categoryName = firstCategoryDict.Keys.First();

            // Soru listesi
            List<Soru> sorular = firstCategoryDict[categoryName];

            Debug.Log($"Kategori Adı: {categoryName}");
            Debug.Log($"Toplam Soru Sayısı: {sorular.Count}");

        }
        catch (System.Exception ex)
        {
            Debug.LogError("❌ JSON okunurken hata oluştu: " + ex.Message);
        }
    }
}