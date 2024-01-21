namespace Bunkering.Core.Data
{
    public class ApplicationSurveyor
    {
        public int Id { get; set; }
        public int NominatedSurveyorId { get; set; }
        public int ApplicationId { get; set; }
        public decimal Volume { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
