using System;
using MQTTnet;

namespace Wta.Infrastructure.Mqtt;

public interface IMqttServerService
{
    void Receive(string clientId, MqttApplicationMessage applicationMessage);
    bool Valid(string clientId, string userName, string password);
}
