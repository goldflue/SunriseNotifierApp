using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace SunriseNotifier.Models
{
	public class SunrisePrediction
	{

        public bool ShouldRecommend { get; set; }

        public string SunriseTime { get; set; }
        public string DegreesCelcius { get; set; }
        public string Wind{ get; set; }

		public SunrisePrediction() { }
		public SunrisePrediction(bool shouldRecommend, string sunriseTime, string degreesCelcius, string wind)
		{
			ShouldRecommend = shouldRecommend;
			SunriseTime = sunriseTime;	
			DegreesCelcius = degreesCelcius;
			Wind = wind;
		}
	}
}
