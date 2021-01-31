﻿using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MultiNote.Channels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MultiNote
{
    public interface INotifier 
    {
        Task InfoAsync(params string[] messages);

        Task AlertAsync(params string[] messages);
    }

    public class Notifier : INotifier
    {
        private readonly IEnumerable<INotifierChannel> _channels;
        private readonly ILogger<INotifier> _logger;

        public Notifier(IEnumerable<INotifierChannel> channels, ILogger<INotifier> logger)
        {
            _channels = channels;
            _logger = logger;
        }

        

        public async Task InfoAsync(params string[] messages)
        {
            try
            {
                foreach (var channel in _channels)
                {
                    await channel.InfoAsync(messages);
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError($"Exception while sending info message. {ex.Message}");
            }
        }

        public async Task AlertAsync(params string[] messages)
        {
            try
            {
                foreach (var channel in _channels)
                {
                    await channel.AlertAsync(messages);
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError($"Exception while sending alert message. {ex.Message}");
            }
        }

        
    }
}
