using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialControl.Domain.Helper
{
	public static class ConfigurationManager
	{
		private static Key appSettings;
		public static Key AppSettings
		{
			get
			{
				if (appSettings == null)
					appSettings = new Key();

				return appSettings;
			}
		}

		public static bool Exists(string key)
		{
			return AppSettings[key] != null;
		}

		public class Key
		{
			private IConfigurationRoot _configuration;
			public IConfigurationRoot Configuration
			{
				get
				{
					if (_configuration == null)
					{
						var builder = new ConfigurationBuilder()
							.SetBasePath(Directory.GetCurrentDirectory())
							.AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true)
							.AddJsonFile("appsettings.json", optional: true)
							.AddEnvironmentVariables();
						_configuration = builder.Build();
					}

					return _configuration;
				}
			}

			public string this[string key]
			{
				get
				{
					try
					{
						return Configuration[key];
					}
					catch (Exception e) { }
					return string.Empty;
				}
			}

			public void AddRange(IDictionary<string, string> items)
			{
				var builder = new ConfigurationBuilder()
					.SetBasePath(Directory.GetCurrentDirectory())
					.AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true)
					.AddJsonFile("appsettings.json", optional: true)
					.AddEnvironmentVariables()
					.AddInMemoryCollection(items);
				_configuration = builder.Build();
			}
		}
	}
}
