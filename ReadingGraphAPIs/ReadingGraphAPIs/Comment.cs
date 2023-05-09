using Newtonsoft.Json;

namespace ReadingGraphAPIs
{
    public class Comment
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("created_time")]
        public DateTime CreatedTime { get; set; }

        [JsonProperty("comment_count")]
        public int CommentCount { get; set; }        
    }
}
