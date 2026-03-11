# TelemaSense – GitHub Pages’e Yayınlama

Bu proje GitHub Actions ile otomatik olarak GitHub Pages’e deploy edilir.

## 1. Repoyu GitHub’a yükleme

Proje klasöründe (FleetDashboard kök dizininde) terminalde:

```bash
git init
git add .
git commit -m "İlk commit: TelemaSense Blazor WASM"
git branch -M main
git remote add origin https://github.com/KULLANICI_ADIN/FleetDashboard.git
git push -u origin main
```

- `KULLANICI_ADIN` yerine kendi GitHub kullanıcı adınızı yazın.
- Repo daha önce oluşturulduysa: GitHub’da **New repository** ile boş bir repo açın, adını örn. `FleetDashboard` yapın; yukarıdaki `origin` adresini kendi repo URL’inizle değiştirin.

## 2. GitHub Pages’i açma

1. Repo sayfasında **Settings** → **Pages**.
2. **Build and deployment** bölümünde:
   - **Source:** **GitHub Actions** seçin.
3. Kaydedin.

## 3. İlk deploy

- `main` branch’ine yapılan her push’ta workflow otomatik çalışır.
- İlk seferde **Actions** sekmesinden **Deploy to GitHub Pages** workflow’unu açıp çalıştığını kontrol edebilirsiniz.
- Gerekirse **Run workflow** ile manuel tetikleyebilirsiniz.

## 4. Site adresi

- **Proje repo** (örn. `FleetDashboard`) için adres:
  - `https://KULLANICI_ADIN.github.io/FleetDashboard/`
- Son slash (`/`) önemli; base path otomatik ayarlanır.

## 5. Sorun çıkarsa

- **Actions** sekmesinde workflow’u açıp hata mesajına bakın.
- **Settings → Pages**’te kaynak **GitHub Actions** olarak seçili mi kontrol edin.
- İlk deploy 1–2 dakika sürebilir; sayfa bir süre 404 verebilir.
