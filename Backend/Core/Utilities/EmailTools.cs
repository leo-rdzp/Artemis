using System.Net.Mail;
using System.Text.RegularExpressions;

namespace Artemis.Backend.Core.Utilities
{
    public class EmailTools
    {
        private readonly ILogger<EmailTools> _logger;
        private readonly IConfiguration _configuration;

        public EmailTools(
            ILogger<EmailTools> logger,
            IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<ResultNotifier> SendEmailAsync(MailMessage message, ItemList? parameters = null)
        {
            try
            {
                if (!ValidateEmailMessage(message, out var validationResult))
                {
                    return validationResult;
                }

                ProcessMessageParameters(message, parameters);

                using var smtpClient = CreateSmtpClient();
                await smtpClient.SendMailAsync(message);

                return ResultNotifier.Success(null, "Email sent successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending email");
                return ResultNotifier.Failure(ex.Message, ex.ToString());
            }
        }

        private bool ValidateEmailMessage(MailMessage message, out ResultNotifier validationResult)
        {
            if (!message.To.Any())
            {
                validationResult = ResultNotifier.Failure("Address To Is Required");
                return false;
            }

            if (message.From == null || string.IsNullOrEmpty(message.From.Address))
            {
                validationResult = ResultNotifier.Failure("Address From Is Required");
                return false;
            }

            if (string.IsNullOrEmpty(message.Subject))
            {
                validationResult = ResultNotifier.Failure("Email Subject Is Required");
                return false;
            }

            validationResult = null!;
            return true;
        }

        private void ProcessMessageParameters(MailMessage message, ItemList? parameters)
        {
            if (parameters == null) return;

            AddDateParameters(parameters);
            ProcessSubject(message, parameters);
            ProcessBody(message, parameters);
        }

        private static void AddDateParameters(ItemList parameters)
        {
            var now = DateTime.Now;
            parameters["day"] = now.ToString("dd");
            parameters["month"] = now.ToString("MM");
            parameters["dayMonth"] = now.ToString("m");
            parameters["year"] = now.ToString("yy");
            parameters["Year"] = now.ToString("yyyy");
            parameters["dayWeek"] = now.ToString("ddd");
            parameters["DayWeek"] = now.ToString("dddd");
            parameters["hour"] = now.ToString("hh");
            parameters["min"] = now.ToString("mm");
            parameters["sec"] = now.ToString("ss");
        }

        private static void ProcessSubject(MailMessage message, ItemList parameters)
        {
            message.Subject = ProcessVariables(message.Subject, parameters);
        }

        private static void ProcessBody(MailMessage message, ItemList parameters)
        {
            if (!string.IsNullOrEmpty(message.Body))
            {
                message.Body = ProcessVariables(message.Body, parameters);
            }
        }

        private static string ProcessVariables(string text, ItemList parameters)
        {
            var variables = Regex.Matches(text, @"\|(.*?)\|")
                .Cast<Match>()
                .Select(m => m.Value)
                .ToList();

            foreach (var variable in variables)
            {
                var key = variable.Trim('|');
                if (parameters.ContainsKey(key))
                {
                    text = text.Replace(variable, parameters.GetString(key));
                }
            }

            return text;
        }

        private SmtpClient CreateSmtpClient()
        {
            var smtpConfig = GetSmtpConfig();
            return new SmtpClient
            {
                Host = smtpConfig.Host,
                Port = smtpConfig.Port,
                EnableSsl = smtpConfig.EnableSsl,
                UseDefaultCredentials = smtpConfig.UseDefaultCredentials,
                Credentials = smtpConfig.UseDefaultCredentials ? null :
                    new System.Net.NetworkCredential(smtpConfig.Username, smtpConfig.Password)
            };
        }

        private SmtpConfig GetSmtpConfig()
        {
            var config = new SmtpConfig
            {
                Host = _configuration.GetValue<string>("Email:Server")
                    ?? throw new InvalidOperationException("SMTP server not configured"),
                Port = _configuration.GetValue<int>("Email:Port", 587),
                EnableSsl = _configuration.GetValue<bool>("Email:EnableSsl", true),
                UseDefaultCredentials = _configuration.GetValue<bool>("Email:UseDefaultCredentials", false),
                Username = _configuration.GetValue<string>("Email:Username"),
                Password = _configuration.GetValue<string>("Email:Password")
            };

            return config;
        }
    }

    public class SmtpConfig
    {
        public string Host { get; set; } = string.Empty;
        public int Port { get; set; }
        public bool EnableSsl { get; set; }
        public bool UseDefaultCredentials { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
    }

    public static class EmailExtensions
    {
        public static MailMessage CreateEmailMessage(
            string from,
            string to,
            string subject,
            string body,
            bool isBodyHtml = false)
        {
            var message = new MailMessage
            {
                From = new MailAddress(from),
                Subject = subject,
                Body = body,
                IsBodyHtml = isBodyHtml
            };

            foreach (var address in to.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries))
            {
                message.To.Add(address.Trim());
            }

            return message;
        }

        public static void AddCcRecipients(this MailMessage message, string? ccAddresses)
        {
            if (string.IsNullOrEmpty(ccAddresses)) return;

            foreach (var address in ccAddresses.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries))
            {
                message.CC.Add(address.Trim());
            }
        }

        public static void AddBccRecipients(this MailMessage message, string? bccAddresses)
        {
            if (string.IsNullOrEmpty(bccAddresses)) return;

            foreach (var address in bccAddresses.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries))
            {
                message.Bcc.Add(address.Trim());
            }
        }

        public static void AddAttachments(this MailMessage message, IEnumerable<string>? attachmentPaths)
        {
            if (attachmentPaths == null) return;

            foreach (var path in attachmentPaths)
            {
                if (File.Exists(path))
                {
                    message.Attachments.Add(new Attachment(path));
                }
            }
        }
    }
}