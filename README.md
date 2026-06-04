<div align="center">

# ⚖️ Huquqim.AI — Backend

### Advokati yo'q fuqarolar uchun sun'iy intellekt huquqiy yordamchisi

O'zbekiston fuqarolariga kichik sud ishlarida yordam beruvchi AI-platformaning **backend** qismi.
Holatni tushuntiradi, hujjat tayyorlaydi va sudga tayyorlaydi — oddiy tilda, o'zbek tilida.

[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![PostgreSQL](https://img.shields.io/badge/PostgreSQL-16-4169E1?logo=postgresql&logoColor=white)](https://www.postgresql.org/)
[![Vertex AI](https://img.shields.io/badge/Google_Vertex_AI-Gemini-4285F4?logo=googlecloud&logoColor=white)](https://cloud.google.com/vertex-ai)
[![Clean Architecture](https://img.shields.io/badge/Clean-Architecture-2ea44f)](https://blog.cleancoder.com/)

[Frontend repo](https://github.com/AloidinAkramov/huquqim-ai-web) · [🌐 Demo](https://frontend-mu-lilac-so8jhtnqsj.vercel.app)

</div>

---

## 📋 Loyiha haqida

**Huquqim.AI** — advokat yollay olmaydigan oddiy fuqarolar uchun mikro-SaaS huquqiy yordamchi. Foydalanuvchi muammosini oddiy tilda yozadi, tizim esa:

- 🔍 **Toifalaydi** — muammoni huquqiy turlarga (fuqarolik, jinoiy, ma'muriy, mehnat, iste'molchi) ajratib, foiz beradi
- 💬 **Tushuntiradi** — AI chat orqali huquqlarni va qonun moddalarini oddiy tilda izohlaydi
- 📄 **Hujjat tayyorlaydi** — 51+ tayyor shablon (da'vo arizasi, pretenziya, shikoyat) → Word formatda
- ⚖️ **Yo'naltiradi** — jiddiy (jinoiy) ishlarda malakali advokatga murojaat tavsiya qiladi

> ⚠️ **Muhim:** Bu xizmat advokat yoki yuristni **almashtirmaydi**. U faqat huquqiy ma'lumot va yordam beruvchi vositadir.

---

## 🚀 Asosiy imkoniyatlar

- ✅ **JWT autentifikatsiya** — ro'yxatdan o'tish, kirish, profil
- ✅ **Aqlli toifalash (Triage)** — AI muammoni toifalarga ajratib, foiz beradi
- ✅ **AI chat + RAG** — bilim bazasidan tegishli qonun moddalari topiladi, manba ko'rsatiladi
- ✅ **Hujjat generatori** — 51+ shablon, to'ldirish va `.docx` eksport
- ✅ **Premium model** — bepul daraja cheklangan, to'liq xizmat pullik
- ✅ **Advokat tavsiyasi** — jiddiy ishlarda yuristga yo'naltirish

---

## 🏗️ Arxitektura

Toza **Clean Architecture** — 4 qatlam:

| Qatlam | Vazifa |
|--------|--------|
| **Domain** | Entity'lar, enum'lar, `Result<T>` pattern |
| **Application** | Feature servislar (Identity, Cases, Conversations, Documents, Triage) |
| **Infrastructure** | EF Core (PostgreSQL), Vertex AI broker, JWT, docx generator |
| **Api** | Controllers, ProblemDetails envelope, Swagger |

---

## 🛠️ Texnologiyalar

| Komponent | Texnologiya |
|-----------|-------------|
| **Til / Framework** | C# 12, .NET 8 |
| **Ma'lumotlar bazasi** | PostgreSQL 16 + EF Core |
| **AI** | Google Vertex AI — Gemini 2.5 Flash |
| **Auth** | JWT (email/parol) |
| **Hujjat** | Open XML (.docx) |
| **Konteyner** | Docker |

---

<div align="center">

**Huquqim.AI** · O'zbekiston fuqarolari uchun · 2026

</div>
