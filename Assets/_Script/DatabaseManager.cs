using UnityEngine;
using System.Collections.Generic;
using TMPro; // TextMeshPro kütüphanesi
using UnityEngine.UI;

// --- 1. VERİ YAPILARI (MODELS) ---

[System.Serializable]
public class Soru
{
    public string soruMetni;
    public string A, B, C, D; // Seçenekler
    public string dogruCevap; // "A", "B", "C" veya "D"

    public Soru(string soru, string a, string b, string c, string d, string dogru)
    {
        this.soruMetni = soru;
        this.A = a;
        this.B = b;
        this.C = c;
        this.D = d;
        this.dogruCevap = dogru;
    }
}

[System.Serializable]
public class Kategori
{
    public string kategoriAdi;
    public List<Soru> sorular = new List<Soru>();
}

// --- 2. YÖNETİCİ SINIF (MANAGER) ---

public class DatabaseManager : MonoBehaviour
{
    [Header("UI Bağlantıları (Sürükle-Bırak)")]
    public TextMeshProUGUI kategoriBaslikText; // Hangi kategorideyiz?
    public TextMeshProUGUI puanText;           // Puan Durumu
    public TextMeshProUGUI soruMetniText;      // Soru Yazısı
    public TextMeshProUGUI secenekAText;       // A Butonu Yazısı
    public TextMeshProUGUI secenekBText;       // B Butonu Yazısı
    public TextMeshProUGUI secenekCText;       // C Butonu Yazısı
    public TextMeshProUGUI secenekDText;       // D Butonu Yazısı
    public TextMeshProUGUI sonucText;          // Doğru/Yanlış Bildirimi

    public List<Kategori> tumKategoriler = new List<Kategori>();

    private List<Soru> aktifSoruListesi;
    private int suankiSoruIndex = 0;
    private string dogruSik;
    
    private int toplamPuan = 0;
    private bool cevapVerildiMi = false;

    void Start()
    {
        // 1. Verileri oluştur (Matematik Soruları)
        VeritabaniniDoldur();
        
        // 2. Puanı sıfırla
        PuanGuncelle(0);

        // 3. Oyunu başlat (Varsayılan olarak Temel İşlemler açılır)
        KategoriBaslat("Temel İşlemler");
    }

    // --- VERİ GİRİŞİ (MATEMATİK SORULARI) ---
    void VeritabaniniDoldur()
    {
        tumKategoriler.Clear();

        // ---------------- 1. TEMEL İŞLEMLER ----------------
        Kategori temel = new Kategori { kategoriAdi = "Temel İşlemler" };
        temel.sorular.Add(new Soru("12 x 8 işleminin sonucu kaçtır?", "86", "96", "104", "108", "B"));
        temel.sorular.Add(new Soru("144 / 12 işleminin sonucu kaçtır?", "10", "11", "12", "14", "C"));
        temel.sorular.Add(new Soru("25 + 18 - 10 işleminin sonucu kaçtır?", "33", "35", "43", "23", "A"));
        temel.sorular.Add(new Soru("5'in küpü (5³) kaçtır?", "25", "75", "100", "125", "D"));
        temel.sorular.Add(new Soru("İşlem önceliğine göre: 5 + 3 x 4 = ?", "32", "17", "23", "20", "B"));
        temel.sorular.Add(new Soru("Hangi sayı 2'ye kalansız bölünemez?", "128", "54", "91", "200", "C"));
        temel.sorular.Add(new Soru("En küçük asal sayı kaçtır?", "1", "2", "3", "5", "B"));
        temel.sorular.Add(new Soru("Yüzde 20'si 10 olan sayı kaçtır?", "40", "50", "60", "100", "B"));
        tumKategoriler.Add(temel);

        // ---------------- 2. CEBİR ----------------
        Kategori cebir = new Kategori { kategoriAdi = "Cebir" };
        cebir.sorular.Add(new Soru("2x = 10 ise x kaçtır?", "2", "5", "8", "10", "B"));
        cebir.sorular.Add(new Soru("3x + 5 = 20 denkleminde x kaçtır?", "3", "4", "5", "6", "C"));
        cebir.sorular.Add(new Soru("x - 7 = 13 ise x kaçtır?", "6", "13", "20", "21", "C"));
        cebir.sorular.Add(new Soru("4a = 24 ise, a + 5 kaçtır?", "6", "9", "11", "15", "C"));
        cebir.sorular.Add(new Soru("y/3 = 4 ise y kaçtır?", "7", "1", "12", "16", "C"));
        cebir.sorular.Add(new Soru("2(x+1) = 12 ise x kaçtır?", "4", "5", "6", "7", "B"));
        cebir.sorular.Add(new Soru("x'in karesi 49 ise x pozitif olarak kaçtır?", "5", "6", "7", "9", "C"));
        tumKategoriler.Add(cebir);

        // ---------------- 3. GEOMETRİ ----------------
        Kategori geometri = new Kategori { kategoriAdi = "Geometri" };
        geometri.sorular.Add(new Soru("Bir üçgenin iç açıları toplamı kaç derecedir?", "90", "180", "270", "360", "B"));
        geometri.sorular.Add(new Soru("Karenin bir kenarı 5 cm ise alanı kaç cm² dir?", "20", "10", "15", "25", "D"));
        geometri.sorular.Add(new Soru("Dik açılı bir üçgende en uzun kenara ne ad verilir?", "Dik kenar", "Hipotenüs", "Yanal kenar", "Taban", "B"));
        geometri.sorular.Add(new Soru("Dairenin çevresini hesaplarken kullanılan sabit sayı hangisidir?", "Pi (π)", "E sayısı", "Delta", "Sigma", "A"));
        geometri.sorular.Add(new Soru("Dört kenarı eşit ve açıları 90 derece olan dörtgen hangisidir?", "Dikdörtgen", "Eşkenar Dörtgen", "Kare", "Yamuk", "C"));
        geometri.sorular.Add(new Soru("Bir doğru açının ölçüsü kaç derecedir?", "90", "180", "360", "45", "B"));
        geometri.sorular.Add(new Soru("Çapı 10 cm olan bir çemberin yarıçapı kaç cm'dir?", "2", "5", "10", "20", "B"));
        tumKategoriler.Add(geometri);

        // ---------------- 4. PROBLEMLER ----------------
        Kategori problemler = new Kategori { kategoriAdi = "Problemler" };
        problemler.sorular.Add(new Soru("Saatte 60 km hızla giden bir araç 3 saatte kaç km gider?", "120", "150", "180", "200", "C"));
        problemler.sorular.Add(new Soru("Ali'nin yaşı 12, Ayşe'nin yaşı Ali'den 5 fazladır. Ayşe kaç yaşındadır?", "15", "16", "17", "18", "C"));
        problemler.sorular.Add(new Soru("Bir manavda elmanın kilosu 5 TL'dir. 4 kilo elma ne kadar tutar?", "15 TL", "20 TL", "25 TL", "30 TL", "B"));
        problemler.sorular.Add(new Soru("Yarısı 15 olan sayının tamamı kaçtır?", "25", "30", "45", "7.5", "B"));
        problemler.sorular.Add(new Soru("Bir deste kalem kaç adettir?", "10", "12", "15", "20", "A"));
        problemler.sorular.Add(new Soru("Bir düzine kalem kaç adettir?", "10", "12", "15", "20", "B"));
        tumKategoriler.Add(problemler);

        Debug.Log("✅ Matematik soruları başarıyla yüklendi.");
    }

    // --- 3. OYUN MANTIĞI VE KONTROLLER (Aynı Kaldı) ---

    public void KategoriBaslat(string ad)
    {
        Kategori secilen = tumKategoriler.Find(x => x.kategoriAdi == ad);

        if (secilen != null && secilen.sorular.Count > 0)
        {
            if (kategoriBaslikText != null) kategoriBaslikText.text = secilen.kategoriAdi;
            
            aktifSoruListesi = new List<Soru>(secilen.sorular);
            ListeyiKaristir(aktifSoruListesi);

            PuanGuncelle(0);
            suankiSoruIndex = 0;
            SoruGoster(0);
        }
        else
        {
            Debug.LogError("HATA: '" + ad + "' adında bir kategori bulunamadı!");
        }
    }

    void ListeyiKaristir(List<Soru> liste)
    {
        for (int i = 0; i < liste.Count; i++)
        {
            Soru temp = liste[i];
            int randomIndex = Random.Range(i, liste.Count);
            liste[i] = liste[randomIndex];
            liste[randomIndex] = temp;
        }
    }

    public void SoruGoster(int index)
    {
        if (aktifSoruListesi == null || index >= aktifSoruListesi.Count)
        {
            sonucText.text = "Oyun Bitti! Toplam Puanın: " + toplamPuan;
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
        
        sonucText.text = ""; 
    }

    public void CevapVer(string secim)
    {
        if (cevapVerildiMi == true) return;

        cevapVerildiMi = true; 

        if (secim == dogruSik)
        {
            sonucText.text = "<color=green>DOĞRU! (+50 Puan)</color>";
            toplamPuan += 50;
            PuanGuncelle(toplamPuan);
        }
        else
        {
            sonucText.text = "<color=red>YANLIŞ!</color>";
        }
    }

    void PuanGuncelle(int puan)
    {
        toplamPuan = puan;
        if (puanText != null)
        {
            puanText.text = "Puan: " + toplamPuan.ToString();
        }
    }

    public void SonrakiSoru()
    {
        suankiSoruIndex++;
        if (aktifSoruListesi != null && suankiSoruIndex < aktifSoruListesi.Count)
        {
            SoruGoster(suankiSoruIndex);
        }
        else
        {
            sonucText.text = "Sorular Tamamlandı! Skor: " + toplamPuan;
        }
    }
}