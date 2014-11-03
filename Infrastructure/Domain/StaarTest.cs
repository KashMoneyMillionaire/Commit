using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Domain
{
    public class StaarTest : EntityBase<long>
    {
        public int Year { get; set; }
        public string Grade { get; set; }
        public decimal Value { get; set; }
        public virtual Campus Campus { get; set; }
        public virtual Language Language { get; set; }
        public virtual Subject Subject { get; set; }
        public virtual CategoryDetail CategoryDetail { get; set; }
        public virtual DemographicDetail DemographicDetail { get; set; }

        public long Campus_Id { get; set; }
        public long Language_Id { get; set; }
        public long Subject_Id { get; set; }
        public long CategoryDetail_Id { get; set; }
        public long DemographicDetail_Id { get; set; }
    }
}
