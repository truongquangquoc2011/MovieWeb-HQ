using Newtonsoft.Json;

namespace MovieWeb_HQ.Models.Momo
{
    public class MomoExecuteResponseModel
    {
        public string OrderId { get; set; }
        public string Amount { get; set; }
        public string OrderInfo { get; set; }

        [JsonProperty("resultCode")] // ✅ Đảm bảo ánh xạ đúng JSON trả về
        public string Status { get; set; } 

    }
}
