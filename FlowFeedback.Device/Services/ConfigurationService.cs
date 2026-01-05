using FlowFeedback.Device.Interfaces;

namespace FlowFeedback.Device.Services;

public class ConfigurationService
{
    private readonly IDeviceIdentifier _deviceIdentifier;

    public ConfigurationService(IDeviceIdentifier deviceIdentifier)
    {
        _deviceIdentifier = deviceIdentifier;
    }

    public string ApiUrl => AppConfig.BaseApiUrl;

    public string DeviceId => _deviceIdentifier.GetIdentifier();
}