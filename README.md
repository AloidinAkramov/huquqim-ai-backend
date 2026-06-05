<div align="center">

# ⚖️ Huquqim.AI — Backend

### Advokati yo'q fuqarolar uchun sun'iy intellekt huquqiy yordamchisi

*O'zbekiston fuqarolariga kichik sud ishlarida yordam beruvchi AI-platforma.*
*Holatni tushuntiradi, hujjat tayyorlaydi, sudga tayyorlaydi — oddiy tilda, o'zbek tilida.*

[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![PostgreSQL](https://img.shields.io/badge/PostgreSQL-16-4169E1?logo=postgresql&logoColor=white)](https://www.postgresql.org/)
[![Vertex AI](https://img.shields.io/badge/Gemini_2.5_Flash-Vertex_AI-4285F4?logo=googlecloud&logoColor=white)](https://cloud.google.com/vertex-ai)
[![RAG](https://img.shields.io/badge/RAG-Retrieval_Augmented-0F4FB3)](#-rag--ai-qonunni-toqib-chiqarmaydi)
[![Clean Architecture](https://img.shields.io/badge/Clean-Architecture-2ea44f)](#%EF%B8%8F-arxitektura--clean-architecture)

<br/>

### [![Jonli demo](https://img.shields.io/badge/🌐_JONLI_DEMO-huquqim--ai.vercel.app-0F4FB3?style=for-the-badge)](https://huquqim-ai.vercel.app) &nbsp; [![Frontend repo](https://img.shields.io/badge/💻_FRONTEND_REPO-huquqim--ai--web-14213D?style=for-the-badge&logo=github)](https://github.com/AloidinAkramov/huquqim-ai-web)

</div>

---

## 📋 Loyiha haqida

O'zbekistonda ko'p fuqaro advokat yollay olmaydi yoki kichik ish uchun kerak deb hisoblamaydi — natijada o'z huquqini bilmaydi, qanday ariza yozishni bilmaydi.

**Huquqim.AI** aynan shu muammoni hal qiladi. Foydalanuvchi muammosini oddiy tilda yozadi, platforma esa:

| | |
|---|---|
| 🔍 **Toifalaydi** | Muammoni huquqiy turlarga ajratib, **ishonch foizini** beradi (Jinoiy 85%, Ma'muriy 10%...) |
| 💬 **Tushuntiradi** | Huquqlar va qonun moddalarini oddiy tilda, **manba havolasi** bilan izohlaydi |
| 📄 **Hujjat tayyorlaydi** | 50+ shablon (da'vo arizasi, pretenziya, shikoyat) → tayyor **Word** hujjat |
| ⚖️ **Yo'naltiradi** | Jiddiy (jinoiy) ishlarda **malakali advokatga** murojaatni tavsiya qiladi |

> ⚠️ Bu xizmat advokat yoki yuristni **almashtirmaydi** — faqat huquqiy ma'lumot beruvchi vosita.
> To'liq shartlar: [Ommaviy oferta](#-ommaviy-oferta-publik-shartnoma).

---

## 🧠 RAG — AI qonunni "to'qib chiqarmaydi"

Eng katta xavf: AI qonunni **noto'g'ri to'qib chiqarishi** (hallyutsinatsiya). Huquqiy platformada bu xavfli. Yechim — **RAG (Retrieval-Augmented Generation)**: AI o'z xotirasidan emas, **bizning bazadagi haqiqiy qonun moddalaridan** javob beradi.

```
   Foydalanuvchi savoli
   "Do'kondan buzuq telefon oldim, pulni qaytarmayapti"
           │
           ▼
   ┌─────────────────────────────────────────────┐
   │  1. TRIAGE  (TriageService)                 │
   │     AI muammoni o'qib toifalaydi            │
   │     → Iste'molchi 95%  ·  ish turi belgilandi│
   └─────────────────────────────────────────────┘
           │
           ▼
   ┌─────────────────────────────────────────────┐
   │  2. RETRIEVAL  (KnowledgeRetriever)   ◀── R │
   │     • savolni kalit so'zga bo'ladi          │
   │     • faqat Iste'molchi qonunidan qidiradi  │
   │     • mosligi bo'yicha ball berib tanlaydi  │
   │     → "Iste'molchilar qonuni 13-modda"      │
   └─────────────────────────────────────────────┘
           │
           ▼
   ┌─────────────────────────────────────────────┐
   │  3. AUGMENT  (SystemPrompts)          ◀── A │
   │     Topilgan modda + "FAQAT shundan          │
   │     foydalan, o'zingdan to'qima" ko'rsatmasi │
   └─────────────────────────────────────────────┘
           │
           ▼
   ┌─────────────────────────────────────────────┐
   │  4. GENERATION  (Gemini 2.5 Flash)    ◀── G │
   │     Faqat berilgan moddadan javob yozadi    │
   └─────────────────────────────────────────────┘
           │
           ▼
   Javob + manba: "Iste'molchilar qonuni 13-modda → lex.uz"
```

**Natija:** har javob **haqiqiy qonunga** asoslanadi va **manbasi** ko'rsatiladi — tekshirib bo'ladigan, ishonchli.

---

## 🏗️ Arxitektura — Clean Architecture

Toza **Clean / Onion Architecture** — 4 qatlam, bog'liqlik faqat ichkariga:

```
   ┌───────────────────────────────────────────┐
   │  Api            Controllers · HTTP · JWT   │  ← kirish nuqtasi
   │  ┌─────────────────────────────────────┐  │
   │  │  Infrastructure   EF Core · Vertex  │  │  ← tashqi dunyo
   │  │  ┌───────────────────────────────┐  │  │
   │  │  │  Application   servislar      │  │  │  ← biznes mantiq
   │  │  │  ┌─────────────────────────┐  │  │  │
   │  │  │  │  Domain   entity·Result │  │  │  │  ← yadro (yuragi)
   │  │  │  └─────────────────────────┘  │  │  │
   │  │  └───────────────────────────────┘  │  │
   │  └─────────────────────────────────────┘  │
   └───────────────────────────────────────────┘
         Api → Infrastructure → Application → Domain
                 (strelka faqat ICHKARIGA)
```

| Qatlam | Mas'uliyat |
|--------|-----------|
| **Domain** | Entity'lar, enum'lar, `Result<T>` pattern — hech kimga bog'liq emas |
| **Application** | Feature servislar: Identity, Cases, **Triage**, Conversations (**RAG**), Documents |
| **Infrastructure** | EF Core (PostgreSQL), Vertex AI broker, JWT, PBKDF2, docx generator, seeder |
| **Api** | Controllers, ProblemDetails, global exception handler |

**Afzalligi:** AI'ni (Gemini → boshqa) yoki bazani almashtirsa — faqat bitta qatlam o'zgaradi, qolgani daxlsiz.

---

## ✨ Asosiy imkoniyatlar

- 🔐 **JWT autentifikatsiya** — parol PBKDF2 (100k iteratsiya) bilan xeshlanadi
- 🧭 **Aqlli Triage** — AI muammoni toifalarga ajratib foiz beradi; jinoiy 70%+ → advokat tavsiyasi (server tomonida ham kafolatlangan)
- 💬 **AI chat + RAG** — bilim bazasidan modda topiladi, manba ko'rsatiladi
- 📄 **Hujjat generatori** — 50+ shablon, `{{placeholder}}` to'ldirish, Times New Roman + A4 **`.docx`** eksport
- 💳 **Premium model** — bepul daraja cheklangan, to'liq xizmat pullik (402 paywall)
- 📜 **Ommaviy oferta** — FK 367/369/370-moddalariga muvofiq publik shartnoma

---

## 📜 Ommaviy oferta (publik shartnoma)

Platforma O'zbekiston Respublikasi **Fuqarolik kodeksining 367-, 369- va 370-moddalariga** muvofiq ommaviy oferta asosida ishlaydi. Foydalanuvchi ro'yxatdan o'tishda ofertaga **aniq rozilik** bildiradi (majburiy checkbox).

Oferta quyidagilarni qamrab oladi: xizmat predmeti, **mas'uliyat cheklovi**, AI-xizmatlar shartlari, shaxsiy ma'lumotlar himoyasi («Shaxsga doir ma'lumotlar to'g'risida»gi qonun), to'lov tartibi. Uch tilda (o'zbek/rus/ingliz). Hujjat: [`docs/`](docs/) papkasida.

---

## 🛠️ Texnologiyalar

| Komponent | Texnologiya |
|-----------|-------------|
| **Til / Framework** | C# 12 · .NET 8 |
| **Ma'lumotlar bazasi** | PostgreSQL 16 · EF Core (snake_case) |
| **AI** | Google Vertex AI — **Gemini 2.5 Flash** (MaxTokens 8192) |
| **RAG** | Bilim bazasi retriever (kalit so'z + toifa filtri) |
| **Auth** | JWT (email/parol) · PBKDF2 |
| **Hujjat** | Open XML `.docx` — tashqi kutubxonasiz |
| **Validatsiya** | FluentValidation |
| **Konteyner** | Docker |

---

<div align="center">

**Huquqim.AI** · O'zbekiston fuqarolari uchun huquqiy adolat · 2026

</div>
