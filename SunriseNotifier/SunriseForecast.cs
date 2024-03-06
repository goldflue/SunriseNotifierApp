using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using SunriseNotifier.Services;
using System.Linq;
using System.Net.Http.Headers;

namespace SunriseNotifier
{

    public static class SunriseForecast
    {
		// See https://sunsethue.com/app for inspiration
		private static readonly HttpClient client = new HttpClient();
		private static readonly WeatherService weatherService = new WeatherService(client);
		[FunctionName("SunriseForecast")]
		public static async Task<IActionResult> Run(
		[HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
		ILogger log)
		{
			log.LogInformation("SunriseForecast triggered");


			var forecast = await weatherService.GetFiveDayForecastAsync();
			var nextSunrise = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(forecast.City.Sunrise).AddDays(1).ToLocalTime();

			var closestDataPoints = weatherService.FindClosestWeatherDataPoints(forecast.List, nextSunrise);

			

			// Prepare the request to the email API.
			var emailData = new
			{
				from = "onboarding@resend.dev",
				to = "goldflue@gmail.com",
				subject = "Test fra function app",
				html = "<p>Denne email er blevet sendt</p>"
			};

			var jsonContent = JsonConvert.SerializeObject(emailData);
			var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
			// Create HttpRequestMessage
			var request = new HttpRequestMessage(HttpMethod.Post, "https://api.resend.com/emails")
			{
				Content = content
			};

			var apiKey = Environment.GetEnvironmentVariable("ResendAPIKey", EnvironmentVariableTarget.Process);
			request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiKey); 
			
			// Send the request
			var response = await client.SendAsync(request);
			var responseContent = await response.Content.ReadAsStringAsync();

			if (response.IsSuccessStatusCode)
			{
				log.LogInformation($"Email sent successfully: {responseContent}");
				return new OkObjectResult("Email sent successfully.");
			}
			else
			{
				log.LogError($"Failed to send email: {responseContent}");
				return new BadRequestObjectResult($"Failed to send email: {responseContent}");
			}
		}
	}
}
