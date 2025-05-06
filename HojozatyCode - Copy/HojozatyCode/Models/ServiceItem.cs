using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HojozatyCode.Models
{
	public class ServiceItem
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public double Price { get; set; }		
		public Guid ServiceId { get; set; }
	}
}
