using UnityEngine;   using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMenuManager : MonoBehaviour
{
    [Header("UI Buttons")]
    public Button startButton;
    public Button optionsButton;
    public Button quitButton;

    [Header("Audio Sources")]
    public AudioSource backgroundMusic; // Menü müziği
    public AudioSource menuSFX;         // Buton sesi

    [Header("Scene Names")]
    public string gameSceneName = "CityScene";
    public string menuSceneName = "SampleScene";

    void Start()
    {
        // Butonlara fonksiyonları bağla
        if (startButton != null)
            startButton.onClick.AddListener(StartGame);
        if (optionsButton != null)
            optionsButton.onClick.AddListener(OpenOptions);
        if (quitButton != null)
            quitButton.onClick.AddListener(QuitGame);

        // Arka plan müziğini başlat (sahnede varsa)
        if (backgroundMusic != null && !backgroundMusic.isPlaying)
            backgroundMusic.Play();
    }

    void PlayButtonSound()
    {
        if (menuSFX != null)
            menuSFX.Play();
    }

    public void StartGame()
    {
        PlayButtonSound();
        // SFX sesi bittikten sonra sahne geçişi
        StartCoroutine(LoadSceneAfterSound(gameSceneName));
    }

    public void OpenOptions()
    {
        PlayButtonSound();
        Debug.Log("Options menüsü açılacak (henüz eklenmedi).");
    }

    public void QuitGame()
    {
        PlayButtonSound();
        Debug.Log("Oyun kapatılıyor...");
        Application.Quit();
    }

    System.Collections.IEnumerator LoadSceneAfterSound(string sceneName)
    {
        // Ses bitene kadar bekle (varsa)
        if (menuSFX != null)
            yield return new WaitForSeconds(menuSFX.clip.length);

        SceneManager.LoadScene(sceneName);
    }
} 
    
    
    
 

