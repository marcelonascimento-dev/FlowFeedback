using Android.Provider;
using FlowFeedback.Device.Interfaces;
using Application = Android.App.Application;

namespace FlowFeedback.Device.Platforms.Android;

public class AndroidDeviceIdentifier : IDeviceIdentifier
{
    public string GetIdentifier()
    {
        var context = Application.Context;
        // O Android ID é um hexadecimal de 64 bits único para o dispositivo/app
        string id = Settings.Secure.GetString(context.ContentResolver, Settings.Secure.AndroidId);
        return id?.ToUpper() ?? "UNKNOWN_DEVICE";
    }
}