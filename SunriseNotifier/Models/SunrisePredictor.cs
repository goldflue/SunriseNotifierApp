using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunriseNotifier.Models
{
	public static  class SunrisePredictor
	{
		public static string PredictSunriseQuality(WeatherDataPoint beforeSunrise, WeatherDataPoint afterSunrise)
		{
			// Example criteria for a good sunrise
			const int idealCloudCover = 40; // Ideal cloud cover in percentage
			const int maxCloudCover = 70; // Maximum cloud cover percentage for a potential good sunrise
			const double minVisibility = 10000; // Minimum visibility in meters (10 km is generally clear)
			const double maxPrecipitation = 0.1; // Maximum precipitation in mm

			// Check the cloud cover, precipitation, and visibility criteria
			bool isCloudCoverIdeal = beforeSunrise.Clouds.All <= idealCloudCover && afterSunrise.Clouds.All <= maxCloudCover;
			bool isVisibilityClear = beforeSunrise.Visibility >= minVisibility && afterSunrise.Visibility >= minVisibility;
			bool isPrecipitationLow = (beforeSunrise.Rain?.ThreeHours ?? 0) <= maxPrecipitation && (afterSunrise.Rain?.ThreeHours ?? 0) <= maxPrecipitation;

			if (isCloudCoverIdeal && isVisibilityClear && isPrecipitationLow)
			{
				return "High potential for a good sunrise";
			}
			else if (!isCloudCoverIdeal)
			{
				return "Cloud cover is not ideal for a good sunrise";
			}
			else if (!isVisibilityClear)
			{
				return "Visibility is low, which may hinder a good sunrise";
			}
			else if (!isPrecipitationLow)
			{
				return "Precipitation may obscure the sunrise";
			}
			else
			{
				return "Conditions are not ideal for a good sunrise";
			}
		}
	}
}
