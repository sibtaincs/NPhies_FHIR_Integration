//using Cyclus.UnLock.Claims.Data;
//using Cyclus.UnLock.Claims.Enums;
//using Cyclus.UnLock.Claims.Models;
using Newtonsoft.Json.Linq;
using Nphies.Core.Data.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Nphies.Core.Data.Common
{
    public static class ObservationCreator
    {
        public static readonly string[] LabCPTCodes = new string[]
        {
        "83036",
        "80061",
        "82465",
        "83718",
        "83721",
        "84478",
        "82043",
        "82565",
        "82040",
        "84075",
        "84078",
        "84080",
        "84460",
        "84450",
        "82947",
        "87635-CASH",
        "87635",
        "87899-CASH",
        "80076"
        };

        public static readonly ArrayList alLabCPTCodes82043 = new ArrayList();
        public static readonly ArrayList alLabCPTCodes82565 = new ArrayList();
        public static readonly ArrayList alLabCPTCodes82947 = new ArrayList();
        public static readonly ArrayList alLabCPTCodes87635 = new ArrayList();
        public static readonly ArrayList alLabCPTCodes80061 = new ArrayList();
        public static readonly ArrayList alLabCPTCodes80076 = new ArrayList();
        public static readonly ArrayList arrayListLabTestForDRGChanges = new ArrayList();

        public static readonly Hashtable cptTestPackage = new Hashtable();


        static ObservationCreator()
        {
            arrayListLabTestForDRGChanges = new ArrayList {"12","32","38","44","50","56","57","58","144","209","219","222","229","237","239","244","263","265","266","311","331",
                                    "334","335","338","340","341","342","343","344","347","348","349","351","364","366","367","370","392","406","412","413","414","420","423","424","428","430","436","441","450","454","456",
                                    "474","497","508","510","511","519","527","529","530","530","568","579","584","605","619","622","634","635","638","641","642","646","653","665","667","681","682","697","709","710","795",
                                    "796","803","808","810","815","819","827","829","874","901","919","931","932","934","938","946","959","961","985","995","1003","1036","1041","1049","1058","1059","1060","1080","1082",
                                    "1083","1084","1090","1091","1092","1094","1103","1104","1121","1135","1142","1144","1150","1155","1166","1168","1169","1174","1176","1183","1189","1190","1191","1192","1199",
                                    "1204","1207","1208","1209","1213","1214","1217","1228","1231","1232","1250","1258","1264","1266","1274","1282","1291","1314","1318","1330","1337","1338","1343","1344","1351",
                                    "1364","1383","1414","1419","1428","1435","1436","1443","1444","1457","1458","1459","1468","1488","1488","1494","1504","1514","1521","1545","1568","1576","1591","1603","1610",
                                    "1615","1625","1698","1706","1707","1717","1724","1727","1731","1755","1757","1763","1764","1765","1769","1771","1777","1787","1810","1824","1844","1846","1853","1856","1861",
                                    "1878","1884","1891","1895","1897","1900","1911","1914","1924","1927","1934","1962","1963","1964","1965","1968","1969","1970","1977","2017","2018","2028","2049","2054","2070",
                                    "2073","2090","2090","2090","2099","2100","2130","2143","2152","2153","2170" };

            // Initializing ArrayLists with values
            alLabCPTCodes82043.Clear();
            alLabCPTCodes82565.Clear();
            alLabCPTCodes82947.Clear();
            alLabCPTCodes87635.Clear();
            alLabCPTCodes80061.Clear();
            alLabCPTCodes80076.Clear();

            alLabCPTCodes82043.Add("697");
            alLabCPTCodes82043.Add("1282");

            alLabCPTCodes82565.Add("144");
            alLabCPTCodes82565.Add("1444");

            alLabCPTCodes82947.Add("508");
            alLabCPTCodes82947.Add("819");
            alLabCPTCodes82947.Add("946");

            alLabCPTCodes87635.Add("2133");
            alLabCPTCodes87635.Add("2152");

            alLabCPTCodes80061.Add("56");
            alLabCPTCodes80061.Add("1824");

            alLabCPTCodes80076.Add("44");
            alLabCPTCodes80076.Add("1826");

            cptTestPackage.Add("83036", "568");
            cptTestPackage.Add("80061", "56");
            cptTestPackage.Add("82565", "144");
            cptTestPackage.Add("83718", "589");
            cptTestPackage.Add("83721", "667");
            cptTestPackage.Add("84478", "931");
            cptTestPackage.Add("82465", "401");
            cptTestPackage.Add("82040", "209");
            cptTestPackage.Add("84075", "222");
            cptTestPackage.Add("84080", "1453");
            cptTestPackage.Add("84460", "229");
            cptTestPackage.Add("84450", "265");
            cptTestPackage.Add("82947", "819");
        }

        public static ArrayList GetLabCPTCodesByActivityCode(string activityCode)
        {
            ArrayList arlTestId = new ArrayList();
            arlTestId.Clear();

            switch (activityCode)
            {
                case "82043":
                    arlTestId = alLabCPTCodes82043;
                    break;
                case "82565":
                    arlTestId = alLabCPTCodes82565;
                    break;
                case "82947":
                    arlTestId = alLabCPTCodes82947;
                    break;
                case "87635":
                    arlTestId = alLabCPTCodes87635;
                    break;
                case "80061":
                    arlTestId = alLabCPTCodes80061;
                    break;
                case "80076":
                    arlTestId = alLabCPTCodes80076;
                    break;
                default:
                    // Handle the case where activityCode does not match any of the predefined codes
                    break;
            }

            return arlTestId;
        }
        public static DataTable ConvertJArrayToDataTable(JArray jsonArray)
        {
            DataTable dataTable = new DataTable();

            if (jsonArray == null || jsonArray.Count == 0 || !jsonArray.HasValues)
                return dataTable;

            // Create columns based on JObject properties
            foreach (JProperty column in jsonArray[0].Children<JProperty>())
            {
                dataTable.Columns.Add(column.Name, typeof(string));
            }

            // Add rows to the DataTable
            foreach (JObject obj in jsonArray.Children<JObject>())
            {
                DataRow dataRow = dataTable.NewRow();
                foreach (JProperty property in obj.Properties())
                {
                    dataRow[property.Name] = property.Value.ToString();
                }
                dataTable.Rows.Add(dataRow);
            }

            return dataTable;
        }
        public static string GetActivityCode(string standardCode, bool replaceVidaCode=false)
        {
            var finalActivityCode = "";
            if (replaceVidaCode)
                finalActivityCode = standardCode;
            else
            {
                string switchcode = standardCode != null && standardCode.Length > 1 ? standardCode.Substring(0, 1) : "0";
                switch (switchcode)
                {
                    case "D":
                       finalActivityCode = standardCode;
                        break;
                    case "+":
                        // activity.Type = 4; //HCPCS
                       finalActivityCode = standardCode.Substring(1);
                        break;

                    default:

                       finalActivityCode = standardCode;
                        break;

                }
            }
            finalActivityCode = finalActivityCode?.ToString().Trim();
            var Type = GetActivityTypeAsperCodeFormat(Convert.ToString(finalActivityCode));//by vikash
            if (Type != 5)
            {
                if (finalActivityCode?.ToString().Contains("-") ?? false)
                {
                    finalActivityCode = finalActivityCode.ToString().Split('-')[0].ToString();
                }
            }

            return finalActivityCode;
        }
        public static List<DHPO_Observation> FillObservation(RcmClaimServicesDetail dataRow, DataTable labresult, string ActivityCode)
        {
            //FORMART THE STANDARD CODE OF SERVICE
            ActivityCode = GetActivityCode(ActivityCode);

            DataTable dtLabProcedure = new DataTable();
            List<DHPO_Observation> observationList = new List<DHPO_Observation>();
            var arlTestId = GetLabCPTCodesByActivityCode(ActivityCode);

                if (arlTestId.Count != 0)
            {
                foreach (string strTestId in arlTestId)
                {

                    dtLabProcedure = GetLabResult(labresult, Convert.ToInt32(strTestId == null ? "0" : strTestId), Convert.ToInt32(dataRow.ServiceReferenceNumber));
                    if (dtLabProcedure.Rows.Count == 0 && ActivityCode == "82043")
                        dtLabProcedure = GetLabResult(labresult, 1266, Convert.ToInt32(dataRow.ServiceReferenceNumber));


                    try
                    {

                        if (ActivityCode == "80061" && dtLabProcedure != null && dtLabProcedure.Rows.Count == 4)
                        {

                            string CHOLValue = dtLabProcedure.AsEnumerable().Where(p => p.Field<string>("ProcedureID") == "CHOL").Select(v => v.Field<string>("InternalProcedureID")).FirstOrDefault();
                            string HDLValue = dtLabProcedure.AsEnumerable().Where(p => p.Field<string>("ProcedureID") == "HDL").Select(v => v.Field<string>("InternalProcedureID")).FirstOrDefault();

                            DataRow row = dtLabProcedure.NewRow();
                            row["ProcedureID"] = "NONHDL";
                            row["OrderDate"] = Convert.ToDateTime(dtLabProcedure.Rows[0]["OrderDate"]);
                            row["LabPerformDate"] = Convert.ToDateTime(dtLabProcedure.Rows[0]["LabPerformDate"]);
                            row["ResultValueFlag"] = (dtLabProcedure.Rows[0]["ResultValueFlag"]);
                            row["PROCID"] = (dtLabProcedure.Rows[0]["PROCID"]);
                            row["InternalProcedureID"] = Math.Round(((Convert.ToDouble(String.IsNullOrEmpty(CHOLValue) ? "0" : CHOLValue)) - (Convert.ToDouble(String.IsNullOrEmpty(HDLValue) ? "0" : HDLValue))), 2);
                            dtLabProcedure.Rows.Add(row);
                        }


                    }
                    catch
                    {
                    }
                         Activity activity = new Activity(ActivityCode);
                    DHPO_Observation abnormalObservation = null;
                    for (int i = 0; i < dtLabProcedure.Rows.Count; i++)
                    {
                   
                        observationList.Add( FillLabObservation(activity, dtLabProcedure.Rows[i],out abnormalObservation));
                        if (abnormalObservation != null)
                        {
                            observationList.Add(abnormalObservation);
                        }
                    }


                }
                return observationList;
            }
            else
            {

                dtLabProcedure = GetLabResult(labresult, Convert.ToInt32(cptTestPackage[ActivityCode] == null ? "0" : cptTestPackage[ActivityCode]), Convert.ToInt32(dataRow.ServiceReferenceNumber));
                if (dtLabProcedure.Rows.Count == 0 && ActivityCode == "82043")
                    dtLabProcedure = GetLabResult(labresult, 1266, Convert.ToInt32(dataRow.ServiceReferenceNumber));

                Activity activity = new Activity(ActivityCode);
                DHPO_Observation abnormalObservation = null;
                for (int i = 0; i < dtLabProcedure.Rows.Count; i++)
                {
                 
                    observationList.Add(FillLabObservation(activity, dtLabProcedure.Rows[i], out abnormalObservation));
                    if (abnormalObservation != null)
                    {
                        observationList.Add(abnormalObservation);
                    }
                }
                return observationList;
            }
        }

        public static DHPO_Observation FillLabObservation(Activity activity, DataRow dataRow,out DHPO_Observation abnormalObservation)
        {
            bool RemoveZero  = false;
            abnormalObservation = null;
            try
            {
                if (dataRow["ProcedureID"]?.ToString()== "NON-HDL CH")
                {
                    dataRow["ProcedureID"] = "NONHDL";
                }
                string InternalProcedureID = dataRow["InternalProcedureID"].ToString();
                if (RemoveZero && InternalProcedureID.StartsWith("0"))
                    InternalProcedureID = InternalProcedureID.Substring(1);

                string ResultedOn = Convert.ToDateTime(dataRow["LabPerformDate"]).ToString("dd/MM/yyyy");

                if (activity.Code == "82947")
                {
                    if (Convert.ToString(dataRow["ProcedureID"].ToString()) == "FBS")//added by vikash 
                    {
                        DHPO_Observation observationFBS = new DHPO_Observation();
                        observationFBS.Type = (short)ObservationParameterType.Result;//"Result";
                        observationFBS.Code = (short)ResultCodes.FBS;// "FBS";
                        observationFBS.Value = InternalProcedureID;
                        try
                        {
                            observationFBS.Value = Math.Ceiling(Convert.ToDecimal(InternalProcedureID)).ToString();//Add value to value tag
                            string observationValue = observationFBS.Value;
                            GetObservationMinandMaxValue(activity.Code, ResultCodes.FBS.ToString(), ref observationValue);
                            observationFBS.Value = observationValue;
                        }
                        catch
                        {
                            observationFBS.Value = InternalProcedureID;
                        }
                        observationFBS.ValueType = ResultedOn;//Add value Value Type to tag
                        return observationFBS;
           
                    }
                    if (Convert.ToString(dataRow["ProcedureID"].ToString()) == "RBS")//added by vikash 
                    {
                        DHPO_Observation observationRBS = new DHPO_Observation();
                        observationRBS.Type = (short)ObservationParameterType.Result;
                        observationRBS.Code = (short)ResultCodes.RBS;// "RBS";
                        try
                        {
                            observationRBS.Value = Math.Ceiling(Convert.ToDecimal(InternalProcedureID)).ToString();//Add value to value tag                      
                            string observationValue = observationRBS.Value;
                            GetObservationMinandMaxValue(activity.Code, ResultCodes.RBS.ToString(), ref observationValue);
                            observationRBS.Value = observationValue;
                        }
                        catch
                        {
                            observationRBS.Value = InternalProcedureID;
                        }
                        observationRBS.ValueType = ResultedOn;//Add value Value Type to tag

                        //observationList.Add(observationRBS);
                        return observationRBS;
                        // activity.Observation.Add(observationRBS);


                    }
                }

                if (Convert.ToString(dataRow["ResultValueFlag"].ToString()) == "H" || Convert.ToString(dataRow["ResultValueFlag"].ToString()) == "L") //Abnormal Lab Result(resultvalueflag = 'H' or 'L'
                {
                    DHPO_Observation abnormalObservaltion = new DHPO_Observation();
                    abnormalObservaltion.Type = (short)ObservationParameterType.Text;
                    abnormalObservaltion.Code = (short)TextCodes.Description;
                    try
                    {
                        abnormalObservaltion.Value = "Abnormal Test Result:" + Math.Ceiling(Convert.ToDecimal(InternalProcedureID)).ToString() + " " + dataRow["InternalProcedureName"].ToString() + "( Ref. Range " + dataRow["ReferenceRange"].ToString() + " " + dataRow["InternalProcedureName"].ToString() + ")";
                    }
                    catch
                    {

                        abnormalObservaltion.Value = "Abnormal Test Result:" + InternalProcedureID + " " + dataRow["InternalProcedureName"].ToString() + "( Ref. Range " + dataRow["ReferenceRange"].ToString() + " " + dataRow["InternalProcedureName"].ToString() + ")";
                    }
                    abnormalObservaltion.ValueType = ((short)ValueTypeCodes.Other).ToString();
                     //observationList.Add(abnormalObservaltion);
                    abnormalObservation= abnormalObservaltion;
                    //activity.Observation.Add(abnormalObservaltion);
                }

             

                if (activity.Code == "83036")
                {
                    DHPO_Observation observation = new DHPO_Observation();
                    observation.Type = (short)ObservationParameterType.Result;
                    observation.Code = (short)ResultCodes.A1C;//"A1C";
                    observation.Value = InternalProcedureID;
                    try
                    {
                        string observationValue = observation.Value;
                        GetObservationMinandMaxValue(activity.Code, ResultCodes.A1C.ToString(), ref observationValue);
                        observation.Value = observationValue;
                    }
                    catch
                    {
                        observation.Value = InternalProcedureID;
                    }
                    observation.ValueType = ResultedOn;
                    return observation;
                    // activity.Observation.Add(observation);
                }
                if (activity.Code == "82465")
                {
                    DHPO_Observation observation = new DHPO_Observation();
                    observation.Type = (short)ObservationParameterType.Result;
                    observation.Code = (short)ResultCodes.CHOL;//"CHOL";
                    observation.Value = InternalProcedureID;
                    observation.ValueType = ResultedOn;
                    return observation;
                    // activity.Observation.Add(observation);
                }
                if (activity.Code == "83718")
                {
                    DHPO_Observation observation = new DHPO_Observation();
                    observation.Type = (short)ObservationParameterType.Result;
                    observation.Code =  (short)ResultCodes.HDL;// "HDL";
                    observation.Value = InternalProcedureID;
                    observation.ValueType = ResultedOn;
                    return observation;
                    // activity.Observation.Add(observation);
                }
                if (activity.Code == "83721")
                {
                    DHPO_Observation observation = new DHPO_Observation();
                    observation.Type = (short)ObservationParameterType.Result;
                    observation.Code = (short)ResultCodes.LDL; //"LDL";
                    observation.Value = InternalProcedureID;
                    observation.ValueType = ResultedOn;
                    return observation;
                    //   activity.Observation.Add(observation);
                }
                if (activity.Code == "84478")
                {
                    DHPO_Observation observation = new DHPO_Observation();
                    observation.Type = (short)ObservationParameterType.Result;
                    observation.Code = (short)ResultCodes.TRIG;//"TRIG";
                    observation.Value = InternalProcedureID;
                    observation.ValueType = ResultedOn;
                    return observation;
                    //  activity.Observation.Add(observation);
                }
                if (activity.Code == "82043")
                {
                    DHPO_Observation observation = new DHPO_Observation();
                    observation.Type = (short)ObservationParameterType.Result;
                    observation.Code = (short)ResultCodes.UA;

                    

                    string cleanedValue = Regex.Replace(InternalProcedureID ?? string.Empty, @"[^0-9.]+", "");
                    float value = float.TryParse(cleanedValue, out float result) ? result : 0;

                    //float value = !string.IsNullOrEmpty(InternalProcedureID) ? float.Parse(InternalProcedureID) : 0;//Added by Shahabudeen on 26-Mar-2019 for data conversion issue.
                    if (value <= 30)
                    {
                        observation.Value = "1";
                    }
                    else if (value >= 31 && value <= 299)
                    {
                        observation.Value = "2";
                    }
                    else
                    {
                        observation.Value = "3";
                    }

                    observation.ValueType = ResultedOn;
                    return observation;
                    // activity.Observation.Add(observation);
                }
                if (activity.Code == "82565")
                {
                    DHPO_Observation observation = new DHPO_Observation();
                    observation.Type = (short)ObservationParameterType.Result;
                    observation.Code = (short)ResultCodes.CR;//"CR";
                    observation.Value = InternalProcedureID;
                    try
                    {
                        try
                        {
                            string observationValue = observation.Value;
                            GetObservationMinandMaxValue(activity.Code, ResultCodes.CR.ToString(), ref observationValue);
                            observation.Value = observationValue;
                        }
                        catch
                        {
                        }
                    }
                    catch
                    {
                        observation.Value = InternalProcedureID;
                    }

                    observation.ValueType = ResultedOn;
                    return observation;
                    //   activity.Observation.Add(observation);
                }
                if (activity.Code == "82040")
                {
                    DHPO_Observation observation = new DHPO_Observation();
                    observation.Type = (short)ObservationParameterType.Result;
                    observation.Code = (short)ResultCodes.Albumin;//"Albumin";
                    observation.Value = InternalProcedureID;
                    observation.ValueType = ResultedOn;
                    return observation;
                    //  activity.Observation.Add(observation);
                }
                if (activity.Code == "84075")
                {
                    DHPO_Observation observation = new DHPO_Observation();
                    observation.Type = (short)ObservationParameterType.Result;
                    observation.Code = (short)ResultCodes.ALP;//"ALP";
                    observation.Value = InternalProcedureID;
                    observation.ValueType = ResultedOn;
                    return observation;
                    // activity.Observation.Add(observation);
                }
                if (activity.Code == "84078")
                {
                    DHPO_Observation observation = new DHPO_Observation();
                    observation.Type = (short)ObservationParameterType.Result;
                    observation.Code = (short)ResultCodes.ALP;//"ALP";
                    observation.Value = InternalProcedureID;
                    observation.ValueType = ResultedOn;
                    return observation;
                    //  activity.Observation.Add(observation);
                }
                if (activity.Code == "84080")
                {
                    DHPO_Observation observation = new DHPO_Observation();
                    observation.Type = (short)ObservationParameterType.Result;
                    observation.Code = (short)ResultCodes.ALP;//"ALP";
                    observation.Value = InternalProcedureID;
                    observation.ValueType = ResultedOn;
                    return observation;
                    //  activity.Observation.Add(observation);
                }
                if (activity.Code == "84460")
                {
                    DHPO_Observation observation = new DHPO_Observation();
                    observation.Type = (short)ObservationParameterType.Result;
                    observation.Code = (short)ResultCodes.ALT;//"ALT";
                    observation.Value = InternalProcedureID;
                    observation.ValueType = ResultedOn;
                    return observation;
                    //  activity.Observation.Add(observation);
                }
                if (activity.Code == "84450")
                {
                    DHPO_Observation observation = new DHPO_Observation();
                    observation.Type = (short)ObservationParameterType.Result;
                    observation.Code = (short)ResultCodes.AST;//"AST";
                    observation.Value = InternalProcedureID;
                    observation.ValueType = ResultedOn;
                    return observation;  //  activity.Observation.Add(observation);
                }

                if (activity.Code == "80061")//by vikash on 2 may 2021
                {
                    DHPO_Observation observation = new DHPO_Observation();
                    observation.Type = (short)ObservationParameterType.Result;
                    switch (dataRow["PROCID"] == DBNull.Value ? "" : dataRow["PROCID"].ToString())
                    {
                        case "401":
                            if (dataRow[0].ToString() != "NONHDL")
                            {
                                observation.Code = (short)ResultCodes.CHOL;// "Albumin";
                                observation.Value = InternalProcedureID;
                                try
                                {
                                    string observationValue = observation.Value;
                                    GetObservationMinandMaxValue(activity.Code, ResultCodes.CHOL.ToString(), ref observationValue);
                                    observation.Value = observationValue;
                                }
                                catch
                                {
                                    observation.Value = InternalProcedureID;
                                }
                            }
                            else
                            {
                                observation.Code = (short)ResultCodes.NONHDL;// "Albumin";
                                observation.Value = InternalProcedureID;
                                try
                                {
                                    string observationValue = observation.Value;
                                    GetObservationMinandMaxValue(activity.Code, ResultCodes.NONHDL.ToString(), ref observationValue);
                                    observation.Value = observationValue;
                                }
                                catch
                                {
                                    observation.Value = InternalProcedureID;
                                }
                            }
                            break;
                        case "589":
                            observation.Code = (short)ResultCodes.HDL;// "Albumin";
                            observation.Value = InternalProcedureID;
                            try
                            {
                                string observationValue = observation.Value;
                                GetObservationMinandMaxValue(activity.Code, ResultCodes.HDL.ToString(), ref observationValue);
                                observation.Value = observationValue;
                            }
                            catch
                            {
                                observation.Value = InternalProcedureID;
                            }
                            break;
                        case "931":
                            observation.Code = (short)ResultCodes.TRIG;// "Albumin";
                            observation.Value = InternalProcedureID;
                            try
                            {
                                string observationValue = observation.Value;
                                GetObservationMinandMaxValue(activity.Code, ResultCodes.TRIG.ToString(), ref observationValue);
                                observation.Value = observationValue;
                            }
                            catch
                            {
                                observation.Value = InternalProcedureID;
                            }
                            break;
                        case "667":
                            observation.Code = (short)ResultCodes.LDL;// "Albumin";
                            observation.Value = InternalProcedureID;
                            try
                            {
                                string observationValue = observation.Value;
                                GetObservationMinandMaxValue(activity.Code, ResultCodes.LDL.ToString(), ref observationValue);
                                observation.Value = observationValue;
                            }
                            catch
                            {
                                observation.Value = InternalProcedureID;
                            }
                            break;
                        default:
                            if (dataRow["ProcedureID"] != DBNull.Value && dataRow["ProcedureID"].ToString() == "NONHDL")
                            {
                                observation.Code = (short)ResultCodes.NONHDL;// "Albumin";
                                observation.Value = InternalProcedureID;
                                try
                                {
                                    string observationValue = observation.Value;
                                    GetObservationMinandMaxValue(activity.Code, ResultCodes.NONHDL.ToString(), ref observationValue);
                                    observation.Value = observationValue;
                                }
                                catch
                                {
                                    observation.Value = InternalProcedureID;
                                }
                            }

                            break;
                       
                    }
                    //observation.Code = (short)dataRow["PROCID"].ToString();
                    //observation.Value = InternalProcedureID;
                    //try
                    //{
                    //    string observationValue = observation.Value;
                    //    GetObservationMinandMaxValue(activity.Code, Convert.ToString(observation.Code) , ref observationValue);
                    //    observation.Value = observationValue;

                    //}
                    //catch
                    //{
                    //    observation.Value = InternalProcedureID;
                    //}

                    observation.ValueType = ResultedOn;
                    return observation;
                    //activity.Observation.Add(observation);
                }
                if (activity.Code == "80076")//by vikash on 2 may 2021
                {
                    DHPO_Observation observation = new DHPO_Observation();
                    observation.Type = (short)ObservationParameterType.Result;
                    switch (dataRow["PROCID"] == DBNull.Value ? "" : dataRow["PROCID"].ToString())
                    {
                        case "209":
                            observation.Code = (short)ResultCodes.Albumin;// "Albumin";
                            observation.Value = InternalProcedureID;
                            try
                            {
                                string observationValue = observation.Value;
                                GetObservationMinandMaxValue(activity.Code, ResultCodes.Albumin.ToString(), ref observationValue);
                                observation.Value = observationValue;
                            }
                            catch
                            {
                                observation.Value = InternalProcedureID;
                            }
                            break;
                        case "229":
                            observation.Code = (short)ResultCodes.ALT;// "ALT";
                            observation.Value = InternalProcedureID;
                            try
                            {
                                string observationValue = observation.Value;
                                GetObservationMinandMaxValue(activity.Code, ResultCodes.ALT.ToString(), ref observationValue);
                                observation.Value = observationValue;
                            }
                            catch
                            {
                                observation.Value = InternalProcedureID;
                            }
                            break;
                        case "222":
                            observation.Code = (short)ResultCodes.ALP;// "ALP";
                            observation.Value = InternalProcedureID;
                            try
                            {
                                string observationValue = observation.Value;
                                GetObservationMinandMaxValue(activity.Code, ResultCodes.ALP.ToString(), ref observationValue);
                                observation.Value = observationValue;
                            }
                            catch
                            {
                                observation.Value = InternalProcedureID;
                            }
                            break;
                        case "265":
                            observation.Code = (short)ResultCodes.AST;// "AST";
                            observation.Value = InternalProcedureID;
                            try
                            {
                                string observationValue = observation.Value;
                                GetObservationMinandMaxValue(activity.Code, ResultCodes.AST.ToString(), ref observationValue);
                                observation.Value = observationValue;
                            }
                            catch
                            {
                                observation.Value = InternalProcedureID;
                            }
                            break;
                        case "903":
                            observation.Code = (short)ResultCodes.TotalProtein;// "TotalProtein";
                            observation.Value = InternalProcedureID;
                            try
                            {
                                string observationValue = observation.Value;
                                GetObservationMinandMaxValue(activity.Code, ResultCodes.TotalProtein.ToString(), ref observationValue);
                                observation.Value = observationValue;
                            }
                            catch
                            {
                                observation.Value = InternalProcedureID;
                            }
                            break;
                        case "456":
                            observation.Code = (short)ResultCodes.DirectBilirubin;// "DirectBilirubin";
                            observation.Value = InternalProcedureID;
                            try
                            {
                                string observationValue = observation.Value;
                                GetObservationMinandMaxValue(activity.Code, ResultCodes.DirectBilirubin.ToString(), ref observationValue);
                                observation.Value = observationValue;
                            }
                            catch
                            {
                                observation.Value = InternalProcedureID;
                            }
                            break;
                        case "901":
                            observation.Code = (short)ResultCodes.TotalBilirubin;// "TotalBilirubin";
                            observation.Value = InternalProcedureID;
                            try
                            {
                                string observationValue = observation.Value;
                                GetObservationMinandMaxValue(activity.Code, ResultCodes.TotalBilirubin.ToString(), ref observationValue);
                                observation.Value = observationValue;
                            }
                            catch
                            {
                            }
                            break;
                        //default:
                        //    observation.Code = Convert.ToString(dataRow["ProcedureID"].ToString());
                        //    observation.Value = InternalProcedureID;
                        //    try
                        //    {
                        //        string observationValue = observation.Value;
                        //        GetObservationMinandMaxValue(activity.Code, observation.Code, ref observationValue);
                        //        observation.Value = observationValue;
                        //    }
                        //    catch
                        //    {
                        //    }
                        //    break;
                    }
                    //observation.Code = Convert.ToString(dataRow["ProcedureID"].ToString()).StartsWith("T")?"TotalBilirubin": "DirectBilirubin";
                    //observation.Value = InternalProcedureID;
                    observation.ValueType = ResultedOn;
                    return observation;
                    //  activity.Observation.Add(observation);
                }


                bool IsCovidObservationExist = false;

                if (activity.Code == "87635" || activity.Code == "87635-CASH" || activity.Code == "87899-CASH") //FOR COVID //|| activity.Code == "87635 -CASH" || activity.Code == "87899-CASH"
                {
                    DHPO_Observation observation = new DHPO_Observation();
                    observation.Type = (short)ObservationParameterType.Text;
                    observation.Code = (short)TextCodes.Description;//"Description";
                    observation.Value = Convert.ToString(InternalProcedureID).ToString().ToUpper().StartsWith("DETECTED") || Convert.ToString(InternalProcedureID).ToString().ToUpper().StartsWith("POSITIVE") ? "Positive" : "Negative";
                    observation.ValueType = ((short)ValueTypeCodes.Other).ToString();
                 //   activity.Observation.Add(observation);

                    IsCovidObservationExist = true;
                    return observation;
                }


                if (!IsCovidObservationExist && activity.Code != "80061" && activity.Code != "80076")
                {
                    if (arrayListLabTestForDRGChanges.Contains(Convert.ToString((dataRow["PROCID"] == DBNull.Value ? 0 : dataRow["PROCID"]))))
                    {
                        DHPO_Observation otherObservation = new DHPO_Observation();
                        otherObservation.Type = (short)ObservationParameterType.Text;
                        otherObservation.Code = (short)TextCodes.Description;
                        otherObservation.Value = dataRow["InternalProcedureID"].ToString();
                        otherObservation.ValueType = ((short)ValueTypeCodes.Other).ToString();
                       

                        try
                        {
                            string observationValue = otherObservation.Value;
                            GetObservationMinandMaxValue(activity.Code, Convert.ToString(dataRow["ProcedureID"].ToString()), ref observationValue);
                            otherObservation.Value = observationValue;
                            return otherObservation;
                           // observationList.Add(otherObservation);
                        }
                        catch
                        { }

                        if (Convert.ToString(dataRow["ResultValueFlag"].ToString()) == "H" || Convert.ToString(dataRow["ResultValueFlag"].ToString()) == "L" || activity.Code == "82947")
                        {
                            try
                            {
                                otherObservation.Value = Math.Ceiling(Convert.ToDecimal(otherObservation.Value)).ToString();
                                return otherObservation;
                                //observationList.Add(otherObservation);
                            }
                            catch
                            {
                            }
                        }


                      //  activity.Observation.Add(otherObservation);
                    }
                }

                return new DHPO_Observation();

            }


            catch (Exception ex)
            {
                throw new Exception( $" {ex.Message}", ex);
            }

        }

        private static DataTable GetLabResult(DataTable dtLabResults, int PackageID, int InvoiceNo)
        {
            DataTable dt = new DataTable();
            try
            {
                if (dtLabResults != null && dtLabResults.Rows.Count > 0)
                {
                    var datarows = dtLabResults.Select("packageID='" + PackageID + "' and  invoiceNo='" + InvoiceNo + "'");

                    dt.Columns.Add("ProcedureID", typeof(string));
                    dt.Columns.Add("InternalProcedureID", typeof(string));
                    dt.Columns.Add("OrderDate", typeof(DateTime));
                    dt.Columns.Add("LabPerformDate", typeof(DateTime));
                    dt.Columns.Add("ResultValueFlag", typeof(string));
                    dt.Columns.Add("PROCID", typeof(string));
                    dt.Columns.Add("InternalProcedureName", typeof(string));
                    dt.Columns.Add("ReferenceRange", typeof(string));
                    foreach (DataRow dr in datarows)
                    {

                        var dr1 = dt.NewRow();
                        dr1["ProcedureID"] = dr["TestCode"];
                        dr1["InternalProcedureID"] = dr["labResult"];
                        dr1["OrderDate"] = dr["OrderDate"];
                        dr1["LabPerformDate"] = dr["ResultReleaseDate"];
                        dr1["ResultValueFlag"] = dr["LabFlag"];
                        dr1["PROCID"] = dr["TestID"];
                        dr1["InternalProcedureName"] = dr["testName"];
                        dr1["ReferenceRange"] = dr["referenceRange"];
                        dt.Rows.Add(dr1);

                    }
                }
            }
            catch (Exception ex)
            {

            }
            return dt;
        }
        public static bool IsDigitsOnly(string str)
        {
            float f = 0;
            bool success = float.TryParse(str, out f);
            return success;
        }
        public static int GetActivityTypeAsperCodeFormat(string ActivityID)
        {
            int activityType = 3;
            try
            {

                if (ActivityID == null | ActivityID == string.Empty)
                {
                    return activityType;
                }
                else if (IsDigitsOnly(ActivityID) == true)
                {
                    string strVal = ActivityID.Replace(".", "");

                    if (strVal.Length == 5)
                    {
                        if (Convert.ToDouble(ActivityID) > 00000 && Convert.ToDouble(ActivityID) <= 99999)
                            return 3;
                    }
                    if (Convert.ToDouble(ActivityID) >= 1 && Convert.ToDouble(ActivityID) <= 100)
                    {
                        return 8;
                    }
                    if (ActivityID.Length == 6)
                    {
                        return 9;
                    }
                }
                else if (IsDigitsOnly(ActivityID) == false)
                {
                    if ((!(ActivityID.StartsWith("D"))) && !ActivityID.Contains("-"))//by vikash !start with D means A-Z except D
                    {
                        if (Regex.IsMatch(ActivityID, "^[a-c e-z]", RegexOptions.IgnoreCase))
                            return 4;
                    }
                    if (ActivityID.StartsWith("D") && !ActivityID.Contains("-"))
                    {
                        return 6;
                    }
                    if (ActivityID.Contains("-"))
                    {
                        if (Convert.ToString(ActivityID).Count(f => (f == '-')) == 2)
                            return 5;
                        else
                            return 3;
                    }
                }
                else
                {
                    return 3;
                }
            }
            catch (Exception ex)
            {
              
                return activityType;
            }
            return activityType;
        }

        public static void GetObservationMinandMaxValue(string ActivityCode, string ObservationCode, ref string Observationvalue)
        {
            ObservationCode = ObservationCode.ToUpper();
            #region SetMinAndMaxValuePerCPT
            string _83036_A1C_MIN = "4.0";
            string _83036_A1C_MAX = "20.0";

            string _80061_CHOL_MIN = "100.0";
            string _80061_CHOL_MAX = "1000.0";

            string _80061_HDL_MIN = "10.0";
            string _80061_HDL_MAX = "1000.0";

            string _80061_NONHDL_MIN = "50.0";
            string _80061_NONHDL_MAX = "850.0";

            string _80061_LDL_MIN = "40.0";
            string _80061_LDL_MAX = "800.0";

            string _80061_TRIG_MIN = "50.0";
            string _80061_TRIG_MAX = "1500.0";

            string _82565_CR_MIN = "0.4";
            string _82565_CR_MAX = "12.0";

            string _80076_Albumin_MIN = "2.5";
            string _80076_Albumin_MAX = "6.5";

            string _80076_ALP_MIN = "35.0";
            string _80076_ALP_MAX = "500.0";

            string _80076_ALT_MIN = "5.0";
            string _80076_ALT_MAX = "400.0";

            string _80076_AST_MIN = "5.0";
            string _80076_AST_MAX = "500.0";

            string _80076_TotalBilirubin_MIN = "0.1";
            string _80076_TotalBilirubin_MAX = "500.0";

            string _80076_DirectBilirubin_MIN = "0.1";
            string _80076_DirectBilirubin_MAX = "400.0";

            string _80076_TotalProtein_MIN = "1.0";
            string _80076_TotalProtein_MAX = "300.0";

            string _82947_FBS_MIN = "20";
            string _82947_FBS_MAX = "1000";

            string _82947_RBS_MIN = "20";
            string _82947_RBS_MAX = "1000";

            string BPS_MIN = "60";
            string BPS_MAX = "240";

            string BPD_MIN = "40";
            string BPD_MAX = "180";

            string BMI_MIN = "14.0";
            string BMI_MAX = "70.0";

            #endregion

            #region Vital Sign Min and Max check and set
            if (ActivityCode == "" && (ObservationCode == "BPS" || ObservationCode == "BPD" || ObservationCode == "BMI"))
            {
                if (ObservationCode.ToUpper() == "BPS" && Convert.ToDouble(Observationvalue) < Convert.ToDouble(BPS_MIN))
                {
                    Observationvalue = BPS_MIN;
                }
                else if (ObservationCode.ToUpper() == "BPS" && Convert.ToDouble(Observationvalue) > Convert.ToDouble(BPS_MAX))
                {
                    Observationvalue = BPS_MAX;
                }

                if (ObservationCode.ToUpper() == "BPD" && Convert.ToDouble(Observationvalue) < Convert.ToDouble(BPD_MIN))
                {
                    Observationvalue = BPD_MIN;
                }
                else if (ObservationCode.ToUpper() == "BPD" && Convert.ToDouble(Observationvalue) > Convert.ToDouble(BPD_MAX))
                {
                    Observationvalue = BPD_MAX;
                }

                if (ObservationCode.ToUpper() == "BMI" && Convert.ToDouble(Observationvalue) < Convert.ToDouble(BMI_MIN))
                {
                    Observationvalue = BMI_MIN;
                }
                else if (ObservationCode.ToUpper() == "BMI" && Convert.ToDouble(Observationvalue) > Convert.ToDouble(BMI_MAX))
                {
                    Observationvalue = BMI_MAX;
                }
                Observationvalue = Math.Ceiling(Convert.ToDecimal(Observationvalue)).ToString();
            }
            #endregion

            switch (ActivityCode)
            {
                case "83036":
                    if (ObservationCode.ToUpper() == "A1C".ToUpper() && Convert.ToDouble(Observationvalue) < Convert.ToDouble(_83036_A1C_MIN))
                    {
                        Observationvalue = _83036_A1C_MIN;
                    }
                    else if (ObservationCode.ToUpper() == "A1C".ToUpper() && Convert.ToDouble(Observationvalue) > Convert.ToDouble(_83036_A1C_MAX))
                    {
                        Observationvalue = _83036_A1C_MAX;
                    }
                    break;
                case "80061":
                    if (ObservationCode.ToUpper() == "CHOL".ToUpper() && Convert.ToDouble(Observationvalue) < Convert.ToDouble(_80061_CHOL_MIN))
                    {
                        Observationvalue = _80061_CHOL_MIN;
                    }
                    else if (ObservationCode.ToUpper() == "CHOL".ToUpper() && Convert.ToDouble(Observationvalue) > Convert.ToDouble(_80061_CHOL_MAX))
                    {
                        Observationvalue = _80061_CHOL_MAX;
                    }

                    else if (ObservationCode.ToUpper() == "HDL".ToUpper() && Convert.ToDouble(Observationvalue) < Convert.ToDouble(_80061_HDL_MIN))
                    {
                        Observationvalue = _80061_HDL_MIN;
                    }
                    else if (ObservationCode.ToUpper() == "HDL".ToUpper() && Convert.ToDouble(Observationvalue) > Convert.ToDouble(_80061_HDL_MAX))
                    {
                        Observationvalue = _80061_HDL_MAX;
                    }

                    else if (ObservationCode.ToUpper() == "NONHDL".ToUpper() && Convert.ToDouble(Observationvalue) < Convert.ToDouble(_80061_NONHDL_MIN))
                    {
                        Observationvalue = _80061_NONHDL_MIN;
                    }
                    else if (ObservationCode.ToUpper() == "NONHDL".ToUpper() && Convert.ToDouble(Observationvalue) > Convert.ToDouble(_80061_NONHDL_MAX))
                    {
                        Observationvalue = _80061_NONHDL_MAX;
                    }

                    else if (ObservationCode.ToUpper() == "LDL".ToUpper() && Convert.ToDouble(Observationvalue) < Convert.ToDouble(_80061_LDL_MIN))
                    {
                        Observationvalue = _80061_LDL_MIN;
                    }
                    else if (ObservationCode.ToUpper() == "LDL".ToUpper() && Convert.ToDouble(Observationvalue) > Convert.ToDouble(_80061_LDL_MAX))
                    {
                        Observationvalue = _80061_LDL_MAX;
                    }

                    else if (ObservationCode.ToUpper() == "TRIG".ToUpper() && Convert.ToDouble(Observationvalue) < Convert.ToDouble(_80061_TRIG_MIN))
                    {
                        Observationvalue = _80061_TRIG_MIN;
                    }
                    else if (ObservationCode.ToUpper() == "TRIG".ToUpper() && Convert.ToDouble(Observationvalue) > Convert.ToDouble(_80061_TRIG_MAX))
                    {
                        Observationvalue = _80061_TRIG_MAX;
                    }
                    break;
                case "82043":
                    break;
                case "82565":
                    if (ObservationCode.ToUpper() == "CR".ToUpper() && Convert.ToDouble(Observationvalue) < Convert.ToDouble(_82565_CR_MIN))
                    {
                        Observationvalue = _82565_CR_MIN;
                    }
                    else if (ObservationCode.ToUpper() == "CR".ToUpper() && Convert.ToDouble(Observationvalue) > Convert.ToDouble(_82565_CR_MAX))
                    {
                        Observationvalue = _82565_CR_MAX;
                    }
                    break;
                case "80076":
                    if (ObservationCode.ToUpper() == "ALBUMIN".ToUpper() && Convert.ToDouble(Observationvalue) < Convert.ToDouble(_80076_Albumin_MIN))
                    {
                        Observationvalue = _80076_Albumin_MIN;
                    }
                    else if (ObservationCode.ToUpper() == "ALBUMIN".ToUpper() && Convert.ToDouble(Observationvalue) > Convert.ToDouble(_80076_Albumin_MAX))
                    {
                        Observationvalue = _80076_Albumin_MAX;
                    }
                    else if (ObservationCode.ToUpper() == "ALP".ToUpper() && Convert.ToDouble(Observationvalue) < Convert.ToDouble(_80076_ALP_MIN))
                    {
                        Observationvalue = _80076_ALP_MIN;
                    }
                    else if (ObservationCode.ToUpper() == "ALP".ToUpper() && Convert.ToDouble(Observationvalue) > Convert.ToDouble(_80076_ALP_MAX))
                    {
                        Observationvalue = _80076_ALP_MAX;
                    }

                    else if (ObservationCode.ToUpper() == "ALT".ToUpper() && Convert.ToDouble(Observationvalue) < Convert.ToDouble(_80076_ALT_MIN))
                    {
                        Observationvalue = _80076_ALT_MIN;
                    }
                    else if (ObservationCode.ToUpper() == "ALT".ToUpper() && Convert.ToDouble(Observationvalue) > Convert.ToDouble(_80076_ALT_MAX))
                    {
                        Observationvalue = _80076_ALT_MAX;
                    }

                    else if (ObservationCode.ToUpper() == "AST".ToUpper() && Convert.ToDouble(Observationvalue) < Convert.ToDouble(_80076_AST_MIN))
                    {
                        Observationvalue = _80076_AST_MIN;
                    }
                    else if (ObservationCode.ToUpper() == "AST".ToUpper() && Convert.ToDouble(Observationvalue) > Convert.ToDouble(_80076_AST_MAX))
                    {
                        Observationvalue = _80076_AST_MAX;
                    }

                    else if (ObservationCode.ToUpper() == "TOTALBILIRUBIN".ToUpper() && Convert.ToDouble(Observationvalue) < Convert.ToDouble(_80076_TotalBilirubin_MIN))
                    {
                        Observationvalue = _80076_TotalBilirubin_MIN;
                    }
                    else if (ObservationCode.ToUpper() == "TOTALBILIRUBIN".ToUpper() && Convert.ToDouble(Observationvalue) > Convert.ToDouble(_80076_TotalBilirubin_MAX))
                    {
                        Observationvalue = _80076_TotalBilirubin_MAX;
                    }

                    else if (ObservationCode.ToUpper() == "DIRECTBILIRUBIN".ToUpper() && Convert.ToDouble(Observationvalue) < Convert.ToDouble(_80076_DirectBilirubin_MIN))
                    {
                        Observationvalue = _80076_DirectBilirubin_MIN;
                    }
                    else if (ObservationCode == "DIRECTBILIRUBIN" && Convert.ToDouble(Observationvalue) > Convert.ToDouble(_80076_DirectBilirubin_MAX))
                    {
                        Observationvalue = _80076_DirectBilirubin_MAX;
                    }

                    else if (ObservationCode.ToUpper() == "TOTALPROTEIN" && Convert.ToDouble(Observationvalue) < Convert.ToDouble(_80076_TotalProtein_MIN))
                    {
                        Observationvalue = _80076_TotalProtein_MIN;
                    }
                    else if (ObservationCode.ToUpper() == "TOTALPROTEIN" && Convert.ToDouble(Observationvalue) > Convert.ToDouble(_80076_TotalProtein_MAX))
                    {
                        Observationvalue = _80076_TotalProtein_MAX;
                    }

                    break;
                case "82947":
                    if (ObservationCode.ToUpper() == "FBS" && Convert.ToDouble(Observationvalue) < Convert.ToDouble(_82947_FBS_MIN))
                    {
                        Observationvalue = _82947_FBS_MIN;
                    }
                    else if (ObservationCode.ToUpper() == "FBS" && Convert.ToDouble(Observationvalue) > Convert.ToDouble(_82947_FBS_MAX))
                    {
                        Observationvalue = _82947_FBS_MAX;
                    }
                    else if (ObservationCode.ToUpper() == "RBS" && Convert.ToDouble(Observationvalue) < Convert.ToDouble(_82947_RBS_MIN))
                    {
                        Observationvalue = _82947_RBS_MIN;
                    }
                    else if (ObservationCode.ToUpper() == "RBS" && Convert.ToDouble(Observationvalue) > Convert.ToDouble(_82947_RBS_MAX))
                    {
                        Observationvalue = _82947_RBS_MAX;
                    }
                    Observationvalue = Math.Ceiling(Convert.ToDecimal(Observationvalue)).ToString();
                    break;
            }
        }


    }


}
