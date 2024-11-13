using ServiceReference;
using System.ServiceModel;

namespace ScadaClient
{
    public class Callback : ISubscriberCallback
    {
        public void MessageArrived(string variableName, int variableValue, string plcAddress, string panelName)
        {
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"); 

            Console.WriteLine($"{timestamp} - Panel: {panelName} - {variableName} [{plcAddress}] - {variableValue}");
        }
    }
    internal class Program
    {
        static SubscriberClientBase subClient;
        static void Main(string[] args)
        {

            Console.WriteLine("Enter panel name: ");
            string panelName = Console.ReadLine();
            Console.WriteLine("Enter variable name: ");
            string variableName = Console.ReadLine();

            InstanceContext ic = new InstanceContext(new Callback());
            WSDualHttpBinding wSDualHttpBinding = new WSDualHttpBinding();
            EndpointAddress endpointAddress = new EndpointAddress("http://localhost:50114/ScadaService.svc/sub");
            subClient = new SubscriberClientBase(ic, wSDualHttpBinding, endpointAddress);

            subClient.InitSub(panelName, variableName);
            Console.ReadLine();
        }
    }
}
