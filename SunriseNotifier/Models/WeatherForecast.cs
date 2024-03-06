using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunriseNotifier.Models;
public class WeatherForecast
{
	public string Cod { get; set; }
	public int Message { get; set; }
	public string DailyWeatherReport { get; set; }
	public int Cnt { get; set; }
	public List<WeatherDataPoint> List { get; set; }
	public City City { get; set; }
}

public class WeatherDataPoint
{
	public long Dt { get; set; }
	public Main Main { get; set; }
	public List<Weather> Weather { get; set; }
	public Clouds Clouds { get; set; }
	public Wind Wind { get; set; }
	public int Visibility { get; set; }
	public double Pop { get; set; }
	public Sys Sys { get; set; }

	private string _dtTxt;
	public string Dt_Txt
	{
		get => _dtTxt;
		set
		{
			_dtTxt = value;
			if (DateTime.TryParseExact(value, "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out DateTime parsedDate))
			{
				DtTxtDateTime = parsedDate;
			}
		}
	}

	// Add a new property to store the DateTime representation of DtTxt
	public DateTime DtTxtDateTime { get; private set; }
	public Rain Rain { get; set; }
	public Snow Snow { get; set; }
}

public class Main
{
	public double Temp { get; set; }
	public double FeelsLike { get; set; }
	public double TempMin { get; set; }
	public double TempMax { get; set; }
	public int Pressure { get; set; }
	public int SeaLevel { get; set; }
	public int GrndLevel { get; set; }
	public int Humidity { get; set; }
	public double TempKf { get; set; }
}

public class Weather
{
	public int Id { get; set; }
	public string Main { get; set; }
	public string Description { get; set; }
	public string Icon { get; set; }
}

public class Clouds
{
	public int All { get; set; }
}

public class Wind
{
	public double Speed { get; set; }
	public int Deg { get; set; }
	public double Gust { get; set; }
}

public class Sys
{
	public string Pod { get; set; }
}

public class Rain
{
	[JsonProperty("3h")]
	public double ThreeHours { get; set; }
}

public class Snow
{
	[JsonProperty("3h")]
	public double ThreeHours { get; set; }
}

public class City
{
	public int Id { get; set; }
	public string Name { get; set; }
	public Coord Coord { get; set; }
	public string Country { get; set; }
	public int Population { get; set; }
	public int Timezone { get; set; }
	public long Sunrise { get; set; }
	public long Sunset { get; set; }
}

public class Coord
{
	public double Lat { get; set; }
	public double Lon { get; set; }
}
