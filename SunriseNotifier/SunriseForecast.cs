using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using SunriseNotifier.Services;
using System.Linq;
using System.Net.Http.Headers;
using SunriseNotifier.Models;
using System.Net.Mail;

namespace SunriseNotifier;


    public static class SunriseForecast
    {
	// See https://sunsethue.com/app for inspiration

	private static readonly SmtpClient smtpClient = new SmtpClient();
	private static readonly EmailService emailService = new EmailService(smtpClient);

	private static readonly HttpClient client = new HttpClient();
	private static readonly WeatherService weatherService = new WeatherService(client);

	[FunctionName("SunriseForecast")]
	public static async Task Run([TimerTrigger("0 30 17 * * 1-5")] TimerInfo myTimer, ILogger log) 
	{
		log.LogInformation("SunriseForecast triggered");

		try
		{
			var forecast = await weatherService.GetFiveDayForecastAsync();
			var nextSunrise = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(forecast.City.Sunrise).AddDays(1).ToLocalTime();

			var closestDataPoints = weatherService.FindClosestWeatherDataPoints(forecast.List, nextSunrise);
			var prediction = SunrisePredictor.PredictSunriseQuality(closestDataPoints[0], closestDataPoints[1], nextSunrise);

			if (prediction.ShouldRecommend == true)
			{
				var emails = Environment.GetEnvironmentVariable("ReceiverEmails", EnvironmentVariableTarget.Process)?.Split(';');

				foreach (var email in emails)
				{
					await emailService.SendVerdictEmail(prediction, email);
				}
			}
		}
		catch (Exception ex)
		{
			log.LogError(ex.Message);
			throw;
		}
		
		
	}
}
