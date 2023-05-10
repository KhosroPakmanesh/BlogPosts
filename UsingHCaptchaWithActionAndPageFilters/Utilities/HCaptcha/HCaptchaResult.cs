using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace UsingHCaptchaWithActionAndPageFilters.Utilities.HCaptcha
{
    public class HCaptchaResult
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("challenge_ts")]
        public DateTime ChallengeTs { get; set; }

        [JsonPropertyName("hostname")]
        public string Hostname { get; set; }

        [JsonPropertyName("error-codes")]
        public List<string> ErrorCodes { get; set; }
    }
}
