using System;

namespace SampleFirebase.Models
{
    public class TaskModel
    {
        public string askPrice { get; set; }
        public string askQty { get; set; }
        public string bidPrice { get; set; }
        public string bidQty { get; set; }
        public Int64 openTime { get; set; }
        public Int64 closeTime { get; set; }
        public Int64 count { get; set; }
        public Int64 firstId { get; set; }
        public string highPrice { get; set; }
        public Int64 lastId { get; set; }
        public string lastPrice { get; set; }
        public string lastQty { get; set; }
        public string lowPrice { get; set; }
        public string openPrice { get; set; }
        public string prevClosePrice { get; set; }
        public string priceChange { get; set; }
        public string priceChangePercent { get; set; }
        public string quoteVolume { get; set; }
        public string symbol { get; set; }
        public string volume { get; set; }
        public string weightedAvgPrice { get; set; }
    }
}
