namespace Vezeeta_Clone.Service.ExternalServices.Abstract
{
    public interface IEmailService
    {
        Task<bool> SendEmail(string email, string Message, string? reason);
        public async Task<string> LoadEmailTemplateAsync(string templateName, Dictionary<string, string> variables)
        {
            var templateFolder = Path.Combine(AppContext.BaseDirectory, "Templates");
            var path = Path.Combine(templateFolder, templateName);

            if (!File.Exists(path))
                throw new FileNotFoundException($"Email template not found: {path}");

            var content = await File.ReadAllTextAsync(path);

            foreach (var kv in variables)
                content = content.Replace($"{{{kv.Key}}}", kv.Value);

            return content;
        }
    }
}
