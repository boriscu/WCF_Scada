using ServiceReference;
using System.ServiceModel;

namespace ScadaClient
{
    public class Callback : ISubscriberCallback
    {
        private const int timestampWidth = 24;
        private const int panelWidth = 15;
        private const int variableWidth = 20;
        private const int addressWidth = 15;
        private const int valueWidth = 10;

        public void MessageArrived(string variableName, int variableValue, string plcAddress, string panelName)
        {
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");

            string message = $"{timestamp.PadRight(timestampWidth)} | " +
                             $"{panelName.PadRight(panelWidth)} | " +
                             $"{variableName.PadRight(variableWidth)} | " +
                             $"{plcAddress.PadRight(addressWidth)} | " +
                             $"{variableValue.ToString().PadLeft(valueWidth)}";

            Console.WriteLine(message);
        }
    }

    internal class Program
    {
        static SubscriberClientBase subClient;

        static void Main(string[] args)
        {
            Console.WriteLine("Initializing subscription...");

            string header = $"{"Timestamp".PadRight(24)} | " +
                            $"{"Panel".PadRight(15)} | " +
                            $"{"Variable".PadRight(20)} | " +
                            $"{"Address".PadRight(15)} | " +
                            $"{"Value".PadLeft(10)}";


            Console.WriteLine("Enter panel name: ");
            string panelName = Console.ReadLine();
            Console.WriteLine("Enter variable name: ");
            string variableName = Console.ReadLine();

            Console.WriteLine(header);
            Console.WriteLine(new string('-', header.Length));

            InstanceContext ic = new InstanceContext(new Callback());
            WSDualHttpBinding wSDualHttpBinding = new WSDualHttpBinding();
            EndpointAddress endpointAddress = new EndpointAddress("http://localhost:50114/ScadaService.svc/sub");
            subClient = new SubscriberClientBase(ic, wSDualHttpBinding, endpointAddress);

            subClient.InitSub(panelName, variableName);
            Console.ReadLine();
        }
    }
}
