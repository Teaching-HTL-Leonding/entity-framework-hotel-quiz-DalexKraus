using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotelExercise.Controller;
using HotelExercise.Models;
using HotelExercise.Util;

namespace HotelExercise
{
    class Program
    {
        private static HotelController _hotelController;
        
        public static async Task<Hotel> RequestHotel()
        {
            var hotel = new Hotel();
            Console.Write("Enter hotel title: ");
            hotel.Name = Console.ReadLine();
            
            // Parse the hotel address (wtf?)
            Console.Write("Enter hotel address: ");
            var rawConsoleAddress = Console.ReadLine();
            if (rawConsoleAddress == null) return null;
            
            var rawAddressSegments = rawConsoleAddress.Split(", ");
            var citySegments = rawAddressSegments[1].Split(" ");
            hotel.Address = new Address()
            {
                Street = rawAddressSegments[0],
                ZipCode = int.Parse(citySegments[0]),
                City = citySegments[1]
            };
                        
            // Parse hotel specials
            Console.Write("Enter hotel specials: ");
            hotel.Specialities = Console.ReadLine()?
                .Trim().Split(", ")
                .Select(s => new Speciality() { Name = s }).ToList();
                        
            // Parse room types
            var rooms = RequestRoomTypes(hotel);
            hotel.Rooms = rooms.Select(r => r.Item1).ToList();
            
            // Register room prices
            await _hotelController.AddPrices(rooms.Select(r => r.Item2).ToList());
            
            return hotel;
        }

        public static List<(RoomType, Price)> RequestRoomTypes(Hotel currentHotel)
        {
            var rooms = new List<(RoomType, Price)>();
            while (true)
            {
                Console.Write("Add room types? (yes|no): ");
                var rawRoomType = Console.ReadLine();
                if (rawRoomType == null || rawRoomType != "yes") break;

                Console.Write("Enter available room count for this hotel: ");
                var availableRoomsInput = Console.ReadLine();
                var availableRooms = string.IsNullOrEmpty(availableRoomsInput) ? 0 : int.Parse(availableRoomsInput);

                Console.Write("Enter room size in m^2: ");
                var roomSizeInput = Console.ReadLine();
                var roomSize = string.IsNullOrEmpty(roomSizeInput) ? 0 : int.Parse(roomSizeInput);

                Console.Write("Enter room title: ");
                var roomTitle = Console.ReadLine();

                Console.Write("Enter room description: ");
                var roomDescription = Console.ReadLine();

                Console.Write("Room accessible for disabled people (yes|no): ");
                var accessibleForDisabledPeople = Console.ReadLine() == "yes";

                var roomToAdd = new RoomType()
                {
                    Title = roomTitle,
                    Size = roomSize,
                    Description = roomDescription,
                    DisabilityAccessible = accessibleForDisabledPeople,
                    Hotel = currentHotel,
                    AvailableRooms = availableRooms
                };

                Console.Write("Enter price per night: ");
                var rawPricePerNight = Console.ReadLine();
                var pricePerNight = string.IsNullOrEmpty(rawPricePerNight) ? 0 : int.Parse(rawPricePerNight);
                var price = new Price()
                {
                    Room = roomToAdd,
                    PricePerNight = pricePerNight
                };
                rooms.Add((roomToAdd, price));
            }
            return rooms;
        }
        
        public static async Task Main(string[] args)
        {
            _hotelController = new HotelController(args);

            var shouldExit = false;
            do
            {
                Console.Write("Enter mode (1 = add hotel | 2 = display hotels | else = exit): ");
                var mode = Console.ReadLine();
                switch (mode?.Trim())
                {
                    case "1":
                        var hotelToAdd = await RequestHotel();
                        await _hotelController.AddHotel(hotelToAdd);
                        break;
                    case "2":
                        var hotels = await _hotelController.GetAllHotels();
                        foreach (var hotel in hotels)
                        {
                            Console.WriteLine($"\n{await MarkdownFormatter.FormatHotel(_hotelController, hotel)}\n\n");
                        }
                        break;
                    default:
                        shouldExit = true;
                        break;
                }
            } while (!shouldExit);
        }
    }
}
