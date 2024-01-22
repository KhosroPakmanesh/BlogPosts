using System;

namespace OAuth2MultiClientIntegrator.DateTimerProvider
{
    internal interface IDateTimeProvider
    {
        DateTime GetUTCDateTimeNow { get; }
    }
}
