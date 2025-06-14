﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HojozatyCode.Models
{

	//Model To dealing with the Specific Booking for the Venue (Booking and the Venue Properties)
	public class BookingWithVenue
	{
		public Guid BookingId { get; set; }
		public Guid VenueId { get; set; }
		public DateTime StartDateTime { get; set; }
		public DateTime EndDateTime { get; set; }
		public double TotalPrice { get; set; }
		public DateTime CreatedAt { get; set; }

		// Venue Info
		public string VenueName { get; set; }
		public int Capacity { get; set; }
		public string Location { get; set; }
		public string VenueEmail { get; set; }
		public List<string> ImageUrls { get; set; }

		public string CancellationPolicy {  get; set; }
	}
}
