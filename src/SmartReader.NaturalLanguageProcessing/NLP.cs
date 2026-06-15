using LanguageTeller;

namespace SmartReader.NaturalLanguageProcessing
{
    public static class NLP
    {
        private static readonly FastText LanguageTeller = new();
        
        /// <summary>
        /// Enable the automatic identification of the language.
        /// </summary>
        public static void Enable()
        {
            Article.LanguageIdentification = IdentifyLanguageUsingNlp;
        }

        /// <summary>
        /// Restore default behavior, which consist in returning the metadata.
        /// </summary>
        public static void RestoreDefaults()
        {
            Article.LanguageIdentification = (text, metadata) => metadata;
        }

        private static string IdentifyLanguageUsingNlp(string text, string language) => LanguageTeller.TellLanguage(text).Language;
    }
}
