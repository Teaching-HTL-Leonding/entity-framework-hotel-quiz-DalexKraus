using System.ComponentModel.DataAnnotations;

namespace HotelExercise.Models
{
    public class Address
    {
        public int Id { get; set; }
        [MaxLength(64)]
        public string Street { get; set; }
        public int ZipCode { get; set; }
        [MaxLength(64)]
        public string City { get; set; }
    }
}