using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Slack.Webhooks;
using System;
using System.Threading.Tasks;

namespace MultiNote
{
    public interface INotifier
    {
        Task InfoAsync(params string[] messages);

        Task AlertAsync(params string[] messages);
    }

    internal class Notifier : INotifier
    {
        private readonly IOptions<NotifierConfig> _config;
        private readonly ILogger<INotifier> _logger;

        public Notifier(IOptions<NotifierConfig> config, ILogger<INotifier> logger)
        {
            _config = config;
            _logger = logger;
        }

        public async Task InfoAsync(params string[] messages)
        {
            try
            {
                var slackClient = new SlackClient(_config.Value.InfoWebhook);

                foreach (var message in messages)
                {
                    var slackMessage = new SlackMessage
                    {
                        Text = $"{_config.Value.MessagePrefix}: {message}",
                    };
                    await slackClient.PostAsync(slackMessage);
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError($"Exception while sending Slack info message. {ex.Message}");
            }
        }

        public async Task AlertAsync(params string[] messages)
        {
            try
            {
                var slackClient = new SlackClient(_config.Value.AlertWebhook);

                foreach (var message in messages)
                {
                    var slackMessage = new SlackMessage
                    {
                        Text = $"{_config.Value.MessagePrefix}: {message}"
                    };
                    await slackClient.PostAsync(slackMessage);
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError($"Exception while sending Slack alert message. {ex.Message}");
            }
        }
    }
}
