using BookingService.DTOs.Requests;
using BookingService.DTOs.Responses;
using BookingService.Grpc.Clients;
using BookingService.Services;
using FlightServiceProtoService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookingService.Controllers
{
    [Route("api/booking")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService bookingService;

        public BookingController(IBookingService bookingService)
        {
            this.bookingService = bookingService;
        }


        [HttpGet]
        public BaseResponse GetBookings()
        {
            return this.bookingService.GetBookings();
        }
        
        [HttpPost("filter")]
        public BaseResponse FilterBookings(FilterBookingsRequest request)
        {
            return this.bookingService.FilterBookings(request);
        }

        [HttpPost("make")]
        public BaseResponse MakeBooking(MakeBookingRequest request)
        {
            return this.bookingService.MakeBooking(request);
        }
        
    }
}
