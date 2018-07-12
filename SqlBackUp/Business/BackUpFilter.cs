using Microsoft.AspNetCore.Mvc.Filters;
using RabbitMQ.Client;
using Newtonsoft.Json;
using System.Text;

namespace SqlFilter.Filters
{
    public class BackUpFilter : IActionFilter
    {
        //Vehicle data;
        public void OnActionExecuting(ActionExecutingContext context)
        {
            context.ActionArguments.TryGetValue("model", out object _vehcile);
            context.HttpContext.Items["__vehcile__"] = (Vehicle)_vehcile;
            //this.data = (Vehicle)_vehcile;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {           
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "Vehicle",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);
                var data = context.HttpContext.Items["__vehcile__"];

                //var vehicleData = JsonConvert.SerializeObject(this.data);
                var vehicleData = JsonConvert.SerializeObject(data);
                var body = Encoding.UTF8.GetBytes(vehicleData);

                channel.BasicPublish(exchange: "",
                                     routingKey: "Vehicle",
                                     basicProperties: null,
                                     body: body);                
            }
        }
    }
}