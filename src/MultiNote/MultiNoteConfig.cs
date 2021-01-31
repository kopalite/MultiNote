namespace MultiNote
{
    public class MultiNoteConfig
    {
        public SlackConfig SlackConfig { get; set; }
    }

    public class SlackConfig
    {
        public bool IsEnabled { get; set; }

        public string InfoPrefix { get; set; }

        public string InfoWebhook { get; set; }

        public string AlertPrefix { get; set; }

        public string AlertWebhook { get; set; }
    }
}