using System.Text;
using System.Threading.Tasks;
using HotelExercise.Controller;
using HotelExercise.Models;

namespace HotelExercise.Util
{
    public class MarkdownFormatter
    {
        public static async Task<string> FormatHotel(HotelController controller, Hotel hotel)
        {
            StringBuilder builder = new StringBuilder();

            // Basic stuff
            builder.AppendLine($"# {hotel.Name}\n");
            var addr = hotel.Address;
            builder.AppendLine($"## Location\n\n{addr.Street}\n{addr.ZipCode} {addr.City}\n");
            
            // Specials
            builder.AppendLine("## Specials\n");
            foreach (var speciality in hotel.Specialities) builder.AppendLine($"* {speciality.Name}");
            
            // Room types
            builder.AppendLine("|   Room Type   | Size | Price Valid From | Price Valid To | Price in € |");
            foreach (var room in hotel.Rooms)
            {
                var price = await controller.GetPrice(room);
                builder.AppendLine($"|{room.Title}|{room.Size:D4}m²|{price.FromDate}|{price.UntilDate}|{price.PricePerNight:D9} € |");
            }
            builder.AppendLine("\n");
            return builder.ToString();
        }
    }
}