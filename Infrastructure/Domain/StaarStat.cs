using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Domain
{
    public class StaarStat : EntityBase<long>
    {
        public long Campus_Id { get; set; }
        [ForeignKey("Campus_Id")]
        public virtual Campus Campus { get; set; }

        public long SubCatField_Id { get; set; }
        [ForeignKey("SubCatField_Id")]
        public virtual SubCatField SubCatField { get; set; }

        public long YearGradeLang_Id { get; set; }
        [ForeignKey("YearGradeLang_Id")]
        public YearGradeLang YearGradeLang { get; set; }

        public string Value { get; set; }
    }
}
