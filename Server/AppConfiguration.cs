using System;
using System.IO;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;

namespace LabApp.Server
{
	public static class AppConfiguration
	{
		private const string DefaultAppSecret = "393EAC2E-1957-46B2-A9E9-91CAF0BB7DB7";
		private const string DefaultJwtIssuer = "http://localhost:5000";

		private const string PostgresConnection =
			"Server=127.0.0.1;Port=5435;Database=LabApp;User Id=postgres;Password=Qwerty123321;";

		private static readonly string DefaultStoragePath = Path.Combine(Directory.GetCurrentDirectory(), "storage");

		public static string AppSecret => Get(nameof(AppSecret)) ?? DefaultAppSecret;

		public static string JwtIssuer => Get(nameof(JwtIssuer)) ?? DefaultJwtIssuer;

		public static string ConnectionString { get; } =
			Environment.GetEnvironmentVariable(nameof(ConnectionString)) ?? PostgresConnection;

		public static string StorageUrl { get; } = "";

		public static string LocalStoragePath => Get(nameof(LocalStoragePath)) ?? DefaultStoragePath;


		private static string Get(string var) => Environment.GetEnvironmentVariable("LABAPP_" + var.ToUpper());
	}
}