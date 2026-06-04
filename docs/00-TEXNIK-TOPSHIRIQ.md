# TEXNIK TOPSHIRIQ (TZ)

# Huquqim.AI

**Advokati yo'q fuqarolar uchun sun'iy intellekt yordamchisi**

Mikro-SaaS loyihasi · Loyiha hujjati va AI uchun promt · 2026

---

## 0. Muhim Huquqiy Ogohlantirish

Bu loyiha advokat yoki yuristni **ALMASHTIRMAYDI**. U faqat huquqiy ma'lumot va yordam beruvchi vositadir. Bu farq loyihaning butun arxitekturasi va marketingida aniq aks etishi shart.

**Tizim quyidagilarni QILADI:**
- huquqiy ma'lumotni oddiy tilda tushuntiradi
- kerakli hujjat turini aniqlaydi
- hujjat shablonini to'ldirishga yordam beradi
- jarayonni bosqichma-bosqich ko'rsatadi

**Tizim quyidagilarni QILMAYDI:**
- yakuniy huquqiy maslahat bermaydi
- sud natijasini kafolatlamaydi
- advokat o'rnini bosmaydi
- fuqaro nomidan qaror qabul qilmaydi

**Har bir javobda quyidagi disklaymer ko'rsatilishi shart:**

> "Bu ma'lumot umumiy xususiyatga ega. Murakkab holatlar uchun malakali yuristga murojaat qiling."

---

## 1. Loyiha Haqida Umumiy Ma'lumot

### 1.1. Muammo

Ko'plab fuqarolar advokat majburiy bo'lmagan kichik sud ishlariga (ma'muriy huquqbuzarliklar, kichik fuqarolik nizolari, iste'molchi shikoyatlari) duch keladi. Bunday holatlarda:

- Fuqaro advokat yollashga pul topa olmaydi yoki kerak deb hisoblamaydi
- Fuqaro o'z huquqlarini va qonunni bilmaydi
- Qanday hujjat (ariza, e'tiroz, shikoyat) yozishni bilmaydi
- Sud jarayonida o'zini qanday himoya qilishni tushunmaydi

### 1.2. Yechim

**HuquqYordam.AI** — fuqaroga o'z ishini tushunishga, kerakli hujjatlarni tayyorlashga va sud jarayoniga tayyorlanishga yordam beruvchi sun'iy intellektga asoslangan onlayn xizmat. Foydalanuvchi oddiy tilda o'z muammosini aytadi, tizim esa unga tushunarli yo'l-yo'riq, tayyor hujjat va keyingi qadamlarni beradi.

### 1.3. Maqsadli auditoriya

- **Asosiy:** advokat yollay olmaydigan yoki kerak deb hisoblamaydigan oddiy fuqarolar
- **Ikkilamchi:** kichik tadbirkorlar, ijarachilar, iste'molchilar, mehnat nizosi bo'lgan xodimlar

### 1.4. Nima uchun Mikro-SaaS

Loyiha ataylab kichik va fokuslangan: bitta aniq muammoni (advokati yo'q fuqaroga yordam) yaxshi hal qiladi. Bu kichik jamoa (1-3 kishi) tomonidan ishlab chiqilishi va boshqarilishi mumkin. Keng qamrovli platforma emas, balki bitta ishni mukammal bajaradigan vosita.

---

## 2. Asosiy Funksiyalar (MVP)

### Modul 1: Aqlli suhbat (Triage)

Foydalanuvchi o'z muammosini erkin shaklda yozadi ("Meni do'kon aldab, buzuq telefon sotdi"). Tizim savol-javob orqali holatni aniqlaydi:

- Bu qaysi turdagi ish? (iste'molchi, mehnat, ma'muriy, ijara va h.k.)
- Bu ish advokat majburiy bo'lgan toifagami? (agar ha — yuristga yo'naltiradi)
- Qaysi davlat organi yoki sud bilan ishlash kerak?
- Qanday muddatlar amal qiladi? (da'vo muddati va h.k.)

### Modul 2: Huquqiy tushuntirish

Tizim foydalanuvchining holatiga tegishli huquqlarni va qonun moddalarini **ODDIY tilda** tushuntiradi. Har bir tushuntirish haqiqiy qonun manbasiga (Lex.uz) havola bilan beriladi. Yuridik jargon ishlatilmaydi.

### Modul 3: Hujjat generatori

Tizim foydalanuvchiga kerakli hujjatni tayyorlashda yordam beradi:

- Ariza (da'vo arizasi, shikoyat, e'tiroz)
- Pretenziya (rasmiy talabnoma)
- Tushuntirish xati
- Apellyatsiya shikoyati

Hujjat shablonga asoslanadi, foydalanuvchi ma'lumotlari bilan to'ldiriladi va tahrirlanadigan formatda (Word/PDF) yuklab olinadi.

### Modul 4: Sudga tayyorgarlik

Tizim foydalanuvchiga sud jarayoniga tayyorlanishda yordam beradi: nima deyish kerak, qanday dalil keltirish, sud zalida o'zini qanday tutish, qanday hujjatlarni olib borish. Bu maslahat emas, balki amaliy tayyorgarlik yo'riqnomasi.

### Modul 5: Hujjatlarni saqlash

Foydalanuvchi o'z ishiga oid barcha hujjatlarni bitta joyda saqlaydi, muddatlarni kuzatadi va eslatma oladi.

---

## 3. Foydalanuvchi Yo'li (User Flow)

1. Foydalanuvchi saytga kiradi va muammosini erkin tilda yozadi
2. Tizim savol-javob orqali holatni aniqlaydi (triage)
3. Agar ish advokat majburiy toifada bo'lsa — yuristga yo'naltiriladi va to'xtaydi
4. Aks holda — tizim holatni tushuntiradi va huquqlarni ko'rsatadi
5. Tizim kerakli hujjat(lar)ni taklif qiladi
6. Foydalanuvchi ma'lumotlarini kiritadi, tizim hujjatni to'ldiradi
7. Foydalanuvchi hujjatni yuklab oladi
8. Tizim keyingi qadamlar va sud tayyorgarligi yo'riqnomasini beradi
9. Hujjatlar va muddatlar shaxsiy kabinetda saqlanadi

---

## 4. Texnik Arxitektura

### 4.1. Tavsiya etilgan stek (mikro-SaaS uchun)

| Komponent | Texnologiya |
|---|---|
| Frontend | Next.js (React) + Tailwind CSS |
| Backend | .Net 9 (yoki FastAPI) |
| AI model | Claude API (Anthropic) — asosiy LLM |
| Bilim bazasi | Vektor DB (Pinecone/Qdrant) + RAG |
| Ma'lumotlar bazasi | PostgreSQL (Supabase) |
| Autentifikatsiya | Supabase Auth yoki Clerk |
| To'lov | Click, Payme (mahalliy) + Stripe |
| Hujjat generatsiyasi | docx kutubxonasi yoki PDF generator |
| Hosting | Vercel + mahalliy data saqlash (O'zbekiston) |

### 4.2. Eng muhim texnik tamoyil: RAG

Hallyutsinatsiya (AI'ning qonunni "to'qib chiqarishi") — eng katta xavf. Buni oldini olish uchun **RAG (Retrieval-Augmented Generation)** ishlatiladi:

- AI o'zidan qonun moddasi to'qimaydi
- Faqat haqiqiy bilim bazasidan (Lex.uz qonunlari, kodekslar) iqtibos keltiradi
- Har bir javobda manba havolasi ko'rsatiladi
- Agar bilim bazasida ma'lumot bo'lmasa — "bilmayman, yuristga murojaat qiling" deydi

### 4.3. Ma'lumotlar xavfsizligi

- Foydalanuvchi ma'lumotlari O'zbekiston ichida saqlanadi
- Shaxsiy ma'lumotlar shifrlanadi (AES-256)
- Mijoz ma'lumotlari AI modelini o'qitishda ishlatilmaydi
- "Shaxsiy ma'lumotlar to'g'risida"gi qonunga muvofiqlik

---

## 5. Bilim Bazasi (Eng Muhim Resurs)

Loyihaning sifati to'liq bilim bazasiga bog'liq. Quyidagilar yig'iladi va tuzilmalanadi:

- Ma'muriy javobgarlik to'g'risidagi kodeks
- Fuqarolik kodeksi (tegishli bo'limlar)
- Iste'molchilar huquqlarini himoya qilish to'g'risidagi qonun
- Mehnat kodeksi (tegishli bo'limlar)
- Fuqarolik protsessual kodeksi (jarayon qoidalari)
- Hujjat shablonlari (ariza, shikoyat, pretenziya namunalari)
- Tez-tez beriladigan savollar (FAQ) bazasi

**Manba:** Lex.uz (rasmiy huquqiy ma'lumotlar portali). Har bir qonun moddasi vektor bazaga indekslanadi.

---

## 6. Biznes Model

| Tarif | Narx | Nimani o'z ichiga oladi |
|---|---|---|
| Bepul | 0 | Holatni aniqlash + asosiy tushuntirish |
| Bir martalik | ~50 000 so'm | 1 ta to'liq ish: tushuntirish + hujjat + yo'riqnoma |
| Obuna (oylik) | ~99 000 so'm | Cheksiz ishlar + saqlash + eslatmalar |
| Yuristga ulanish | Komissiya | Murakkab ish bo'lsa — hamkor yuristga yo'naltirish |

**Qo'shimcha daromad:** hamkor advokatlar bazasiga yo'naltirish komissiyasi (murakkab ishlar uchun). Bu "advokat o'rnini bosmaslik" tamoyilini ham mustahkamlaydi.

---

## 7. Ishga Tushirish Bosqichlari

- **1-oy (MVP):** Bitta yo'nalish (masalan, iste'molchi nizolari) + suhbat + 3 ta hujjat shabloni
- **2-3 oy:** Bilim bazasini kengaytirish, RAG sozlash, 20-30 ta beta foydalanuvchi
- **4-6 oy:** To'lov tizimi, shaxsiy kabinet, yangi yo'nalishlar (mehnat, ma'muriy)
- **6-12 oy:** Yuristlar bazasi, mobil versiya, marketing va o'sish

---

## 8. Asosiy Risklar

- **Huquqiy risk:** noto'g'ri maslahat berib qo'yish. *Yechim:* aniq disklaymer, RAG, "maslahat emas" pozitsiyasi
- **Texnik risk:** AI hallyutsinatsiyasi. *Yechim:* faqat bilim bazasidan javob, manba ko'rsatish
- **Ishonch riski:** fuqarolar AI'ga ishonmasligi. *Yechim:* shaffoflik, muvaffaqiyat hikoyalari, davlat/palata hamkorligi
- **Bozor riski:** to'lashga ko'nmaslik. *Yechim:* bepul daraja + arzon bir martalik to'lov

---

## 9. Claude AI Uchun Tizim Promti (System Prompt)

Quyidagi promtni Claude API'ning system prompt qismiga joylashtiring. Bu AI'ning butun xatti-harakatini belgilaydi:

```
# ROL
Sen "Huquqim.AI" — O'zbekiston fuqarolari uchun huquqiy
yordamchisan. Sening vazifang: advokati yo'q fuqaroga o'z ishini
tushunishga, hujjat tayyorlashga va sudga tayyorlanishga yordam
berish. Sen DO'STONA, SABRLI va ODDIY tilda gaplashasan.

# ENG MUHIM QOIDALAR (hech qachon buzma)
1. Sen advokat EMASSAN va yakuniy huquqiy maslahat BERMAYSAN.
2. Har javob oxirida disklaymer qo'sh: "Bu umumiy ma'lumot.
   Murakkab holatda malakali yuristga murojaat qiling."
3. Faqat senga berilgan BILIM BAZASIDAGI qonunlardan foydalan.
   Qonun moddasini O'ZINGDAN TO'QIMA. Agar bilmasang — ochiq ayt:
   "Bu haqda aniq ma'lumotim yo'q, yuristga murojaat qiling."
4. Har bir qonuniy da'voda manbani ko'rsat (masalan: "FK 15-modda").
5. Agar ish advokat MAJBURIY bo'lgan toifada bo'lsa (og'ir jinoiy
   ish, voyaga yetmagan ayblanuvchi va h.k.) — DARHOL yuristga
   yo'naltir va davom etma.

# ISH JARAYONI
1-qadam (TRIAGE): Foydalanuvchi muammosini tingla. Savol berib
  aniqlashtir: ish turi qanaqa? qaysi organ/sud? muddatlar qanday?
2-qadam (TUSHUNTIRISH): Foydalanuvchining huquqlarini va tegishli
  qonunni ODDIY tilda, yuridik jargonsiz tushuntir.
3-qadam (HUJJAT): Kerakli hujjatni aniqla va uni tayyorlashda yordam
  ber. Foydalanuvchidan kerakli ma'lumotlarni so'ra.
4-qadam (TAYYORGARLIK): Keyingi qadamlarni va sudga tayyorgarlikni
  bosqichma-bosqich tushuntir.

# USLUB
- Qisqa, aniq, tushunarli gaplar ishlat.
- Murakkab yuridik atamani ishlatsang — darhol oddiy tilda izohla.
- Bir vaqtda bitta savol ber, foydalanuvchini ko'mib tashlama.
- Empatiya bildir: odam stress holatida bo'lishi mumkin.
- Hech qachon natijani KAFOLATLAMA ("siz albatta yutasiz" DEMA).

# CHEGARALAR
- Jinoiy ishda jazo miqdorini bashorat qilma.
- Sud qarorini KAFOLATLAMA.
- Foydalanuvchi o'rniga qaror QABUL QILMA — variantlarni ko'rsat.
- Tibbiy, moliyaviy yoki boshqa soha maslahatini berma.
```

### 9.1. Loyihani qurish uchun Claude'ga beriladigan promt

Agar siz Claude (yoki Claude Code) yordamida loyihani **QURMOQCHI** bo'lsangiz, quyidagi promtni ishlating:

```
Men "Huquqim.AI" nomli mikro-SaaS qurmoqchiman — O'zbekiston
fuqarolari uchun huquqiy AI-yordamchi. U advokati yo'q fuqaroga
kichik sud ishlarida (iste'molchi, mehnat, ma'muriy nizolar) yordam
beradi: holatni tushuntiradi, hujjat tayyorlaydi, sudga tayyorlaydi.

Quyidagilarni bosqichma-bosqich yarat:

1. Avval ma'lumotlar bazasi sxemasini tuz (PostgreSQL):
   - foydalanuvchilar, ishlar, hujjatlar, suhbatlar jadvallari

2. Keyin backend API'ni yoz (FastAPI yoki Next.js):
   - Claude API bilan RAG asosida ishlash
   - hujjat generatsiyasi (docx)
   - autentifikatsiya va to'lov

3. Keyin frontend'ni yarat (Next.js + Tailwind):
   - suhbat interfeysi (chat)
   - hujjat ko'rish/tahrirlash ekrani
   - shaxsiy kabinet

Texnik talablar:
- RAG ishlatib, AI faqat haqiqiy qonun bazasidan javob bersin
- har javobda manba va disklaymer ko'rsatilsin
- mobil-birinchi (mobile-first) dizayn, o'zbek tilida
- ma'lumotlar xavfsiz va shifrlangan bo'lsin

Har bir bosqichda menga kodni va tushuntirishni ber. Birinchi
bosqichdan (ma'lumotlar bazasi) boshla.
```

---

## 10. Keyingi Qadamlar

1. Bitta tor yo'nalishni tanlang (tavsiya: iste'molchi nizolari)
2. Shu yo'nalish bo'yicha 20-30 ta haqiqiy holatni o'rganing
3. Lex.uz'dan tegishli qonunlarni yig'ing (bilim bazasi)
4. 5 ta eng kerakli hujjat shablonini tayyorlang
5. Claude API bilan oddiy prototip quring (yuqoridagi promt bilan)
6. 10 ta fuqaroga bepul sinab ko'rdiring va fikrlarini oling
7. Yurist bilan maslahatlashing (huquqiy chegaralarni aniqlash uchun)
