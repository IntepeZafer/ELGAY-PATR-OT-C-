using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

// --- VERİ YAPILARI ---

[System.Serializable]
public class Soru
{
    public string soruMetni;
    public string A, B, C, D;
    public string dogruCevap; // "A", "B", "C", "D"

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

// --- YÖNETİCİ SINIF ---

public class DatabaseManager : MonoBehaviour
{
    [Header("UI Bağlantıları")]
    public TextMeshProUGUI kategoriBaslikText;
    public TextMeshProUGUI soruMetniText;
    public TextMeshProUGUI secenekAText;
    public TextMeshProUGUI secenekBText;
    public TextMeshProUGUI secenekCText;
    public TextMeshProUGUI secenekDText;
    public TextMeshProUGUI sonucText;

    // Ana Veritabanı
    public List<Kategori> tumKategoriler = new List<Kategori>();

    // Oynanan listenin kopyası (Karıştırılmış hali burada duracak)
    private List<Soru> aktifSoruListesi;
    private int suankiSoruIndex = 0;
    private string dogruSik;

    void Start()
    {
        VeritabaniniDoldur();

        // Oyunu başlat (İstediğin kategori adını yaz)
        KategoriBaslat("Sinir Sistemi");
    }

    // --- MANUEL VERİ GİRİŞİ ---
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

    // --- KONTROL VE RASTGELELEŞTİRME ---

    public void KategoriBaslat(string ad)
    {
        Kategori secilen = tumKategoriler.Find(x => x.kategoriAdi == ad);
        if (secilen != null && secilen.sorular.Count > 0)
        {
            if (kategoriBaslikText != null) kategoriBaslikText.text = secilen.kategoriAdi;
            
            // LİSTEYİ KOPYALIYORUZ (Ana veritabanı bozulmasın diye)
            aktifSoruListesi = new List<Soru>(secilen.sorular);
            
            // --- KARIŞTIRMA İŞLEMİ (SHUFFLE) BURADA YAPILIYOR ---
            ListeyiKaristir(aktifSoruListesi);
            // ----------------------------------------------------

            suankiSoruIndex = 0;
            SoruGoster(0);
        }
        else
        {
            Debug.LogError("Kategori bulunamadı: " + ad);
            if(tumKategoriler.Count > 0)
            {
                aktifSoruListesi = new List<Soru>(tumKategoriler[0].sorular);
                ListeyiKaristir(aktifSoruListesi);
                SoruGoster(0);
            }
        }
    }

    // Fisher-Yates Shuffle Algoritması (Profesyonel Karıştırma)
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
            sonucText.text = "Kategori Tamamlandı!";
            return;
        }

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
        if (secim == dogruSik)
        {
            sonucText.text = "<color=green>DOĞRU!</color>";
        }
        else
        {
            sonucText.text = "<color=red>YANLIŞ!</color>";
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
            sonucText.text = "Sorular Bitti!";
        }
    }
}