using FlowFeedback.Device.Interfaces;

namespace FlowFeedback.Device.Services;

public class ConfigurationService
{
    private const string DeviceIdKey = "device_internal_id";
    private const string TenantIdKey = "tenant_id";

    private readonly IDeviceIdentifier _deviceIdentifier;

    public ConfigurationService(IDeviceIdentifier deviceIdentifier)
    {
        _deviceIdentifier = deviceIdentifier;
    }

    public string ApiUrl => AppConfig.BaseApiUrl;

    public Guid TenantId
    {
        get
        {
            var id = Preferences.Default.Get(TenantIdKey, string.Empty);
            return string.IsNullOrEmpty(id) ? Guid.Empty : Guid.Parse(id);
        }
        set => Preferences.Default.Set(TenantIdKey, value.ToString());
    }

    public string DeviceId
    {
        get
        {
            var id = Preferences.Default.Get(DeviceIdKey, string.Empty);
            if (string.IsNullOrEmpty(id))
            {
                id = _deviceIdentifier.GetIdentifier();
                Preferences.Default.Set(DeviceIdKey, id);
            }
            return id;
        }
    }
}