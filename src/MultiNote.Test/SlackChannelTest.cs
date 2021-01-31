using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using MultiNote.Channels.Slack;
using MultiNote.Test.Mock;
using System.Threading.Tasks;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Net.Http;

namespace MultiNote.Test
{
    public class SlackChannelTest
    {

        private readonly SlackChannel _slackChannel;
        private bool _isEnabled;
        private bool _isWebhookOK;
        private SlackMessageLog _mesageLog;

        public SlackChannelTest()
        {
            var configMock = new Mock<IOptions<MultiNoteConfig>>();
            configMock.Setup(x => x.Value).Returns(() => new MultiNoteConfig
            {
                SlackConfig = new SlackConfig
                {
                    InfoWebhook = _isWebhookOK ? "http://localhost/push" : "sdfsdf",
                    AlertWebhook = _isWebhookOK ? "http://localhost/push" : "sdfsdf",
                    InfoPrefix = ":info_emoji:",
                    AlertPrefix = ":alert_emoji:",
                    IsEnabled = _isEnabled
                }
            });

            var httpServer = new TestServer(new WebHostBuilder().UseStartup<SlackStartup>());
            var httpClient = httpServer.CreateClient();
            _mesageLog = httpServer.Services.GetService<SlackMessageLog>();

            var loggerMock = new Mock<ILogger<SlackChannel>>();

            _slackChannel = new SlackChannel(configMock.Object, httpClient, loggerMock.Object);
        }

        [Fact]
        public async Task ShouldNotSendAnythingOnWrongUrl()
        {
            //Arrange
            _isEnabled = true;
            _isWebhookOK = false;

            //Act
            await _slackChannel.InfoAsync("test_message1");
            await _slackChannel.AlertAsync("test_message1");

            //Assert
            Assert.True(_mesageLog.Messages.Count() == 0);
        }

        [Fact]
        public async Task InfoAsync_InfoShouldSendCorrectMessages()
        {
            //Arrange
            _isEnabled = true;
            _isWebhookOK = true;

            //Act
            await _slackChannel.InfoAsync("test_message1");
            await _slackChannel.InfoAsync("test_message2");

            //Assert
            Assert.True(_mesageLog.Messages.Count() == 2);
            Assert.True(_mesageLog.Messages.First() == ":info_emoji: : test_message1");
            Assert.True(_mesageLog.Messages.Last() == ":info_emoji: : test_message2");
        }

        [Fact]
        public async Task InfoAsync_AlertShouldSendCorrectMessages()
        {
            //Arrange
            _isEnabled = true;
            _isWebhookOK = true;

            //Act
            await _slackChannel.AlertAsync("test_message1");
            await _slackChannel.AlertAsync("test_message2");

            //Assert
            Assert.True(_mesageLog.Messages.Count() == 2);
            Assert.True(_mesageLog.Messages.First() == ":alert_emoji: : test_message1");
            Assert.True(_mesageLog.Messages.Last() == ":alert_emoji: : test_message2");
        }


        [Fact]
        public async Task InfoAsync_ShouldSendInfoIfEnabled()
        {
            //Arrange
            _isEnabled = true;
            _isWebhookOK = true;

            //Act
            await _slackChannel.InfoAsync("test_message");

            //Assert
            Assert.True(_mesageLog.Messages.Count() == 1);
        }

        [Fact]
        public async Task InfoAsync_ShouldSendAlertIfEnabled()
        {
            //Arrange
            _isEnabled = true;
            _isWebhookOK = true;

            //Act
            await _slackChannel.AlertAsync("test_message");

            //Assert
            Assert.True(_mesageLog.Messages.Count() == 1);
        }

        [Fact]
        public async Task InfoAsync_ShouldNotSendInfoIfNotEnabled()
        {
            //Arrange
            _isEnabled = false;
            _isWebhookOK = true;

            //Act
            await _slackChannel.InfoAsync("test_message");

            //Assert
            Assert.True(_mesageLog.Messages.Count() == 0);
        }

        [Fact]
        public async Task InfoAsync_ShouldNotSendAlertIfNotEnabled()
        {
            //Arrange
            _isEnabled = false;
            _isWebhookOK = true;

            //Act
            await _slackChannel.AlertAsync("test_message");

            //Assert
            Assert.True(_mesageLog.Messages.Count() == 0);
        }
    }
}
