using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SunriseNotifier.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
namespace SunriseNotifier.Services;

public class WeatherService
{
	private readonly string _apiKey = Environment.GetEnvironmentVariable("OpenWeatherMap_APIKey", EnvironmentVariableTarget.Process);
	private readonly HttpClient _httpClient;
	public WeatherService(HttpClient client)
	{
		_httpClient = client;
	}

	public async Task<WeatherForecast> GetFiveDayForecastAsync()
	{
		string url = $"http://api.openweathermap.org/data/2.5/forecast?lat=56.1518&lon=10.2064&APPID={_apiKey}&units=metric"; // Potentially refactor to BaseUrl in HTTPClient implementation 
		try
		{
			// Make the HTTP request
			var response = await _httpClient.GetAsync(url);
			response.EnsureSuccessStatusCode();

			// Parse the JSON response
			string jsonContent = await response.Content.ReadAsStringAsync();
			var weatherForecast = JsonConvert.DeserializeObject<WeatherForecast>(jsonContent);

			return weatherForecast;
		}
		catch (HttpRequestException e)
		{
			// Handle any exceptions (e.g., network error, bad request, etc.)
			Console.WriteLine("\nException Caught!");
			Console.WriteLine("Message :{0} ", e.Message);
		}

		return null;
	}


	public List<WeatherDataPoint> FindClosestWeatherDataPoints(List<WeatherDataPoint> dataPoints, DateTime targetDateTime)
	{
		// Ensure there's at least one data point before and after the target time
		var beforeTarget = dataPoints.Where(dp => dp.DtTxtDateTime < targetDateTime)
									 .OrderByDescending(dp => dp.DtTxtDateTime)
									 .Take(1)
									 .ToList(); // This ensures the data points are in descending order, you may reverse if needed

		var afterTarget = dataPoints.Where(dp => dp.DtTxtDateTime >= targetDateTime)
									.OrderBy(dp => dp.DtTxtDateTime)
									.Take(1)
									.ToList();

		// Reverse the 'before' list to get chronological order if desired
		beforeTarget.Reverse();

		// Combine the lists to get the final ordered sequence
		var closestDataPoints = beforeTarget.Concat(afterTarget).ToList();

		return closestDataPoints;
	}
}
