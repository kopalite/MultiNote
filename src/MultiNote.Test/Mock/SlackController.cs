using Microsoft.AspNetCore.Mvc;
using MultiNote.Channels.Slack;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace MultiNote.Test.Mock
{
    public class SlackController : Controller
    {
        private readonly SlackMessageLog _messageLog;

        public SlackController(SlackMessageLog mesageLog)
        {
            _messageLog = mesageLog;
        }

        [HttpPost, Route("push")]
        public IActionResult PushAsync([FromBody] SlackMessage message)
        {
            _messageLog.Save(message?.Text);
            return Ok();
        }
    }
}
