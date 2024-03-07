using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunriseNotifier.Models
{
	public class SunrisePrediction
	{
		public SunrisePrediction() { }

        public bool ShouldRecommend { get; set; }

        public string SunriseTime { get; set; }
        public string DegreesCelcius { get; set; }
        public string Wind{ get; set; }
    }
}
