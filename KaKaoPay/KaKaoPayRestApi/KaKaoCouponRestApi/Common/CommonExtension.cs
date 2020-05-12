using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml;

namespace KaKaoCouponRestApi.Common
{
    public static class CommonExtension
    {
        private const string DIR_PATH = @"C:\temp\";
        private const string SEQUENCE_FILE = "SEQUENCES.xml";
        private const string COUPON_FILE = "COUPON.xml";
        private const string PoolString = "abcdefghizklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789"; //문자 생성 풀        

        //실제DB에는 "-"(하이픈)은 제거하고 저장하지만 디스플레이는 "-"(하이픈을 붙여서 보여주기 위해)
        public static string FormatString(string pCoupon)
        {            
            pCoupon = pCoupon.Insert(4, "-");
            pCoupon = pCoupon.Insert(9, "-");
            pCoupon = pCoupon.Insert(14, "-");
            return pCoupon;
        }
        public static bool FileExist(string pDir)
        {
            //해당 경로에 파일이 있는지 체크            
            FileInfo fileInfo = new FileInfo(pDir);
                        
            return fileInfo.Exists;
        }
        public static void CreateSequences()
        {
            XmlDocument xdoc = new XmlDocument();
            XmlNode root = null;
            XmlNode seq = null;
            XmlAttribute attr = null;

            //C:temp 폴더가 있는지 체크 후 없으면 생성한다.
            DirectoryInfo di = new DirectoryInfo(DIR_PATH);

            if (di.Exists == false)
            {
                di.Create();
            }

            //SEQUENCES.xml 파일 검색
            FileInfo fileInfo = new FileInfo(DIR_PATH + SEQUENCE_FILE);

            if (fileInfo.Exists == false)
            {
                // 루트노드
                root = xdoc.CreateElement("SEQUENCES");
                xdoc.AppendChild(root);
                // SEQ
                seq = xdoc.CreateElement("NEXT_SEQ");
                attr = xdoc.CreateAttribute("SEQ");
                attr.Value = "1000000000";
                seq.Attributes.Append(attr);
                root.AppendChild(seq);

                xdoc.Save(@"C:\temp\SEQUENCES.xml");
            }
        }
        public static string NextSequences()
        {
            //데이터베이스의 시퀀스 개념을 로컬에 만듬.
            XmlDocument xdoc = new XmlDocument();
            string seq = string.Empty;
            Int64 iNextSeq = 0;

            // XML 데이타를 파일에서 로드
            xdoc.Load(@"C:\temp\SEQUENCES.xml");
            seq = xdoc.SelectSingleNode("/SEQUENCES/NEXT_SEQ").Attributes["SEQ"].Value;

            //현재 나온값에 +1 해서 다시 저장한다.
            iNextSeq = Convert.ToInt64(xdoc.SelectSingleNode("/SEQUENCES/NEXT_SEQ").Attributes["SEQ"].Value);
            xdoc.SelectSingleNode("/SEQUENCES/NEXT_SEQ").Attributes["SEQ"].Value = (iNextSeq + 1).ToString();

            xdoc.Save(@"C:\temp\SEQUENCES.xml");
            return seq;
        }

        private static Random random = new Random((int)DateTime.Now.Ticks & 0x0000FFFF);//랜덤 시드값
        public static string RandomString(int pLength = 6)
        {
            char[] chRandom = new char[pLength];

            for (int i = 0; i < pLength; i++)
            {
                chRandom[i] = PoolString[random.Next(PoolString.Length)];
            }

            return new string(chRandom);
        }
    }
}