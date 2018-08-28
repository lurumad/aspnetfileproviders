using System;
using System.Globalization;
using System.Linq;

namespace RazorEmailTemplates.Features.Shared
{
    public static class LanguageValidator
    {
        private static CultureInfo[] specificCultures;

        static LanguageValidator()
        {
            specificCultures = CultureInfo.GetCultures(CultureTypes.SpecificCultures);
        }

        public static bool IsValid(string language)
        {
            return specificCultures.Any(c => c.Name.Equals(language, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}
