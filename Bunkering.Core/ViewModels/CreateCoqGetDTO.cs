using Bunkering.Core.Data;

namespace Bunkering.Core.ViewModels
{
    public class CreateCoqGetDTO
    {
        public string ProductName { get; set; }
        public string ProductType { get; set; }
        public List<TankDTO> Tanks { get; set; }
        public List<SubmittedDocument> RequiredDocuments { get; set; }
        public object ApiData { get; set; }
    }
}
