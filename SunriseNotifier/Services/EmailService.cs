using Newtonsoft.Json;
using SunriseNotifier.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace SunriseNotifier.Services;

public class EmailService
{
	private readonly string _apiKey = Environment.GetEnvironmentVariable("ResendAPIKey", EnvironmentVariableTarget.Process);
	private readonly HttpClient _httpClient;
	public EmailService(HttpClient client)
	{
		_httpClient = client;
	}

	
	public async Task SendVerdictEmail(SunrisePrediction prediction, string receiverEmail) {
		// Prepare the request to the email API.

		string htmlTemplate = @"
				<p>Hej 👋,</p>
				<p>Her er din solopgangsprognose for i morgen:</p>
				<ul>
					<li>Solen står op: <strong>{0}</strong> 🌅</li>
					<li>Temperatur: <strong>{1} °C</strong> 🌡️</li>
					<li>Vind: <strong>{2} m/s</strong> 🌬️</li>
				</ul>
				<p>Hav en fantastisk dag!</p>
				<p>Knus, møs og kram,<br>Den meget meget pålidelige solopgangsprognose-mand 🧙‍♂️</p>";
		var emailData = new
		{
			from = "onboarding@resend.dev", // Change this to configured resend domain email
			to = $"{receiverEmail}",
			subject = "🌄 Der er sku solopgang i morgen 🌅",
			html = string.Format(htmlTemplate, prediction.SunriseTime, prediction.DegreesCelcius, prediction.Wind)
		};



		var jsonContent = JsonConvert.SerializeObject(emailData);
		var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
		// Create HttpRequestMessage
		var request = new HttpRequestMessage(HttpMethod.Post, "https://api.resend.com/emails")
		{
			Content = content
		};

		request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);

		// Send the request
		var response = await _httpClient.SendAsync(request);
		var responseContent = await response.Content.ReadAsStringAsync();

		if (response.IsSuccessStatusCode)
		{
			//log.LogInformation($"Email sent successfully: {responseContent}");
		}
		else
		{
			//log.LogError($"Failed to send email: {responseContent}");
		}
	}

	
}
