using WebApiTraining.Models;

namespace WebApiTraining.Data
{
    public static class DataStore
    {
        public static List<Room> Rooms { get; set; } = new()
        {
            new Room { Id = 1, Name = "Lab 101", BuildingCode = "WAW", Floor = 1, Capacity = 30, HasProjector = true, IsActive = true },
            new Room { Id = 2, Name = "Lab 204", BuildingCode = "B", Floor = 2, Capacity = 24, HasProjector = true, IsActive = true },
            new Room { Id = 3, Name = "Sala Konferencyjna", BuildingCode = "WAW", Floor = 3, Capacity = 10, HasProjector = false, IsActive = true },
            new Room { Id = 4, Name = "Magazyn", BuildingCode = "C", Floor = -1, Capacity = 5, HasProjector = false, IsActive = false }
        };

        public static List<Reservation> Reservations { get; set; } = new()
        {
            new Reservation { Id = 1, RoomId = 2, OrganizerName = "Anna Kowalska", Topic = "Warsztaty z HTTP i REST", Date = new DateOnly(2026, 5, 10), StartTime = new TimeOnly(10, 0), EndTime = new TimeOnly(12, 30), Status = "confirmed" },
            new Reservation { Id = 2, RoomId = 1, OrganizerName = "Jan Nowak", Topic = "Szkolenie C#", Date = new DateOnly(2026, 5, 10), StartTime = new TimeOnly(9, 0), EndTime = new TimeOnly(15, 0), Status = "planned" }
        };
    }
}