using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using KaKaoCouponRestApi.Models;
using System.Security.Cryptography;
using System.Text;
using KaKaoCouponRestApi.TempDataBase;

namespace KaKaoCouponRestApi.Controllers
{
    [RoutePrefix("api")]
    public class KaKaoCouponController : ApiController
    {

        public KaKaoCouponController()
        {

        }

        /// <summary>
        /// name         : 쿠폰생성
        /// desc         : 랜덤한 코드의 쿠폰을 N개 생성하여 데이터베이스에 보관하는 API를 구현하세요.
        /// author       : jungyeontal 
        /// create date  : 2020-05-12
        /// update date  : 최종 수정 일자, 수정자, 수정개요 
        /// </summary>
        [HttpPost]
        [Route("create/coupon/v1")]
        public string CreateCoupon([FromBody]string inPut)
        {
            int iCouponCnt = 0;
            KaKaoCouponResultModel model = new KaKaoCouponResultModel();
            List<string> listCoupon = new List<string>();

            //호출한 파라미터가 숫자형인지 체크한다.
            if (int.TryParse(inPut, out iCouponCnt) == false)
            {
                model.code = "-200";
                model.msg = string.Format("{0}", "형변환 오류가 있습니다.");

                return JsonConvert.SerializeObject(model, Formatting.Indented);
            }

            //요청온 갯수만큼 쿠폰번호를 생성한다.            
            for (int i = 0; i < iCouponCnt; i++)
            {
                listCoupon.Add(Common.CommonExtension.RandomString(6));
            }

            if (listCoupon.Count > 0)
            {                                                
                string result = DataBase.ExecuteNonQuery(listCoupon, this.ActionContext.Request.RequestUri.ToString());

                model.code = "200";
                model.msg = string.Format("{0} (요청건: {1} 발급건: {2})", "쿠폰 생성이 완료되었습니다.", inPut.ToString(), result);
            }
            else
            {
                model.code = "-201";
                model.msg = "생성 할 쿠폰이 없습니다.";
            }

            return JsonConvert.SerializeObject(model, Formatting.Indented);
        }
        /// <summary>
        /// name         : 쿠폰발급
        /// desc         : 생성된 쿠폰중 하나를 사용자에게 지급하는 API를 구현하세요.
        /// author       : jungyeontal 
        /// create date  : 2020-05-12
        /// update date  : 최종 수정 일자, 수정자, 수정개요 
        /// </summary>
        [HttpGet]
        [Route("get/coupon/v1")]
        public string GetCoupon()
        {
            KaKaoCouponModel model = new KaKaoCouponModel();

            string result = DataBase.Fill(this.ActionContext.Request.RequestUri.ToString());

            if (result != "-1")
            {                
                model.coupon_code = result;
                model.code = "200";
                model.msg = "쿠폰발급 완료";
            }
            else
            {
                model.code = "-201";
                model.msg = "발급 할 쿠폰번호가 존재하지않습니다.";
            }            
            return JsonConvert.SerializeObject(model, Formatting.Indented);
        }

        /// <summary>
        /// name         : 지급된 쿠폰 조회
        /// desc         : 사용자에게 지급된 쿠폰을 조회하는 API를 구현하세요.
        /// author       : jungyeontal 
        /// create date  : 2020-05-12
        /// update date  : 최종 수정 일자, 수정자, 수정개요 
        /// </summary>
        [HttpGet]
        [Route("get/buy/coupon/v1")]
        public string GetBuyCoupon()
        {
            KaKaoCouponModel model = new KaKaoCouponModel();
                        
            string result = DataBase.Fill(this.ActionContext.Request.RequestUri.ToString());

            if (result != "-1")
            {
                model.coupon_code = result;
                model.code = "200";
                model.msg = "지급된 쿠폰 조회";
            }
            else
            {
                model.code = "-201";
                model.msg = "지급된 쿠폰번호가 없습니다.";
            }

            return JsonConvert.SerializeObject(model, Formatting.Indented);
        }

        /// <summary>
        /// name         : 지급된 쿠폰 사용
        /// desc         : 지급된 쿠폰중 하나를 사용하는 API를 구현하세요.
        /// author       : jungyeontal 
        /// create date  : 2020-05-12
        /// update date  : 최종 수정 일자, 수정자, 수정개요 
        /// </summary>
        [HttpPost]
        [Route("use/coupon/v1")]
        public string UseCoupon([FromBody]string inPut)
        {                        
            KaKaoCouponResultModel model = new KaKaoCouponResultModel();

            string result = DataBase.ExecuteNonQuery(inPut, this.ActionContext.Request.RequestUri.ToString());

            if (result == "99")
            {
                model.code = "200";
                model.msg = "사용처리 되었습니다.";
            }
            else if (result == "-1")
            {
                model.code = "-201";
                model.msg = "쿠폰번호가 존재하지 않습니다";
            }
            else if (result == "-2")
            {
                model.code = "-202";
                model.msg = "사용처리가 완료된 쿠폰입니다.";
            }
            else if (result == "-3")
            {
                model.code = "-203";
                model.msg = "구입하지 않은 쿠폰 입니다.";
            }

            return JsonConvert.SerializeObject(model, Formatting.Indented);
        }
        /// <summary>
        /// name         : 쿠폰 취소
        /// desc         : 지급된 쿠폰중 하나를 사용 취소하는 API를 구현하세요.
        /// author       : jungyeontal 
        /// create date  : 2020-05-12
        /// update date  : 최종 수정 일자, 수정자, 수정개요 
        /// </summary>
        [HttpPost]
        [Route("cnlcel/coupon/v1")]
        public string CancelCoupon([FromBody]string inPut)
        {            
            KaKaoCouponResultModel model = new KaKaoCouponResultModel();

            string result = DataBase.ExecuteNonQuery(inPut, this.ActionContext.Request.RequestUri.ToString());

            if (result == "99")
            {
                model.code = "200";
                model.msg = "사용취소 되었습니다.";
            }
            else if (result == "-1")
            {
                model.code = "-201";
                model.msg = "쿠폰번호가 존재하지 않습니다.";
            }
            else if (result == "-2")
            {
                model.code = "-202";
                model.msg = "사용하지 않은 쿠폰은 취소처리 할 수 없습니다.";
            }
            else if (result == "-3")
            {
                model.code = "-203";
                model.msg = "이미 취소처리된 쿠폰입니다.";
            }
            else if (result == "-4")
            {
                model.code = "-204";
                model.msg = "구입하지 않은 쿠폰입니다.";
            }

            return JsonConvert.SerializeObject(model, Formatting.Indented);
        }

        /// <summary>
        /// name         : 당일 만료쿠폰
        /// desc         : 발급된 쿠폰중 당일 만료된 전체 쿠폰 목록을 조회하는 API를 구현하세요
        /// author       : jungyeontal 
        /// create date  : 2020-05-12
        /// update date  : 최종 수정 일자, 수정자, 수정개요 
        /// </summary>
        [HttpGet]
        [Route("expire/coupon/v1")]
        public string ExpireCoupon()
        {                         
            KaKaoCouponModel model = new KaKaoCouponModel();

            string result = DataBase.Fill(this.ActionContext.Request.RequestUri.ToString());
            if (result != "-1")
            {
                model.code = "200";
                model.msg = "당일 만료된 쿠폰목록";
                model.coupon_code = result;                
            }
            else
            {
                model.code = "-201";
                model.msg = "만료되는 쿠폰목록이 존재하지 않습니다.";
            }
            return JsonConvert.SerializeObject(model, Formatting.Indented);
        }

        /// <summary>
        /// name         : 만료3일전 SMS 전송
        /// desc         : 만료3일전 SMS 전송
        /// author       : jungyeontal 
        /// create date  : 2020-05-12
        /// update date  : 최종 수정 일자, 수정자, 수정개요 
        /// </summary>
        [HttpPost]
        [Route("expire/sms/coupon/v1")]
        public string ExpireSMSCoupon()
        {
            KaKaoCouponModel model = new KaKaoCouponModel();

            string result = DataBase.Fill(this.ActionContext.Request.RequestUri.ToString());
            if (result != "-1")
            {
                model.code = "200";
                model.msg = "SMS 발송이 완료되었습니다.";
                model.coupon_code = result;                
            }
            else
            {
                model.code = "-201";
                model.msg = "발송 할 SMS가 없습니다.";
            }
            return JsonConvert.SerializeObject(model, Formatting.Indented);
        }
    }
}
