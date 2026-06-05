using Huquqim.Domain.Enums;

namespace Huquqim.Infrastructure.Persistence;

/// <summary>
/// 50+ sud ishlari va huquqiy hujjat shablonlari ma'lumotlari.
/// Har biri: nom, tavsif, ish turi, hujjat turi, maydonlar, andoza matni.
/// </summary>
public static class TemplateSeedData
{
    public record Field(string Key, string Label, bool Required = true, string? Placeholder = null);

    public record TemplateDef(
        EDocumentType Type,
        ECaseType CaseType,
        string Name,
        string Description,
        Field[] Fields,
        string Body);

    // Ko'p shablonlarda takrorlanadigan umumiy maydonlar
    private static readonly Field Fio = new("fio", "Sizning F.I.O.", true, "Familiya Ism Otasining ismi");
    private static readonly Field Manzil = new("manzil", "Yashash manzilingiz", true);
    private static readonly Field Tel = new("telefon", "Telefon raqamingiz", true, "+998...");
    private static readonly Field Sud = new("sud", "Sud nomi", true, "... tumanlararo fuqarolik ishlari bo'yicha sudi");

    private const string Disclaimer =
        "\n\n---\nBu umumiy ma'lumot. Murakkab holatlar uchun malakali yuristga murojaat qiling.";

    public static IReadOnlyList<TemplateDef> All { get; } = Build();

    private static List<TemplateDef> Build()
    {
        var list = new List<TemplateDef>();

        // ============ RASMIY DA'VO ARIZASI (FPK — to'liq to'ldirilgan namuna) ============
        // Maydonlarsiz — bosilganda darrov to'ldirilgan namuna docx yuklanadi.
        list.Add(new(EDocumentType.Claim, ECaseType.Civil,
            "Da'vo arizasi (rasmiy namuna — FPK)",
            "To'liq to'ldirilgan rasmiy namuna. Bosing — tayyor Word hujjatni darrov yuklab oling, keyin o'zingizga moslang.",
            Array.Empty<Field>(),
            "[c]Chilonzor tuman (shahar) fuqarolik ishlari bo'yicha sudiga\n" +
            "[c]**Da'vogar:** Karimov Akmal Botir o'g'li\n" +
            "[c]yashash manzili: Toshkent sh., Chilonzor tumani, Bunyodkor ko'chasi 12-uy\n" +
            "[c]telefon: +998 90 123 45 67   e-mail: karimov.akmal@mail.uz\n" +
            "\n" +
            "[c]**Javobgar:** \"TexnoSavdo\" MChJ\n" +
            "[c]manzili (joylashgan yeri): Toshkent sh., Yunusobod tumani, Amir Temur ko'chasi 45-uy\n" +
            "[c]telefon / rekvizitlar: +998 71 200 00 00, STIR: 301234567\n" +
            "\n" +
            "[c]Uchinchi shaxs (bo'lsa): —\n" +
            "\n" +
            "[c]**Da'vo (nizo) qiymati:** 5 000 000 so'm\n" +
            "[c]**To'langan davlat boji:** 150 000 so'm\n" +
            "\n" +
            "[c]**DA'VO ARIZASI**\n" +
            "[c]*(da'vo predmeti: qarz summasini undirish to'g'risida)*\n" +
            "\n" +
            "**1. Ishning holatlari**\n" +
            "2026-yil 10-yanvarda men bilan javobgar \"TexnoSavdo\" MChJ o'rtasida No 27-sonli " +
            "oldi-sotdi shartnomasi tuzilgan. Shartnomaga ko'ra men javobgarga 5 000 000 so'm " +
            "miqdorida oldindan to'lov amalga oshirganman. Biroq javobgar shartnomada ko'rsatilgan " +
            "muddatda (2026-yil 1-fevral) o'z majburiyatini bajarmadi — tovar yetkazib bermadi va " +
            "to'langan summani ham qaytarmadi. Mening yozma talabnomamga ham javob bermadi.\n" +
            "\n" +
            "**2. Huquqiy asos (qonun moddalariga havola)**\n" +
            "Yuqorida bayon etilgan holatlar va O'zbekiston Respublikasi Fuqarolik kodeksining " +
            "327-moddasi hamda O'zbekiston Respublikasi Fuqarolik protsessual kodeksining " +
            "188, 189 va 191-moddalariga asosan,\n" +
            "\n" +
            "**3. SO'RAYMAN:**\n" +
            "1) Javobgar \"TexnoSavdo\" MChJ dan mening foydamga 5 000 000 so'm miqdoridagi " +
            "qarz summasini undirib berishni;\n" +
            "2) Da'vo arizasini berishda men tomonimdan to'langan 150 000 so'm davlat bojini " +
            "javobgardan undirib berishni;\n" +
            "3) Majburiyat bajarilmagani uchun qonunda belgilangan miqdorda penya undirib berishni.\n" +
            "\n" +
            "**4. Ilova qilinadigan hujjatlar (FPK 191-modda):**\n" +
            "1. Da'vo arizasining javobgar (va uchinchi shaxslar) soni bo'yicha nusxalari;\n" +
            "2. Davlat boji to'langanligini tasdiqlovchi hujjat (kvitansiya / to'lov topshiriqnomasi);\n" +
            "3. Da'vo talabini asoslovchi hujjatlar nusxalari: oldi-sotdi shartnomasi, to'lov " +
            "topshiriqnomasi, yozma talabnoma nusxasi;\n" +
            "4. Vakil orqali berilsa — ishonchnoma yoki vakolatni tasdiqlovchi hujjat.\n" +
            "\n" +
            "Sana: \"____\" ____________ 20__ y.        Imzo: ____________ / Karimov A.B." + Disclaimer));

        // ============ ISTE'MOLCHI (Consumer) ============
        list.Add(new(EDocumentType.Pretension, ECaseType.Consumer,
            "Iste'molchi pretenziyasi (talabnoma)",
            "Do'kon yoki sotuvchiga rasmiy talabnoma. Sudga murojaatdan oldingi bosqich.",
            new[] {
                new Field("sotuvchi", "Sotuvchi (do'kon) nomi", true, "MChJ \"...\""),
                Fio, Manzil, Tel,
                new Field("tovar", "Tovar nomi", true),
                new Field("sana", "Xarid sanasi", true, "2026-01-01"),
                new Field("summa", "To'langan summa (so'm)", true),
                new Field("muammo", "Muammo tavsifi", true),
                new Field("talab", "Talabingiz", true, "Pulni qaytarish / almashtirish"),
            },
            "{{sotuvchi}} ga\n\n{{fio}} dan\nManzil: {{manzil}}\nTelefon: {{telefon}}\n\nPRETENZIYA (TALABNOMA)\n\n" +
            "{{sana}} sanasida men Sizning do'koningizdan \"{{tovar}}\" tovarini {{summa}} so'mga sotib oldim.\n\n" +
            "Quyidagi muammo aniqlandi: {{muammo}}.\n\n" +
            "\"Iste'molchilarning huquqlarini himoya qilish to'g'risida\"gi qonunning 13-moddasiga muvofiq, " +
            "Sizdan quyidagini TALAB QILAMAN: {{talab}}.\n\n" +
            "Ushbu talabnoma topshirilgan kundan boshlab 10 kun ichida javob berishingizni so'rayman.\n\n" +
            "Sana: ____________        Imzo: ____________" + Disclaimer));

        list.Add(new(EDocumentType.Claim, ECaseType.Consumer,
            "Da'vo arizasi (iste'molchi)",
            "Sudga beriladigan da'vo arizasi namunasi (iste'molchi nizosi).",
            new[] {
                Sud, Fio, Manzil,
                new Field("javobgar", "Javobgar (do'kon/tashkilot)", true),
                new Field("javobgar_manzil", "Javobgar manzili", false),
                new Field("holat", "Ish holati bayoni", true),
                new Field("talab", "Da'vo talabi", true),
                new Field("summa", "Da'vo summasi (so'm)", false),
            },
            "{{sud}} ga\n\nDa'vogar: {{fio}}, manzil: {{manzil}}\nJavobgar: {{javobgar}}, manzil: {{javobgar_manzil}}\n\n" +
            "DA'VO ARIZASI\n\nIsh holati: {{holat}}\n\n" +
            "\"Iste'molchilarning huquqlarini himoya qilish to'g'risida\"gi qonun va Fuqarolik kodeksining " +
            "15-moddasiga asoslanib,\n\nSO'RAYMAN:\n{{talab}}\nDa'vo summasi: {{summa}} so'm.\n\n" +
            "Ilova: dalil hujjatlar nusxalari.\n\nSana: ____________        Imzo: ____________" + Disclaimer));

        list.Add(new(EDocumentType.Complaint, ECaseType.Consumer,
            "Iste'molchilar agentligiga shikoyat",
            "Raqobat va iste'molchilar huquqlarini himoya qilish qo'mitasiga shikoyat.",
            new[] {
                Fio, Manzil, Tel,
                new Field("sotuvchi", "Sotuvchi (do'kon) nomi", true),
                new Field("holat", "Nima sodir bo'ldi", true),
                new Field("talab", "Talabingiz", true),
            },
            "Raqobatni rivojlantirish va iste'molchilar huquqlarini himoya qilish qo'mitasiga\n\n" +
            "{{fio}} dan\nManzil: {{manzil}}\nTelefon: {{telefon}}\n\nSHIKOYAT\n\n" +
            "{{sotuvchi}} tomonidan mening iste'molchi huquqlarim buzildi.\n\nHolat: {{holat}}\n\n" +
            "So'rayman: {{talab}}.\n\nSana: ____________        Imzo: ____________" + Disclaimer));

        // ============ MEHNAT (Labor) ============
        list.Add(new(EDocumentType.Pretension, ECaseType.Labor,
            "Ish haqi talabnomasi (ish beruvchiga)",
            "To'lanmagan ish haqini talab qilish uchun ish beruvchiga rasmiy xat.",
            new[] {
                new Field("tashkilot", "Tashkilot (ish beruvchi) nomi", true),
                Fio, new Field("lavozim", "Lavozimingiz", true),
                new Field("davr", "Qaysi davr uchun (oylar)", true, "Yanvar-Fevral 2026"),
                new Field("summa", "Talab qilinayotgan summa (so'm)", true),
            },
            "{{tashkilot}} rahbariga\n\n{{fio}} dan\nLavozim: {{lavozim}}\n\nTALABNOMA\n\n" +
            "Men {{tashkilot}} da {{lavozim}} lavozimida ishlayman. {{davr}} uchun ish haqim to'lanmadi.\n\n" +
            "Mehnat kodeksiga muvofiq, ish haqi o'z vaqtida to'liq to'lanishi shart.\n\n" +
            "Shuning uchun {{summa}} so'm miqdoridagi ish haqimni 3 kun ichida to'lashingizni TALAB QILAMAN.\n\n" +
            "Aks holda mehnat nizolari komissiyasi yoki sudga murojaat qilaman.\n\n" +
            "Sana: ____________        Imzo: ____________" + Disclaimer));

        list.Add(new(EDocumentType.Claim, ECaseType.Labor,
            "Da'vo arizasi (ishga tiklash)",
            "Noqonuniy ishdan bo'shatilganda ishga qayta tiklanish uchun sud da'vosi.",
            new[] {
                Sud, Fio, Manzil,
                new Field("tashkilot", "Tashkilot (javobgar)", true),
                new Field("lavozim", "Lavozimingiz", true),
                new Field("bushatish_sana", "Ishdan bo'shatilgan sana", true),
                new Field("sabab", "Bo'shatish sababi (buyruqdagi)", true),
            },
            "{{sud}} ga\n\nDa'vogar: {{fio}}, manzil: {{manzil}}\nJavobgar: {{tashkilot}}\n\n" +
            "DA'VO ARIZASI (ishga tiklash haqida)\n\n" +
            "Men {{tashkilot}} da {{lavozim}} lavozimida ishlaganman. {{bushatish_sana}} sanasida " +
            "\"{{sabab}}\" sababi bilan ishdan bo'shatildim. Bu noqonuniy deb hisoblayman.\n\n" +
            "Mehnat kodeksiga asoslanib, SO'RAYMAN:\n" +
            "1. Meni avvalgi lavozimimga tiklash;\n" +
            "2. Majburiy progul davri uchun o'rtacha ish haqini undirish.\n\n" +
            "Sana: ____________        Imzo: ____________" + Disclaimer));

        list.Add(new(EDocumentType.Explanatory, ECaseType.Labor,
            "Tushuntirish xati (ish beruvchiga)",
            "Intizomiy nizo yoki holat bo'yicha ish beruvchiga tushuntirish.",
            new[] {
                new Field("tashkilot", "Tashkilot nomi", true),
                Fio, new Field("lavozim", "Lavozimingiz", true),
                new Field("holat", "Tushuntirish (nima bo'ldi)", true),
            },
            "{{tashkilot}} rahbariga\n\n{{fio}} ({{lavozim}}) dan\n\nTUSHUNTIRISH XATI\n\n{{holat}}\n\n" +
            "Sana: ____________        Imzo: ____________" + Disclaimer));

        // ============ MA'MURIY (Administrative) ============
        list.Add(new(EDocumentType.Complaint, ECaseType.Administrative,
            "Ma'muriy jarima ustidan shikoyat",
            "Noto'g'ri berilgan ma'muriy jarima (protokol) ustidan yuqori organ yoki sudga shikoyat.",
            new[] {
                new Field("organ", "Qaror chiqargan organ", true, "... YPX bo'limi"),
                Fio, Manzil, Tel,
                new Field("qaror_raqam", "Qaror/protokol raqami", true),
                new Field("qaror_sana", "Qaror sanasi", true),
                new Field("sabab", "Nega rozi emassiz", true),
            },
            "{{organ}} yuqori turuvchi organiga\n(yoki ma'muriy sudga)\n\n" +
            "{{fio}} dan\nManzil: {{manzil}}\nTelefon: {{telefon}}\n\nSHIKOYAT\n\n" +
            "{{qaror_sana}} sanasida {{organ}} tomonidan {{qaror_raqam}}-sonli ma'muriy jarima qarori chiqarilgan.\n\n" +
            "Men ushbu qaror bilan rozi emasman, chunki: {{sabab}}.\n\n" +
            "Ma'muriy javobgarlik to'g'risidagi kodeksning 307 va 308-moddalariga asoslanib, qarorni BEKOR QILISHNI so'rayman.\n\n" +
            "Eslatma: shikoyat qaror nusxasi olingan kundan 10 kun ichida beriladi.\n\n" +
            "Sana: ____________        Imzo: ____________" + Disclaimer));

        list.Add(new(EDocumentType.Objection, ECaseType.Administrative,
            "Protokolga e'tiroz",
            "Tuzilgan ma'muriy huquqbuzarlik protokoliga e'tiroz bildirish.",
            new[] {
                new Field("organ", "Protokol tuzgan organ", true),
                Fio, new Field("protokol", "Protokol raqami va sanasi", true),
                new Field("etiroz", "E'tirozingiz mazmuni", true),
            },
            "{{organ}} ga\n\n{{fio}} dan\n\nE'TIROZ\n\n" +
            "{{protokol}} protokoliga quyidagi e'tirozni bildiraman:\n\n{{etiroz}}\n\n" +
            "Sana: ____________        Imzo: ____________" + Disclaimer));

        // ============ FUQAROLIK (Civil) ============
        list.Add(new(EDocumentType.Claim, ECaseType.Civil,
            "Da'vo arizasi (qarzni undirish)",
            "Berilgan qarzni qaytarib olish uchun sud da'vosi.",
            new[] {
                Sud, Fio, Manzil,
                new Field("qarzdor", "Qarzdor F.I.O.", true),
                new Field("qarzdor_manzil", "Qarzdor manzili", false),
                new Field("summa", "Qarz summasi (so'm)", true),
                new Field("sana", "Qarz berilgan sana", true),
                new Field("holat", "Qisqa bayon", true),
            },
            "{{sud}} ga\n\nDa'vogar: {{fio}}, manzil: {{manzil}}\nJavobgar: {{qarzdor}}, manzil: {{qarzdor_manzil}}\n\n" +
            "DA'VO ARIZASI (qarzni undirish)\n\n" +
            "{{sana}} sanasida men javobgarga {{summa}} so'm qarz berdim. {{holat}}\n" +
            "Belgilangan muddatda qarz qaytarilmadi.\n\n" +
            "Fuqarolik kodeksiga asoslanib, SO'RAYMAN: javobgardan {{summa}} so'm qarzni undirish.\n\n" +
            "Ilova: qarz tilxati/dalil nusxalari.\n\nSana: ____________        Imzo: ____________" + Disclaimer));

        list.Add(new(EDocumentType.Claim, ECaseType.Civil,
            "Da'vo arizasi (yetkazilgan zarar)",
            "Mol-mulkka yoki sog'liqqa yetkazilgan zararni qoplash uchun da'vo.",
            new[] {
                Sud, Fio, Manzil,
                new Field("javobgar", "Javobgar F.I.O./tashkilot", true),
                new Field("zarar", "Zarar tavsifi", true),
                new Field("summa", "Zarar summasi (so'm)", true),
            },
            "{{sud}} ga\n\nDa'vogar: {{fio}}, manzil: {{manzil}}\nJavobgar: {{javobgar}}\n\n" +
            "DA'VO ARIZASI (zararni qoplash)\n\nJavobgar tomonidan menga zarar yetkazildi: {{zarar}}\n\n" +
            "Fuqarolik kodeksining 985-moddasiga (zarar yetkazish oqibatida vujudga keladigan majburiyatlar) asoslanib, " +
            "SO'RAYMAN: javobgardan {{summa}} so'm zararni undirish.\n\n" +
            "Sana: ____________        Imzo: ____________" + Disclaimer));

        list.Add(new(EDocumentType.Claim, ECaseType.Civil,
            "Da'vo arizasi (shartnoma bo'yicha)",
            "Shartnoma majburiyatlari bajarilmaganda da'vo.",
            new[] {
                Sud, Fio, Manzil,
                new Field("javobgar", "Javobgar", true),
                new Field("shartnoma", "Shartnoma raqami va sanasi", true),
                new Field("buzilish", "Qanday buzildi", true),
                new Field("talab", "Talabingiz", true),
            },
            "{{sud}} ga\n\nDa'vogar: {{fio}}, manzil: {{manzil}}\nJavobgar: {{javobgar}}\n\n" +
            "DA'VO ARIZASI (shartnoma bo'yicha)\n\n" +
            "{{shartnoma}} shartnomasi tuzilgan. Javobgar majburiyatini bajarmadi: {{buzilish}}\n\n" +
            "SO'RAYMAN: {{talab}}.\n\nSana: ____________        Imzo: ____________" + Disclaimer));

        // ============ OILA (Civil deb belgilanadi) ============
        list.Add(new(EDocumentType.Claim, ECaseType.Civil,
            "Da'vo arizasi (nikohdan ajralish)",
            "Nikohni bekor qilish (ajralish) uchun sud da'vosi.",
            new[] {
                Sud, Fio, Manzil,
                new Field("turmush_ortok", "Turmush o'rtog'i F.I.O.", true),
                new Field("nikoh_sana", "Nikoh sanasi", true),
                new Field("sabab", "Ajralish sababi", true),
                new Field("bolalar", "Voyaga yetmagan bolalar (bo'lsa)", false),
            },
            "{{sud}} ga\n\nDa'vogar: {{fio}}, manzil: {{manzil}}\nJavobgar: {{turmush_ortok}}\n\n" +
            "DA'VO ARIZASI (nikohdan ajralish)\n\n" +
            "{{nikoh_sana}} sanasida nikohdan o'tganmiz. Birga yashash imkoni yo'q: {{sabab}}\n" +
            "Voyaga yetmagan bolalar: {{bolalar}}\n\n" +
            "Oila kodeksiga asoslanib, SO'RAYMAN: nikohni bekor qilish.\n\n" +
            "Sana: ____________        Imzo: ____________" + Disclaimer));

        list.Add(new(EDocumentType.Claim, ECaseType.Civil,
            "Da'vo arizasi (aliment undirish)",
            "Bolalar uchun aliment (nafaqa) undirish da'vosi.",
            new[] {
                Sud, Fio, Manzil,
                new Field("javobgar", "Javobgar F.I.O.", true),
                new Field("bolalar", "Bolalar (ism, tug'ilgan yili)", true),
            },
            "{{sud}} ga\n\nDa'vogar: {{fio}}, manzil: {{manzil}}\nJavobgar: {{javobgar}}\n\n" +
            "DA'VO ARIZASI (aliment undirish)\n\nUmumiy voyaga yetmagan bolalarimiz: {{bolalar}}.\n" +
            "Javobgar bolalarni ta'minlashda qatnashmayapti.\n\n" +
            "Oila kodeksiga asoslanib, SO'RAYMAN: bolalar uchun aliment undirish.\n\n" +
            "Sana: ____________        Imzo: ____________" + Disclaimer));

        // ============ UY-JOY / IJARA (Rent) ============
        list.Add(new(EDocumentType.Pretension, ECaseType.Rent,
            "Ijara talabnomasi (ijara haqi)",
            "Ijaraga oid nizo bo'yicha rasmiy talabnoma.",
            new[] {
                new Field("taraf", "Qarama-qarshi taraf F.I.O.", true),
                Fio, Tel,
                new Field("obyekt", "Ijara obyekti (manzil)", true),
                new Field("talab", "Talabingiz", true),
            },
            "{{taraf}} ga\n\n{{fio}} dan\nTelefon: {{telefon}}\n\nTALABNOMA\n\n" +
            "{{obyekt}} ijara shartnomasi yuzasidan: {{talab}}.\n\n" +
            "10 kun ichida javob berishingizni so'rayman.\n\nSana: ____________        Imzo: ____________" + Disclaimer));

        list.Add(new(EDocumentType.Claim, ECaseType.Rent,
            "Da'vo arizasi (ijara nizosi)",
            "Uy-joy yoki mulk ijarasi bo'yicha sud da'vosi.",
            new[] {
                Sud, Fio, Manzil,
                new Field("javobgar", "Javobgar", true),
                new Field("obyekt", "Ijara obyekti", true),
                new Field("holat", "Nizo bayoni", true),
                new Field("talab", "Talab", true),
            },
            "{{sud}} ga\n\nDa'vogar: {{fio}}, manzil: {{manzil}}\nJavobgar: {{javobgar}}\n\n" +
            "DA'VO ARIZASI (ijara nizosi)\n\nObyekt: {{obyekt}}\nHolat: {{holat}}\n\n" +
            "SO'RAYMAN: {{talab}}.\n\nSana: ____________        Imzo: ____________" + Disclaimer));

        // ============ UMUMIY SUD HUJJATLARI (har xil) ============
        list.Add(new(EDocumentType.Appeal, ECaseType.Civil,
            "Apellyatsiya shikoyati",
            "Birinchi instansiya sud qarori ustidan apellyatsiya shikoyati.",
            new[] {
                new Field("apel_sud", "Apellyatsiya sudi nomi", true),
                Fio, Manzil,
                new Field("birinchi_sud", "Qaror chiqargan sud", true),
                new Field("qaror_sana", "Qaror sanasi", true),
                new Field("sabab", "Nega rozi emassiz", true),
            },
            "{{apel_sud}} ga\n\nApellyatsiya beruvchi: {{fio}}, manzil: {{manzil}}\n\n" +
            "APELLYATSIYA SHIKOYATI\n\n" +
            "{{birinchi_sud}} tomonidan {{qaror_sana}} sanasida chiqarilgan qaror bilan rozi emasman.\n\n" +
            "Sabab: {{sabab}}\n\n" +
            "SO'RAYMAN: birinchi instansiya sud qarorini bekor qilish va yangi qaror chiqarish.\n\n" +
            "Eslatma: apellyatsiya muddati qaror e'lon qilingandan keyin belgilangan muddat ichida.\n\n" +
            "Sana: ____________        Imzo: ____________" + Disclaimer));

        list.Add(new(EDocumentType.Objection, ECaseType.Civil,
            "Da'voga e'tiroz (javobgar uchun)",
            "Sizga qo'yilgan da'voga qarshi e'tiroz (javob).",
            new[] {
                Sud, Fio, Manzil,
                new Field("davogar", "Da'vogar F.I.O.", true),
                new Field("etiroz", "E'tirozingiz (nega rozi emassiz)", true),
            },
            "{{sud}} ga\n\nJavobgar: {{fio}}, manzil: {{manzil}}\nDa'vogar: {{davogar}}\n\n" +
            "E'TIROZ (da'voga javob)\n\nDa'vogarning talablari bilan rozi emasman, chunki:\n\n{{etiroz}}\n\n" +
            "SO'RAYMAN: da'voni rad etish.\n\nSana: ____________        Imzo: ____________" + Disclaimer));

        list.Add(new(EDocumentType.Explanatory, ECaseType.Civil,
            "Ariza (umumiy shakl)",
            "Davlat organi yoki tashkilotga umumiy ariza shakli.",
            new[] {
                new Field("organ", "Kimga (organ/tashkilot)", true),
                Fio, Manzil, Tel,
                new Field("mazmun", "Ariza mazmuni", true),
                new Field("soral", "So'rovingiz", true),
            },
            "{{organ}} ga\n\n{{fio}} dan\nManzil: {{manzil}}\nTelefon: {{telefon}}\n\nARIZA\n\n" +
            "{{mazmun}}\n\nSo'rayman: {{soral}}.\n\nSana: ____________        Imzo: ____________" + Disclaimer));

        // ============ QO'SHIMCHA AMALIY SHABLONLAR (50+ ga yetkazish) ============

        // — Iste'molchi qo'shimcha
        list.Add(new(EDocumentType.Pretension, ECaseType.Consumer,
            "Xizmat sifatsizligi uchun talabnoma",
            "Sifatsiz ko'rsatilgan xizmat (ta'mir, kafe, salon va h.k.) uchun talabnoma.",
            new[] { new Field("ijrochi", "Xizmat ko'rsatuvchi", true), Fio, Tel,
                new Field("xizmat", "Qanday xizmat", true), new Field("summa", "To'langan summa", true),
                new Field("talab", "Talabingiz", true) },
            "{{ijrochi}} ga\n\n{{fio}} dan\nTelefon: {{telefon}}\n\nTALABNOMA\n\n" +
            "Menga \"{{xizmat}}\" xizmati sifatsiz ko'rsatildi. To'langan: {{summa}} so'm.\n\n" +
            "TALAB QILAMAN: {{talab}}.\n\nSana: ____________   Imzo: ____________" + Disclaimer));

        list.Add(new(EDocumentType.Claim, ECaseType.Consumer,
            "Kafolat majburiyati bo'yicha da'vo",
            "Kafolat muddatida buzilgan tovar bo'yicha sud da'vosi.",
            new[] { Sud, Fio, Manzil, new Field("sotuvchi", "Sotuvchi", true),
                new Field("tovar", "Tovar", true), new Field("summa", "Summa", true) },
            "{{sud}} ga\n\nDa'vogar: {{fio}}, {{manzil}}\nJavobgar: {{sotuvchi}}\n\n" +
            "DA'VO ARIZASI (kafolat)\n\n\"{{tovar}}\" kafolat muddatida buzildi, sotuvchi javob bermadi.\n\n" +
            "SO'RAYMAN: {{summa}} so'm qaytarish yoki almashtirish.\n\nSana: ______   Imzo: ______" + Disclaimer));

        // — Mehnat qo'shimcha
        list.Add(new(EDocumentType.Complaint, ECaseType.Labor,
            "Mehnat inspeksiyasiga shikoyat",
            "Mehnat huquqlari buzilganda Mehnat inspeksiyasiga shikoyat.",
            new[] { Fio, Tel, new Field("tashkilot", "Ish beruvchi", true),
                new Field("buzilish", "Qanday huquq buzildi", true) },
            "Davlat mehnat inspeksiyasiga\n\n{{fio}} dan\nTelefon: {{telefon}}\n\nSHIKOYAT\n\n" +
            "{{tashkilot}} tomonidan mehnat huquqlarim buzildi: {{buzilish}}\n\n" +
            "Tekshiruv o'tkazishingizni so'rayman.\n\nSana: ______   Imzo: ______" + Disclaimer));

        list.Add(new(EDocumentType.Explanatory, ECaseType.Labor,
            "Ariza (mehnat ta'tili)",
            "Mehnat ta'tili yoki ta'tilsiz kun so'rash arizasi.",
            new[] { new Field("tashkilot", "Tashkilot", true), Fio,
                new Field("lavozim", "Lavozim", true), new Field("muddat", "Qaysi sanadan-qaysi sanagacha", true),
                new Field("sabab", "Sabab", false) },
            "{{tashkilot}} rahbariga\n\n{{fio}} ({{lavozim}}) dan\n\nARIZA\n\n" +
            "{{muddat}} davriga ta'til berishingizni so'rayman. {{sabab}}\n\n" +
            "Sana: ______   Imzo: ______" + Disclaimer));

        list.Add(new(EDocumentType.Explanatory, ECaseType.Labor,
            "Ariza (ishdan bo'shash)",
            "O'z xohishi bilan ishdan bo'shash arizasi.",
            new[] { new Field("tashkilot", "Tashkilot", true), Fio,
                new Field("lavozim", "Lavozim", true), new Field("sana", "Bo'shash sanasi", true) },
            "{{tashkilot}} rahbariga\n\n{{fio}} ({{lavozim}}) dan\n\nARIZA\n\n" +
            "Meni {{sana}} sanasidan o'z xohishimga ko'ra ishdan bo'shatishingizni so'rayman.\n\n" +
            "Sana: ______   Imzo: ______" + Disclaimer));

        // — Ma'muriy qo'shimcha
        list.Add(new(EDocumentType.Complaint, ECaseType.Administrative,
            "Tezlik jarimasi ustidan shikoyat",
            "Yo'l harakati (radar, tezlik) jarimasi ustidan shikoyat.",
            new[] { new Field("organ", "YPX bo'limi", true), Fio, Manzil, Tel,
                new Field("qaror", "Qaror/jarima raqami", true), new Field("sabab", "Nega rozi emassiz", true) },
            "{{organ}} yuqori organiga (yoki ma'muriy sudga)\n\n{{fio}} dan\nManzil: {{manzil}}\nTel: {{telefon}}\n\n" +
            "SHIKOYAT\n\n{{qaror}}-sonli tezlik jarimasi bilan rozi emasman: {{sabab}}\n\n" +
            "MJTK 307-308-moddalariga asoslanib, qarorni BEKOR QILISHNI so'rayman (10 kun ichida).\n\n" +
            "Sana: ______   Imzo: ______" + Disclaimer));

        list.Add(new(EDocumentType.Explanatory, ECaseType.Administrative,
            "Tushuntirish xati (ma'muriy ish)",
            "Ma'muriy ish bo'yicha organga tushuntirish.",
            new[] { new Field("organ", "Organ", true), Fio, new Field("holat", "Tushuntirish", true) },
            "{{organ}} ga\n\n{{fio}} dan\n\nTUSHUNTIRISH\n\n{{holat}}\n\nSana: ______   Imzo: ______" + Disclaimer));

        // — Fuqarolik / mulk qo'shimcha
        list.Add(new(EDocumentType.Claim, ECaseType.Civil,
            "Da'vo arizasi (mulkni talab qilish)",
            "O'z mulkini boshqa shaxsdan qaytarib olish da'vosi.",
            new[] { Sud, Fio, Manzil, new Field("javobgar", "Javobgar", true),
                new Field("mulk", "Mulk tavsifi", true) },
            "{{sud}} ga\n\nDa'vogar: {{fio}}, {{manzil}}\nJavobgar: {{javobgar}}\n\n" +
            "DA'VO ARIZASI (mulkni talab qilish)\n\nMening mulkim javobgarda: {{mulk}}\n\n" +
            "FK 228-moddasiga asoslanib, SO'RAYMAN: mulkni qaytarish.\n\nSana: ______   Imzo: ______" + Disclaimer));

        list.Add(new(EDocumentType.Claim, ECaseType.Civil,
            "Da'vo arizasi (meros bo'yicha)",
            "Meros mulkini taqsimlash yoki huquqni tan olish da'vosi.",
            new[] { Sud, Fio, Manzil, new Field("merosxor", "Boshqa merosxo'rlar", false),
                new Field("mulk", "Meros mulki", true), new Field("talab", "Talabingiz", true) },
            "{{sud}} ga\n\nDa'vogar: {{fio}}, {{manzil}}\n\nDA'VO ARIZASI (meros)\n\n" +
            "Meros mulki: {{mulk}}\nBoshqa merosxo'rlar: {{merosxor}}\n\nSO'RAYMAN: {{talab}}.\n\n" +
            "Sana: ______   Imzo: ______" + Disclaimer));

        list.Add(new(EDocumentType.Claim, ECaseType.Civil,
            "Da'vo arizasi (sha'n va qadr-qimmat)",
            "Sha'n, qadr-qimmat yoki ishchanlik obro'sini himoya qilish da'vosi.",
            new[] { Sud, Fio, Manzil, new Field("javobgar", "Javobgar", true),
                new Field("holat", "Qanday tuhmat/haqorat", true) },
            "{{sud}} ga\n\nDa'vogar: {{fio}}, {{manzil}}\nJavobgar: {{javobgar}}\n\n" +
            "DA'VO ARIZASI (sha'n va qadr-qimmat)\n\n{{holat}}\n\n" +
            "FK 100-moddasiga asoslanib, SO'RAYMAN: ma'lumotni rad etish va ma'naviy zararni qoplash.\n\n" +
            "Sana: ______   Imzo: ______" + Disclaimer));

        // — Sud jarayoni hujjatlari
        list.Add(new(EDocumentType.Explanatory, ECaseType.Civil,
            "Iltimosnoma (sudga)",
            "Sud jarayonida turli iltimos (dalil so'rash, muddatni uzaytirish va h.k.).",
            new[] { Sud, Fio, new Field("ish", "Ish raqami (bo'lsa)", false),
                new Field("iltimos", "Iltimosingiz", true) },
            "{{sud}} ga\n\n{{fio}} dan\nIsh: {{ish}}\n\nILTIMOSNOMA\n\n{{iltimos}}\n\n" +
            "Sana: ______   Imzo: ______" + Disclaimer));

        list.Add(new(EDocumentType.Explanatory, ECaseType.Civil,
            "Sud xarajatlarini undirish arizasi",
            "Yutgan tomon sud xarajatlarini qaytarib olish arizasi.",
            new[] { Sud, Fio, new Field("summa", "Xarajat summasi", true),
                new Field("javobgar", "Kimdan", true) },
            "{{sud}} ga\n\n{{fio}} dan\n\nARIZA (sud xarajatlari)\n\n" +
            "Ish bo'yicha {{summa}} so'm xarajat qildim. SO'RAYMAN: {{javobgar}} dan undirish.\n\n" +
            "Sana: ______   Imzo: ______" + Disclaimer));

        list.Add(new(EDocumentType.Explanatory, ECaseType.Civil,
            "Ijro varaqasini berish arizasi",
            "Sud qarori kuchga kirgach ijro varaqasi olish.",
            new[] { Sud, Fio, new Field("ish", "Ish raqami", true) },
            "{{sud}} ga\n\n{{fio}} dan\n\nARIZA\n\n{{ish}} ish bo'yicha qaror kuchga kirdi. " +
            "Ijro varaqasini berishingizni so'rayman.\n\nSana: ______   Imzo: ______" + Disclaimer));

        // — Ijro (sud qarorini bajartirish)
        list.Add(new(EDocumentType.Explanatory, ECaseType.Civil,
            "Ariza (ijro byurosiga)",
            "Sud qarorini majburiy ijro qildirish uchun ijro byurosiga ariza.",
            new[] { Fio, Manzil, Tel, new Field("qaror", "Qaror/ijro varaqasi", true),
                new Field("qarzdor", "Qarzdor", true) },
            "Majburiy ijro byurosiga\n\n{{fio}} dan\nManzil: {{manzil}}\nTel: {{telefon}}\n\nARIZA\n\n" +
            "{{qaror}} asosida {{qarzdor}} dan undirishni majburiy ijro etishingizni so'rayman.\n\n" +
            "Sana: ______   Imzo: ______" + Disclaimer));

        // — Prokuratura / nazorat
        list.Add(new(EDocumentType.Complaint, ECaseType.Civil,
            "Prokuraturaga ariza",
            "Huquq buzilganda yoki nazorat uchun prokuraturaga murojaat.",
            new[] { Fio, Manzil, Tel, new Field("mazmun", "Murojaat mazmuni", true),
                new Field("soral", "So'rovingiz", true) },
            "Prokuraturaga\n\n{{fio}} dan\nManzil: {{manzil}}\nTel: {{telefon}}\n\nARIZA\n\n" +
            "{{mazmun}}\n\nSo'rayman: {{soral}}.\n\nSana: ______   Imzo: ______" + Disclaimer));

        // — Jinoiy ish (jabrlanuvchi uchun — advokat tavsiya qilinadi)
        list.Add(new(EDocumentType.Explanatory, ECaseType.RequiresLawyer,
            "Ariza (jinoyat haqida xabar)",
            "Sodir bo'lgan jinoyat haqida ichki ishlar organiga ariza. DIQQAT: jinoiy ishda advokat zarur.",
            new[] { new Field("organ", "IIB/militsiya bo'limi", true), Fio, Manzil, Tel,
                new Field("voqea", "Nima sodir bo'ldi", true) },
            "{{organ}} ga\n\n{{fio}} dan\nManzil: {{manzil}}\nTel: {{telefon}}\n\nARIZA\n\n" +
            "Quyidagi jinoyat sodir bo'ldi: {{voqea}}\n\n" +
            "Tekshiruv o'tkazib, aybdorlarni javobgarlikka tortishingizni so'rayman.\n\n" +
            "DIQQAT: jinoiy ishlarda malakali advokat yordami zarur.\n\n" +
            "Sana: ______   Imzo: ______" + Disclaimer));

        // — Notarial / boshqa
        list.Add(new(EDocumentType.Explanatory, ECaseType.Civil,
            "Ishonchnoma (oddiy shakl)",
            "Boshqa shaxsga vakolat berish uchun ishonchnoma (notarial tasdiqlash tavsiya).",
            new[] { new Field("beruvchi", "Ishonchnoma beruvchi F.I.O.", true),
                new Field("oluvchi", "Ishonchli vakil F.I.O.", true),
                new Field("vakolat", "Qanday vakolat", true) },
            "ISHONCHNOMA\n\nMen, {{beruvchi}}, quyidagi shaxsga ishonaman: {{oluvchi}}.\n\n" +
            "Vakolat: {{vakolat}}\n\nSana: ______   Imzo: ______\n\n" +
            "(Notarial tasdiqlash tavsiya etiladi.)" + Disclaimer));

        list.Add(new(EDocumentType.Explanatory, ECaseType.Civil,
            "Tilxat (qarz berish)",
            "Qarz berilganini tasdiqlovchi tilxat.",
            new[] { new Field("qarz_oluvchi", "Qarz oluvchi F.I.O.", true),
                new Field("qarz_beruvchi", "Qarz beruvchi F.I.O.", true),
                new Field("summa", "Summa (so'm)", true),
                new Field("qaytarish", "Qaytarish sanasi", true) },
            "TILXAT\n\nMen, {{qarz_oluvchi}}, {{qarz_beruvchi}} dan {{summa}} so'm qarz oldim.\n" +
            "Qaytarish sanasi: {{qaytarish}}.\n\nSana: ______   Imzo: ______" + Disclaimer));

        list.Add(new(EDocumentType.Complaint, ECaseType.Civil,
            "Bank/moliya xizmati shikoyati",
            "Bank yoki moliya tashkiloti xizmati bo'yicha shikoyat.",
            new[] { new Field("bank", "Bank/tashkilot", true), Fio, Tel,
                new Field("holat", "Muammo", true), new Field("talab", "Talab", true) },
            "{{bank}} ga\n\n{{fio}} dan\nTel: {{telefon}}\n\nSHIKOYAT\n\n{{holat}}\n\n" +
            "So'rayman: {{talab}}.\n\nSana: ______   Imzo: ______" + Disclaimer));

        list.Add(new(EDocumentType.Complaint, ECaseType.Civil,
            "Hokimlik/davlat organiga murojaat",
            "Hokimlik yoki davlat organiga muammo bo'yicha murojaat.",
            new[] { new Field("organ", "Organ/hokimlik", true), Fio, Manzil, Tel,
                new Field("mazmun", "Muammo", true), new Field("soral", "So'rov", true) },
            "{{organ}} ga\n\n{{fio}} dan\nManzil: {{manzil}}\nTel: {{telefon}}\n\nMUROJAAT\n\n" +
            "{{mazmun}}\n\nSo'rayman: {{soral}}.\n\nSana: ______   Imzo: ______" + Disclaimer));

        list.Add(new(EDocumentType.Claim, ECaseType.Civil,
            "Da'vo arizasi (qo'shni nizosi)",
            "Yer, devor, suv yoki boshqa qo'shnichilik nizolari bo'yicha da'vo.",
            new[] { Sud, Fio, Manzil, new Field("qoshni", "Qo'shni (javobgar)", true),
                new Field("nizo", "Nizo mohiyati", true), new Field("talab", "Talab", true) },
            "{{sud}} ga\n\nDa'vogar: {{fio}}, {{manzil}}\nJavobgar: {{qoshni}}\n\n" +
            "DA'VO ARIZASI (qo'shnichilik nizosi)\n\n{{nizo}}\n\nSO'RAYMAN: {{talab}}.\n\n" +
            "Sana: ______   Imzo: ______" + Disclaimer));

        list.Add(new(EDocumentType.Claim, ECaseType.Rent,
            "Da'vo arizasi (kommunal to'lov nizosi)",
            "Kommunal xizmatlar (suv, gaz, svet) bo'yicha nizo da'vosi.",
            new[] { Sud, Fio, Manzil, new Field("tashkilot", "Kommunal tashkilot", true),
                new Field("holat", "Nizo", true), new Field("talab", "Talab", true) },
            "{{sud}} ga\n\nDa'vogar: {{fio}}, {{manzil}}\nJavobgar: {{tashkilot}}\n\n" +
            "DA'VO ARIZASI (kommunal nizo)\n\n{{holat}}\n\nSO'RAYMAN: {{talab}}.\n\n" +
            "Sana: ______   Imzo: ______" + Disclaimer));

        list.Add(new(EDocumentType.Pretension, ECaseType.Civil,
            "Pretenziya (umumiy shakl)",
            "Har qanday fuqarolik nizosi bo'yicha sudgacha rasmiy talabnoma.",
            new[] { new Field("taraf", "Qarama-qarshi taraf", true), Fio, Tel,
                new Field("mohiyat", "Nizo mohiyati", true), new Field("talab", "Talab", true) },
            "{{taraf}} ga\n\n{{fio}} dan\nTel: {{telefon}}\n\nPRETENZIYA\n\n{{mohiyat}}\n\n" +
            "TALAB QILAMAN: {{talab}}. 10 kun ichida javob bering, aks holda sudga murojaat qilaman.\n\n" +
            "Sana: ______   Imzo: ______" + Disclaimer));

        list.Add(new(EDocumentType.Explanatory, ECaseType.Administrative,
            "Ariza (haydovchilik guvohnomasi)",
            "Haydovchilik guvohnomasi bilan bog'liq ariza (qaytarish, almashtirish).",
            new[] { new Field("organ", "YPX/organ", true), Fio, Tel,
                new Field("masala", "Masala", true) },
            "{{organ}} ga\n\n{{fio}} dan\nTel: {{telefon}}\n\nARIZA\n\n{{masala}}\n\n" +
            "Sana: ______   Imzo: ______" + Disclaimer));

        list.Add(new(EDocumentType.Objection, ECaseType.Labor,
            "Intizomiy jazoga e'tiroz",
            "Ish beruvchi bergan intizomiy jazoga (hayfsan, tanbeh) e'tiroz.",
            new[] { new Field("tashkilot", "Tashkilot", true), Fio,
                new Field("jazo", "Qaysi jazo va buyruq", true), new Field("etiroz", "E'tiroz", true) },
            "{{tashkilot}} rahbariga\n\n{{fio}} dan\n\nE'TIROZ\n\n" +
            "{{jazo}} intizomiy jazosi bilan rozi emasman: {{etiroz}}\n\nSana: ______   Imzo: ______" + Disclaimer));

        list.Add(new(EDocumentType.Claim, ECaseType.Labor,
            "Da'vo arizasi (mehnat ta'tili kompensatsiyasi)",
            "Foydalanilmagan ta'til uchun pul kompensatsiyasi da'vosi.",
            new[] { Sud, Fio, Manzil, new Field("tashkilot", "Tashkilot", true),
                new Field("kunlar", "Foydalanilmagan kunlar", true) },
            "{{sud}} ga\n\nDa'vogar: {{fio}}, {{manzil}}\nJavobgar: {{tashkilot}}\n\n" +
            "DA'VO ARIZASI (ta'til kompensatsiyasi)\n\nFoydalanilmagan ta'til: {{kunlar}} kun.\n\n" +
            "SO'RAYMAN: kompensatsiya undirish.\n\nSana: ______   Imzo: ______" + Disclaimer));

        list.Add(new(EDocumentType.Explanatory, ECaseType.Consumer,
            "Onlayn xarid bo'yicha shikoyat",
            "Internet do'koni yoki marketplace orqali xarid bo'yicha shikoyat.",
            new[] { new Field("dokon", "Onlayn do'kon/platforma", true), Fio, Tel,
                new Field("buyurtma", "Buyurtma raqami", false), new Field("holat", "Muammo", true),
                new Field("talab", "Talab", true) },
            "{{dokon}} ga\n\n{{fio}} dan\nTel: {{telefon}}\nBuyurtma: {{buyurtma}}\n\nSHIKOYAT\n\n" +
            "{{holat}}\n\nTalab: {{talab}}.\n\nSana: ______   Imzo: ______" + Disclaimer));

        list.Add(new(EDocumentType.Claim, ECaseType.Civil,
            "Da'vo arizasi (sug'urta to'lovi)",
            "Sug'urta kompaniyasidan to'lovni undirish da'vosi.",
            new[] { Sud, Fio, Manzil, new Field("sugurta", "Sug'urta kompaniyasi", true),
                new Field("holat", "Sug'urta hodisasi", true), new Field("summa", "Talab summasi", true) },
            "{{sud}} ga\n\nDa'vogar: {{fio}}, {{manzil}}\nJavobgar: {{sugurta}}\n\n" +
            "DA'VO ARIZASI (sug'urta to'lovi)\n\n{{holat}}\n\nSO'RAYMAN: {{summa}} so'm undirish.\n\n" +
            "Sana: ______   Imzo: ______" + Disclaimer));

        list.Add(new(EDocumentType.Claim, ECaseType.Rent,
            "Da'vo arizasi (ijarani bo'shatish)",
            "Ijarachini uy-joydan chiqarish (bo'shatish) da'vosi.",
            new[] { Sud, Fio, Manzil, new Field("ijarachi", "Ijarachi (javobgar)", true),
                new Field("obyekt", "Uy-joy manzili", true), new Field("sabab", "Sabab", true) },
            "{{sud}} ga\n\nDa'vogar: {{fio}}, {{manzil}}\nJavobgar: {{ijarachi}}\n\n" +
            "DA'VO ARIZASI (ijarani bo'shatish)\n\nObyekt: {{obyekt}}\nSabab: {{sabab}}\n\n" +
            "SO'RAYMAN: ijarachini uy-joydan chiqarish.\n\nSana: ______   Imzo: ______" + Disclaimer));

        list.Add(new(EDocumentType.Explanatory, ECaseType.Civil,
            "Ariza (FHDYo - tug'ilganlik/o'lim guvohnomasi)",
            "Fuqarolik holati dalolatnomalari yozish organiga ariza.",
            new[] { Fio, Manzil, new Field("masala", "Qanday hujjat kerak", true) },
            "FHDYo bo'limiga\n\n{{fio}} dan\nManzil: {{manzil}}\n\nARIZA\n\n{{masala}}\n\n" +
            "Sana: ______   Imzo: ______" + Disclaimer));

        list.Add(new(EDocumentType.Complaint, ECaseType.Administrative,
            "Davlat xizmatchisi ustidan shikoyat",
            "Mansabdor shaxs harakati (yoki harakatsizligi) ustidan shikoyat.",
            new[] { new Field("organ", "Yuqori organ", true), Fio, Manzil, Tel,
                new Field("mansabdor", "Mansabdor shaxs/organ", true), new Field("holat", "Nima bo'ldi", true) },
            "{{organ}} ga\n\n{{fio}} dan\nManzil: {{manzil}}\nTel: {{telefon}}\n\nSHIKOYAT\n\n" +
            "{{mansabdor}} tomonidan huquqlarim buzildi: {{holat}}\n\n" +
            "Tekshiruv o'tkazib, choralar ko'rishingizni so'rayman.\n\nSana: ______   Imzo: ______" + Disclaimer));

        list.Add(new(EDocumentType.Claim, ECaseType.Civil,
            "Da'vo arizasi (yer nizosi)",
            "Yer uchastkasi chegarasi yoki huquqi bo'yicha da'vo.",
            new[] { Sud, Fio, Manzil, new Field("javobgar", "Javobgar", true),
                new Field("yer", "Yer uchastkasi (manzil/raqam)", true), new Field("nizo", "Nizo", true) },
            "{{sud}} ga\n\nDa'vogar: {{fio}}, {{manzil}}\nJavobgar: {{javobgar}}\n\n" +
            "DA'VO ARIZASI (yer nizosi)\n\nYer: {{yer}}\nNizo: {{nizo}}\n\n" +
            "SO'RAYMAN: yer uchastkasiga bo'lgan huquqimni tan olish.\n\nSana: ______   Imzo: ______" + Disclaimer));

        list.Add(new(EDocumentType.Explanatory, ECaseType.Consumer,
            "Tovarni qaytarish arizasi (14 kun)",
            "Sifatli tovarni 14 kun ichida qaytarish/almashtirish arizasi.",
            new[] { new Field("dokon", "Do'kon", true), Fio, Tel,
                new Field("tovar", "Tovar", true), new Field("sana", "Xarid sanasi", true) },
            "{{dokon}} ga\n\n{{fio}} dan\nTel: {{telefon}}\n\nARIZA\n\n" +
            "{{sana}} sanasida \"{{tovar}}\" sotib oldim. Qonun bo'yicha 14 kun ichida " +
            "sifatli tovarni almashtirish/qaytarish huquqimdan foydalanmoqchiman.\n\n" +
            "Sana: ______   Imzo: ______" + Disclaimer));

        return list;
    }
}
