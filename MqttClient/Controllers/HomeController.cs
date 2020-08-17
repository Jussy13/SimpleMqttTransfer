using System.Linq;
using System.Threading.Tasks;
using Database.Models;
using Microsoft.AspNetCore.Mvc;
using MqttClient.Helpers;
using MqttClient.Models;
using MqttClient.Services;

namespace MqttClient.Controllers
{
    [Route("Home")]
    public class HomeController : BaseController
    {
        private readonly IMqttClientService _mqttClient;
        private readonly CalculationContext _dbContext;
        private readonly SortSourceByQueryParameterHelper<Message> _sortJobsByQueryParameter;

        public HomeController(
            IMqttClientService mqttClient,
            CalculationContext dbContext,
            SortSourceByQueryParameterHelper<Message> sortJobsByQueryParameter
        )
        {
            _mqttClient = mqttClient;
            _dbContext = dbContext;
            _sortJobsByQueryParameter = sortJobsByQueryParameter;
        }

        [HttpGet]
        [Route("")]
        [Route("messages")]
        [Route("/messages")]
        [Route("/")]
        public IActionResult GetMessages(
            [FromQuery]string topic = null,
            [FromQuery] string sortBy = null,
            [FromQuery] PaginationParameter pagination = null
        )
        {
           var messages =  _dbContext.Messages.Select(m => m).AsEnumerable();

           if (!(topic is null))
           {
               messages = messages.Where(m => m.Topic == topic);
           }

           messages = _sortJobsByQueryParameter.Sort(messages, sortBy);

           return new ObjectResult(Pagination(messages, pagination));
        }

        [HttpPost]
        [Route("send-message")]
        [Route("/send-message")]
        public Task SendMessageToTopic([FromBody] string payload)
        {
            return _mqttClient.PublishAsync(payload);
        }
    }
}
