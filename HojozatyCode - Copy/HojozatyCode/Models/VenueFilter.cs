using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HojozatyCode.Models
{
	//Model to deal with filter inside the Category 
    public class VenueFilter
    {
		public double MinPrice { get; set; }
		public double MaxPrice { get; set; }
		public int MinCapacity { get; set; }
		public int MaxCapacity { get; set; }
		public CitieisEnum FilterCity { get; set; }

	}
}
