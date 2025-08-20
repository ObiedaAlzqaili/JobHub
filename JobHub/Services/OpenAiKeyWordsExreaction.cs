using JobHub.Interfaces.AiInterfaces;
using Microsoft.SemanticKernel;

namespace JobHub.Services
{
    public class OpenAiKeywordExtraction : IOpenAiKeywordExtraction
    {
        private readonly Kernel _kernel;

        public OpenAiKeywordExtraction(Kernel kernel)
        {
            _kernel = kernel;
        }

        public async Task<string> ExtractKeywordsAsync(string inputText)
        {
            string promptTemplate = @"
You are an expert job analyzer. Extract the most important keywords and skills from the following job post.
Focus on these categories:

1. Technical Skills (programming languages, frameworks, tools, technologies)
2. Professional Skills (management, communication, leadership, soft skills)
3. Job Titles and Roles
4. Industry-specific terms
5. Experience levels (Junior, Mid-level, Senior)
6. Education requirements
7. Certifications

Format requirements:
- Return only a comma-separated list
- No additional explanations or text
- Minimum 5 keywords, maximum 15
- Prioritize specific technical skills over generic terms
- Include seniority level if mentioned

JOB POST TEXT:
{{$input}}

KEYWORDS:";

            var prompt = _kernel.CreateFunctionFromPrompt(promptTemplate);

            var result = await prompt.InvokeAsync(_kernel, new() { ["input"] = inputText });

            return result?.ToString()?.Trim() ?? string.Empty;
        }

        public async Task<string> ExtractKeywordsFromResumeAsync(string resumeText)
        {
            return await ExtractKeywordsAsync(resumeText);
        }

        public async Task<string> ExtractKeywordsFromJobPostAsync(string jobPostText)
        {
            return await ExtractKeywordsAsync(jobPostText);
        }
    }
}