using Supabase.Postgrest.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HojozatyCode.Models
{
	[Table("HostRules")]  // Ensure this matches the actual Supabase table name
	public class HostRules : Supabase.Postgrest.Models.BaseModel
	{
		//store the venue id
		[PrimaryKey("host_rule_id", false)]
		public Guid HostRuleId { get; set; } = Guid.NewGuid(); // Default to gen_random_uuid()

		//Store the venue name
		[Required(ErrorMessage = "Venue Name is required")]
		[Column("host_rule_name")]
		public string HostRuleName { get; set; }

		//Store the description for the venue 
		[Column("host_rule_description")]
		public string HostRuleDescription { get; set; }

	}
}
