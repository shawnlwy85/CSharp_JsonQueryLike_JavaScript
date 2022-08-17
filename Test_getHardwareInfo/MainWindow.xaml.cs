using Eclipse_MyOSU;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Test_getHardwareInfo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }


        MyOSU myOSU = new MyOSU();
        string gpu_shortName = "";


        private string get_HexPower_Example()
        {
            //string URL = "https://jsonplaceholder.typicode.com/users";
            //HttpWebRequest req = (HttpWebRequest)WebRequest.Create(URL);
            //req.Method = "GET";

            //WebResponse res = req.GetResponse();
            //using (Stream webStream = res.GetResponseStream() ?? Stream.Null)
            //using (StreamReader res_StreamReader = new StreamReader(webStream))
            //{
            //    Object[] items = JsonConvert.DeserializeObject<Object[]>(res_StreamReader.ReadToEnd());

            //    var z = JObject.Parse(
            //                JObject.Parse(
            //                    JObject.Parse(
            //                        items[0].ToString()
            //                                ).GetValue("address").ToString()
            //                             ).GetValue("geo").ToString()
            //                         );

            //    return z.GetValue("lat").ToString();
            //}
            //---------------------------------------------------------------------------------------------------------------------

            //string URL = "http://localhost:8080/api/v1/status";
            string URL = "https://jsonkeeper.com/b/H60E";
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(URL);
            req.Method = "GET";


            WebResponse res = req.GetResponse();

            using (Stream webStream = res.GetResponseStream() ?? Stream.Null)
            using (StreamReader res_StreamReader = new StreamReader(webStream))
            {
                try
                {
                    Object items = JsonConvert.DeserializeObject<Object>(res_StreamReader.ReadToEnd());

                    var r1 = JObject.Parse(items.ToString());
                    var r2 = JObject.Parse(r1.GetValue("miner").ToString());
                    var r3 = JsonConvert.DeserializeObject<object[]>(r2.GetValue("devices").ToString())[0];
                    var r4 = JObject.Parse(r3.ToString());
                    var r5 = r4.GetValue("info").ToString();



                    //var r0 = JObject.Parse(JObject.Parse(items.ToString()).GetValue("miner").ToString()).GetValue("total_hashrate").ToString();

                    return r5.ToString();
                }
                catch (Exception ex)
                {

                    return "err2" + ex.Message + "\n";
                }

            }

        }


        private string get_GET_JsonOBJ_Info(string _URL_API, List<string> _JSON_QueryString_Arr)
        {
            // -------------------------------------------------------- Check query string items
            foreach (var item in _JSON_QueryString_Arr)
            {
                printLn(item + "");
            }

            int JSON_Query_Length = _JSON_QueryString_Arr.Count;
            printLn("Length = " + JSON_Query_Length + "");


            // -------------------------------------------------------- Operations
            //string URL = "http://localhost:8080/api/v1/status";
            //string URL = "https://jsonkeeper.com/b/H60E";
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(_URL_API);
            req.Method = "GET";
            WebResponse res = req.GetResponse();

            using (Stream webStream = res.GetResponseStream() ?? Stream.Null)
            using (StreamReader res_StreamReader = new StreamReader(webStream))
            {
                try
                {
                    Object JSON_Obj = JsonConvert.DeserializeObject<Object>(res_StreamReader.ReadToEnd());
                    //Object[] JSON_ArrObj = JsonConvert.DeserializeObject<Object[]>(res_StreamReader.ReadToEnd());


                    // test code......................................

                    var resul_Lv1 = (dynamic)null;
                    string lv1_Query = "";
                    if (JSON_Query_Length > 0)
                    {
                        lv1_Query = _JSON_QueryString_Arr[0];
                        if (lv1_Query.Contains('[') == true)
                        {
                            string obj_Lv1_Name = lv1_Query.Split('[')[0];
                            int obj_Lv1_Name_Index = Int32.Parse(lv1_Query.Split('[')[1].Replace("[", "").Replace("]", ""));

                            printLn("# p1 lv1.1 = |" + lv1_Query + "|");
                            printLn("# obj_Lv1_Name = |" + obj_Lv1_Name + "|");
                            printLn("# obj_Lv1_Name_Index = |" + obj_Lv1_Name_Index + "|");

                            //MessageBox.Show("cp 1.1");

                            resul_Lv1 = JsonConvert.DeserializeObject<object[]>(resul_Lv1.GetValue(obj_Lv1_Name).ToString())[obj_Lv1_Name_Index];

                        }
                        else
                        {
                            printLn("# lv1.2 = |" + lv1_Query + "|");
                            printLn("# p1 lv1.2 = |" + lv1_Query + "|");
                            //MessageBox.Show("cp 1.2");

                            resul_Lv1 = JObject.Parse(JSON_Obj.ToString()).GetValue(lv1_Query);
                        }
                    }


                    var resul_Lv2 = (dynamic)null;
                    string lv2_Query = "";
                    if (JSON_Query_Length > 1)
                    {
                        lv2_Query = _JSON_QueryString_Arr[1];

                        if (lv2_Query.Contains('[') == true)
                        {
                            string obj_Lv2_Name = lv2_Query.Split('[')[0];
                            int obj_Lv2_Name_Index = Int32.Parse(lv2_Query.Split('[')[1].Replace("[", "").Replace("]", ""));

                            printLn("# p1 lv2.1 = |" + lv2_Query + "|");
                            printLn("# obj_Lv2_Name = |" + obj_Lv2_Name + "|");
                            printLn("# obj_Lv2_Name_Index = |" + obj_Lv2_Name_Index + "|");

                            //MessageBox.Show("cp 2.1");

                            resul_Lv2 = JsonConvert.DeserializeObject<object[]>(resul_Lv1.GetValue(obj_Lv2_Name).ToString())[obj_Lv2_Name_Index];

                        }
                        else
                        {
                            printLn("# lv2.2 = |" + lv2_Query + "|");
                            printLn("# p1 lv2.2 = |" + lv2_Query + "|");
                            //MessageBox.Show("cp 2.2");

                            resul_Lv2 = JObject.Parse(resul_Lv1.ToString()).GetValue(lv2_Query); ;

                        }
                    }




                    var resul_Lv3 = (dynamic)null;
                    string lv3_Query = "";
                    if (JSON_Query_Length > 2)
                    {
                        lv3_Query = _JSON_QueryString_Arr[2];

                        if (lv3_Query.Contains('[') == true)
                        {
                            string obj_Lv3_Name = lv3_Query.Split('[')[0];
                            int obj_Lv3_Name_Index = Int32.Parse(lv3_Query.Split('[')[1].Replace("[", "").Replace("]", ""));

                            printLn("# p1 lv3.1 = |" + lv3_Query + "|");
                            printLn("# obj_Lv3_Name = |" + obj_Lv3_Name + "|");
                            printLn("# obj_Lv3_Name_Index = |" + obj_Lv3_Name_Index + "| ");

                            //MessageBox.Show("cp 3.1");

                            resul_Lv3 = JsonConvert.DeserializeObject<object[]>(resul_Lv2.GetValue(obj_Lv3_Name).ToString())[obj_Lv3_Name_Index];

                        }
                        else
                        {
                            printLn("# lv3.2 = |" + lv3_Query + "| ");
                            printLn("# p1 lv3.2 = |" + lv3_Query + "| ");
                            //MessageBox.Show("cp 3.2");

                            resul_Lv3 = JObject.Parse(resul_Lv2.ToString()).GetValue(lv3_Query); ;

                        }
                    }





                    if (resul_Lv3 != null)
                    {
                        return resul_Lv3.ToString();
                    }
                    else if (resul_Lv2 != null)
                    {
                        return resul_Lv2.ToString();
                    }
                    else if (resul_Lv1 != null)
                    {
                        return resul_Lv1.ToString();
                    }
                    else
                    {
                        return "NO RESULT";
                    }

                }
                catch (Exception ex)
                {
                    return "err2" + ex.Message;
                }

            }

        }




        private void btn_Test_Click(object sender, RoutedEventArgs e)
        {
            ////string JSON_QueryString = "miner.devices[0].info";
            string JSON_QueryString = tb_jsonquery.Text;//"miner.devices[0].info";

            List<string> JSON_QueryString_Arr = new List<string>();


            if (JSON_QueryString.Contains(".") == true)
            {
                JSON_QueryString_Arr.AddRange(JSON_QueryString.Split('.'));

            }
            else
            {
                JSON_QueryString_Arr.Add(JSON_QueryString);
            }

            printLn("_____________________");
            printLn(">>> Result = " + get_GET_JsonOBJ_Info(tb_url_api.Text, JSON_QueryString_Arr));
            printLn("_____________________");



        }

        private void btn_Test2_Click(object sender, RoutedEventArgs e)
        {


            //tb_Disp.Text += get_HexPower_Example() + "\n";


        }


        private void printLn(string message)
        {
            tb_Disp.Text += message + "\n";

            tb_Disp.ScrollToEnd();
        }
    }
}
