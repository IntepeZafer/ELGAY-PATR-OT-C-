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

    // Veri girişini kolaylaştırmak için Kurucu Fonksiyon
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

    // Tüm verilerin tutulduğu ana liste
    public List<Kategori> tumKategoriler = new List<Kategori>();

    // Oyun esnasında kullanılan değişkenler
    private List<Soru> aktifSoruListesi;
    private int suankiSoruIndex = 0;
    private string dogruSik;
    
    // Puan ve Durum Değişkenleri
    private int toplamPuan = 0;
    private bool cevapVerildiMi = false; // Spam engellemek için kilit

    void Start()
    {
        // 1. Verileri oluştur
        VeritabaniniDoldur();
        
        // 2. Puanı sıfırla
        PuanGuncelle(0);

        // 3. Oyunu başlat (Buraya başlamak istediğin kategorinin adını yaz)
        // İleride burayı bir menüden butonla çağırabilirsin.
        KategoriBaslat("Sinir Sistemi");
    }

    // --- VERİ GİRİŞİ (HARDCODED DATA) ---
    void VeritabaniniDoldur()
    {
        tumKategoriler.Clear();

        // ---------------- SİNİR SİSTEMİ ----------------
        Kategori sinir = new Kategori { kategoriAdi = "Sinir Sistemi" };
        sinir.sorular.Add(new Soru("Beyin hangi kemik yapısının içinde yer alır?", "Kas", "Kalp", "Deri", "Kafatası", "D"));
        sinir.sorular.Add(new Soru("Vücut sıcaklığını düzenleyen beyin bölgesi hangisidir?", "Hipofiz", "Hipotalamus", "Hipokamp", "Epitalamus", "B"));
        sinir.sorular.Add(new Soru("Hafıza ve öğrenme süreçlerinde en çok rol oynayan beyin bölgesi hangisidir?", "Serebellum", "Hipotalamus", "Hipokampus", "Epitalamus", "C"));
        sinir.sorular.Add(new Soru("Denge ve koordinasyon hangi beyin bölgesiyle sağlanır?", "Hipokampus", "Hipotalamus", "Serebrum", "Serebellum", "D"));
        sinir.sorular.Add(new Soru("Vücutta ağrı algısını ileten sinir lifleri hangileridir?", "A lifleri", "B lifleri", "C lifleri", "D lifleri", "C"));
        sinir.sorular.Add(new Soru("Beyin-omurilik sıvısı nerede üretilir?", "Koroid Pleksus", "Korteks", "Substans Paryetalis", "Substans Nigra", "A"));
        sinir.sorular.Add(new Soru("Beynin görme ile ilgili bölgesi hangisidir?", "Frontal lop", "Parietal lop", "Oksipital lop", "Temporal lop", "C"));
        sinir.sorular.Add(new Soru("Beyin ve omuriliği çevreleyen koruyucu zarlar hangileridir?", "Pia, archoid, dura mater", "Pia, dura mater, meninx", "Pia, dura mater, arachnoid", "Pia, archoid, meninx", "A"));
        sinir.sorular.Add(new Soru("Beyindeki bilgi aktarım hızını artıran yapı nedir?", "Akson ucu", "Dendirit", "Miyelin kılıf", "Sinirli kas", "C"));
        sinir.sorular.Add(new Soru("Beynin sol ve sağ yarım küresini bağlayan yapı nedir?", "Corpus callosum", "Splenum", "Pons", "Medulla oblongata", "A"));
        sinir.sorular.Add(new Soru("Beyindeki duyusal bilgilerin toplanıp yönlendirildiği merkez hangisidir?", "Hipofiz", "Hipokamp", "Talamus", "Hipotalamus", "C"));
        sinir.sorular.Add(new Soru("Beyindeki açlık ve tokluk merkezleri hangi yapıda bulunur?", "Hipofiz", "Hipokamp", "Talamus", "Hipotalamus", "D"));
        sinir.sorular.Add(new Soru("Beyindeki hormon salgılayan bez hangisidir?", "Hipofiz", "Hipokamp", "Talamus", "Hipotalamus", "D"));
        sinir.sorular.Add(new Soru("Beyindeki hafıza oluşumunda görev alan yapı hangisidir?", "Hipofiz", "Hipokampus", "Talamus", "Hipotalamus", "B"));
        sinir.sorular.Add(new Soru("Beyindeki işitme ve konuşma merkezleri hangi lobda yer alır?", "Frontal", "Occipital", "Parietal", "Temporal", "D"));
        tumKategoriler.Add(sinir);

        // ---------------- DOLAŞIM SİSTEMİ ----------------
        Kategori dolasim = new Kategori { kategoriAdi = "Dolaşım Sistemi" };
        dolasim.sorular.Add(new Soru("Hangi organ oksijenli kanı vücuda pompalar?", "Kas", "Kalp", "Deri", "Akciğer", "B"));
        dolasim.sorular.Add(new Soru("Oksijenli kanı vücuda pompalayan kalp bölümü hangisidir?", "Sol ventrikül", "Sağ ventrikül", "Aort kapağı", "Trakea", "A"));
        dolasim.sorular.Add(new Soru("Kemik iliği hangi hücreleri üretir?", "Kas hücreleri", "Kan hücreleri", "Sperm hücreleri", "Kemik hücreleri", "B"));
        dolasim.sorular.Add(new Soru("Kan basıncını düzenleyen hormon sistemi hangisidir?", "Adrenalin-Norepinefrin", "Renin-Angiotensin-Aldosteron", "Insulin-Glikojen", "Estradiol-Östrojen", "B"));
        dolasim.sorular.Add(new Soru("Kanın vücutta dolaşımını sağlayan damar türü hangisidir?", "Arter", "Ven", "Arterioven", "Kapiller", "A"));
        dolasim.sorular.Add(new Soru("Alyuvarların ömrü ortalama ne kadardır?", "30 gün", "60 gün", "90 gün", "120 gün", "D"));
        dolasim.sorular.Add(new Soru("Kanın kalbe geri dönüşünü sağlayan damarlar hangileridir?", "Arterler", "Venler", "Arterioven", "Kapiller", "B"));
        dolasim.sorular.Add(new Soru("Kalbin kas dokusu hangi özel kas türüdür?", "Sinirli kas", "İskelet kası", "Kalp kası", "Sarıkas", "C"));
        dolasim.sorular.Add(new Soru("Kanın damar dışına çıkmasını engelleyen süreç nedir?", "Transkripsiyon", "Koagülasyon", "Metabolizma", "Oksidatif fosforilasyon", "B"));
        dolasim.sorular.Add(new Soru("Kalbin sağ kulakçığına gelen kan hangi damardan gelir?", "Vena Cava", "Vena Subclavia", "Vena Jugularis", "Vena Subclavia", "A"));
        dolasim.sorular.Add(new Soru("Kalbin kasılma sırasında kanı pompaladığı evreye ne ad verilir?", "Diastol", "Sistol", "Eksol", "Vetrikül", "B"));
        tumKategoriler.Add(dolasim);

        // ---------------- İSKELET SİSTEMİ ----------------
        Kategori iskelet = new Kategori { kategoriAdi = "İskelet Sistemi" };
        iskelet.sorular.Add(new Soru("İnsan vücudunda kaç kemik vardır (yetişkinlerde)?", "206", "200", "204", "208", "A"));
        iskelet.sorular.Add(new Soru("Kemik iliği hangi hücreleri üretir?", "Kas Hücreleri", "Kan hücreleri", "Sperm hücreleri", "Kemik hücreleri", "B"));
        iskelet.sorular.Add(new Soru("Kemiklerin uç kısımlarında bulunan ve büyümeyi sağlayan yapı nedir?", "Osteoblast", "Osteoklast", "Epifiz plağı", "Osteokondrit", "C"));
        iskelet.sorular.Add(new Soru("Kemik dokusunun yıkımını gerçekleştiren hücreler hangileridir?", "Osteoklastlar", "Osteoblastlar", "Osteokondritler", "Osteofiller", "A"));
        iskelet.sorular.Add(new Soru("Kemik dokusunun yeniden yapılandırılmasında rol oynayan hücreler hangileridir?", "Osteoklastlar", "Osteoblastlar", "Osteokondritler", "Osteofiller", "B"));
        iskelet.sorular.Add(new Soru("Kemiklerin sertliğini sağlayan mineral nedir?", "Kalsiyum fosfat", "Kalsiyum klorür", "Kalsiyum karbonat", "Kalsiyum sitrat", "A"));
        iskelet.sorular.Add(new Soru("Kasların kemiğe bağlandığı yapılar hangileridir?", "Kemik", "Kas", "Tendon", "Miyofibril", "C"));
        iskelet.sorular.Add(new Soru("Kıkırdak dokunun beslenmesi nasıl gerçekleşir?", "Difüzyonla", "Endokrin yolla", "Kolonizasyonla", "Oksidatif fosforilasyonla", "A"));
        tumKategoriler.Add(iskelet);

        // ---------------- KAS SİSTEMİ ----------------
        Kategori kas = new Kategori { kategoriAdi = "Kas Sistemi" };
        kas.sorular.Add(new Soru("Kas kasılması sırasında hangi iyon hücre içine girerek süreci başlatır?", "Kalsiyum", "Potasyum", "Sodyum", "Klor", "A"));
        kas.sorular.Add(new Soru("Solunum sırasında diyafram kasıldığında ne olur?", "Hava dışarı çıkar", "Göğüs boşluğu daralır", "Akciğerler küçülür", "Akciğerlere hava girer", "D"));
        kas.sorular.Add(new Soru("Mide hangi tür kaslarla kasılır?", "Sinirli Kas", "Düz Kas", "İskelet Kası", "Sarıkas", "B"));
        kas.sorular.Add(new Soru("Kasların oksijen ihtiyacını karşılayan pigment hangisidir?", "Eritrosit", "Hemoglobin", "Mioglobin", "Klorofil", "C"));
        kas.sorular.Add(new Soru("Kalbin kas dokusu hangi özel kas türüdür?", "Sinirli kas", "İskelet kası", "Kalp kası", "Sarıkas", "C"));
        kas.sorular.Add(new Soru("Kas dokusunun hücresel yapı taşlarından biri olan miyofibrillerin içindeki proteinler hangisidir?", "Hemoglobin ve mioglobin", "Kollajen ve elastin", "Eritrosit ve trombosit", "Aktin ve miyozin", "D"));
        tumKategoriler.Add(kas);

        // ---------------- SİNDİRİM SİSTEMİ ----------------
        Kategori sindirim = new Kategori { kategoriAdi = "Sindirim Sistemi" };
        sindirim.sorular.Add(new Soru("Sindirim sisteminde besinlerin emildiği ana organ hangisidir?", "Kas", "Kalp", "Deri", "Bağırsak", "D"));
        sindirim.sorular.Add(new Soru("Hangi organ hem endokrin hem de ekzokrin işlev görür?", "Pankreas", "Beyin", "Kalp", "Orta Beyin", "A"));
        sindirim.sorular.Add(new Soru("Sindirim sisteminde yağların emilimi esas olarak nerede gerçekleşir?", "Kolosterol", "Mide", "Kalın bağırsak", "İnce bağırsak", "D"));
        sindirim.sorular.Add(new Soru("Mide hangi tür kaslarla kasılır?", "Sinirli Kas", "Düz Kas", "İskelet Kası", "Sarıkas", "B"));
        sindirim.sorular.Add(new Soru("Mide mukozasını koruyan madde hangisidir?", "Hemoglobin", "Müsin", "Klorofil", "Eritrosit", "B"));
        sindirim.sorular.Add(new Soru("Sindirim sisteminde enzimlerin etkili olabilmesi için gerekli pH ortamı nerede asidiktir?", "Pankreas", "Kalın bağırsak", "İnce bağırsak", "Mide", "D"));
        tumKategoriler.Add(sindirim);

        // ---------------- HÜCRE BİYOLOJİSİ ----------------
        Kategori hucre = new Kategori { kategoriAdi = "Hücre Biyolojisi" };
        hucre.sorular.Add(new Soru("İnsan vücudunda en küçük hücre hangisidir?", "Kas hücresi", "Kan hücresi", "Sperm hücresi", "Kemik hücresi", "C"));
        hucre.sorular.Add(new Soru("Genetik materyalin bulunduğu hücre bölümü hangisidir?", "Sitoplazma", "Lizozom", "Çekirdek", "Mitokondri", "C"));
        hucre.sorular.Add(new Soru("Hücrede proteinlerin paketlenip taşınmasını sağlayan organel nedir?", "Mitokondri", "Lizozom", "Golgi aygıtı", "Ribozom", "C"));
        hucre.sorular.Add(new Soru("Hücrede genetik bilginin okunmasını sağlayan süreç nedir?", "Transkripsiyon", "Translasyon", "Metabolizma", "Oksidatif fosforilasyon", "A"));
        hucre.sorular.Add(new Soru("Hücrelerde enerji üreten enzim hangisidir?", "ATPaz", "Kloroz", "Klorofil", "Eritrosit", "A"));
        hucre.sorular.Add(new Soru("Hücre zarının seçici geçirgenliğini sağlayan özellik nedir?", "Fosfolipid çift tabaka", "Translasyon", "Metabolizma", "Oksidatif fosforilasyon", "A"));
        hucre.sorular.Add(new Soru("Hücre bölünmesinde kromozomların ayrıldığı evre nedir?", "Anafaz", "İnterfaz", "Metafaz", "Telofaz", "A"));
        hucre.sorular.Add(new Soru("Hücrede enerji üretiminde oksijenin kullanıldığı süreç nedir?", "Fermantasyon", "Aerobik solunum", "Anaerobik solunum", "Kolik asit sentezi", "C"));
        tumKategoriler.Add(hucre);

        // ---------------- ENDOKRİN SİSTEM ----------------
        Kategori endokrin = new Kategori { kategoriAdi = "Endokrin Sistem" };
        endokrin.sorular.Add(new Soru("İnsan vücudunda hangi sistem hormon üretir?", "Sinir sistemi", "Endokrin sistem", "Sindirim sistemi", "Endokrin sistem", "B"));
        endokrin.sorular.Add(new Soru("Hangi organ hem endokrin hem de ekzokrin işlev görür?", "Pankreas", "Beyin", "Kalp", "Orta Beyin", "A"));
        endokrin.sorular.Add(new Soru("Kan basıncını düzenleyen hormon sistemi hangisidir?", "Adrenalin-Norepinefrin", "Renin-Angiotensin-Aldosteron", "Insulin-Glikojen", "Estradiol-Östrojen", "B"));
        endokrin.sorular.Add(new Soru("Beyindeki açlık ve tokluk merkezleri hangi yapıda bulunur?", "Hipofiz", "Hipokamp", "Talamus", "Hipotalamus", "D"));
        endokrin.sorular.Add(new Soru("Vücutta stresle mücadelede görev alan hormon hangisidir?", "Adrenalin", "Kortizol", "Testosteron", "Östrojen", "B"));
        tumKategoriler.Add(endokrin);

        // ---------------- SOLUNUM SİSTEMİ ----------------
        Kategori solunum = new Kategori { kategoriAdi = "Solunum Sistemi" };
        solunum.sorular.Add(new Soru("Vücutta oksijen alışverişi nerede gerçekleşir?", "Kas", "Kalp", "Deri", "Akciğerler", "D"));
        solunum.sorular.Add(new Soru("Akciğerlerde gaz değişimi hangi yapıda gerçekleşir?", "Alveol", "Kanallar", "Kemik", "Kas", "A"));
        solunum.sorular.Add(new Soru("Solunum sırasında diyafram kasıldığında ne olur?", "Hava dışarı çıkar", "Göğüs boşluğu daralır", "Akciğerler küçülür", "Akciğerlere hava girer", "D"));
        tumKategoriler.Add(solunum);

        // ---------------- BAĞIŞIKLIK SİSTEMİ ----------------
        Kategori bagisiklik = new Kategori { kategoriAdi = "Bağışıklık Sistemi" };
        bagisiklik.sorular.Add(new Soru("Hangi sistem vücudu hastalıklara karşı korur?", "Sinir sistemi", "Bağışıklık sistemi", "Sindirim sistemi", "Endokrin sistem", "B"));
        bagisiklik.sorular.Add(new Soru("İmmün sistemde antikor üreten hücreler hangileridir?", "T lenfositler", "B lenfositler", "T ve B lenfositler", "Makrofajlar", "B"));
        bagisiklik.sorular.Add(new Soru("Bağışıklık sisteminde fagositoz yapan hücreler hangileridir?", "Makrofajlar", "T lenfositler", "B lenfositler", "Eritrositler", "A"));
        tumKategoriler.Add(bagisiklik);

        // ---------------- GENEL FİZYOLOJİ ----------------
        Kategori genel = new Kategori { kategoriAdi = "Genel Fizyoloji" };
        genel.sorular.Add(new Soru("İnsan vücudunda en büyük organ hangisidir?", "Kas", "Kalp", "Deri", "Akciğer", "C"));
        genel.sorular.Add(new Soru("Vücutta pH dengesini koruyan tampon sistemlerden biri hangisidir?", "Sodyum-potasyum sistem", "Glikoz metobolizması", "Karbonik asit-bikarbonat sistem", "Kalsiyum-potasyum sistem", "C"));
        genel.sorular.Add(new Soru("Vücutta suyun en fazla bulunduğu yer neresidir?", "Kan", "Hücre içi sıvı", "Hücre zarı", "Hücre dışı sıvı", "B"));
        tumKategoriler.Add(genel);

        Debug.Log("✅ Tüm veriler kod üzerinden başarıyla yüklendi.");
    }

    // --- 3. OYUN MANTIĞI VE KONTROLLER ---

    public void KategoriBaslat(string ad)
    {
        // 1. İstenen kategoriyi bul
        Kategori secilen = tumKategoriler.Find(x => x.kategoriAdi == ad);

        if (secilen != null && secilen.sorular.Count > 0)
        {
            // UI Başlığını güncelle
            if (kategoriBaslikText != null) kategoriBaslikText.text = secilen.kategoriAdi;
            
            // 2. Soruların kopyasını al (Ana listeyi bozmamak için)
            aktifSoruListesi = new List<Soru>(secilen.sorular);
            
            // 3. Soruları karıştır (Randomize)
            ListeyiKaristir(aktifSoruListesi);

            // 4. Puanı sıfırla ve oyunu başlat
            PuanGuncelle(0);
            suankiSoruIndex = 0;
            SoruGoster(0);
        }
        else
        {
            Debug.LogError("HATA: '" + ad + "' adında bir kategori bulunamadı!");
        }
    }

    // Fisher-Yates Karıştırma Algoritması
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
        // Soru bitti mi kontrolü
        if (aktifSoruListesi == null || index >= aktifSoruListesi.Count)
        {
            sonucText.text = "Oyun Bitti! Toplam Puanın: " + toplamPuan;
            // Buraya "Oyun Sonu Paneli Aç" kodu gelebilir.
            return;
        }

        // Yeni soruya geçince kilitleri aç
        cevapVerildiMi = false; 

        Soru s = aktifSoruListesi[index];
        soruMetniText.text = s.soruMetni;
        secenekAText.text = "A) " + s.A;
        secenekBText.text = "B) " + s.B;
        secenekCText.text = "C) " + s.C;
        secenekDText.text = "D) " + s.D;
        dogruSik = s.dogruCevap;
        
        sonucText.text = ""; // Sonuç yazısını temizle
    }

    // Butonlara bağlanacak fonksiyon
    public void CevapVer(string secim)
    {
        // Eğer zaten cevap verildiyse (spam yapılıyorsa) dur.
        if (cevapVerildiMi == true) return;

        cevapVerildiMi = true; // Kilidi kapat

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
        // Liste bitmediyse devam et
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