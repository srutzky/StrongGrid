﻿using Shouldly;
using System.Net;
using Xunit;

namespace StrongGrid.UnitTests
{
	public class ClientTests
	{
		private const string API_KEY = "my_api_key";

		[Fact]
		public void Version_is_not_empty()
		{
			// Arrange

			// Act
			var result = Client.Version;

			// Assert
			result.ShouldNotBeNullOrEmpty();
		}

		[Fact]
		public void Dispose()
		{
			// Arrange
			var client = new Client(API_KEY, (IWebProxy)null);

			// Act
			client.Dispose();

			// Assert
			// Nothing to assert. We just want to confirm that 'Dispose' did not throw any exception
		}
	}
}
