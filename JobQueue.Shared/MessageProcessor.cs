using System;
using System.ComponentModel;
using System.Threading.Tasks;

using Hangfire;

namespace JobQueue.Shared
{
    public class MessageProcessor
    {
        [Queue("express")]
        [DisplayName("JobId: {1}")]
        [AutomaticRetry(Attempts = 5, DelaysInSeconds = [1,3,4,7,11])]
        public static async Task ProcessExpressMessageAsync(MessageModel message, Guid messageId)
        {
            var delay = TimeSpan.FromSeconds(new Random().Next(1, 4));
            if (delay.TotalSeconds == 3)
            {
                Console.WriteLine("ApplicationException in {0} message {1}", message.Category, message.Entity);
                throw new ApplicationException();
            }

            await Task.Delay(delay);
            Console.WriteLine("Processed {0} message {1} in {2} seconds", message.Category, message.Entity, delay.TotalSeconds);
        }

        [Queue("normal")]
        [DisplayName("JobId: {1}")]
        [AutomaticRetry(Attempts = 1, DelaysInSeconds = [5])]
        public static async Task ProcessNormalMessageAsync(MessageModel message, Guid messageId)
        {
            var delay = TimeSpan.FromSeconds(new Random().Next(1, 3));
            if (delay.TotalSeconds == 3)
            {
                Console.WriteLine("ApplicationException in {0} message {1}", message.Category, message.Entity);
                throw new ApplicationException();
            }

            await Task.Delay(delay);
            Console.WriteLine("Processed {0} message {1} in {2} seconds", message.Category, message.Entity, delay.TotalSeconds);
        }
    }
}