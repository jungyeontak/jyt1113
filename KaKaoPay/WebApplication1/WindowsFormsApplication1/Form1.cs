using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string str = "asdfbh1000000000";
            Regex reg = new Regex(str);
            str = reg.Replace("(.{3})(.+)(.{4})", @"\1-\2-\3");

            string sendData = "";
            byte[] byteArray = System.Text.Encoding.UTF8.GetBytes(sendData);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://localhost:52592/api/get/buy/coupon/v1");
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
                            string retJson = reader.ReadToEnd();                            
                            JObject jobj = JObject.Parse(retJson);
                            
                        }
                    }
                }
            }
        }
    }
}
