using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using System.IO;
using Newtonsoft.Json;

namespace KaKaoRestApiCallSample
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnRestApi_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtURL.Text))
            {
                MessageBox.Show("호출 URL을 입력하세요");
                return;
            }            
            JObject jobj;
            object retJson;
            string sendData = txtParam.Text;
            byte[] byteArray = System.Text.Encoding.UTF8.GetBytes(sendData);
                       
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(txtURL.Text);
            request.Method = "POST";
            request.ContentType = "application/json";
            request.ContentLength = byteArray.Length;
            using (Stream requstStream = request.GetRequestStream())
            {
                requstStream.Write(byteArray, 0, byteArray.Length);
                using (WebResponse response = request.GetResponse())
                {
                    using (Stream responseStream = response.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(responseStream))
                        {
                             retJson = JsonConvert.DeserializeObject(reader.ReadToEnd());                            
                            jobj = JObject.Parse(retJson.ToString());
                        }
                    }
                }
            }
            lblMsg.Text = string.Format("msg:{0}", jobj["msg"].ToString());
            lblCode.Text = string.Format("code:{0}", jobj["code"].ToString());
            if (jobj["coupon_code"] != null)
            {
                string coupon_code = string.Empty;
                if (jobj["coupon_code"].ToString().Contains(","))
                {
                     coupon_code = string.Join(", ", JsonConvert.DeserializeObject<List<string>>(jobj["coupon_code"].ToString()).ToArray());
                }
                else
                {
                    coupon_code = jobj["coupon_code"].ToString();
                }
                lblResult.Text = string.Format("result:{0}", coupon_code);
            }                       
        }      
    }
}
