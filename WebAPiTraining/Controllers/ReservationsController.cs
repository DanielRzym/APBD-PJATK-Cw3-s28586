using Microsoft.AspNetCore.Mvc;
using WebApiTraining.Data;
using WebApiTraining.Models;

namespace WebApiTraining.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservationsController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<Reservation>> GetReservations([FromQuery] DateOnly? date, [FromQuery] string status, [FromQuery] int? roomId)
        {
            var query = DataStore.Reservations.AsQueryable();

            if (date.HasValue) 
                query = query.Where(r => r.Date == date.Value);
            
            if (!string.IsNullOrEmpty(status)) 
                query = query.Where(r => r.Status.Equals(status, StringComparison.OrdinalIgnoreCase));
            
            if (roomId.HasValue) 
                query = query.Where(r => r.RoomId == roomId.Value);

            return Ok(query.ToList());
        }

        [HttpGet("{id}")]
        public ActionResult<Reservation> GetReservation(int id)
        {
            var reservation = DataStore.Reservations.FirstOrDefault(r => r.Id == id);
            if (reservation == null) return NotFound();
            
            return Ok(reservation);
        }

        [HttpPost]
        public ActionResult<Reservation> CreateReservation([FromBody] Reservation reservation)
        {
            var room = DataStore.Rooms.FirstOrDefault(r => r.Id == reservation.RoomId);
            if (room == null) return BadRequest("Wskazana sala nie istnieje.");
            if (!room.IsActive) return BadRequest("Nie można zarezerwować nieaktywnej sali.");

            bool isConflict = DataStore.Reservations.Any(r => 
                r.RoomId == reservation.RoomId && 
                r.Date == reservation.Date &&
                r.StartTime < reservation.EndTime && 
                r.EndTime > reservation.StartTime);

            if (isConflict) 
                return Conflict("Rezerwacja koliduje czasowo z inną rezerwacją dla tej samej sali.");

            reservation.Id = DataStore.Reservations.Any() ? DataStore.Reservations.Max(r => r.Id) + 1 : 1;
            DataStore.Reservations.Add(reservation);

            return CreatedAtAction(nameof(GetReservation), new { id = reservation.Id }, reservation);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateReservation(int id, [FromBody] Reservation updatedReservation)
        {
            var reservation = DataStore.Reservations.FirstOrDefault(r => r.Id == id);
            if (reservation == null) return NotFound();

            var room = DataStore.Rooms.FirstOrDefault(r => r.Id == updatedReservation.RoomId);
            if (room == null) return BadRequest("Wskazana sala nie istnieje.");
            if (!room.IsActive) return BadRequest("Nie można zarezerwować nieaktywnej sali.");

            bool isConflict = DataStore.Reservations.Any(r => 
                r.Id != id && 
                r.RoomId == updatedReservation.RoomId && 
                r.Date == updatedReservation.Date &&
                r.StartTime < updatedReservation.EndTime && 
                r.EndTime > updatedReservation.StartTime);

            if (isConflict) 
                return Conflict("Zaktualizowane godziny kolidują z inną rezerwacją.");

            reservation.RoomId = updatedReservation.RoomId;
            reservation.OrganizerName = updatedReservation.OrganizerName;
            reservation.Topic = updatedReservation.Topic;
            reservation.Date = updatedReservation.Date;
            reservation.StartTime = updatedReservation.StartTime;
            reservation.EndTime = updatedReservation.EndTime;
            reservation.Status = updatedReservation.Status;

            return Ok(reservation);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteReservation(int id)
        {
            var reservation = DataStore.Reservations.FirstOrDefault(r => r.Id == id);
            if (reservation == null) return NotFound();

            DataStore.Reservations.Remove(reservation);
            return NoContent();
        }
    }
}