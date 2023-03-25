namespace PhoneInfo.API.Tests
{
	public static class DoSetup
	{
		public static void Setup()
		{
			_services ??= new ServiceCollection();
			_services.AddSingleton(Configuration);
			_services.AddHttpServiceLayer();
			_services.AddCatalogService();
			_services.AddDealService();
			_services.AddSearchService();
			_services.AddGlossaryService();
		}

		private static IServiceCollection? _services;

		private static IServiceProvider? _serviceProvider;
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
