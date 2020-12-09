using System.Collections.Generic;

namespace HotelExercise.Models
{
    public class Hotel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Address Address { get; set; }
        public List<Speciality> Specialities { get; set; }
        public List<RoomType> Rooms { get; set; }
    }
}