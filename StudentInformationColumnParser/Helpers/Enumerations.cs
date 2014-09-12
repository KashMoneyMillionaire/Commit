using System;
using System.ComponentModel;

namespace CommitParser.Helpers
{
    public enum AccountabilityRating : long
    {
        Exemplary, //E
        Recognized, //R
        AcademicallyAcceptable, //A, 1
        AcademicallyUnacceptable, //L, 2
        NotRated_Other, //X, 3
        NotRated_DataIntegrityIssues, //I
        Undefined
    }

    public enum Grade : long
    {
        EE,
        PK,
        KG,
        G1,
        G2,
        G3,
        G4,
        G5,
        G6,
        G7,
        G8,
        G9,
        G10,
        G11,
        G12
    }

    public enum GradeType : long
    {
        E,M,S,B
    }

    [Flags]
    public enum CackDtl : long
    {
        NotQuallified = 0, //blank
        CombinedAttendance = 1 << 0, //A
        CollegeAdmissions = 1 << 1, //C
        TSI_ELA = 1 << 2, //U
        TSI_Math = 1 << 3, //V
        RecommendedHsProgram = 1 << 4, //R
        AdvancedCourse = 1 << 5, //D
        APIB = 1 << 6, //P
        Commended_ReadingELA = 1 << 7, //E
        Commended_Math = 1 << 8, //M
        Commended_Writing = 1 << 9, //W
        Commended_Science = 1 << 10, //N
        Commended_SocialStudies = 1 << 11, //S
        CI_Math = 1 << 12, //X
        CI_Reading = 1 << 13, //Y
        CI_MathReading = 1 << 14, //Z
        CollegeReady = 1 << 15 //J
    }

    public enum Ethnicity : long
    {
        AfricanAmerican,
        Asian,
        Hispanic,
        NativeAmerican,
        PacificIslander,
        White,
        TwoOrMore
    }

    public enum StaarFieldName : long
    {
        [Description("All Students")]
        all,
        [Description("Male Students")]
        sexm,
        [Description("Female Students")]
        sexf,
        [Description("No Sex Info")]
        sexv,
        [Description("Hispanic/Latino")]
        ethh,
        [Description("American Indian/Alaskan Native")]
        ethi,
        [Description("Asian")]
        etha,
        [Description("Black/African American")]
        ethb,
        [Description("Native Hawaiian/ Pacific Islander")]
        ethp,
        [Description("White")]
        ethw,
        [Description("Two or More Races")]
        eth2,
        [Description("No Ethnicity Info")]
        ethv,
        [Description("Economically Disadvantaged")]
        ecoy,
        [Description("Not Economically Disadvantaged")]
        econ,
        [Description("Free Meals")]
        eco1,
        [Description("Reduced-Price Meals")]
        eco2,
        [Description("Other Economically Disadvantaged")]
        eco9,
        [Description("No Info Economically")]
        ecov,
        [Description("Title-1 Participant")]
        ti1y,
        [Description("Not Title-1 Participant")]
        ti1n,
        [Description("Nonparticipant")]
        ti10,
        [Description("Schoolwide Program Participant")]
        ti16,
        [Description("Targeted Assistance Participant")]
        ti17,
        [Description("Nonparticipant")]
        ti18,
        [Description("Homeless Participant at Non-Title-1 Schools")]
        ti19,
        [Description("No Title")]
        ti1v,
        [Description("Migrant Student")]
        migy,
        [Description("Not Migrant Student")]
        mign,
        [Description("No Migrant Info")]
        migv,
        [Description("Current LEP")]
        lepc,
        [Description("First Year Monitored")]
        lepf,
        [Description("Second Year Monitored")]
        leps,
        [Description("Other Non-LEP")]
        lep0,
        [Description("No LEP")]
        lepv,
        [Description("Bilingual")]
        bily,
        [Description("Not Bilingual")]
        biln,
        [Description("Transitional Bilingual/ Early Exit")]
        bil2,
        [Description("Transitional Bilingual/ Late Exit")]
        bil3,
        [Description("Dual Language Immersion/ Two Way")]
        bil4,
        [Description("Dual Language Immersion/ One Way")]
        bil5,
        [Description("No Info Bilingual")]
        bilv,
        [Description("ESL")]
        esly,
        [Description("Not ESL")]
        esln,
        [Description("ESL/ Content based")]
        esl2,
        [Description("ESL/ Pull-out")]
        esl3,
        [Description("No Info ESL")]
        eslv,
        [Description("Either Bilingual or ESL")]
        esbiy,
        [Description("Neither Bilingual nor ESL")]
        esbin,
        [Description("No Info for both and/or Either Bilingual/ESL")]
        esbiv,
        [Description("Special Ed ")]
        spey,
        [Description("Not Special Ed")]
        spen,
        [Description("No Info on Special Ed")]
        spev,
        [Description("Gifted/Talented")]
        gify,
        [Description("Not Gifted/Talented")]
        gifn,
        [Description("No Info on Gifted/ Talented")]
        gifv,
        [Description("At-Risk")]
        atry,
        [Description("Not At-Risk")]
        atrn,
        [Description("No Info At-Risk")]
        atrv,
        [Description("Career/Tech ")]
        vocy,
        [Description("Not Career/ Tech")]
        vocn,
        [Description("Career/Tech Elective")]
        voc1,
        [Description("Career/Tech Coherent Sequence")]
        voc2,
        [Description("Career/ Tech Tech Prep")]
        voc3,
        [Description("No Info Career/Tech")]
        vocv
    }

    public enum StaarSubjectName : long
    {
        [Description("Algebra I")]
        a1,
        [Description("Algebra II")]
        a2,
        [Description("Writing")]
        w,
        [Description("English I-Writing")]
        w1,
        [Description("English II-Writing")]
        w2,
        [Description("Reading")]
        r,
        [Description("English I-Reading")]
        r1,
        [Description("English II-Reading")]
        r2,
        [Description("U.S. History")]
        us,
        [Description("Biology")]
        bi,
        [Description("World Geography")]
        wg,
        [Description("chemistry")]
        ch,
        [Description("Mathematics")]
        m,
        [Description("Science")]
        s,
        [Description("Social Studies")]
        h,

    }

    public enum StaarCategoryName : long
    {
        [Description("# Tested")]
        d,
        [Description("# Achieved Level II Satisfactory--Phase-in 1")]
        satis_ph1_nm,
        [Description("# Achieved Level II Satisfactory--Phase-in 2 ")]
        satis_ph2_nm,
        [Description("# Achieved Level II Satisfactory--Final Recommended ")]
        satis_rec_nm,
        [Description("# Achieved Level III Advanced--Final Recommended")]
        adv_rec_nm,
        [Description("# Achieved Level I Unsatisfactory--Phase-in 1")]
        unsat_ph1_nm,
        [Description("# Achieved Level I Unsatisfactory--Phase-in 2")]
        unsat_ph2_nm,
        [Description("# Achieved Level I Unsatisfactory--Final Recommended ")]
        unsat_rec_nm,
        [Description("# Achieved Level I Unsatisfactory Minimum--Phase-in 1")]
        min_ph1_nm,
        [Description("# Achieved Level I Unsatisfactory Minimum--Phase-in 2")]
        min_ph2_nm,
        [Description("# Achieved Level I Unsatisfactory Minimum--Final Recommended ")]
        min_rec_nm,
        [Description("% Achieved Level II Satisfactory--Phase-in 1 ")]
        satis_ph1_rm,
        [Description("% Achieved Level II Satisfactory--Phase-in 2")]
        satis_ph2_rm,
        [Description("% Achieved Level II Satisfactory--Final Recommended ")]
        satis_rec_rm,
        [Description("% Achieved Level III Advanced--Final Recommended ")]
        adv_rec_rm,
        [Description("% Achieved Level I Unsatisfactory--Phase-in 1 ")]
        unsat_ph1_rm,
        [Description("% Achieved Level I Unsatisfactory--Phase-in 2")]
        unsat_ph2_rm,
        [Description("% Achieved Level I Unsatisfactory--Final Recommended ")]
        unsat_rec_rm,
        [Description("% Achieved Level I Unsatisfactory Minimum--Phase-in 1 ")]
        min_ph1_rm,
        [Description("% Achieved Level I Unsatisfactory Minimum--Phase-in 2 ")]
        min_ph2_rm,
        [Description("% Achieved Level I Unsatisfactory Minimum--Final Recommended ")]
        min_rec_rm,
        [Description("Average Scale Score")]
        rs,
        [Description("# Avg Items Correct--Reporting Category 1 ")]
        avg_cat1,
        [Description("% Avg Items Correct--Reporting Category 1")]
        pct_cat1,
        [Description("% Avg Items Correct--Reporting Category 2")]
        pct_cat2,
        [Description("# Avg Items Correct--Reporting Category 2 --")]
        avg_cat2,
        [Description("# Avg Items Correct--Reporting Category 3")]
        avg_cat3,
        [Description("% Avg Items Correct--Reporting Category 3")]
        pct_cat3,
        [Description("% Avg Items Correct--Reporting Category 4")]
        pct_cat4,
        [Description("# Avg Items Correct--Reporting Category 4")]
        avg_cat4,
        [Description("# Avg Items Correct--Reporting Category 5 ")]
        avg_cat5,
        [Description("% Avg Items Correct--Reporting Category 5 ")]
        pct_cat5,

    }
}
