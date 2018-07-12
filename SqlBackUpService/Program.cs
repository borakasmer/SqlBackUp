using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Newtonsoft.Json;
using System.Linq;

namespace SqlBackUpService
{
    class Program
    {
        static void Main(string[] args)
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

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var data = Encoding.UTF8.GetString(body);
                    VehicleBackUp vehicleBackUp = JsonConvert.DeserializeObject<VehicleBackUp>(data);
                    Console.WriteLine(" [x] Received {0}", vehicleBackUp.Name + " : " + vehicleBackUp.ID);
                    //Insert, Update Or Delete
                    switch ((OperationTypes)vehicleBackUp.OperationType)
                    {
                        case OperationTypes.Insert:
                            {
                                using (VehicleDataContext context = new VehicleDataContext())
                                {
                                    try
                                    {
                                        context.VehicleBackUp.Add(vehicleBackUp);
                                        context.SaveChanges();
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine(ex.Message);
                                    }
                                }
                                break;
                            }
                        case OperationTypes.Update:
                            {
                                using (VehicleDataContext context = new VehicleDataContext())
                                {
                                    try
                                    {
                                        VehicleBackUp oldVhcBackUp = context.VehicleBackUp.First(vh => vh.ID == vehicleBackUp.ID);
                                        oldVhcBackUp.MFDate = vehicleBackUp.MFDate;
                                        oldVhcBackUp.Name = vehicleBackUp.Name;
                                        oldVhcBackUp.Quantity = vehicleBackUp.Quantity;
                                        context.SaveChanges();
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine(ex.Message);
                                    }
                                }
                                break;
                            }
                        case OperationTypes.Delete:
                            {
                                break;
                            }
                    }
                    //-------------------------
                };
                channel.BasicConsume(queue: "Vehicle",
                                     autoAck: true,
                                     consumer: consumer);

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }
        }
    }
}
