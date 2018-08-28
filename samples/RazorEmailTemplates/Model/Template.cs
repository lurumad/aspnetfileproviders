using Microsoft.WindowsAzure.Storage.Table;
using System.Text.RegularExpressions;

namespace RazorEmailTemplates.Model
{
    public class Template : TableEntity
    {
        public Template(
            string name,
            string language)
        {
            PartitionKey = language.ToLowerInvariant();
            RowKey = Parse(name);
        }

        public Template()
        {

        }

        public string Url { get; set; }

        private string Parse(string text)
        {
            var slug = text.ToLowerInvariant();
            slug = Regex.Replace(slug, @"[^a-z0-9\s-]", "");
            slug = Regex.Replace(slug, @"[\s-]+", " ").Trim();
            slug = slug.Substring(0, slug.Length).Trim();
            slug = Regex.Replace(slug, @"\s", "");
            return slug;
        }
    }
}
