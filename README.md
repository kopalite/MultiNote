# MultiNote

MultiNote is notification library utilizing multiple messaging channels like email, Slack webhooks etc.

Usage:

1. Add configuration as a top level JSON property (MultiNoteConfig is mandatory name):

```

{
   "MultiNoteConfig": {
		"SlackConfig": {
		   "IsEnabled": true,
		   "InfoPrefix": ":some_emoji:",
		   "InfoWebhook": "https://yourslackwebhook1.com",
		   "AlertPrefix": ":some_emoji:",
		   "AlertWebhook": "https://yourslackwebhook2.com",
		}
   }
}

```

2. Call extension method to build dependency injection for INotifier component:

```
		public void ConfigureServices(IServiceCollection services)
        {
            services.AddNotifier(Configuration);
        }
```

3. Inject INotifier into your component and enjoy!

```
		public class SomeService
		{
			private readonly INotifier _notifier;
			
			public SomeService(INotifier notifier)
			{
			    _notifier = notifier;
			}
			
			public async Task DoAsync()
			{
			    _notifier.InfoAsync("some_message");
				_notifier.AlertAsync("some_message");
			}
		}
```