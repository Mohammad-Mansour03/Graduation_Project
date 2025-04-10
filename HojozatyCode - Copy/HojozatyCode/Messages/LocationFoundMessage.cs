using Microsoft.Maui.Maps;

namespace HojozatyCode.Messages
{
    public class LocationFoundMessage
    {
        public Location Location { get; }

        public LocationFoundMessage(Location location)
        {
            Location = location;
        }
    }
}