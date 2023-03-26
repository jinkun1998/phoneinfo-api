namespace PhoneInfo.API.Tests
{
	public static class DoSetup
	{
		public static void Setup()
		{
			Services.AddSingleton(Configuration);
            Services.AddHttpServiceLayer();
            Services.AddCatalogService();
            Services.AddDealService();
            Services.AddSearchService();
            Services.AddGlossaryService();
		}

		private static IServiceCollection _services;
		private static IServiceCollection Services
		{
			get
			{
				if (_services == null)
				{
					_services = new ServiceCollection();
				}
				return _services;
            }
		}

		private static IServiceProvider _serviceProvider;
		private static IServiceProvider ServiceProvider
		{
			get
			{
				_serviceProvider ??= _services.BuildServiceProvider();
				return _serviceProvider;
			}
		}

		private static IConfiguration? _config;
		private static IConfiguration Configuration
		{
			get
			{
				if (_config == null)
				{
					var builder = new ConfigurationBuilder().AddJsonFile($"appsettings.Development.json", optional: false);
					_config = builder.Build();
				}

				return _config;
			}
		}

		public static T GetService<T>() => ServiceProvider.GetRequiredService<T>();
	}
}
