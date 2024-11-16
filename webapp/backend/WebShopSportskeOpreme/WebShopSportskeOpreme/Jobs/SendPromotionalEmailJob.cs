using Quartz;
using WebShopSportskeOpreme.Interfaces;
using WebShopSportskeOpreme.Services;

namespace WebShopSportskeOpreme.Jobs
{
    public class SendPromotionalEmailJob : IJob
    {
        private readonly IEmailService _emailService;
        private readonly IUserService _userService;
        private readonly ILogger<SendPromotionalEmailJob> _logger;

        public SendPromotionalEmailJob(IEmailService emailService, IUserService userService, ILogger<SendPromotionalEmailJob> logger)
        {
            _emailService = emailService;
            _userService = userService;
            _logger = logger;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var users = _userService.GetAllUsers();
            string emailContent = GetPromotionalEmailContent();

            foreach (var user in users)
            {
                try
                {
                    await _emailService.SendEmail(user.Email, "Quality Products at Our Webshop!", emailContent);
                    _logger.LogInformation($"Promotional email sent to {user.Email}");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Failed to send promotional email to {user.Email}");
                }
            }
        }

        private string GetPromotionalEmailContent()
        {
            return @"
            <html>
            <body>
            <h2>Poštovani,</h2>
            <p>Željeli smo vas podsjetiti na visokokvalitetne proizvode dostupne u našem web shopu.</p>
            <p>Naša posvećenost izvrsnosti znači:</p>
            <ul>
                <li>Pažljivo odabrani materijali</li>
                <li>Stroga kontrola kvaliteta</li>
                <li>Proizvodi dizajnirani da traju</li>
            </ul>
            <p>Posjetite naš web shop danas i istražite našu ponudu vrhunskih artikala!</p>
            <p>Srdačan pozdrav,<br>Vaš Webshop tim</p>
            </body>
            </html>";

        }
    }
}