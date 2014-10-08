using System.Collections.Generic;

namespace Infrastructure.Domain
{
    public class YearStat
    {
        public Dictionary<Grade, long> StudentCount { get; set; }
        public Dictionary<Ethnicity, long> EthnicityCount { get; set; }
        public Dictionary<Ethnicity, long> GraduateEthnicityCount { get; set; }

        public Dictionary<Grade, long> RegularEdRetentionRate { get; set; }
        public Dictionary<Grade, long> SpecialEdRetentionRate { get; set; }

        public long GiftedTalentedCount { get; set; }
        public long EconomicallyDisadvantagedCount { get; set; }
        public long LEPCount { get; set; }
        public long DAEPCount { get; set; }
        public long BillingualESLCount { get; set; }
        public long AtRiskCount { get; set; }
        public long SpecialEdCount { get; set; }
        public long SpecialEdGraduateCount { get; set; }
        public long RecommendedProgramCount { get; set; }
        public long MinimumProgramCount { get; set; }
        public long CareerAndTechnicalEducationCount { get; set; }
        public long MobilityNumerator { get; set; }

        public decimal GiftedTalentedPercent { get { return GiftedTalentedCount / TotalStudents; } }
        public decimal EconomicallyDisadvantagedPercent { get { return EconomicallyDisadvantagedCount / TotalStudents; } }
        public decimal LEPPercentage { get { return LEPCount / TotalStudents; } }
        public decimal DAEPPercentage { get { return DAEPCount / TotalStudents; } }
        public decimal BillingualESLPercentage { get { return BillingualESLCount / TotalStudents; } }
        public decimal AtRiskPercentage { get { return AtRiskCount / TotalStudents; } }
        public decimal SpecialEdPercentage { get { return SpecialEdCount / TotalStudents; } }
        public decimal SpecialEdGraduatePercentage { get { return SpecialEdGraduateCount / TotalStudents; } }
        public decimal RecommendedProgramPercentage { get { return RecommendedProgramCount / TotalStudents; } }
        public decimal MinimumProgramPercentage { get { return MinimumProgramCount / TotalStudents; } }
        public decimal CareerAndTechnicalEducationPercentage { get { return CareerAndTechnicalEducationCount/TotalStudents; } }
        public decimal MobilityPercentage { get { return MobilityNumerator/TotalStudents; } }

        public decimal TotalStudents { get; set; }

        
        public decimal StudentPercent(Grade grade)
        {
            if (StudentCount.ContainsKey(grade))
                return StudentCount[grade] / TotalStudents;
            return 0;
        }
        public decimal EthnicityPercent(Ethnicity ethnicity)
        {
            if (EthnicityCount.ContainsKey(ethnicity))
                return EthnicityCount[ethnicity] / TotalStudents;
            return 0;
        }
        public decimal GraduateEthnicityPercent(Ethnicity ethnicity)
        {
            if (GraduateEthnicityCount.ContainsKey(ethnicity))
                return GraduateEthnicityCount[ethnicity] / TotalStudents;
            return 0;
        }
    }
}
