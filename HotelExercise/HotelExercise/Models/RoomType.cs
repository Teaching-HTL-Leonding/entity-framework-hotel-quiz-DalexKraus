using System.ComponentModel.DataAnnotations;

namespace HotelExercise.Models
{
    public class RoomType
    {
        public int Id { get; set; }
        public Hotel Hotel { get; set; }
        [MaxLength(50)]
        public string Title { get; set; }
        [MaxLength(250)]
        public string Description { get; set; }
        public int Size { get; set; }
        public bool DisabilityAccessible { get; set; } = true;
        public int AvailableRooms { get; set; }
    }
}