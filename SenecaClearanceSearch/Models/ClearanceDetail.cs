using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;

namespace SenecaClearanceSearch.Models
{
    public class ApplicantDetail
    {
        public int aNewSubmissionIdentity { get; set; }
        public string aRecievedDate { get; set; }
        public string aInsured { get; set; }
        public string aEffectiveDate { get; set; }
        public string aPolicyNum { get; set; }
        public string aAddr1 { get; set; }
        public string aStateCode { get; set; }
    }

    public class ShowApplicantsDetail
    {
        public List<ApplicantDetail> ApplicantsSearch(List<string> InsuredInformation)
        {
            StringBuilder BuildDetailSql = new StringBuilder();
            BuildDetailSql.Append("select * from [SenecaInsurance].[dbo].[ApplicationSubmissions] where ");
            BuildDetailSql.AppendFormat("[StateCode] = '{0}' AND (", "NY");

            int listCount = InsuredInformation.Count;

            if (listCount <= 2)
            {
                for (int i = 0; i < listCount; i++)
                {
                    BuildDetailSql.Append("[Insured] like '");

                    for (int j = 0; j < listCount - i; j++)
                    {
                        if(j == listCount - i - 1)
                            BuildDetailSql.AppendFormat("%{0}", InsuredInformation[j]);
                        else
                            BuildDetailSql.AppendFormat("%{0}", InsuredInformation[j]);
                    }

                    if (i < listCount - 1)
                        BuildDetailSql.Append("%' OR ");
                    else
                        BuildDetailSql.Append("%')");
                }
            }
            else
            {
                for (int i = 0; i < listCount - 1; i++)
                {
                    BuildDetailSql.Append("[Insured] like '");

                    for (int j = 0; j < listCount - i; j++)
                    {
                        BuildDetailSql.AppendFormat("%{0}", InsuredInformation[j]);
                    }

                    if (i < listCount - 2)
                        BuildDetailSql.Append("%' OR ");
                    else
                        BuildDetailSql.Append("%')");
                }
            }

            //if (InsuredInformation.Count <= 2)
            //{
            //    if (listCount == 1)
            //        BuildDetailSql.AppendFormat("[Insured] like '%{0}%'", InsuredInformation[0]);
            //    if (listCount == 2)
            //        BuildDetailSql.AppendFormat("[Insured] like '%{0}%{1}%'", InsuredInformation[0], InsuredInformation[1]);
            //}
            //else
            //{
            //    if (listCount >= 3)
            //        BuildDetailSql.AppendFormat("[Insured] like '%{0}%{1}%' OR [Insured] like '%{0}%{1}%{2}%'", InsuredInformation[0], InsuredInformation[1], InsuredInformation[2]);

            //    if (listCount > 4)
            //    {
            //        if (InsuredInformation[2] == "Vit.")
            //            BuildDetailSql.AppendFormat("[Insured] like '%{0}{1}%' OR [Insured] like '%{0}%{1}%{2}%' OR [Insured] like '%{0}%{1}%{3}%'", InsuredInformation[0], InsuredInformation[1], InsuredInformation[3]);
            //    }

            //}
            string DataQuerySql = BuildDetailSql.ToString(0, BuildDetailSql.Length);
            return QueryApplicantsList(DataQuerySql);
        }
        protected List<ApplicantDetail> QueryApplicantsList(string txtSql)
        {
            List<ApplicantDetail> aList = new List<ApplicantDetail>();
            SqlCommand cmd = GenerateSqlCommand(txtSql);
            using (cmd.Connection)
            {
                SqlDataReader dReader = cmd.ExecuteReader();
                if (dReader.HasRows)
                {
                    while (dReader.Read())
                    {
                        aList.Add(ReadValue(dReader));
                    }
                }
            }
            return aList;
        }
        protected SqlCommand GenerateSqlCommand(string cmdText)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["dbSIConnection"].ConnectionString);
            SqlCommand cmd = new SqlCommand(cmdText, con);
            cmd.Connection.Open();
            return cmd;
        }
        protected ApplicantDetail ReadValue(SqlDataReader reader)
        {
            ApplicantDetail dt = new ApplicantDetail();
            dt.aNewSubmissionIdentity = (int)reader["NewSubmissionIdentity"];
            dt.aRecievedDate = reader["RecievedDate"].ToString();
            dt.aInsured = (string)reader["Insured"];
            dt.aEffectiveDate = reader["EffectiveDate"].ToString();
            dt.aPolicyNum = (string)reader["PolicyNum"];
            dt.aAddr1 = (string)reader["Addr1"];
            dt.aStateCode = (string)reader["StateCode"];
            return dt;
        }
    }

}