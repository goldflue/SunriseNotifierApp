using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunriseNotifier.Models
{
	public static  class SunrisePredictor
	{
		public static SunrisePrediction PredictSunriseQuality(WeatherDataPoint beforeSunrise, WeatherDataPoint afterSunrise, DateTime nextSunrise)
		{
			// Criteria for a good sunrise
			const int idealCloudCover = 40; // Ideal cloud cover in percentage
			const int maxCloudCover = 70; // Maximum cloud cover percentage for a potential good sunrise
			const double minVisibility = 10000; // Minimum visibility in meters (10 km is generally clear)
			const double maxPrecipitation = 0.1; // Maximum precipitation in mm

			// Check the cloud cover, precipitation, and visibility criteria
			bool isCloudCoverIdeal = beforeSunrise.Clouds.All <= idealCloudCover && afterSunrise.Clouds.All <= maxCloudCover;
			bool isVisibilityClear = beforeSunrise.Visibility >= minVisibility && afterSunrise.Visibility >= minVisibility;
			bool isPrecipitationLow = (beforeSunrise.Rain?.ThreeHours ?? 0) <= maxPrecipitation && (afterSunrise.Rain?.ThreeHours ?? 0) <= maxPrecipitation;

			// Determine if the sunrise should be recommended
			bool shouldRecommend = isCloudCoverIdeal && isVisibilityClear && isPrecipitationLow;

			if (!isCloudCoverIdeal)
			{
				shouldRecommend = true;
			}

			return new SunrisePrediction(shouldRecommend,
										 nextSunrise.ToString(),
										 beforeSunrise.Main.Temp.ToString(),
										 beforeSunrise.Wind.Speed.ToString());
		}

	}
}
