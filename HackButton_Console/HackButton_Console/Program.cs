using SignalRNotifier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackButton_Console
{
    class Program
    {
        private static int notificationCount = 1;
        static void Main(string[] args)
        {
            try
            {


                //Create and connect to SignalR client
                SignalRClient client = new SignalRClient();
                client.ConnectToSignalRHub().Wait();
                //Subscribe to callback method for notification is sent via SignalR.
                client.OnDataReceivedCallBack(Datareceived);

                //Guid buttonGuid = Guid.Parse("Add Guid of button you want to subscribe to.");
                Guid buttonGuid = Guid.Parse("ba611b0d-2224-e611-80c3-000d3a3384f5");
                Console.WriteLine($"Subsribing to button: {buttonGuid.ToString()}\n");
                //Subscribe to button via its Guid identifier.
                client.SubscribeToEntity(buttonGuid, ButtonTypeEnum.TwoRockerButton);


                Console.WriteLine("Currentely subscribed to following buttons:");
                //Print out current subscriptions. Should only be one as we have only subscribed to one button at this time.
                foreach (var item in client.GetCurrentSubsriptions())
                {
                    Console.WriteLine("------------------------------------------------------------------------------");
                    Console.WriteLine($"Button Id : {item.Key}\nButton Type : {item.Value}");
                    Console.WriteLine("------------------------------------------------------------------------------");
                }

                Console.WriteLine("\n\nPress enter to exit\n");
                Console.ReadLine();
            }
            catch (Exception ex)
            {

            }
        }
        /// <summary>
        /// Callback method used for when SignalR notification is sent. This method prints out the notification data to the console in a formatted form.
        /// </summary>
        /// <param name="data"></param>
        public static void Datareceived(Notification data)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            string number = string.Empty;
            switch (notificationCount)
            {
                case 1:
                    number = "1st";
                    break;
                case 2:
                    number = "2nd";
                    break;
                case 3:
                    number = "3rd";
                    break;

                default:
                    number = notificationCount.ToString() + "th";
                    break;
            }
            Console.WriteLine($"--------------------------- {number} Notification received! ---------------------------");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Button Number Pressed: {data.ButtonNumberPressed.ToString()}");
            Console.WriteLine($"Button Type: {data.ButtonType.ToString()}");
            Console.WriteLine($"Button Id : {data.EntityId.ToString()}");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"{FormatEndString(79 + number.Length)}\n");
            Console.ForegroundColor = ConsoleColor.White;
            notificationCount++;
        }
        /// <summary>
        /// Helper method for printing correct number of bottom dashes based on length of number string.
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        private static string FormatEndString(int count)
        {
            string dashes = string.Empty;
            for (int i = 0; i < count; i++)
            {
                dashes += "-";
            }
            return dashes;
        }
    }
}
