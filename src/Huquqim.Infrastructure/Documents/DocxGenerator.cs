using System.IO.Compression;
using System.Security;
using System.Text;
using Huquqim.Application.Commons.Documents;

namespace Huquqim.Infrastructure.Documents;

/// <summary>
/// Tashqi kutubxonasiz .docx (Open XML / WordprocessingML) generator.
/// Docx — bu bir nechta XML faylli ZIP arxiv. Minimal valid tuzilma yasaymiz.
/// </summary>
public class DocxGenerator : IDocxGenerator
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

        // Sarlavha (qalin, kattaroq)
        if (!string.IsNullOrWhiteSpace(title))
        {
            sb.Append("""<w:p><w:pPr><w:jc w:val="center"/></w:pPr>""");
            sb.Append("""<w:r><w:rPr><w:b/><w:sz w:val="28"/></w:rPr>""");
            sb.Append($"<w:t xml:space=\"preserve\">{Escape(title)}</w:t>");
            sb.Append("</w:r></w:p>");
        }

        // Matn — qatorlarga bo'lib har birini paragraf qilamiz
        var lines = (body ?? string.Empty).Replace("\r\n", "\n").Split('\n');
        foreach (var line in lines)
        {
            sb.Append("<w:p>");
            if (line.Length > 0)
            {
                sb.Append("<w:r><w:rPr><w:sz w:val=\"24\"/></w:rPr>");
                sb.Append($"<w:t xml:space=\"preserve\">{Escape(line)}</w:t>");
                sb.Append("</w:r>");
            }
            sb.Append("</w:p>");
        }

        sb.Append("</w:body></w:document>");
        return sb.ToString();
    }

    private static string Escape(string text) => SecurityElement.Escape(text) ?? string.Empty;

    private const string ContentTypesXml = """
        <?xml version="1.0" encoding="UTF-8" standalone="yes"?>
        <Types xmlns="http://schemas.openxmlformats.org/package/2006/content-types"><Default Extension="rels" ContentType="application/vnd.openxmlformats-package.relationships+xml"/><Default Extension="xml" ContentType="application/xml"/><Override PartName="/word/document.xml" ContentType="application/vnd.openxmlformats-officedocument.wordprocessingml.document.main+xml"/></Types>
        """;

    private const string RelsXml = """
        <?xml version="1.0" encoding="UTF-8" standalone="yes"?>
        <Relationships xmlns="http://schemas.openxmlformats.org/package/2006/relationships"><Relationship Id="rId1" Type="http://schemas.openxmlformats.org/officeDocument/2006/relationships/officeDocument" Target="word/document.xml"/></Relationships>
        """;
}
