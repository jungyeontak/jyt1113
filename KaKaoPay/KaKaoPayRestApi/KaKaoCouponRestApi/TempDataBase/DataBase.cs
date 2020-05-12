using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml;

namespace KaKaoCouponRestApi.TempDataBase
{
    public class DataBase
    {
        private static DataTable dt = new DataTable("COUPON_TABLE");
        private const string DIR_PATH = @"C:\temp\";
        private const string SEQUENCE_FILE = "SEQUENCES.xml";
        private const string COUPON_FILE = "COUPON.xml";

        static DataBase()
        {
            //COUPON_KEY 쿠폰키
            //STATUS 상태값(I:쿠폰번호 생성, B:쿠폰번호 발급, C:쿠폰사용취소(사용 후 취소), Y:쿠폰사용완료, E:만료)
            //CALL_SITE 어느사이트에서 쿠폰번호 생성 및 사용 했는지 구분
            //CREATE_DATE 쿠폰번호 생성일자
            //BUY_DATE 쿠폰번호 발급일자(구매일자)
            //USE_DATE 쿠폰번호 사용일자
            //CANCEL_DATE 쿠폰번호 사용 취소일자
            //EXPIRE_DATE 만료된 쿠폰번호 업데이트 일자
            //EXPIRE_SMS_YN 만료3일전 SMS전송여부
            DataColumn[] primarykey = new DataColumn[1];

            dt.Columns.Add("COUPON_KEY", typeof(string));
            dt.Columns.Add("STATUS", typeof(string));
            dt.Columns.Add("CALL_SITE", typeof(string));
            dt.Columns.Add("CREATE_DATE", typeof(DateTime));
            dt.Columns.Add("BUY_DATE", typeof(DateTime));
            dt.Columns.Add("USE_DATE", typeof(DateTime));
            dt.Columns.Add("CANCEL_DATE", typeof(DateTime));
            dt.Columns.Add("EXPIRE_DATE", typeof(DateTime));
            dt.Columns.Add("EXPIRE_SMS_YN", typeof(string));

            //PK를 지정한다.                        
            primarykey[0] = dt.Columns["COUPON_KEY"];
            dt.PrimaryKey = primarykey;
            //시퀀스 파일을 생성한다.
            Common.CommonExtension.CreateSequences();
        }
        public static string ExecuteNonQuery(List<string> pCouponList,string pUri)
        {            
            //COUPON.xml이 존재하는경우 미리 등록된 쿠폰 정보를 읽어온다.            
            if (Common.CommonExtension.FileExist(DIR_PATH + COUPON_FILE))
            {
                dt.Clear();
                dt.AcceptChanges();
                                                
                dt.ReadXml(DIR_PATH + COUPON_FILE);
            }

            //데이터 베이스 처리 로직
            int ResultCnt = 0;            

            for (int i = 0; i < pCouponList.Count; i++)
            {
                DataRow dr = dt.NewRow();

                dr["COUPON_KEY"] = pCouponList[i] + Common.CommonExtension.NextSequences();
                dr["STATUS"] = "I";
                dr["CALL_SITE"] = pUri;
                dr["CREATE_DATE"] = DateTime.Now;
                dr["BUY_DATE"] = DBNull.Value;
                dr["USE_DATE"] = DBNull.Value;
                dr["CANCEL_DATE"] = DBNull.Value;
                dr["EXPIRE_DATE"] = DBNull.Value;
                dr["EXPIRE_SMS_YN"] = "N";

                dt.Rows.Add(dr);

                ResultCnt++;
            }

            //DB에 저장
            dt.WriteXml(DIR_PATH + COUPON_FILE);

            return ResultCnt.ToString();
        }
        public static string ExecuteNonQuery(string pCouponCode, string pUri)
        {            
            //COUPON.xml이 존재하는경우 미리 등록된 쿠폰 정보를 읽어온다.            
            if (Common.CommonExtension.FileExist(DIR_PATH + COUPON_FILE))
            {
                dt.Clear();
                dt.AcceptChanges();

                dt.ReadXml(DIR_PATH + COUPON_FILE);
            }

            //데이터 베이스 처리 로직
            string ResultMsg = "99";
            string CouponCode = pCouponCode.Replace("-", "");
            //구매한 이력이 있는 쿠폰중에서 요청한 쿠폰번호가 있는지 찾는다.
            var expireQuery = from dr in dt.AsEnumerable()
                              where dr.Field<string>("COUPON_KEY") == CouponCode
                              select dr;
                        
            if (expireQuery.FirstOrDefault() == null)
            {
                //해당 쿠폰번호가 존재하지않음.
                return "-1";
            }

            if (pUri.Contains("/use/coupon/v1"))
            {
                //사용처리 

                //구매한 쿠폰 이거나 사용이 취소된 쿠폰이면 사용가능                
                if (expireQuery.FirstOrDefault()["STATUS"].ToString() == "B" || expireQuery.FirstOrDefault()["STATUS"].ToString() == "C")
                {
                    expireQuery.FirstOrDefault()["USE_DATE"] = DateTime.Now;
                    expireQuery.FirstOrDefault()["STATUS"] = "Y";                                        
                }
                else if (expireQuery.FirstOrDefault().Field<string>("STATUS") ==  "Y")
                {
                    //이미 사용이 완료된 쿠펀
                    ResultMsg = "-2";
                }
                else
                {
                    ResultMsg = "-3";
                }
            }
            else
            {
                //취소처리
                //사용처리가 됬는지 체크
                if (expireQuery.FirstOrDefault()["STATUS"].ToString() == "Y")
                {
                    expireQuery.FirstOrDefault()["CANCEL_DATE"] = DateTime.Now;
                    expireQuery.FirstOrDefault()["STATUS"] = "C";                    
                }
                else if (expireQuery.FirstOrDefault().Field<string>("STATUS").ToString() == "B")//구매만 하고 사용하지 않은쿠폰
                {
                    ResultMsg = "-2";
                }
                else if (expireQuery.FirstOrDefault().Field<string>("STATUS").ToString() == "C")//이미 취소처리된 쿠폰
                {
                    ResultMsg = "-3";
                }
                else if (expireQuery.FirstOrDefault().Field<string>("STATUS").ToString() == "I")
                {
                    ResultMsg = "-4";
                }                                            
            }
            //DB에 저장
            dt.WriteXml(DIR_PATH + COUPON_FILE);

            return ResultMsg;
        }


        public static string Fill(string pURI)
        {            
            //COUPON.xml이 존재하는경우 미리 등록된 쿠폰 정보를 읽어온다.
            if (Common.CommonExtension.FileExist(DIR_PATH + COUPON_FILE))
            {
                dt.Clear();
                dt.AcceptChanges();

                dt.ReadXml(DIR_PATH + COUPON_FILE);
            }
            DataTable temp;
            //사용자가 쿠폰번호 요청(구매)
            if (pURI.Contains("get/coupon"))
            {
                //DataRow dr = dt.AsEnumerable().Where(r => r.Field<string>("STATUS") == "I").OrderByDescending(o => o.Field<DateTime>("CREATE_DATE")).FirstOrDefault();
                var expireQuery = from dr in dt.AsEnumerable()
                                  where dr.Field<string>("STATUS") == "I"
                                  orderby dr.Field<DateTime>("CREATE_DATE")
                                  select dr;

                if (expireQuery.FirstOrDefault() == null)
                {
                    return "-1";
                }

                expireQuery.FirstOrDefault()["BUY_DATE"] = DateTime.Now;
                expireQuery.FirstOrDefault()["STATUS"] = "B";

                dt.WriteXml(DIR_PATH + COUPON_FILE);
                dt.AcceptChanges();

                return Common.CommonExtension.FormatString(expireQuery.FirstOrDefault()["COUPON_KEY"].ToString());
            }
            else if (pURI.Contains("get/buy/coupon"))//사용자에게 지급된 쿠폰 조회 
            {
                var expireQuery = from dr in dt.AsEnumerable()
                                  where dr.Field<string>("STATUS") == "B"
                                  select dr.Field<string>("COUPON_KEY");

                if (expireQuery.FirstOrDefault() == null)
                {
                    return "-1";
                }
                return JsonConvert.SerializeObject(expireQuery);
            }
            else if (pURI.Contains("expire/coupon"))// 발급된 쿠폰중 당일 만료된 전체 쿠폰목록
            {
                //발급일로 부터 90일이상이면 만료되는 쿠폰으로 본다.
                var expireQuery = from dr in dt.AsEnumerable()
                                  where (dr.Field<string>("STATUS") == "B" || dr.Field<string>("STATUS") == "C") &&
                                        dr.Field<DateTime>("BUY_DATE") != null && dr.Field<DateTime>("BUY_DATE").Date <= DateTime.Today.Date.AddDays(-90)
                                  select dr;

                if (expireQuery.FirstOrDefault() == null)
                {
                    return "-1";
                }

                temp = new DataTable();
                temp = expireQuery.CopyToDataTable();

                expireQuery.ToList().ForEach(r =>
                {
                    r.SetField<DateTime>("EXPIRE_DATE", DateTime.Now);
                    r.SetField<string>("STATUS", "E");
                });

                //DB에 저장
                dt.WriteXml(DIR_PATH + COUPON_FILE);

                return JsonConvert.SerializeObject(temp.AsEnumerable().Select(s => s.Field<string>("COUPON_KEY")));
            }
            else if (pURI.Contains("expire/sms/coupon"))
            {
                var expireQuery = from dr in dt.AsEnumerable()
                                  where (dr.Field<string>("STATUS") == "B" || dr.Field<string>("STATUS") == "C") 
                                        && dr.Field<string>("EXPIRE_SMS_YN") == "N"
                                        && dr.Field<DateTime>("BUY_DATE") != null && (dr.Field<DateTime>("BUY_DATE").Date.AddDays(90) - DateTime.Today.Date).Days == 3
                                  select dr;

                if (expireQuery.FirstOrDefault() == null)
                {
                    return "-1";
                }
                temp = new DataTable();

                temp = expireQuery.CopyToDataTable();

                expireQuery.ToList().ForEach(r =>
                {
                    r.SetField<string>("EXPIRE_SMS_YN", "Y");

                    System.Diagnostics.Debug.WriteLine("쿠폰이 3일 후 만료됩니다.");
                });

                //DB에 저장
                dt.WriteXml(DIR_PATH + COUPON_FILE);

                return JsonConvert.SerializeObject(temp.AsEnumerable().Select(s => s.Field<string>("COUPON_KEY")));
            }

            return "-100";
        }               
    }
}