using BookingService.DTOs;
using BookingService.DTOs.Requests;
using BookingService.DTOs.Responses;
using BookingService.Grpc.Clients;
using BookingService.Models;

namespace BookingService.Services;

public class BookingService : IBookingService
{
    private readonly BookingDbContext bookingDbContext;
    private readonly FlightServiceClient flightServiceClient;

    public BookingService(BookingDbContext bookingDbContext, FlightServiceClient flightServiceClient)
    {
        this.bookingDbContext = bookingDbContext;
        this.flightServiceClient = flightServiceClient;
    }


    public BaseResponse MakeBooking(MakeBookingRequest request)
    {
        try
        {
            // check flight availability
            BaseResponse getFlightResponse = flightServiceClient.GetFlight(request.flightId);
            BaseResponse response;
            
            if (getFlightResponse.statusCode == StatusCodes.Status500InternalServerError)
            {
                // some connectivity error
                response = new BaseResponse(StatusCodes.Status412PreconditionFailed, "Precondition Failed");
            }
            else if (getFlightResponse.statusCode == StatusCodes.Status400BadRequest)
            {
                // flight not found
                response = new BaseResponse(StatusCodes.Status400BadRequest, "Invalid flight id");
            }
            else
            {
                // flight is found
                FlightDTO? flightDTO = getFlightResponse.data as FlightDTO;
                
                if (flightDTO == null)
                {
                    response = new BaseResponse(StatusCodes.Status412PreconditionFailed, "Unable to parse flight data");
                }
                else
                {
                    // check seat availability
                    int bookedTicketCount = bookingDbContext.Bookings.Where(b => b.flightId == request.flightId)
                        .Sum(b => b.ticketCount);

                    if (flightDTO.availableSeats > bookedTicketCount &&
                        (flightDTO.availableSeats - bookedTicketCount) >= request.ticketCount)
                    {
                        // able to make the booking
                        bookingDbContext.Bookings.Add(new BookingModel(request));
                        bookingDbContext.SaveChanges();
                        response = new BaseResponse(StatusCodes.Status200OK, "Flight booking successfull");
                    }
                    else
                    {
                        // unable to make the booking
                        response = new BaseResponse(StatusCodes.Status400BadRequest, $"Insufficient available seats. Available seat count is {flightDTO.availableSeats - bookedTicketCount}");
                    }
                }
               
            }
            
            return response;
        }
        catch (Exception e)
        {
            Console.Error.WriteLine(e);
            return  new BaseResponse(StatusCodes.Status500InternalServerError, "Internal error");
        }
    }

    public BaseResponse GetBookings()
    {
        try
        {
            // get bookings
            List<BookingDTO> bookings = bookingDbContext.Bookings.Select(b => new BookingDTO(b)).ToList();
            return new BaseResponse(StatusCodes.Status200OK, bookings);
        }
        catch (Exception e)
        {
            Console.Error.WriteLine(e);
            return  new BaseResponse(StatusCodes.Status500InternalServerError, "Internal error");
        }
    }


    public BaseResponse FilterBookings(FilterBookingsRequest request)
    {
        try
        {
            // get bookings
            IQueryable<BookingModel> query = bookingDbContext.Bookings;

            if (request.flightId != null && request.flightId > 0)
            {
                query = query.Where(b => b.flightId == request.flightId);
            }

            if (request.userId != null  && request.userId > 0)
            {
                query = query.Where(b => b.userId == request.userId);
            }
            
            List<BookingDTO> bookings = query.Select(b => new BookingDTO(b)).ToList();
            return new BaseResponse(StatusCodes.Status200OK, bookings);
        }
        catch (Exception e)
        {
            Console.Error.WriteLine(e);
            return  new BaseResponse(StatusCodes.Status500InternalServerError, "Internal error");
        }
    }
}