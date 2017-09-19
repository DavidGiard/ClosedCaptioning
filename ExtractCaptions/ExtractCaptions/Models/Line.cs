namespace ExtractCaptions.Models
{
    public class Line
    {
        //public int Id { get; set; }
        //public int ParticipantId { get; set; }

        public TimeRange TimeRange { get; set; }
        public string Text { get; set; }
        //public bool IsIncluded { get; set; }

        public decimal Confidence { get; set; }




    }
}