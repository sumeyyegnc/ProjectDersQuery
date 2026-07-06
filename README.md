# ProjectDersQuery - Kurs Yönetim Sistemi

Bu proje, bir eğitim merkezindeki Eğitmen, Kurs ve Öğrenci kayıtlarının yönetilmesini sağlayan kapsamlı bir **ASP.NET Core** uygulamasıdır. Projede modern web standartları takip edilmiş ve istenen tüm gereksinimler eksiksiz bir şekilde entegre edilmiştir.

## 🚀 Kullanılan Teknolojiler
* **Backend:** ASP.NET Core MVC (.NET 8.0)
* **Veritabanı Erişimi:** ADO.NET (`SqlConnection`, `SqlCommand`)
* **Veritabanı:** Microsoft SQL Server (LocalDB)
* **Frontend:** HTML5, CSS3, Bootstrap 5, jQuery
* **Raporlama:** DataTables.js (PDF ve Excel dışa aktarımı)
* **Kimlik Doğrulama:** .NET Core Cookie Authentication

## 🌟 Proje Özellikleri

1. **Gelişmiş CRUD İşlemleri:**
   * **Öğrenci, Kurs ve Eğitmen** modelleri üzerinde eksiksiz Ekleme (Create), Okuma (Read), Güncelleme (Update) ve Silme (Delete) işlemleri Ajax tabanlı modallar kullanılarak sayfa yenilenmeden yapılmaktadır.
   * Tüm veritabanı işlemleri Entity Framework yerine **ADO.NET** mimarisiyle parametreli SQL (`@param`) sorguları kullanılarak gerçekleştirilir (SQL Injection'a karşı tam güvenlik).

2. **Arama (Search) Sistemi:**
   * Tüm listeleme sayfalarında, sunucu tarafında (Server-side) çalışan bir arama algoritması bulunur.
   * Kullanıcı arama kutusuna yazı yazdığında, ADO.NET üzerinden `LIKE` sorgusu ile veritabanında anlık filtreleme yapılır ve sonuçlar Ajax ile ekrana yansıtılır.

3. **Raporlama (Excel ve PDF Çıktısı):**
   * Listelenen tablolar, profesyonel *DataTables.js* kütüphanesiyle zenginleştirilmiştir.
   * Tabloların sağ üst köşesinde yer alan butonlar sayesinde ekrandaki güncel veriler tek bir tıklamayla **Excel (.xlsx)** veya **PDF** formatında rapor olarak dışa aktarılabilir.

4. **Üyelik ve Güvenlik (Login / Register):**
   * Cookie tabanlı kimlik doğrulama sistemi (Authentication) entegre edilmiştir.
   * Kayıt Ol (`/Account/Register`) ve Giriş Yap (`/Account/Login`) sayfaları mevcuttur. Üyelik işlemleri yine doğrudan ADO.NET aracılığıyla `Kullanicilar` tablosunda saklanır.
   * Tüm Controller'lar (Öğrenci, Kurs, Eğitmen) `[Authorize]` kısıtlaması ile korunmaktadır. Yetkisiz ziyaretçiler sistem tarafından otomatik olarak Login sayfasına yönlendirilir.
   * Şifre güvenliği ve kullanıcı doğrulaması stabil şekilde çalışmaktadır.

## 🛠️ Kurulum ve Çalıştırma

1. Projeyi bilgisayarınıza klonlayın:
```bash
git clone https://github.com/sumeyyegnc/ProjectDersQuery.git
```
2. Proje dizinine gidin ve bağımlılıkları yükleyin:
```bash
cd ProjectDersQuery/ProjectDersQuery
dotnet restore
```
3. Proje klasöründe uygulamayı başlatın:
```bash
dotnet run
```
4. Tarayıcınızda `http://localhost:<port>` (konsolda belirtilen port) adresine giderek uygulamayı kullanmaya başlayabilirsiniz. Sistem ilk çalıştığında eğer `Kullanicilar` tablosu yoksa otomatik olarak oluşturacaktır.

---
**Not:** Bu proje, istenilen "ASP.NET Core, ADO.NET, Arama, Raporlama (Excel/PDF) ve Üyelik İşlemleri" gereksinimlerini tam olarak karşılayacak şekilde hazırlanmıştır.