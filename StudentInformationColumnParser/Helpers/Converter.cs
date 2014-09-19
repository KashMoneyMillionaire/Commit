using System;
using System.ComponentModel;
using System.Linq;

namespace CommitParser.Helpers
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
            return new[] { "Y", "y", "True", "true", "1", "Yes", "yes" }.Contains(input);
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

        public static StaarSubjectName GetSubject(string thing)
        {
            switch (thing)
            {
                case "a1":
                    return StaarSubjectName.a1;
                case "a2":
                    return StaarSubjectName.a2;
                case "w":
                    return StaarSubjectName.w;
                case "w1":
                    return StaarSubjectName.w1;
                case "w2":
                    return StaarSubjectName.w2;
                case "r":
                    return StaarSubjectName.r;
                case "e1":
                    return StaarSubjectName.e1;
                case "e2":
                    return StaarSubjectName.e2;
                case "us":
                    return StaarSubjectName.us;
                case "bi":
                    return StaarSubjectName.bi;
                case "wg":
                    return StaarSubjectName.wg;
                case "ch":
                    return StaarSubjectName.ch;
                case "m":
                    return StaarSubjectName.m;
                case "s":
                    return StaarSubjectName.s;
                case "h":
                    return StaarSubjectName.h;
                default:
                    return StaarSubjectName.Unknown;
            }
        }

        public static StaarFieldName GetField(string thing)
        {
            switch (thing)
            {
                case "all":
                    return StaarFieldName.all;
                case "sexm":
                    return StaarFieldName.sexm;
                case "sexf":
                    return StaarFieldName.sexf;
                case "sexv":
                    return StaarFieldName.sexv;
                case "ethh":
                    return StaarFieldName.ethh;
                case "ethi":
                    return StaarFieldName.ethi;
                case "etha":
                    return StaarFieldName.etha;
                case "ethb":
                    return StaarFieldName.ethb;
                case "ethp":
                    return StaarFieldName.ethp;
                case "ethw":
                    return StaarFieldName.ethw;
                case "eth2":
                    return StaarFieldName.eth2;
                case "ethv":
                    return StaarFieldName.ethv;
                case "ecoy":
                    return StaarFieldName.ecoy;
                case "econ":
                    return StaarFieldName.econ;
                case "eco1":
                    return StaarFieldName.eco1;
                case "eco2":
                    return StaarFieldName.eco2;
                case "eco9":
                    return StaarFieldName.eco9;
                case "ecov":
                    return StaarFieldName.ecov;
                case "ti1y":
                    return StaarFieldName.ti1y;
                case "ti1n":
                    return StaarFieldName.ti1n;
                case "ti10":
                    return StaarFieldName.ti10;
                case "ti16":
                    return StaarFieldName.ti16;
                case "ti17":
                    return StaarFieldName.ti17;
                case "ti18":
                    return StaarFieldName.ti18;
                case "ti19":
                    return StaarFieldName.ti19;
                case "ti1v":
                    return StaarFieldName.ti1v;
                case "migy":
                    return StaarFieldName.migy;
                case "mign":
                    return StaarFieldName.mign;
                case "migv":
                    return StaarFieldName.migv;
                case "lepc":
                    return StaarFieldName.lepc;
                case "lepf":
                    return StaarFieldName.lepf;
                case "leps":
                    return StaarFieldName.leps;
                case "lep0":
                    return StaarFieldName.lep0;
                case "lepv":
                    return StaarFieldName.lepv;
                case "bily":
                    return StaarFieldName.bily;
                case "biln":
                    return StaarFieldName.biln;
                case "bil2":
                    return StaarFieldName.bil2;
                case "bil3":
                    return StaarFieldName.bil3;
                case "bil4":
                    return StaarFieldName.bil4;
                case "bil5":
                    return StaarFieldName.bil5;
                case "bilv":
                    return StaarFieldName.bilv;
                case "esly":
                    return StaarFieldName.esly;
                case "esln":
                    return StaarFieldName.esln;
                case "esl2":
                    return StaarFieldName.esl2;
                case "esl3":
                    return StaarFieldName.esl3;
                case "eslv":
                    return StaarFieldName.eslv;
                case "esbiy":
                    return StaarFieldName.esbiy;
                case "esbin":
                    return StaarFieldName.esbin;
                case "esbiv":
                    return StaarFieldName.esbiv;
                case "spey":
                    return StaarFieldName.spey;
                case "spen":
                    return StaarFieldName.spen;
                case "spev":
                    return StaarFieldName.spev;
                case "gify":
                    return StaarFieldName.gify;
                case "gifn":
                    return StaarFieldName.gifn;
                case "gifv":
                    return StaarFieldName.gifv;
                case "atry":
                    return StaarFieldName.atry;
                case "atrn":
                    return StaarFieldName.atrn;
                case "atrv":
                    return StaarFieldName.atrv;
                case "vocy":
                    return StaarFieldName.vocy;
                case "vocn":
                    return StaarFieldName.vocn;
                case "voc1":
                    return StaarFieldName.voc1;
                case "voc2":
                    return StaarFieldName.voc2;
                case "voc3":
                    return StaarFieldName.voc3;
                case "vocv":
                    return StaarFieldName.vocv;

                default:
                    return StaarFieldName.Unknown;
            }
        }

        public static StaarCategoryName GetCategory(string thing)
        {
            switch (thing)
            {
                case "d":
                    return StaarCategoryName.d;
                case "satis_ph1_nm":
                    return StaarCategoryName.satis_ph1_nm;
                case "satis_ph2_nm":
                    return StaarCategoryName.satis_ph2_nm;
                case "satis_rec_nm":
                    return StaarCategoryName.satis_rec_nm;
                case "adv_rec_nm":
                    return StaarCategoryName.adv_rec_nm;
                case "unsat_ph1_nm":
                    return StaarCategoryName.unsat_ph1_nm;
                case "unsat_ph2_nm":
                    return StaarCategoryName.unsat_ph2_nm;
                case "unsat_rec_nm":
                    return StaarCategoryName.unsat_rec_nm;
                case "min_ph1_nm":
                    return StaarCategoryName.min_ph1_nm;
                case "min_ph2_nm":
                    return StaarCategoryName.min_ph2_nm;
                case "min_rec_nm":
                    return StaarCategoryName.min_rec_nm;
                case "satis_ph1_rm":
                    return StaarCategoryName.satis_ph1_rm;
                case "satis_ph2_rm":
                    return StaarCategoryName.satis_ph2_rm;
                case "satis_rec_rm":
                    return StaarCategoryName.satis_rec_rm;
                case "adv_rec_rm":
                    return StaarCategoryName.adv_rec_rm;
                case "unsat_ph1_rm":
                    return StaarCategoryName.unsat_ph1_rm;
                case "unsat_ph2_rm":
                    return StaarCategoryName.unsat_ph2_rm;
                case "unsat_rec_rm":
                    return StaarCategoryName.unsat_rec_rm;
                case "min_ph1_rm":
                    return StaarCategoryName.min_ph1_rm;
                case "min_ph2_rm":
                    return StaarCategoryName.min_ph2_rm;
                case "min_rec_rm":
                    return StaarCategoryName.min_rec_rm;
                case "rs":
                    return StaarCategoryName.rs;
                case "avg_cat1":
                    return StaarCategoryName.avg_cat1;
                case "pct_cat1":
                    return StaarCategoryName.pct_cat1;
                case "pct_cat2":
                    return StaarCategoryName.pct_cat2;
                case "avg_cat2":
                    return StaarCategoryName.avg_cat2;
                case "avg_cat3":
                    return StaarCategoryName.avg_cat3;
                case "pct_cat3":
                    return StaarCategoryName.pct_cat3;
                case "pct_cat4":
                    return StaarCategoryName.pct_cat4;
                case "avg_cat4":
                    return StaarCategoryName.avg_cat4;
                case "avg_cat5":
                    return StaarCategoryName.avg_cat5;
                case "pct_cat5":
                    return StaarCategoryName.pct_cat5;
                default:
                    return StaarCategoryName.Unknown;
            }
        }

    }
}

