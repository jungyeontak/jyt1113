using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KaKaoCouponRestApi.Models
{
    public class KaKaoCouponModel : KaKaoCouponResultModel
    {
        private string COUPON_CODE;
        public string coupon_code
        {
            get { return COUPON_CODE; }
            set { COUPON_CODE = value; }
        }   
    }
    public class KaKaoCouponResultModel
    {
        private string MSG;
        private string CODE;

        public string msg
        {
            get { return MSG; }
            set { MSG = value; }
        }
        public string code
        {
            get { return CODE; }
            set { CODE = value; }
        }
    }   
}