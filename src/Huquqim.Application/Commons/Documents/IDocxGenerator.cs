namespace Huquqim.Application.Commons.Documents;

/// <summary>
/// Matnli hujjatdan .docx (Word) fayl baytlarini yasaydi.
/// </summary>
public interface IDocxGenerator
{
    /// <summary>Sarlavha va matndan .docx fayl baytlarini qaytaradi.</summary>
    byte[] Generate(string title, string body);
}
