using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ScadaService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ScadaService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select ScadaService.svc or ScadaService.svc.cs at the Solution Explorer and start debugging.
    public class ScadaService : IPublisher, ISubscriber
    {

        private static Dictionary<string, string> plcAddressMap = new Dictionary<string, string>
        {
            {"Temperature", "AO1"},
            {"Pressure", "AO2"},
            {"Flow Rate", "AO3"},
            {"Level", "AO4"},
            {"Humidity", "AO5"},
            {"Velocity", "AO6"},
            {"Voltage", "AO7"},
            {"Current", "AO8"}
        };

        private static ConcurrentDictionary<string, Dictionary<string, ICallback>> subscriptionsByVariable = new ConcurrentDictionary<string, Dictionary<string, ICallback>>();

        public void InitSub(string panelName, string variableName)
        {
            if (plcAddressMap.ContainsKey(variableName))
            {
                var callback = OperationContext.Current.GetCallbackChannel<ICallback>();
                subscriptionsByVariable.AddOrUpdate(variableName, new Dictionary<string, ICallback> { { panelName, callback } }, (key, oldValue) =>
                {
                    oldValue[panelName] = callback;
                    return oldValue;
                });
            }
        }
        public void SendMessage(string variableName, int variableValue)
        {
            if (subscriptionsByVariable.TryGetValue(variableName, out Dictionary<string, ICallback> panelCallbacks))
            {
                string plcAddress = plcAddressMap[variableName];
                foreach (var entry in panelCallbacks)
                {
                    string panelName = entry.Key;
                    ICallback callback = entry.Value;
                    callback.MessageArrived(variableName, variableValue, plcAddress, panelName);
                }
            }
        }

        public List<string> GetVariableNames()
        {
            return plcAddressMap.Keys.ToList();
        }
    }
}
