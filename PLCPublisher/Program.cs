using ServiceReference;

namespace PLCPublisher
{
    internal class Program
    {
        static ServiceReference.PublisherClient pubClient = new ServiceReference.PublisherClient();
        static Random random = new Random();
        static string[] variableNames = pubClient.GetVariableNames();
        static void Main(string[] args)
        {
            Console.WriteLine("Publishing...");

            while (true)
            {
                foreach (var variable in variableNames)
                {
                    int value = random.Next(0, 101); 
                    pubClient.SendMessage(variable, value);
                }
                //Thread.Sleep(15000);
                // Manji sleep zbog testiranja
                Thread.Sleep(5000);
            }
        }
    }
}
