# 🏠 EvimCebim - Kişisel Gelir/Gider Takip Sistemi

![.NET](https://img.shields.io/badge/.NET-9.0-purple)
![Docker](https://img.shields.io/badge/Docker-Enabled-blue)
![PostgreSQL](https://img.shields.io/badge/Database-PostgreSQL-336791)
![Status](https://img.shields.io/badge/Status-Live-success)

EvimCebim, kullanıcıların kişisel harcamalarını kayıt altına alabildiği, kategorize edebildiği ve geçmişe dönük finansal hareketlerini izleyebildiği web tabanlı bir SaaS (Software as a Service) uygulamasıdır.

Bu proje, **Yozgat Bozok Üniversitesi - Web Programlama Dersi Final Projesi** kapsamında geliştirilmiştir.

🔗 **Canlı Demo:** [https://evimcebim.com.tr](https://evimcebim.com.tr)

---
## 🚀 Proje Hakkında

Bu uygulama, çoklu kullanıcı (Multi-tenancy) mimarisine uygun olarak tasarlanmıştır. Her kullanıcı sisteme kayıt olduktan sonra kendi verilerini yönetir. Bir kullanıcının verisi diğer kullanıcılar tarafından görülemez.

Proje, **Docker** konteynerizasyonu kullanılarak **Render.com** üzerinde canlıya alınmış ve **.com.tr** domaini ile yayınlanmıştır.

## ✅ Özellikler (Gereksinimler)
* **Kullanıcı Sistemi (Identity):** Kayıt Ol, Giriş Yap, Çıkış Yap işlemleri.
* **CRUD İşlemleri:** Harcama Ekleme, Listeleme, Güncelleme ve Silme.
* **Veri Güvenliği:** Her kullanıcı sadece kendi oluşturduğu kayıtları görebilir.
* **Veritabanı:** PostgreSQL kullanılarak veriler kalıcı hale getirilmiştir.
* **Responsive Tasarım:** Mobil ve masaüstü uyumlu arayüz (Bootstrap 5).
---

## 🛠️ Kullanılan Teknolojiler

* **Backend:** ASP.NET Core MVC 9.0
* **Veritabanı:** PostgreSQL (Render Managed DB)
* **ORM:** Entity Framework Core (Code-First)
* **Frontend:** HTML5, CSS3, Bootstrap 5
* **DevOps:** Docker, GitHub Actions (CI/CD Mantığı), Render.com

---

## 📸 Ekran Görüntüleri

| Ana Sayfa | Harcama Listesi |
|-----------|-----------------|
| ![Ana Sayfa](https://via.placeholder.com/400x200?text=Ana+Sayfa+Gorseli) | ![Listeleme](https://via.placeholder.com/400x200?text=Listeleme+Gorseli) |

| Giriş Ekranı | Kayıt Ekranı |
|--------------|--------------|
| ![Giriş](https://via.placeholder.com/400x200?text=Giris+Ekrani) | ![Kayıt](https://via.placeholder.com/400x200?text=Kayit+Ekrani) |

---
## ⚙️ Kurulum (Lokalde Çalıştırma)

Projeyi kendi bilgisayarınızda çalıştırmak için aşağıdaki adımları izleyebilirsiniz:

1.  **Depoyu Klonlayın:**
    ```bash
    git clone [https://github.com/KULLANICIADIN/EvimCebim.git](https://github.com/KULLANICIADIN/EvimCebim.git)
    cd EvimCebim
    ```
2.  **Veritabanı Ayarları:**
    `appsettings.json` dosyasındaki Connection String alanını kendi PostgreSQL veya SQL Server bilgilerinize göre düzenleyin.
3.  **Uygulamayı Ayağa Kaldırın:**
    ```bash
    dotnet restore
    dotnet run
    ```
4.  **Docker ile Çalıştırma (Opsiyonel):**
    ```bash
    docker build -t evimcebim .
    docker run -p 8080:8080 evimcebim
    ```
---
## 📂 Proje Yapısı

* `/Controllers`: Kullanıcı isteklerini yöneten ve iş mantığını içeren sınıflar.
* `/Models`: Veritabanı tablolarını temsil eden sınıflar (Entity).
* `/Views`: Kullanıcı arayüzü dosyaları (Razor Pages).
* `/Data`: Veritabanı bağlantısı ve Identity ayarları.
* `Dockerfile`: Uygulamanın Docker imajı oluşturma talimatları.
---
## 👤 Yazar
**Zeki Emir Kuş**
* GitHub: @ZekiEmir(https://github.com/ZekiEmir)
* LinkedIn: www.linkedin.com/in/zeki-emir-kuş-6632b7387
