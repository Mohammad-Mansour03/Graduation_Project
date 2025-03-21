using Supabase.Postgrest.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HojozatyCode.Models
{
	[Table("HostRules_Venues")]  // Ensure this matches the actual Supabase table name
	public class HostRulesVenues : Supabase.Postgrest.Models.BaseModel
	{
		// Foreign key to Venues table
		[Column("venue_id")]
		public Guid VenueId { get; set; }

		// Foreign key to HostRules table
		[Column("host_rule_id")]
		public Guid HostRuleId { get; set; }

	}
}
