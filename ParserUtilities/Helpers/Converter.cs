using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using Infrastructure;

namespace ParserUtilities.Helpers
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

            var fields = Enum.GetValues(typeof(StaarDemographic)).Cast<StaarDemographic>();
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

        public static StaarDemographic GetField(string thing)
        {
            switch (thing)
            {
                case "all":
                    return StaarDemographic.all;
                case "sexm":
                    return StaarDemographic.sexm;
                case "sexf":
                    return StaarDemographic.sexf;
                case "sexv":
                    return StaarDemographic.sexv;
                case "ethh":
                    return StaarDemographic.ethh;
                case "ethi":
                    return StaarDemographic.ethi;
                case "etha":
                    return StaarDemographic.etha;
                case "ethb":
                    return StaarDemographic.ethb;
                case "ethp":
                    return StaarDemographic.ethp;
                case "ethw":
                    return StaarDemographic.ethw;
                case "eth2":
                    return StaarDemographic.eth2;
                case "ethv":
                    return StaarDemographic.ethv;
                case "ecoy":
                    return StaarDemographic.ecoy;
                case "econ":
                    return StaarDemographic.econ;
                case "eco1":
                    return StaarDemographic.eco1;
                case "eco2":
                    return StaarDemographic.eco2;
                case "eco9":
                    return StaarDemographic.eco9;
                case "ecov":
                    return StaarDemographic.ecov;
                case "ti1y":
                    return StaarDemographic.ti1y;
                case "ti1n":
                    return StaarDemographic.ti1n;
                case "ti10":
                    return StaarDemographic.ti10;
                case "ti16":
                    return StaarDemographic.ti16;
                case "ti17":
                    return StaarDemographic.ti17;
                case "ti18":
                    return StaarDemographic.ti18;
                case "ti19":
                    return StaarDemographic.ti19;
                case "ti1v":
                    return StaarDemographic.ti1v;
                case "migy":
                    return StaarDemographic.migy;
                case "mign":
                    return StaarDemographic.mign;
                case "migv":
                    return StaarDemographic.migv;
                case "lepc":
                    return StaarDemographic.lepc;
                case "lepf":
                    return StaarDemographic.lepf;
                case "leps":
                    return StaarDemographic.leps;
                case "lep0":
                    return StaarDemographic.lep0;
                case "lepv":
                    return StaarDemographic.lepv;
                case "bily":
                    return StaarDemographic.bily;
                case "biln":
                    return StaarDemographic.biln;
                case "bil2":
                    return StaarDemographic.bil2;
                case "bil3":
                    return StaarDemographic.bil3;
                case "bil4":
                    return StaarDemographic.bil4;
                case "bil5":
                    return StaarDemographic.bil5;
                case "bilv":
                    return StaarDemographic.bilv;
                case "esly":
                    return StaarDemographic.esly;
                case "esln":
                    return StaarDemographic.esln;
                case "esl2":
                    return StaarDemographic.esl2;
                case "esl3":
                    return StaarDemographic.esl3;
                case "eslv":
                    return StaarDemographic.eslv;
                case "esbiy":
                    return StaarDemographic.esbiy;
                case "esbin":
                    return StaarDemographic.esbin;
                case "esbiv":
                    return StaarDemographic.esbiv;
                case "spey":
                    return StaarDemographic.spey;
                case "spen":
                    return StaarDemographic.spen;
                case "spev":
                    return StaarDemographic.spev;
                case "gify":
                    return StaarDemographic.gify;
                case "gifn":
                    return StaarDemographic.gifn;
                case "gifv":
                    return StaarDemographic.gifv;
                case "atry":
                    return StaarDemographic.atry;
                case "atrn":
                    return StaarDemographic.atrn;
                case "atrv":
                    return StaarDemographic.atrv;
                case "vocy":
                    return StaarDemographic.vocy;
                case "vocn":
                    return StaarDemographic.vocn;
                case "voc1":
                    return StaarDemographic.voc1;
                case "voc2":
                    return StaarDemographic.voc2;
                case "voc3":
                    return StaarDemographic.voc3;
                case "vocv":
                    return StaarDemographic.vocv;

                default:
                    return StaarDemographic.voc1;
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
                    return StaarCategoryName.pct_cat5;
            }
        }

        public static Grade ParseGrade(this string grade)
        {
            switch (grade)
            {
                case "EE":
                    return Grade.EE;
                case "PK":
                case "Pre-Kindergarten":
                    return Grade.PK;
                case "KG":
                case "Kindergarten":
                    return Grade.KG;
                case "01":
                case "1":
                    return Grade.G1;
                case "02":
                case "2":
                    return Grade.G2;
                case "03":
                case "3":
                    return Grade.G3;
                case "04":
                case "4":
                    return Grade.G4;
                case "05":
                case "5":
                    return Grade.G5;
                case "06":
                case "6":
                    return Grade.G6;
                case "07":
                case "7":
                    return Grade.G7;
                case "08":
                case "8":
                    return Grade.G8;
                case "09":
                case "9":
                    return Grade.G9;
                case "10":
                    return Grade.G10;
                case "11":
                    return Grade.G11;
                case "12":
                    return Grade.G12;
            }
            return Grade.EOC;
        }
    }
}

