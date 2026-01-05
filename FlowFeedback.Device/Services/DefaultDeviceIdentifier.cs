using FlowFeedback.Device.Interfaces;

namespace FlowFeedback.Device.Services;

public class DefaultDeviceIdentifier : IDeviceIdentifier
{
    public string GetIdentifier()
    {
        return $"{DeviceInfo.Current.Name}_{DeviceInfo.Current.Model}".Replace(" ", "_").ToUpper();
    }
}