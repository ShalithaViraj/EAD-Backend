using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Travalers.DTOs.Ticket;
using Travalers.DTOs.Train;
using Travalers.Entities;
using Travalers.Repository;
using Travalers.Services;

namespace Travalers.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository;
        private readonly ITrainRepository _trainRepository;
        private readonly ICurrentUserService _currentUserService;

        public TicketController(ITicketRepository ticketRepository, 
                                IConfiguration configuration, 
                                IUserRepository userRepository,
                                ITrainRepository trainRepository,
                                ICurrentUserService currentUserService)
        {
            _configuration = configuration;
            _ticketRepository = ticketRepository;
            _userRepository = userRepository;
            _trainRepository = trainRepository;
            _currentUserService = currentUserService;
        }

        [HttpPost("ReserveTicket")]
        public async Task<IActionResult> CreateReservation([FromBody] ReserveTicketDto reserveTicketDto)
        {
            try
            {

                if (string.IsNullOrEmpty(reserveTicketDto.Id))
                {
                   
                    var train = await _trainRepository.GetTrainById(reserveTicketDto.TrainId);

                    var curentUser = _currentUserService.UserId;

                    if(train.Seats != 0)
                    {
                        train.Seats = train.Seats - 1;

                        var ticket = new Tickets()
                        {
                            UserId = curentUser,
                            SeatNumber = train.Seats + 1,
                            TrainId = reserveTicketDto.TrainId,
                        };

                        await _trainRepository.UpdateTrainAsync(train);

                        await _ticketRepository.CreateTicketAsync(ticket);

                        return Ok("Ticket Reserved");
                    }
                    else
                    {
                        return BadRequest("Train Seats are Full");
                    }  
                }
                else
                {
                    return BadRequest();
                }


            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetReserveTickets")]
        public async Task<ActionResult<List<ReservationDto>>> GetReservation()
        {
            try
            {
                var tickets = await _ticketRepository.GetAllTicketsAsync();
                var reservationDtos = new List<ReservationDto>();

                foreach (var ticket in tickets)
                {
                    var userName = (await _userRepository.GetUserById(ticket.UserId)).Username;
                    var trainName = (await _trainRepository.GetTrainById(ticket.TrainId)).Name;

                    var reservationDto = new ReservationDto
                    {
                        TrainId = ticket.TrainId,
                        UserId = ticket.UserId,
                        UserName = userName,
                        TrainName = trainName,
                        SeatNumber = ticket.SeatNumber,
                    };

                    reservationDtos.Add(reservationDto);
                }

                return Ok(reservationDtos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}
