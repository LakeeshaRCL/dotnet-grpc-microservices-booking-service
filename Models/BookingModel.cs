using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BookingService.DTOs.Requests;

namespace BookingService.Models;

[Table("booking")]
public class BookingModel
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long id { get; set; }
    
    [Required, Column("flight_id")]
    public long flightId { get; set; }
    
    [Required, Column("user_id")]
    public long userId { get; set; }
    
    [Required, Column("ticket_count")]
    public int ticketCount { get; set; }
    
    [DatabaseGenerated(DatabaseGeneratedOption.Identity), Column("created_at")]
    public DateTime createdAt { get; set; }
    
    [DatabaseGenerated(DatabaseGeneratedOption.Computed), Column("updated_at")]
    public DateTime updatedAt { get; set; }


    public BookingModel()
    {
        // default 
    }
    
    public BookingModel(MakeBookingRequest makeBookingRequest)
    {
        this.flightId = makeBookingRequest.flightId;
        this.userId = makeBookingRequest.userId;
        this.ticketCount = makeBookingRequest.ticketCount;
    }
}