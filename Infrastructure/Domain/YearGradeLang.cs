namespace Infrastructure.Domain
{
    public class YearGradeLang : EntityBase<long>
    {
        public long Year { get; set; }
        public Grade Grade { get; set; }
        public Language Language { get; set; }
    }
}
