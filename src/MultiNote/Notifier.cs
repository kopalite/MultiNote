using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Slack.Webhooks;
using System;
using System.Threading.Tasks;

namespace MultiNote
{
    public interface INotifier
    {
        void Info(params string[] messages);
        void Alert(params string[] messages);
        Task InfoAsync(params string[] messages);
        Task AlertAsync(params string[] messages);
    }

    internal class Notifier : INotifier
    {
        private readonly IOptions<MultiNoteConfig> _config;
        private readonly ILogger<INotifier> _logger;

        public Notifier(IOptions<MultiNoteConfig> config, ILogger<INotifier> logger)
        {
            _config = config;
            _logger = logger;
        }

        public void Info(params string[] messages)
        {
            try
            {
                var webhookUrl = _config?.Value?.SlackConfig?.InfoWebhook;
                var prefix = _config?.Value?.SlackConfig?.MessagePrefix;
                Send(webhookUrl, prefix, messages);
            }
            catch (Exception ex)
            {
                _logger?.LogError($"Exception while sending Slack info message. {ex.Message}");
            }
        }

        public void Alert(params string[] messages)
        {
            try
            {
                var webhookUrl = _config?.Value?.SlackConfig?.AlertWebhook;
                var prefix = _config?.Value?.SlackConfig?.MessagePrefix;
                Send(webhookUrl, prefix, messages);
            }
            catch (Exception ex)
            {
                _logger?.LogError($"Exception while sending Slack alert message. {ex.Message}");
            }
        }

        public async Task InfoAsync(params string[] messages)
        {
            try
            {
                var webhookUrl = _config?.Value?.SlackConfig?.InfoWebhook;
                var prefix = _config?.Value?.SlackConfig?.MessagePrefix;
                await SendAsync(webhookUrl, prefix, messages);
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
                var webhookUrl = _config?.Value?.SlackConfig?.AlertWebhook;
                var prefix = _config?.Value?.SlackConfig?.MessagePrefix;
                await SendAsync(webhookUrl, prefix, messages);
            }
            catch (Exception ex)
            {
                _logger?.LogError($"Exception while sending Slack alert message. {ex.Message}");
            }
        }

        private void Send(string webhookUrl, string prefix, params string[] messages)
        {
            if (string.IsNullOrWhiteSpace(webhookUrl))
            {
                _logger?.LogError($"Alert webhook ({nameof(MultiNoteConfig.SlackConfig.AlertWebhook)}) is not configured!");
                return;
            }

            var slackClient = new SlackClient(webhookUrl);

            foreach (var message in messages)
            {
                var slackMessage = new SlackMessage
                {
                    Text = $"{prefix} : {message}"
                };
                slackClient.Post(slackMessage);
            }
        }

        private async Task SendAsync(string webhookUrl, string prefix, params string[] messages)
        {
            if (string.IsNullOrWhiteSpace(webhookUrl))
            {
                _logger?.LogError($"Alert webhook ({nameof(MultiNoteConfig.SlackConfig.AlertWebhook)}) is not configured!");
                return;
            }

            var slackClient = new SlackClient(webhookUrl);

            foreach (var message in messages)
            {
                var slackMessage = new SlackMessage
                {
                    Text = $"{prefix} : {message}"
                };
                await slackClient.PostAsync(slackMessage);
            }
        }
    }
}
