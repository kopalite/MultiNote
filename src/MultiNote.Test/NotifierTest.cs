using Microsoft.Extensions.Logging;
using Moq;
using MultiNote.Channels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace MultiNote.Test
{
    public class NotifierTest
    {
        private readonly Notifier _notifier;
        private int _callsCounter;

        public NotifierTest()
        {
            
            var c1 = new Mock<INotifierChannel>();
            c1.Setup(x => x.InfoAsync(It.IsAny<string[]>())).Callback(() => _callsCounter++);
            c1.Setup(x => x.AlertAsync(It.IsAny<string[]>())).Callback(() => _callsCounter++);
            var c2 = new Mock<INotifierChannel>();
            c2.Setup(x => x.InfoAsync(It.IsAny<string[]>())).Callback(() => _callsCounter++);
            c2.Setup(x => x.AlertAsync(It.IsAny<string[]>())).Callback(() => _callsCounter++);
            var c3 = new Mock<INotifierChannel>();
            c3.Setup(x => x.InfoAsync(It.IsAny<string[]>())).Throws(new Exception("erroneous channel should not affect others!"));
            c3.Setup(x => x.AlertAsync(It.IsAny<string[]>())).Throws(new Exception("erroneous channel should not affect others!"));
            var channels = new List<INotifierChannel>(new[] { c1.Object, c3.Object, c2.Object  });

            var loggerMock = new Mock<ILogger<Notifier>>();

            _notifier = new Notifier(channels, loggerMock.Object);
        }

        [Fact]
        public async Task ShouldUseAllChannels()
        {
            //Act
            await _notifier.InfoAsync("info");
            await _notifier.AlertAsync("alert");

            //Assert
            Assert.Equal(4, _callsCounter);
        }
    }
}
