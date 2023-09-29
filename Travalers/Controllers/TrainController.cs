using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Travalers.DTOs.Train;
using Travalers.DTOs.User;
using Travalers.Entities;
using Travalers.Repository;

namespace Travalers.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrainController : ControllerBase
    {

        private readonly ITrainRepository _trainRepository;
        private readonly IConfiguration _configuration;

        public TrainController(ITrainRepository trainRepository, IConfiguration configuration)
        {
            _trainRepository = trainRepository;
            _configuration = configuration;
        }

        [HttpPost("addTrain")]
        public async Task<IActionResult> TrainSave([FromBody] TrainDto trainDto)
        {
            try
            {
                

                if (string.IsNullOrEmpty(trainDto.Id))
                {

                    var train = new Entities.Train();
                    {
                        train.Name = trainDto.Name;
                        train.StartPoint = trainDto.StartPoint;
                        train.EndPoint = trainDto.EndPoint;
                        train.StartTime = trainDto.StartTime;
                        train.EndTime = trainDto.EndTime;
                        train.Price = trainDto.Price;
                        train.Discription = trainDto.Discription;
                        train.Seats = trainDto.Seats;
                    }

                    await _trainRepository.CreateTrainAsync(train);

                    return Ok("Train Successfully Added to the System.");
                }
                else
                {
                    var train = await _trainRepository.GetTrainById(trainDto.Id);

                    train.Name = trainDto.Name;
                    train.StartPoint = trainDto?.StartPoint;
                    train.EndPoint = trainDto?.EndPoint;
                    train.StartTime = trainDto.StartTime;
                    train.Price = trainDto?.Price;
                    train.EndTime = trainDto.EndTime;
                    train.Discription = trainDto?.Discription;
                    train.Seats = trainDto.Seats;

                    await _trainRepository.UpdateTrainAsync(train);

                    return Ok("Train Successfully Updated.");
                }
            }catch(Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("getAllTrains")]
        public async Task<ActionResult<Train>> GetAllTrains()
        {
            var train = await _trainRepository.GetAllTrains();

            if (train == null)
            {
                return NotFound();
            }

            return Ok(train);
        }

        [HttpGet("GetTrainById{id}")]
        public async Task<ActionResult<User>> GetTrainById(string id)
        {
            var train = await _trainRepository.GetTrainById(id);

            if (train == null)
            {
                return NotFound();
            }

            return Ok(train);
        }

        [HttpDelete("DeleteTrainById{id}")]
        public async Task<ActionResult> DeleteTrain(string id)
        {
            var train = await _trainRepository.GetTrainById(id);

            if(train == null)
            {
                return NotFound("Train not Found");
            }

            else
            {
                await _trainRepository.DeleteTrainAsync(id);

                return Ok("Train Deleted Successfully");
            }
        }
    }
}
