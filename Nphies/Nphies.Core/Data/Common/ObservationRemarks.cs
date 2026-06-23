using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nphies.Core.Data.Common
{
    public enum ObservationParameterType // select * from RCM_Parameter where ParameterGroup = 7 and ParameterType = 396
    {
        Text = 1,
        Result = 2,
        LOINC = 3,
        Grouping = 4,
        Financial = 5,
        File = 6,
        eRx = 7,
        UniversalDental = 8,
        ROM = 9
    }
    public enum TextCodes  //select* from RCM_Parameter where ParameterGroup = 7 and ParameterType = 397
    {
        Description = 1,        
        Modifier = 2,             
        PresentingComplaint = 3, 
        NonStandardCode = 4,     
        Duration = 5,            
        EncounterID = 6,        
        InvoiceDate = 7,       
        InvoiceNumber = 8      
    }
  // select* from RCM_Parameter where ParameterGroup = 7 and ParameterType = 398
    public enum ResultCodes
    {
        A1C = 1,
        BPD = 2,
        BPS = 3,
        CHOL = 4,
        CR = 5,
        HDL = 6,
        LDL = 7,
       
        UA = 9,

        ALT = 10,
        FBS = 11,
        AST = 12,
        TRIG = 8,
        Albumin = 13,
        ALP = 14,
        TotalProtein = 15,
        DirectBilirubin = 16,
        TotalBilirubin = 17,
        NONHDL = 18,
        RBS = 19

    }
    public enum LOINCCodes       //ParameterType = 399
    {
        LOINC = 1
    }
    public enum GroupingCodes       //ParameterType = 400
    {
        BundleID = 10,
        PackageID=	1
    }
    public enum FinancialCodes   //ParameterType = 401
    {
        ActivityGross = 1,
        PSCoPayment = 2,
        PSDeductible = 3,
        PSOutOfPocket = 4,
        ActivityPatientShare = 5,
        DRGTotalPayment = 6,
        DRGInlierPayment = 7
    }
    public enum FileCodes       //ParameterType = 402
    {
        File = 1
    }
    public enum eRxReferenceCodes       //ParameterType = 403
    {
        eRxReference = 1
    }
    //ParameterType = 404
    public enum UniversalDental
    {
        Code1 = 1,
        Code2 = 2,
        Code3 = 3,
        Code4 = 4,
        Code5 = 5,
        Code6 = 6,
        Code7 = 7,
        Code8 = 8,
        Code9 = 9,
        Code10 = 10,
        Code11 = 11,
        Code12 = 12,
        Code13 = 13,
        Code14 = 14,
        Code15 = 15,
        Code16 = 16,
        Code17 = 17,
        Code18 = 18,
        Code19 = 19,
        Code20 = 20,
        Code21 = 21,
        Code22 = 22,
        Code23 = 23,
        Code24 = 24,
        Code25 = 25,
        Code26 = 26,
        Code27 = 27,
        Code28 = 28,
        Code29 = 29,
        Code30 = 30,
        Code31 = 31,
        Code32 = 32,
        A = 33,
        B = 34,
        C = 35,
        D = 36,
        E = 37,
        F = 38,
        G = 39,
        H = 40,
        I = 41,
        J = 42,
        K = 43,
        L = 44,
        M = 45,
        N = 46,
        O = 47,
        P = 48,
        Q = 49,
        R = 50,
        S = 51,
        T = 52
    }

    //ParameterType = 405
    public enum ROMCodes
    {
        ROMCode =1
    }
    // select* from RCM_Parameter where ParameterGroup = 7 and ParameterType = 406 and ParameterCode = ?
    // where ? is the parameter code from 2nd menu 
    public enum ValueTypeCodes
    {
        Other = 1,
        ModifierCode = 2,
        Days = 3,
        Text = 4,
        Percent = 5,
        mmHg = 6,
        mmolL = 7,
        µmolL = 8,
        mg = 9,
        LOINCcode = 10,
        Float = 11,
        File = 12,
        Reference = 13,
        ToothNumber = 14,
        RiskOfMortality = 15
    }

}
