using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Database.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MqttClient.Helpers;
using MqttClient.Models;

namespace MqttClient.Controllers
{
    public class HomeController : BaseController
    {
        private readonly CalculationContext _dbContext;
        private readonly SortSourceByQueryParameterHelper<Message> _sortJobsByQueryParameter;

        public HomeController(
            CalculationContext dbContext,
            SortSourceByQueryParameterHelper<Message> sortJobsByQueryParameter
        )
        {
            _dbContext = dbContext;
            _sortJobsByQueryParameter = sortJobsByQueryParameter;
        }

        [HttpGet]
        public IActionResult GetMessagesAsync(
            [FromQuery]string topic = null,
            [FromQuery] string sortBy = null,
            [FromQuery] PaginationParameter pagination = null
        )
        {
           var messages =  _dbContext.Messages.Select(m => m).AsEnumerable();

           if (!(topic is null))
           {
               messages = messages.Where(m => m.Topic == topic).ToList();
           }

           messages = _sortJobsByQueryParameter.Sort(messages, sortBy);

           return new ObjectResult(Pagination(messages, pagination));
        }
    }
}