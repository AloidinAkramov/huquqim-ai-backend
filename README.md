<div align="center">

# ⚖️ Huquqim.AI — Backend

### Advokati yo'q fuqarolar uchun sun'iy intellekt huquqiy yordamchisi

O'zbekiston fuqarolariga kichik sud ishlarida yordam beruvchi AI-platformaning **backend** qismi.
Holatni tushuntiradi, hujjat tayyorlaydi va sudga tayyorlaydi — oddiy tilda, o'zbek tilida.

[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![PostgreSQL](https://img.shields.io/badge/PostgreSQL-16-4169E1?logo=postgresql&logoColor=white)](https://www.postgresql.org/)
[![Vertex AI](https://img.shields.io/badge/Google_Vertex_AI-Gemini-4285F4?logo=googlecloud&logoColor=white)](https://cloud.google.com/vertex-ai)
[![Clean Architecture](https://img.shields.io/badge/Clean-Architecture-2ea44f)](https://blog.cleancoder.com/)
[![License](https://img.shields.io/badge/license-MIT-blue)](LICENSE)

[Frontend repo](https://github.com/AloidinAkramov/huquqim-ai-web) · [Demo](https://frontend-mu-lilac-so8jhtnqsj.vercel.app)

</div>

---

## 📋 Loyiha haqida

**Huquqim.AI** — advokat yollay olmaydigan oddiy fuqarolar uchun mikro-SaaS huquqiy yordamchi. Foydalanuvchi muammosini oddiy tilda yozadi, tizim esa:

- 🔍 **Toifalaydi** — muammoni huquqiy turlarga (fuqarolik, jinoiy, ma'muriy, mehnat, iste'molchi) ajratib, foiz beradi
- 💬 **Tushuntiradi** — AI chat orqali huquqlarni va qonun moddalarini oddiy tilda izohlaydi (RAG bilan)
- 📄 **Hujjat tayyorlaydi** — 51+ tayyor shablon (da'vo arizasi, pretenziya, shikoyat) → Word formatda
- ⚖️ **Yo'naltiradi** — jiddiy (jinoiy) ishlarda malakali advokatga murojaat tavsiya qiladi

> ⚠️ **Muhim:** Bu xizmat advokat yoki yuristni **almashtirmaydi**. U faqat huquqiy ma'lumot va yordam beruvchi vositadir.

---

## 🏗️ Arxitektura

Toza **Clean Architecture** — 4 qatlam, qat'iy bog'liqlik yo'nalishi:

```
┌─────────────────────────────────────────────┐
│  Huquqim.Api          (Controllers, HTTP)    │  ← Kirish nuqtasi
├─────────────────────────────────────────────┤
│  Huquqim.Infrastructure  (DB, AI, Auth)      │  ← Tashqi dunyo
├─────────────────────────────────────────────┤
│  Huquqim.Application  (Servislar, mantiq)    │  ← Biznes mantiq
├─────────────────────────────────────────────┤
│  Huquqim.Domain       (Entity, Result)       │  ← Yadro
└─────────────────────────────────────────────┘
         Api → Infrastructure → Application → Domain
```

| Qatlam | Vazifa |
|--------|--------|
| **Domain** | Entity'lar, enum'lar, `Result<T>` pattern, abstraksiyalar |
| **Application** | Feature-folder servislar (Identity, Cases, Conversations, Documents, Triage), FluentValidation, AI/RAG abstraksiyalari |
| **Infrastructure** | EF Core (PostgreSQL), Vertex AI broker, JWT, parol hash (PBKDF2), docx generator, bilim bazasi retriever |
| **Api** | Controllers, ProblemDetails envelope, global exception handler, Swagger |

---

## 🛠️ Texnologiyalar

| Komponent | Texnologiya |
|-----------|-------------|
| **Til / Framework** | C# 12, .NET 8 |
| **Ma'lumotlar bazasi** | PostgreSQL 16 + EF Core (snake_case) |
| **AI** | Google Vertex AI — Gemini 2.5 Flash |
| **RAG** | Bilim bazasi retriever (Lex.uz qonun moddalari) |
| **Auth** | JWT (email/parol) |
| **Validatsiya** | FluentValidation |
| **Hujjat** | Open XML (.docx) — tashqi kutubxonasiz |
| **Konteyner** | Docker |

---

## 🚀 Asosiy imkoniyatlar

- ✅ **JWT autentifikatsiya** — ro'yxatdan o'tish, kirish, profil
- ✅ **Aqlli toifalash (Triage)** — AI muammoni toifalarga ajratib, foiz beradi
- ✅ **AI chat + RAG** — bilim bazasidan tegishli qonun moddalari topiladi, manba ko'rsatiladi
- ✅ **Hujjat generatori** — 51+ shablon, `{{placeholder}}` to'ldirish, `.docx` eksport
- ✅ **Premium model** — bepul daraja cheklangan, to'liq xizmat pullik
- ✅ **Advokat tavsiyasi** — jiddiy ishlarda yuristga yo'naltirish

---

## 📡 API endpointlari

| Metod | Yo'l | Tavsif |
|-------|------|--------|
| `POST` | `/api/auth/register` | Ro'yxatdan o'tish |
| `POST` | `/api/auth/login` | Kirish |
| `GET` | `/api/auth/me` | Joriy foydalanuvchi |
| `POST` | `/api/auth/upgrade` | Premium tarif |
| `GET` `POST` | `/api/cases` | Ishlar ro'yxati / yangi ish |
| `POST` | `/api/cases/{id}/triage` | Ishni toifalarga ajratish |
| `POST` | `/api/cases/{id}/messages` | AI chat (RAG) |
| `POST` | `/api/cases/{id}/documents` | Hujjat generatsiya |
| `GET` | `/api/documents/templates` | Hujjat shablonlari |
| `POST` | `/api/documents/fill` | Shablondan `.docx` yuklab olish |

---

## ⚙️ Ishga tushirish

### Talablar
- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [PostgreSQL 16](https://www.postgresql.org/)
- Google Cloud service account (Vertex AI uchun)

### Qadamlar

```bash
# 1. Reponi klonlash
git clone https://github.com/AloidinAkramov/huquqim-ai-backend.git
cd huquqim-ai-backend

# 2. Sozlamalar — env yoki appsettings orqali:
#    ConnectionStrings__DefaultConnection — PostgreSQL
#    Jwt__SecretKey — 32+ belgili maxfiy kalit
#    Ai__ProjectId — GCP project ID
#    Ai__CredentialsPath — service account JSON yo'li

# 3. Ishga tushirish (migratsiya + seed avtomatik)
dotnet run --project src/Huquqim.Api

# Swagger: https://localhost:xxxx/swagger
```

### Docker bilan

```bash
docker build -t huquqim-api:latest .
docker run -d -p 8087:8080 \
  -e "ConnectionStrings__DefaultConnection=Host=...;Database=huquqim;..." \
  -e "Jwt__SecretKey=..." \
  -e "Ai__ProjectId=..." \
  -v /path/to/credentials.json:/app/vertex-credentials.json:ro \
  huquqim-api:latest
```

---

## 🧠 RAG — hallyutsinatsiyaga qarshi

AI'ning qonunni "to'qib chiqarishi" — eng katta xavf. Buni oldini olish uchun:

1. **Bilim bazasi** — Lex.uz qonun moddalari PostgreSQL'da saqlanadi
2. **Retriever** — foydalanuvchi savoliga mos moddalar topiladi
3. **System prompt** — AI'ga *"faqat shu moddalardan foydalan, o'zingdan to'qima"* ko'rsatiladi
4. **Manba** — har javobda qonun moddasi havolasi ko'rsatiladi

---

## 📁 Loyiha tuzilishi

```
src/
├── Huquqim.Domain/           # Entity, Enum, Result/Error, abstraksiyalar
├── Huquqim.Application/       # Servislar (Identity, Cases, Conversations, Documents, Triage)
├── Huquqim.Infrastructure/    # AppDbContext, VertexBroker, JWT, docx, seeder
└── Huquqim.Api/               # Controllers, Program.cs, ProblemDetails
docs/                          # Texnik topshiriq, yo'l xaritasi
Dockerfile
```

---

## 📜 Litsenziya

MIT — erkin foydalanish mumkin.

---

<div align="center">

**Huquqim.AI** · O'zbekiston fuqarolari uchun · 2026

</div>
