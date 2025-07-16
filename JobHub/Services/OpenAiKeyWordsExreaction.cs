using Microsoft.SemanticKernel;

namespace JobHub.Services
{
    public class OpenAiKeywordExtraction
    {
        private readonly Kernel _kernel;

        public OpenAiKeywordExtraction(Kernel kernel)
        {
            _kernel = kernel;
        }

        public async Task<string> ExtractKeywordsAsync(string inputText)
        {
            string promptTemplate = @"
You are an intelligent assistant that extracts key skills or keywords.

Extract 5 to 10 important and relevant keywords or skills from the following input. 
Return only a comma-separated list of keywords.

Input:
{{input}}

Keywords:";

            var prompt = _kernel.CreateFunctionFromPrompt(promptTemplate);

            var result = await _kernel.InvokeAsync(prompt,new ()
            {
                ["input"] = inputText
            });

            return result?.ToString() ?? string.Empty;
        }
    }
}
