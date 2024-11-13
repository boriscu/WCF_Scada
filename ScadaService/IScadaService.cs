using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ScadaService
{
    [ServiceContract]
    public interface IPublisher
    {
        [OperationContract(IsOneWay = true)]
        void SendMessage(string variableName, int variableValue);

        [OperationContract]
        List<string> GetVariableNames();
    }

    [ServiceContract(CallbackContract = typeof(ICallback), SessionMode = SessionMode.Required)]
    public interface ISubscriber
    {
        [OperationContract(IsOneWay = true)]
        void InitSub(string panelName, string variableName);
    }

    [ServiceContract]
    public interface ICallback
    {
        [OperationContract(IsOneWay = true)]
        void MessageArrived(string variableName, int variableValue, string plcAddress, string panelName);
    }

}
