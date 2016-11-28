﻿using Newtonsoft.Json.Linq;
using StrongGrid.Model;
using StrongGrid.Utilities;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StrongGrid.Resources
{
	/// <summary>
	/// 
	/// </summary>
	/// <remarks>
	/// See https://sendgrid.com/docs/API_Reference/Web_API_v3/user.html
	/// </remarks>
	public class User
	{
		private readonly string _endpoint;
		private readonly IClient _client;

		/// <summary>
		/// Initializes a new instance of the <see cref="User" /> class.
		/// </summary>
		/// <param name="client">SendGrid Web API v3 client</param>
		/// <param name="endpoint">Resource endpoint</param>
		public User(IClient client, string endpoint = "/user/profile")
		{
			_endpoint = endpoint;
			_client = client;
		}

		/// <summary>
		/// Get your user profile
		/// </summary>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns></returns>
		public async Task<UserProfile> GetProfileAsync(CancellationToken cancellationToken = default(CancellationToken))
		{
			var response = await _client.GetAsync(_endpoint, cancellationToken).ConfigureAwait(false);
			response.EnsureSuccess();

			var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
			var profile = JObject.Parse(responseContent).ToObject<UserProfile>();
			return profile;
		}

		/// <summary>
		/// Update your user profile
		/// </summary>
		/// <param name="address">The address.</param>
		/// <param name="city">The city.</param>
		/// <param name="company">The company.</param>
		/// <param name="country">The country.</param>
		/// <param name="firstName">The first name.</param>
		/// <param name="lastName">The last name.</param>
		/// <param name="phone">The phone.</param>
		/// <param name="state">The state.</param>
		/// <param name="website">The website.</param>
		/// <param name="zip">The zip.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns></returns>
		public async Task<UserProfile> UpdateProfileAsync(string address = null, string city = null, string company = null, string country = null, string firstName = null, string lastName = null, string phone = null, string state = null, string website = null, string zip = null, CancellationToken cancellationToken = default(CancellationToken))
		{
			var data = CreateJObjectForUserProfile(address, city, company, country, firstName, lastName, phone, state, website, zip);
			var response = await _client.PatchAsync(_endpoint, data, cancellationToken).ConfigureAwait(false);
			response.EnsureSuccess();

			var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
			var profile = JObject.Parse(responseContent).ToObject<UserProfile>();
			return profile;
		}

		/// <summary>
		/// Get your user account
		/// </summary>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns></returns>
		public async Task<Account> GetAccountAsync(CancellationToken cancellationToken = default(CancellationToken))
		{
			var response = await _client.GetAsync("/user/account", cancellationToken).ConfigureAwait(false);
			response.EnsureSuccess();

			var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
			var account = JObject.Parse(responseContent).ToObject<Account>();
			return account;
		}

		/// <summary>
		/// Retrieve the email address on file for your account
		/// </summary>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns></returns>
		public async Task<string> GetEmailAsync(CancellationToken cancellationToken = default(CancellationToken))
		{
			var response = await _client.GetAsync("/user/email", cancellationToken).ConfigureAwait(false);
			response.EnsureSuccess();

			var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

			// Response looks like this:
			// {
			//  "email": "test@example.com"
			// }
			// We use a dynamic object to get rid of the 'email' property and simply return a string
			dynamic dynamicObject = JObject.Parse(responseContent);
			return dynamicObject.email;
		}

		/// <summary>
		/// Update the email address on file for your account
		/// </summary>
		/// <param name="email">The email.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns></returns>
		public async Task<string> UpdateEmailAsync(string email, CancellationToken cancellationToken = default(CancellationToken))
		{
			var data = new JObject();
			data.Add("email", email);

			var response = await _client.PutAsync("/user/email", data, cancellationToken).ConfigureAwait(false);
			response.EnsureSuccess();

			var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

			// Response looks like this:
			// {
			//  "email": "test@example.com"
			// }
			// We use a dynamic object to get rid of the 'email' property and simply return a string
			dynamic dynamicObject = JObject.Parse(responseContent);
			return dynamicObject.email;
		}

		/// <summary>
		/// Retrieve your account username
		/// </summary>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns></returns>
		public async Task<string> GetUsernameAsync(CancellationToken cancellationToken = default(CancellationToken))
		{
			var response = await _client.GetAsync("/user/username", cancellationToken).ConfigureAwait(false);
			response.EnsureSuccess();

			var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

			// Response looks like this:
			// {
			//  "username": "test_username",
			//  "user_id": 1
			// }
			// We use a dynamic object to get rid of the 'user_id' property and simply return the string content of the 'username' property
			dynamic dynamicObject = JObject.Parse(responseContent);
			return dynamicObject.username;
		}

		/// <summary>
		/// Update your account username
		/// </summary>
		/// <param name="username">The username.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns></returns>
		public async Task<string> UpdateUsernameAsync(string username, CancellationToken cancellationToken = default(CancellationToken))
		{
			var data = new JObject();
			data.Add("username", username);

			var response = await _client.PutAsync("/user/username", data, cancellationToken).ConfigureAwait(false);
			response.EnsureSuccess();

			var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

			// Response looks like this:
			// {
			//  "username": "test_username"
			// }
			// We use a dynamic object to get rid of the 'username' property and simply return a string
			dynamic dynamicObject = JObject.Parse(responseContent);
			return dynamicObject.username;
		}

		/// <summary>
		/// Retrieve the current credit balance for your account
		/// </summary>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns></returns>
		public async Task<UserCredits> GetCreditsAsync(CancellationToken cancellationToken = default(CancellationToken))
		{
			var response = await _client.GetAsync("/user/credits", cancellationToken).ConfigureAwait(false);
			response.EnsureSuccess();

			var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
			var userCredits = JObject.Parse(responseContent).ToObject<UserCredits>();
			return userCredits;
		}

		/// <summary>
		/// Update the password for your account
		/// </summary>
		/// <param name="oldPassword">The old password.</param>
		/// <param name="newPassword">The new password.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns></returns>
		public async Task UpdatePasswordAsync(string oldPassword, string newPassword, CancellationToken cancellationToken = default(CancellationToken))
		{
			var data = new JObject();
			data.Add("new_password", oldPassword);
			data.Add("old_password", newPassword);

			var response = await _client.PutAsync("/user/password", data, cancellationToken).ConfigureAwait(false);
			response.EnsureSuccess();
		}

		/// <summary>
		/// List all available scopes for a user
		/// </summary>
		/// <param name="cancellationToken">Cancellation token</param>
		/// <returns></returns>
		public async Task<string[]> GetPermissionsAsync(CancellationToken cancellationToken = default(CancellationToken))
		{
			var response = await _client.GetAsync("/scopes", cancellationToken).ConfigureAwait(false);
			response.EnsureSuccess();

			// WARNING:
			// The response contains the following header: Content-Type: application/json; charset=utf8
			// The specified charset is not valid. The correct syntax is: charset=utf-8
			// The fact that the charset is slightly misspelled prevents the .Net HttpClient from
			// being able to parse the body of the reponse. The HttpClient throws the following exception
			// when we try to get the content of the response like so: response.Content.ReadAsStreamAsync()
			// 		The character set provided in ContentType is invalid. Cannot read content as string using
			// 		an invalid character set. System.ArgumentException: 'utf8' is not a supported encoding name

			// I contacted SendGrid on 11/23/2016 to report this problem: https://support.sendgrid.com/hc/en-us/requests/806220

			// A support engineer from SendGrid confirmed the issue on 11/24/2016 and said: 
			// 		I will put in a new feature request to our engineers to see if they will be able to have
			// 		the charset removed from that API call

			// Until SendGrid solves the problem on their end by either omiting the charset or fixing the misspelling,
			// we must read the content into a stream and convert the stream to a string which allows us to specify
			// the desired charset (which is Encoding.UTF8 in this case).
			var responseStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
			var responseContent = string.Empty;
			using (var sr = new StreamReader(responseStream, Encoding.UTF8))
			{
				responseContent = await sr.ReadToEndAsync().ConfigureAwait(false);
			}

			// Response looks like this:
			// {
			//  "scopes": [
			//    "mail.send",
			//    "alerts.create",
			//    "alerts.read"
			//  ]
			// }
			// We use a dynamic object to get rid of the 'scopes' property and return an array os strings
			dynamic dynamicObject = JObject.Parse(responseContent);
			dynamic dynamicArray = dynamicObject.scopes;

			var permissions = dynamicArray.ToObject<string[]>();
			return permissions;
		}

		private static JObject CreateJObjectForUserProfile(string address = null, string city = null, string company = null, string country = null, string firstName = null, string lastName = null, string phone = null, string state = null, string website = null, string zip = null)
		{
			var result = new JObject();
			if (!string.IsNullOrEmpty(address)) result.Add("address", address);
			if (!string.IsNullOrEmpty(city)) result.Add("city", city);
			if (!string.IsNullOrEmpty(company)) result.Add("company", company);
			if (!string.IsNullOrEmpty(country)) result.Add("country", country);
			if (!string.IsNullOrEmpty(firstName)) result.Add("first_name", firstName);
			if (!string.IsNullOrEmpty(lastName)) result.Add("last_name", lastName);
			if (!string.IsNullOrEmpty(phone)) result.Add("phone", phone);
			if (!string.IsNullOrEmpty(state)) result.Add("state", state);
			if (!string.IsNullOrEmpty(website)) result.Add("website", website);
			if (!string.IsNullOrEmpty(zip)) result.Add("zip", zip);
			return result;
		}
	}
}
