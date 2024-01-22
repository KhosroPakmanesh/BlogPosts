using System;

namespace OAuth2MultiClientIntegrator.DateTimerProvider
{
    internal sealed class DateTimeProvider : IDateTimeProvider
    {
        public DateTime GetUTCDateTimeNow =>
            DateTime.UtcNow;
    }
}
