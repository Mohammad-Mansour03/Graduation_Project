using HojozatyCode.Models;
using Supabase;
using System.Threading.Tasks;
using static Supabase.Postgrest.Constants;

namespace HojozatyCode.Services
{
    public static class SupabaseConfig
    {
        public static Client SupabaseClient { get; private set; }

        public static async Task InitializeAsync()
        {
            var url = "https://vkfjltultitueghevwhs.supabase.co";
            var key = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6InZrZmpsdHVsdGl0dWVnaGV2d2hzIiwicm9sZSI6ImFub24iLCJpYXQiOjE3Mzg3NzA4NjEsImV4cCI6MjA1NDM0Njg2MX0._sya0tZM3C-zxxVXs_44riDJiEvW833TM9UUsNapkQw";
            var options = new SupabaseOptions
            {
                AutoRefreshToken = true,
                AutoConnectRealtime = true
            };

            SupabaseClient = new Client(url, key, options);
            await SupabaseClient.InitializeAsync();
        }

	
	}
}