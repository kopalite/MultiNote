using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiNote.Test.Mock
{
    public class SlackMessageLog
    {
        private List<string> _messages = new List<string>();
        public IEnumerable<string> Messages => _messages;

        public void Save(string message)
        {
            _messages.Add(message);
        }
    }
}
