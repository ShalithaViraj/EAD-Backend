using Microsoft.AspNetCore.Mvc;
using Travalers.DTOs.Ticket;
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

                    if(reserveTicketDto.NoOfSeats <= 4)
                    {
                        if (train.Seats != 0)
                        {
                            if ((train.StartTime - DateTime.UtcNow).TotalDays < 30)
                            {
                                var seatCount = reserveTicketDto.NoOfSeats; 

                                while(seatCount != 0)
                                {
                                    train.Seats = train.Seats - 1;

                                    var ticket = new Tickets()
                                    {
                                        UserId = curentUser,
                                        SeatNumber = train.Seats + 1,
                                        TrainId = reserveTicketDto.TrainId,
                                        CreatedDate = DateTime.UtcNow,
                                        NoOfSeats = reserveTicketDto.NoOfSeats
                                    };                             

                                    await _ticketRepository.CreateTicketAsync(ticket);

                                    seatCount--;

                                }

                                await _trainRepository.UpdateTrainAsync(train);

                                return Ok("Ticket Reserved");
                            }
                            else
                            {
                                return BadRequest("Too Early to Make a Researvation.");
                            }
                        }
                        else
                        {
                            return BadRequest("Train Seats are Full");
                        }
                    }
                    else
                    {
                        return BadRequest("Maximun Tickets Per One Time iS four..");
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

        [HttpGet("GetReserveTicketsByUserId")]
        public async Task<ActionResult<List<ReservationDto>>> GetReservationsByUserId()
        {
            try
            {
                var userId = _currentUserService.UserId;

                var tickets = await _ticketRepository.GetTicketByUserId(userId);
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

        [HttpGet("GetTicketByUserIdForAdmin{id}")]
        public async Task<ActionResult> GetTicketByUserIdForAdmin(string id)
        {
            try
            {
                var tickets = await _ticketRepository.GetTicketByUserId(id);

                var reservationDtos = new List<ReservationDto>();

                foreach (var ticket in tickets)
                {
                    var userName = (await _userRepository.GetUserById(id)).Username;
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

        [HttpGet("GetTicketByTrainId{id}")]
        public async Task<ActionResult> GetTicketByTrainId(string id)
        {
            try
            {
                var tickets = await _ticketRepository.GetTicketByTrainId(id);

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

        [HttpDelete("CancelTicket{id}")]
        public async Task<ActionResult> CancelTicket(string id)
        {
            try
            {
                var ticket = await _ticketRepository.GetTicketByIdAsync(id);

                var train = await _trainRepository.GetTrainById(ticket.TrainId);

                if (ticket == null)
                {
                    return NotFound("Ticket not Found.");
                }

                else
                {
                    if((train.StartTime - DateTime.Now).TotalDays >= 5 )
                    {
                        await _ticketRepository.CancelTicketAsync(id);

                        train.Seats = train.Seats - 1;

                        await _trainRepository.UpdateTrainAsync(train);

                        return Ok("Ticket Canceled.");
                    }
                    else
                    {
                        return BadRequest("You cannot cancel a ticket now. Minimun date to cancel a ticket is at least five days before the train depature. ");
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
