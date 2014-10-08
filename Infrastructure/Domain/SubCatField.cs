namespace Infrastructure.Domain
{
    public class SubCatField : EntityBase<long>
    {
        public StaarSubjectName Subject { get; set; }
        public StaarCategoryName Category { get; set; }
        public StaarDemographic Field { get; set; }
    }
}
