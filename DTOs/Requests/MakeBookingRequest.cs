using System.ComponentModel.DataAnnotations;

namespace BookingService.DTOs.Requests;

public class MakeBookingRequest
{
    [Required]
    public long userId { get; set; }
    
    [Required]
    public long flightId { get; set; }
    
    [Required]
    public int ticketCount { get; set; }
    
}