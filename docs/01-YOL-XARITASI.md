# Yo'l Xaritasi (Roadmap) — Huquqim.AI

Bu fayl loyihani bosqichma-bosqich ko'tarish uchun amaliy reja. Har bir bosqich tugagach, belgilab boramiz.

---

## Bosqich 0 — Hujjatlashtirish ✅

- [x] Texnik topshiriq (TZ) yozildi
- [x] Loyiha papkasi va tuzilmasi tayyorlandi
- [x] README va yo'l xaritasi yaratildi

---

## Bosqich 1 — Bilim bazasi (knowledge-base)

**Maqsad:** Iste'molchi nizolari yo'nalishi bo'yicha qonun bazasini yig'ish.

- [ ] Yo'nalishni tanlash: **iste'molchi nizolari** (tavsiya)
- [ ] Lex.uz'dan "Iste'molchilar huquqlarini himoya qilish to'g'risida"gi qonunni yig'ish
- [ ] Fuqarolik kodeksi (tegishli moddalar) yig'ish
- [ ] Ma'muriy javobgarlik kodeksi (tegishli moddalar)
- [ ] Har bir moddani strukturalangan formatda saqlash (modda raqami, matn, manba havola)
- [ ] 20-30 ta haqiqiy holatni o'rganish va yozish

**Natija:** `knowledge-base/` papkasida tuzilmalangan qonun matnlari.

---

## Bosqich 2 — Hujjat shablonlari

**Maqsad:** 5 ta eng kerakli hujjat shablonini tayyorlash.

- [ ] Da'vo arizasi shabloni
- [ ] Pretenziya (rasmiy talabnoma) shabloni
- [ ] Shikoyat shabloni
- [ ] Tushuntirish xati shabloni
- [ ] Apellyatsiya shikoyati shabloni

**Natija:** To'ldiriladigan o'zgaruvchilar (placeholder) bilan docx shablonlar.

---

## Bosqich 3 — Backend + RAG prototip

**Maqsad:** Claude API bilan ishlaydigan oddiy backend.

- [ ] PostgreSQL sxema: foydalanuvchilar, ishlar, hujjatlar, suhbatlar
- [ ] Vektor DB sozlash (Qdrant) va qonun bazasini indekslash
- [ ] RAG pipeline: savol → qidiruv → Claude → manbali javob
- [ ] System prompt'ni o'rnatish (TZ 9-bo'lim)
- [ ] Triage logikasi (advokat majburiymi tekshirish)
- [ ] Hujjat generatsiyasi (docx to'ldirish)

**Natija:** API orqali suhbat + hujjat olish mumkin.

---

## Bosqich 4 — Frontend (MVP)

**Maqsad:** Foydalanuvchi interfeysi.

- [ ] Next.js + Tailwind loyiha
- [ ] Chat (suhbat) interfeysi
- [ ] Hujjat ko'rish/tahrirlash/yuklab olish ekrani
- [ ] Disklaymer har joyda ko'rsatilsin
- [ ] Mobil-birinchi dizayn, o'zbek tilida

**Natija:** Ishlaydigan MVP — chatdan hujjatgacha.

---

## Bosqich 5 — Test va sayqal

- [ ] 10 ta fuqaroga bepul sinov
- [ ] Fikrlarni yig'ish va xatolarni tuzatish
- [ ] Yurist bilan huquqiy chegaralarni aniqlash

---

## Bosqich 6 — To'lov, kabinet, o'sish

- [ ] Click / Payme integratsiyasi
- [ ] Shaxsiy kabinet + hujjat saqlash + muddat eslatma
- [ ] Yangi yo'nalishlar (mehnat, ma'muriy)
- [ ] Yuristlar bazasi, marketing

---

## Tavsiya: qaysi bosqichdan boshlaymiz?

Eng to'g'ri yo'l — **Bosqich 1 (bilim bazasi)** dan boshlash, chunki butun loyiha sifati shunga bog'liq. Iste'molchi nizolari yo'nalishini tanlab, Lex.uz'dan qonunlarni yig'ishdan boshlaymiz.
