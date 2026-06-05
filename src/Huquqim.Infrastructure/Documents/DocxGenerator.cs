using System.IO.Compression;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using Huquqim.Application.Commons.Documents;

namespace Huquqim.Infrastructure.Documents;

/// <summary>
/// Tashqi kutubxonasiz .docx (Open XML / WordprocessingML) generator.
/// Rasmiy hujjat ko'rinishi: Times New Roman, qalin sarlavhalar, kursiv izohlar, markazlash.
///
/// Matndagi formatlash belgilari:
///   **matn**  → qalin
///   *matn*    → kursiv
///   [c]matn   → markazlashtirilgan paragraf (qator boshida)
/// </summary>
public partial class DocxGenerator : IDocxGenerator
{
    public byte[] Generate(string title, string body)
    {
        using var ms = new MemoryStream();
        using (var zip = new ZipArchive(ms, ZipArchiveMode.Create, leaveOpen: true))
        {
            WriteEntry(zip, "[Content_Types].xml", ContentTypesXml);
            WriteEntry(zip, "_rels/.rels", RelsXml);
            WriteEntry(zip, "word/document.xml", BuildDocumentXml(title, body));
        }

        return ms.ToArray();
    }

    private static void WriteEntry(ZipArchive zip, string path, string content)
    {
        var entry = zip.CreateEntry(path, CompressionLevel.Optimal);
        using var writer = new StreamWriter(entry.Open(), new UTF8Encoding(false));
        writer.Write(content);
    }

    private static string BuildDocumentXml(string title, string body)
    {
        var sb = new StringBuilder();
        sb.Append("""<?xml version="1.0" encoding="UTF-8" standalone="yes"?>""");
        sb.Append("""<w:document xmlns:w="http://schemas.openxmlformats.org/wordprocessingml/2006/main"><w:body>""");

        // Sarlavha — markazda, qalin
        if (!string.IsNullOrWhiteSpace(title))
            sb.Append(Paragraph(title, center: true, bold: true, sizeHalfPt: 26));

        // Matn — har qator alohida paragraf, formatlash belgilari bilan
        var lines = (body ?? string.Empty).Replace("\r\n", "\n").Split('\n');
        foreach (var raw in lines)
        {
            var line = raw;
            var center = false;

            // [c] — markazlashtirilgan qator
            if (line.StartsWith("[c]"))
            {
                center = true;
                line = line[3..];
            }

            sb.Append(ParagraphWithInlines(line, center));
        }

        // Sahifa sozlamasi: A4, ixcham margin (bir varaqqa sig'ishi uchun)
        sb.Append(SectionProperties);

        sb.Append("</w:body></w:document>");
        return sb.ToString();
    }

    // Ixcham paragraf oraliq (bir varaqqa sig'ishi uchun): oldin/keyin 0, qator oralig'i 1.0
    private const string CompactSpacing = """<w:spacing w:before="0" w:after="20" w:line="240" w:lineRule="auto"/>""";

    // Asosiy matn shrifti — 11pt (half-point = 22)
    private const int BodySize = 22;

    /// <summary>Oddiy bitta uslubli paragraf.</summary>
    private static string Paragraph(string text, bool center = false, bool bold = false, int sizeHalfPt = BodySize)
    {
        var sb = new StringBuilder();
        sb.Append("<w:p><w:pPr>");
        sb.Append(CompactSpacing);
        if (center) sb.Append("""<w:jc w:val="center"/>""");
        sb.Append("</w:pPr>");
        sb.Append(Run(text, bold, italic: false, sizeHalfPt));
        sb.Append("</w:p>");
        return sb.ToString();
    }

    /// <summary>**qalin** va *kursiv* belgilarini tushunadigan paragraf.</summary>
    private static string ParagraphWithInlines(string line, bool center)
    {
        var sb = new StringBuilder();
        sb.Append("<w:p><w:pPr>");
        sb.Append(CompactSpacing);
        if (center) sb.Append("""<w:jc w:val="center"/>""");
        sb.Append("</w:pPr>");

        if (line.Length > 0)
        {
            foreach (var (text, bold, italic) in ParseInline(line))
                sb.Append(Run(text, bold, italic, BodySize));
        }

        sb.Append("</w:p>");
        return sb.ToString();
    }

    /// <summary>Bitta matn bo'lagi (run) — shrift Times New Roman.</summary>
    private static string Run(string text, bool bold, bool italic, int sizeHalfPt)
    {
        var sb = new StringBuilder();
        sb.Append("<w:r><w:rPr>");
        sb.Append("""<w:rFonts w:ascii="Times New Roman" w:hAnsi="Times New Roman"/>""");
        if (bold) sb.Append("<w:b/>");
        if (italic) sb.Append("<w:i/>");
        sb.Append($"<w:sz w:val=\"{sizeHalfPt}\"/>");
        sb.Append("</w:rPr>");
        sb.Append($"<w:t xml:space=\"preserve\">{Escape(text)}</w:t>");
        sb.Append("</w:r>");
        return sb.ToString();
    }

    /// <summary>Qatorni **qalin** va *kursiv* bo'laklarga ajratadi.</summary>
    private static IEnumerable<(string Text, bool Bold, bool Italic)> ParseInline(string line)
    {
        // **bold** va *italic* ni tokenlarga ajratamiz
        var matches = InlineRegex().Matches(line);
        var pos = 0;

        foreach (Match m in matches)
        {
            if (m.Index > pos)
                yield return (line[pos..m.Index], false, false);

            if (m.Value.StartsWith("**"))
                yield return (m.Groups[1].Value, true, false);
            else
                yield return (m.Groups[2].Value, false, true);

            pos = m.Index + m.Length;
        }

        if (pos < line.Length)
            yield return (line[pos..], false, false);
    }

    private static string Escape(string text) => SecurityElement.Escape(text) ?? string.Empty;

    // Sahifa: A4 (11906 x 16838 twip), ixcham margin (~1.5 sm = 850 twip).
    // Bir varaqqa ko'proq sig'ishi uchun marginlar kichik.
    private const string SectionProperties = """
        <w:sectPr><w:pgSz w:w="11906" w:h="16838"/><w:pgMar w:top="850" w:right="850" w:bottom="850" w:left="1134" w:header="720" w:footer="720" w:gutter="0"/></w:sectPr>
        """;

    [GeneratedRegex(@"\*\*([^*]+)\*\*|\*([^*]+)\*")]
    private static partial Regex InlineRegex();

    private const string ContentTypesXml = """
        <?xml version="1.0" encoding="UTF-8" standalone="yes"?>
        <Types xmlns="http://schemas.openxmlformats.org/package/2006/content-types"><Default Extension="rels" ContentType="application/vnd.openxmlformats-package.relationships+xml"/><Default Extension="xml" ContentType="application/xml"/><Override PartName="/word/document.xml" ContentType="application/vnd.openxmlformats-officedocument.wordprocessingml.document.main+xml"/></Types>
        """;

    private const string RelsXml = """
        <?xml version="1.0" encoding="UTF-8" standalone="yes"?>
        <Relationships xmlns="http://schemas.openxmlformats.org/package/2006/relationships"><Relationship Id="rId1" Type="http://schemas.openxmlformats.org/officeDocument/2006/relationships/officeDocument" Target="word/document.xml"/></Relationships>
        """;
}
