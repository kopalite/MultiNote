namespace MultiNote
{
    public class MultiNoteConfig
    {
        public SlackConfig SlackConfig { get; set; }
    }

    public class SlackConfig
    {
        public string MessagePrefix { get; set; }

        public string InfoWebhook { get; set; }

        public string AlertWebhook { get; set; }
    }
}