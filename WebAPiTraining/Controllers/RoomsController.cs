using Microsoft.AspNetCore.Mvc;
using WebApiTraining.Data;
using WebApiTraining.Models;

namespace WebApiTraining.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomsController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<Room>> GetRooms([FromQuery] int? minCapacity, [FromQuery] bool? hasProjector, [FromQuery] bool? activeOnly)
        {
            var query = DataStore.Rooms.AsQueryable();

            if (minCapacity.HasValue) 
                query = query.Where(r => r.Capacity >= minCapacity.Value);
            
            if (hasProjector.HasValue) 
                query = query.Where(r => r.HasProjector == hasProjector.Value);
            
            if (activeOnly.HasValue && activeOnly.Value) 
                query = query.Where(r => r.IsActive);

            return Ok(query.ToList());
        }

        [HttpGet("{id}")]
        public ActionResult<Room> GetRoom(int id)
        {
            var room = DataStore.Rooms.FirstOrDefault(r => r.Id == id);
            if (room == null) return NotFound();
            
            return Ok(room);
        }

        [HttpGet("building/{buildingCode}")]
        public ActionResult<IEnumerable<Room>> GetRoomsByBuilding(string buildingCode)
        {
            string normalizedCode = buildingCode.ToUpper();
            
            switch (normalizedCode)
            {
                case "W":
                case "WAR":
                case "WARSAW":
                    normalizedCode = "WAW";
                    break;
            }

            var rooms = DataStore.Rooms
                .Where(r => r.BuildingCode.Equals(normalizedCode, StringComparison.OrdinalIgnoreCase))
                .ToList();
                
            return Ok(rooms);
        }

        [HttpPost]
        public ActionResult<Room> CreateRoom([FromBody] Room room)
        {
            room.Id = DataStore.Rooms.Any() ? DataStore.Rooms.Max(r => r.Id) + 1 : 1;
            DataStore.Rooms.Add(room);
            
            return CreatedAtAction(nameof(GetRoom), new { id = room.Id }, room);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateRoom(int id, [FromBody] Room updatedRoom)
        {
            var room = DataStore.Rooms.FirstOrDefault(r => r.Id == id);
            if (room == null) return NotFound();

            room.Name = updatedRoom.Name;
            room.BuildingCode = updatedRoom.BuildingCode;
            room.Floor = updatedRoom.Floor;
            room.Capacity = updatedRoom.Capacity;
            room.HasProjector = updatedRoom.HasProjector;
            room.IsActive = updatedRoom.IsActive;

            return Ok(room);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteRoom(int id)
        {
            var room = DataStore.Rooms.FirstOrDefault(r => r.Id == id);
            if (room == null) return NotFound();

            bool hasReservations = DataStore.Reservations.Any(r => r.RoomId == id);
            if (hasReservations) 
                return Conflict("Nie można usunąć sali, która posiada powiązane rezerwacje.");

            DataStore.Rooms.Remove(room);
            return NoContent();
        }
    }
}