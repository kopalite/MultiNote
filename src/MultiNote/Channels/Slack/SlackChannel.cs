using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MultiNote.Channels.Slack
{
    public class SlackChannel : INotifierChannel
    {
        private readonly IOptions<MultiNoteConfig> _config;
        private readonly HttpClient _httpClient;
        private readonly ILogger<SlackChannel> _logger;

        public SlackChannel(IOptions<MultiNoteConfig> config, 
                            HttpClient httpClient, 
                            ILogger<SlackChannel> logger)
        {
            _config = config;
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task InfoAsync(params string[] messages)
        {
            var config = _config?.Value?.SlackConfig;
            if (!(config?.IsEnabled).GetValueOrDefault()) return;
            
            var webhookUrl = config?.InfoWebhook;
            var prefix = config?.InfoPrefix;
            await PostAsync(webhookUrl, prefix, messages);
        }

        public async Task AlertAsync(params string[] messages)
        {
            var config = _config?.Value?.SlackConfig;
            if (!(config?.IsEnabled).GetValueOrDefault()) return;

            var webhookUrl = config?.AlertWebhook;
            var prefix = config?.AlertPrefix;
            await PostAsync(webhookUrl, prefix, messages);
        }

        private async Task PostAsync(string webhookUrl, string prefix, params string[] messages)
        {
            if (!Uri.TryCreate(webhookUrl, UriKind.Absolute, out Uri webhookUri))
            {
                _logger?.LogError("Webhook URL is not configured!");
                return;
            }

            try
            {
                foreach (var message in messages)
                {
                    var slackMessage = new SlackMessage { Text = $"{prefix} : {message}" };
                    var messageContent = new StringContent(slackMessage.AsJson(), Encoding.UTF8, "application/json");
                    using var request = new HttpRequestMessage(HttpMethod.Post, webhookUri) { Content = messageContent };
                    var response = await _httpClient.SendAsync(request).ConfigureAwait(true);
                    if (!response.IsSuccessStatusCode)
                    {
                        _logger?.LogError($"Error happened while sending message to the webhook {webhookUrl}! status code: {response.StatusCode}");
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError($"Exception happened while sending message to the webhook {webhookUrl}! {ex.Message} {ex.StackTrace}");
            }
        }
    }
}
