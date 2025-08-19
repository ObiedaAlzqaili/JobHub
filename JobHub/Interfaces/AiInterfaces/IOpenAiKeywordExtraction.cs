namespace JobHub.Interfaces.AiInterfaces
{
    public interface IOpenAiKeywordExtraction
    {
        public Task<string> ExtractKeywordsAsync(string inputText);

        public Task<string> ExtractKeywordsFromResumeAsync(string resumeText);

        public Task<string> ExtractKeywordsFromJobPostAsync(string jobPostText);
    }
}
