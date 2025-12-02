using UnityEngine;
using System.Collections.Generic;
using TMPro; 
using UnityEngine.UI;
using UnityEngine.SceneManagement; 

// --- 1. VERİ MODELİ ---
[System.Serializable]
public class Soru
{
    public int kategoriID;
    public string soruMetni;
    public string A, B, C, D; 
    public string dogruCevap;

    public Soru(int id, string soru, string a, string b, string c, string d, string dogru)
    {
        this.kategoriID = id;
        this.soruMetni = soru;
        this.A = a;
        this.B = b;
        this.C = c;
        this.D = d;
        this.dogruCevap = dogru;
    }
}

// --- 2. YÖNETİCİ SINIF ---
public class DatabaseManager : MonoBehaviour
{
    [Header("SORU EKRANI NESNELERİ")]
    public TextMeshProUGUI puanText;           
    public TextMeshProUGUI canText; 
    public TextMeshProUGUI soruMetniText;      
    public TextMeshProUGUI secenekAText;       
    public TextMeshProUGUI secenekBText;       
    public TextMeshProUGUI secenekCText;       
    public TextMeshProUGUI secenekDText;       
    public TextMeshProUGUI sonucText;          

    [Header("KARAR PANELİ")]
    public GameObject kararPaneli; 
    public Button btnHarca;        
    public Button btnDevam;        

    [Header("ŞIK BUTONLARI")]
    public Button butonA;
    public Button butonB;
    public Button butonC;
    public Button butonD;

    [Header("AYARLAR")]
    public float beklemeSuresi = 3f;

    [HideInInspector]
    public List<Soru> tumSorularHavuzu = new List<Soru>();

    private List<Soru> aktifSoruListesi;
    private string dogruSik;
    private int toplamPuan = 0;
    private int kalanCan = 3;
    private int sonKontrolPuani = 0;
    private bool cevapVerildiMi = false;

    void Start()
    {
        // Butonları Kur
        butonA.onClick.RemoveAllListeners(); butonA.onClick.AddListener(() => CevapVer("A"));
        butonB.onClick.RemoveAllListeners(); butonB.onClick.AddListener(() => CevapVer("B"));
        butonC.onClick.RemoveAllListeners(); butonC.onClick.AddListener(() => CevapVer("C"));
        butonD.onClick.RemoveAllListeners(); butonD.onClick.AddListener(() => CevapVer("D"));

        if(btnHarca) { btnHarca.onClick.RemoveAllListeners(); btnHarca.onClick.AddListener(MarketSahnesineGit); }
        if(btnDevam) { btnDevam.onClick.RemoveAllListeners(); btnDevam.onClick.AddListener(OyunaDevamEt); }

        if(kararPaneli) kararPaneli.SetActive(false);

        VeritabaniniDoldur(); // Soruları yükle
        
        toplamPuan = 0;
        kalanCan = 3;
        UI_Guncelle();

        // --- KRİTİK NOKTA: HAFIZAYI OKU ---
        // Menü sahnesinde kaydedilen ID'yi çekiyoruz.
        // Eğer kayıt yoksa varsayılan olarak 1 (Sinir) gelsin.
        int gelenID = PlayerPrefs.GetInt("SecilenKategoriID", 1);
        
        Debug.Log("Menüden gelen seçim ID: " + gelenID);
        KategoriBaslat(gelenID);
    }

    // --- OYUN MANTIĞI ---
    public void KategoriBaslat(int istenenID)
    {
        aktifSoruListesi = tumSorularHavuzu.FindAll(x => x.kategoriID == istenenID);

        if (aktifSoruListesi != null && aktifSoruListesi.Count > 0)
        {
            ListeyiKaristir(aktifSoruListesi);
            SoruGoster(0);
        }
        else
        {
            if(sonucText) sonucText.text = "Bu bölge için soru bulunamadı! (ID: " + istenenID + ")";
        }
    }

    public void SoruGoster(int index)
    {
        if (aktifSoruListesi == null || index >= aktifSoruListesi.Count)
        {
            if(sonucText) sonucText.text = "Bölüm Bitti! Menüye dönülüyor...";
            Invoke("AnaMenuyeDon", 3f);
            return;
        }

        cevapVerildiMi = false; 
        Soru s = aktifSoruListesi[index];
        soruMetniText.text = s.soruMetni;
        secenekAText.text = "A) " + s.A;
        secenekBText.text = "B) " + s.B;
        secenekCText.text = "C) " + s.C;
        secenekDText.text = "D) " + s.D;
        dogruSik = s.dogruCevap;
        if(sonucText) sonucText.text = ""; 
    }

    void CevapVer(string secilenSik)
    {
        if (cevapVerildiMi) return;
        cevapVerildiMi = true;

        if (secilenSik == dogruSik)
        {
            toplamPuan += 50;
            sonucText.text = "<color=green>TEBRİKLER! DOĞRU.</color>";
            
            if (toplamPuan > 0 && toplamPuan % 500 == 0 && toplamPuan > sonKontrolPuani)
            {
                sonKontrolPuani = toplamPuan;
                Invoke("PaneliAc", 1.5f);
            }
            else Invoke("SonrakiSoru", 1.5f);
        }
        else
        {
            kalanCan--;
            Soru anlikSoru = aktifSoruListesi[0];
            string dogruMetin = (dogruSik=="A"?anlikSoru.A : (dogruSik=="B"?anlikSoru.B : (dogruSik=="C"?anlikSoru.C : anlikSoru.D)));
            sonucText.text = $"<color=red>YANLIŞ!</color>\nDoğru Cevap: <color=yellow>{dogruMetin}</color>";

            if (kalanCan <= 0) Invoke("MarketSahnesineGit", 4f);
            else Invoke("SonrakiSoru", beklemeSuresi);
        }
        UI_Guncelle();
    }

    public void SonrakiSoru()
    {
        if (aktifSoruListesi.Count > 0)
        {
            aktifSoruListesi.RemoveAt(0); 
            if (aktifSoruListesi.Count > 0) SoruGoster(0);
            else { sonucText.text = "Sorular Bitti!"; Invoke("AnaMenuyeDon", 3f); }
        }
    }

    void AnaMenuyeDon() { SceneManager.LoadScene("MenuScene"); } 
    void PaneliAc() { if(kararPaneli) kararPaneli.SetActive(true); }
    void OyunaDevamEt() { if(kararPaneli) kararPaneli.SetActive(false); SonrakiSoru(); }
    void MarketSahnesineGit() { PlayerPrefs.SetInt("KazanilanPuan", toplamPuan); SceneManager.LoadScene("MarketScene"); }
    void UI_Guncelle() { if(puanText) puanText.text = "PUAN: " + toplamPuan; if(canText) canText.text = "CAN: " + kalanCan; }
    void ListeyiKaristir(List<Soru> liste) { for (int i = 0; i < liste.Count; i++) { Soru temp = liste[i]; int rnd = Random.Range(i, liste.Count); liste[i] = liste[rnd]; liste[rnd] = temp; } }

    void VeritabaniniDoldur()
    {
        tumSorularHavuzu.Clear();
        // ID 1: Sinir
        tumSorularHavuzu.Add(new Soru(1, "Beyin hangi kemik yapısının içinde yer alır?", "Kas", "Kalp", "Deri", "Kafatası", "D"));
        tumSorularHavuzu.Add(new Soru(1, "Vücut sıcaklığını düzenleyen beyin bölgesi hangisidir?", "Hipofiz", "Hipotalamus", "Hipokamp", "Epitalamus", "B"));
        // ID 2: Dolaşım
        tumSorularHavuzu.Add(new Soru(2, "Hangi organ oksijenli kanı vücuda pompalar?", "Kas", "Kalp", "Deri", "Akciğer", "B"));
        // ID 3: İskelet
        tumSorularHavuzu.Add(new Soru(3, "İnsan vücudunda kaç kemik vardır?", "206", "200", "204", "208", "A"));
        tumSorularHavuzu.Add(new Soru(3, "Kemik iliği ne üretir?", "Kas", "Kan", "Sperm", "Kemik", "B"));
        // ... Diğer soruların burada ...
        
        Debug.Log("✅ Veritabanı Hazır.");
    }
}