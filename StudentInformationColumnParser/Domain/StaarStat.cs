using CommitParser.Helpers;

namespace CommitParser.Domain
{
    public class StaarStat : EntityBase<long>
    {
        public virtual Campus Campus { get; set; }
        public long Year { get; set; }
        public Grade Grade { get; set; }
        public Language Language { get; set; }
        public StaarSubjectName Subject { get; set; }
        public StaarFieldName Field { get; set; }
        public StaarCategoryName Category { get; set; }
        public string Value { get; set; }
    }
}
