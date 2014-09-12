using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentInformationColumnParser.Helpers
{
    public static class Converter
    {
        public static Enum Parse(this string input)
        {
            switch (input)
            {
                case "E":
                    return AccountabilityRating.Exemplary;
                case "R":
                    return AccountabilityRating.Recognized;
                case "A":
                case "1":
                    return AccountabilityRating.AcademicallyAcceptable;
                case "L":
                case "2":
                    return AccountabilityRating.AcademicallyUnacceptable;
                case "X":
                case "3":
                    return AccountabilityRating.NotRated_Other;
                case "I":
                    return AccountabilityRating.NotRated_DataIntegrityIssues;
                case "Z":
                case "Q":
                    return AccountabilityRating.Undefined;
            }

            switch (input)
            {
                case "EE":
                    return Grade.EE;
                case "PK":
                    return Grade.PK;
                case "KG":
                    return Grade.KG;
                case "01":
                    return Grade.G1;
                case "02":
                    return Grade.G2;
                case "03":
                    return Grade.G3;
                case "04":
                    return Grade.G4;
                case "05":
                    return Grade.G5;
                case "06":
                    return Grade.G6;
                case "07":
                    return Grade.G7;
                case "08":
                    return Grade.G8;
                case "09":
                    return Grade.G9;
                case "10":
                    return Grade.G10;
                case "11":
                    return Grade.G11;
                case "12":
                    return Grade.G12;
            }

            var fields = Enum.GetValues(typeof(StaarFieldName)).Cast<StaarFieldName>();
            foreach (var acc in fields.Where(acc => acc.ToString() == input))
            {
                return acc;
            }

            var subjects = Enum.GetValues(typeof(StaarSubjectName)).Cast<StaarSubjectName>();
            foreach (var acc in subjects.Where(acc => acc.ToString() == input))
            {
                return acc;
            }

            var categories = Enum.GetValues(typeof(StaarCategoryName)).Cast<StaarCategoryName>();
            foreach (var acc in categories.Where(acc => acc.ToString() == input))
            {
                return acc;
            }

            var gradeTypes = Enum.GetValues(typeof(GradeType)).Cast<GradeType>();
            foreach (var acc in gradeTypes.Where(acc => acc.ToString() == input))
            {
                return acc;
            }

            throw new InvalidEnumArgumentException("No match for parser");
        }

        public static bool ParseBool(this string input)
        {
            return new[] {"Y", "y", "True", "true", "1", "Yes", "yes"}.Contains(input);
        }

        public static CackDtl Parse(this char c)
        {
            switch (c.ToString())
            {
                case "A":
                    return CackDtl.CombinedAttendance;
                case "C":
                    return CackDtl.CollegeAdmissions;
                case "U":
                    return CackDtl.TSI_ELA;
                case "V":
                    return CackDtl.TSI_Math;
                case "R":
                    return CackDtl.RecommendedHsProgram;
                case "D":
                    return CackDtl.AdvancedCourse;
                case "P":
                    return CackDtl.APIB;
                case "E":
                    return CackDtl.Commended_ReadingELA;
                case "M":
                    return CackDtl.Commended_Math;
                case "W":
                    return CackDtl.Commended_Writing;
                case "N":
                    return CackDtl.Commended_Science;
                case "S":
                    return CackDtl.Commended_SocialStudies;
                case "X":
                    return CackDtl.CI_Math;
                case "Y":
                    return CackDtl.CI_Reading;
                case "Z":
                    return CackDtl.CI_MathReading;
                case "J":
                    return CackDtl.CollegeReady;
                case "":
                    return CackDtl.NotQuallified;
            }
            throw new InvalidEnumArgumentException("No match for parser");
        }
    }
}
