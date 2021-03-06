﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using System.Xml.Linq;


namespace DMReport
{
    [ServiceContract(Namespace = "")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class DMService
    {
        // To use HTTP GET, add [WebGet] attribute. (Default ResponseFormat is WebMessageFormat.Json)
        // To create an operation that returns XML,
        //     add [WebGet(ResponseFormat=WebMessageFormat.Xml)],
        //     and include the following line in the operation body:
        //         WebOperationContext.Current.OutgoingResponse.ContentType = "text/xml";
        [OperationContract]
        [WebGet]
        public IList<DateValue> GetRetention(string week)
        {
            int userCount = 0;
            Dictionary<string, int> dic = new Dictionary<string, int>();

            //Rowkey.
            string rowkey = System.Convert.ToBase64String(Encoding.UTF8.GetBytes("2013-3"));
            string url = @"http://10.172.85.68:20550/sposession/" + week;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "Get";
            request.ContentType = "application/json";
            // request.Accept = "text/json";
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                XDocument doc = XDocument.Load(response.GetResponseStream());

                var cells = doc.Root.Descendants("Cell");

                foreach (var cell in cells)
                {
                    if (string.IsNullOrWhiteSpace(cell.Value))
                        continue;

                    var key = Encoding.UTF8.GetString(System.Convert.FromBase64String(cell.Attribute("column").Value));
                    var value = Int32.Parse(Encoding.UTF8.GetString(System.Convert.FromBase64String(cell.Value)));

                    if (key.Contains("usercount"))
                    {
                        userCount = value;
                    }
                    else
                    {
                        string tmp = key.Split(new char[] { ':', })[1];
                        var parts = tmp.Split(new char[] { '_', });

                        if (string.Equals("count", parts[1]))
                        {
                            dic[parts[0]] = value;
                        }
                    }
                }
            }

            IList<DateValue> retentions = new List<DateValue>();
            foreach (string key in dic.Keys.OrderBy(c => c, new WeekComparer()))
            {
                retentions.Add(new DateValue() { Date = key, Value = (double)dic[key] / (double)userCount });
            }

            return retentions;
        }

        // To use HTTP GET, add [WebGet] attribute. (Default ResponseFormat is WebMessageFormat.Json)
        // To create an operation that returns XML,
        //     add [WebGet(ResponseFormat=WebMessageFormat.Xml)],
        //     and include the following line in the operation body:
        //         WebOperationContext.Current.OutgoingResponse.ContentType = "text/xml";
        [OperationContract]
        [WebGet]
        public IList<DateValue> GetUserIncrease()
        {
            int userCount = 0;
            Dictionary<string, int> dic = new Dictionary<string, int>();

            //Rowkey.
            string url = @"http://10.172.85.68:20550/sposession/*/data:usercount";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "Get";
            request.ContentType = "application/json";
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                XDocument doc = XDocument.Load(response.GetResponseStream());

                var rows = doc.Root.Descendants("Row");

                foreach (var row in rows)
                {
                    var key = Encoding.UTF8.GetString(System.Convert.FromBase64String(row.Attribute("key").Value));
                    var cell = row.Descendants("Cell").First();
                    var value = Int32.Parse(Encoding.UTF8.GetString(System.Convert.FromBase64String(cell.Value)));

                    dic[key] = value;
                }
            }

            IList<DateValue> retentions = new List<DateValue>();
            foreach (string key in dic.Keys.OrderBy(c => c, new WeekComparer()))
            {
                retentions.Add(new DateValue() { Date = key, Value = dic[key] });
            }

            return retentions;
        }

        // To use HTTP GET, add [WebGet] attribute. (Default ResponseFormat is WebMessageFormat.Json)
        // To create an operation that returns XML,
        //     add [WebGet(ResponseFormat=WebMessageFormat.Xml)],
        //     and include the following line in the operation body:
        //         WebOperationContext.Current.OutgoingResponse.ContentType = "text/xml";
        [OperationContract]
        [WebGet]
        public IList<DateValue> GetSessionLength(string week)
        {
            int userCount = 0;
            Dictionary<string, int> dic = new Dictionary<string, int>();

            //Rowkey.
            string rowkey = System.Convert.ToBase64String(Encoding.UTF8.GetBytes("2013-3"));
            string url = @"http://10.172.85.68:20550/sposession/" + week;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "Get";
            request.ContentType = "application/json";
            // request.Accept = "text/json";
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                XDocument doc = XDocument.Load(response.GetResponseStream());

                var cells = doc.Root.Descendants("Cell");

                foreach (var cell in cells)
                {
                    if (string.IsNullOrWhiteSpace(cell.Value))
                        continue;

                    var key = Encoding.UTF8.GetString(System.Convert.FromBase64String(cell.Attribute("column").Value));
                    var value = Int32.Parse(Encoding.UTF8.GetString(System.Convert.FromBase64String(cell.Value)));

                    if (key.Contains("usercount"))
                    {
                        userCount = value;
                    }
                    else
                    {
                        string tmp = key.Split(new char[] { ':', })[1];
                        var parts = tmp.Split(new char[] { '_', });

                        if (string.Equals("length", parts[1]))
                        {
                            dic[parts[0]] = value;
                        }
                    }
                }
            }

            IList<DateValue> retentions = new List<DateValue>();
            foreach (string key in dic.Keys.OrderBy(c => c, new WeekComparer()))
            {
                retentions.Add(new DateValue() { Date = key, Value = dic[key] });
            }

            return retentions;
        }

        // To use HTTP GET, add [WebGet] attribute. (Default ResponseFormat is WebMessageFormat.Json)
        // To create an operation that returns XML,
        //     add [WebGet(ResponseFormat=WebMessageFormat.Xml)],
        //     and include the following line in the operation body:
        //         WebOperationContext.Current.OutgoingResponse.ContentType = "text/xml";
        [OperationContract]
        [WebGet]
        public Dictionary<string, int> GetMostUsedFeatures()
        {
            Dictionary<string, int> dic = new Dictionary<string, int>();
            Dictionary<string, int> dict = new Dictionary<string, int>();
            Dictionary<string, int> dics = new Dictionary<string, int>();

            string url = @"http://10.172.85.68:20550/sporequest/*/data/";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "Get";
            request.ContentType = "application/json";
            // request.Accept = "text/json";
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                XDocument doc = XDocument.Load(response.GetResponseStream());

                var rows = doc.Root.Descendants("Row");
                var monthMax = "data:count_2000-00";

                foreach (var row in rows)
                {
                    var key = Encoding.UTF8.GetString(System.Convert.FromBase64String(row.Attribute("key").Value));
                    var cellNum = row.Descendants("Cell").Count();
                    var cellCount = 0;
                    var cellContent = "data:count_2000-00";

                    for (var i = 0; i < cellNum; i++)
                    {
                        var temp1 = row.Descendants("Cell").ElementAt(i).Attribute("column").Value;
                        var temp2 = Convert.FromBase64String(temp1);
                        var temp3 = System.Text.Encoding.Default.GetString(temp2);
                        if ((temp3).IndexOf("data:count_2013-") >= 0)
                        {
                            if (cellContent.CompareTo(temp3) < 0 ? true : false)
                            {
                                cellContent = temp3;
                                cellCount = i;
                            }
                        }
                    }

                    if (cellContent.CompareTo(monthMax) > 0 ? true : false)
                    {
                        monthMax = cellContent;
                        // if the data is not the newest, the data before will be zero
                        foreach (KeyValuePair<string, int> item in dic)
                        {
                            dict[item.Key] = 0;
                        }

                        var cell = row.Descendants("Cell").ElementAt(cellCount);

                        var value = Int32.Parse(Encoding.UTF8.GetString(System.Convert.FromBase64String(cell.Value)));

                        if (!string.Equals("sitecollections", key) && !string.Equals("viewproperties", key))
                        {
                            dic[key] = value;
                            dict[key] = value;
                        }
                    }
                    else if (cellContent.CompareTo(monthMax) == 0 ? true : false)
                    {
                        var cell = row.Descendants("Cell").ElementAt(cellCount);

                        var value = Int32.Parse(Encoding.UTF8.GetString(System.Convert.FromBase64String(cell.Value)));

                        if (!string.Equals("sitecollections", key) && !string.Equals("viewproperties", key))
                        {
                            dic[key] = value;
                            dict[key] = value;
                        }
                    }

                }
                foreach (KeyValuePair<string, int> item in dict)
                {
                    if (dict[item.Key] >= 1)
                    {
                        dics[item.Key] = dict[item.Key];
                    }
                }
            }

            return dics;
        }

        [OperationContract]
        [WebGet]
        public IList<DateValue> GetFeatureTrend(string feature)
        {
            int userCount = 0;
            Dictionary<string, int> dic = new Dictionary<string, int>();

            string url = @"http://10.172.85.68:20550/sporequest/" + feature + "/data";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "Get";
            request.ContentType = "application/json";
            // request.Accept = "text/json";
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                XDocument doc = XDocument.Load(response.GetResponseStream());

                var cells = doc.Root.Descendants("Cell");

                foreach (var cell in cells)
                {
                    var key = Encoding.UTF8.GetString(System.Convert.FromBase64String(cell.Attribute("column").Value));
                    var value = Double.Parse(Encoding.UTF8.GetString(System.Convert.FromBase64String(cell.Value)));

                    if (key.Contains("data") && !key.Contains("data:count"))
                    {
                        string tmp = key.Split(new char[] { ':', })[1];
                        dic[tmp] = (int)value;
                    }
                }
            }

            IList<DateValue> retentions = new List<DateValue>();
            foreach (string key in dic.Keys.OrderBy(c => c, new WeekComparer()))
            {
                retentions.Add(new DateValue() { Date = key, Value = dic[key] });
            }

            return retentions;
        }

        [OperationContract]
        [WebGet]
        public IList<DateValue> GetFeaturePerfTrend(string feature)
        {
            int userCount = 0;
            Dictionary<string, int> dic = new Dictionary<string, int>();

            string url = @"http://10.172.85.68:20550/sporequest/" + feature + "/Perf";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "Get";
            request.ContentType = "application/json";
            // request.Accept = "text/json";
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                XDocument doc = XDocument.Load(response.GetResponseStream());

                var cells = doc.Root.Descendants("Cell");

                foreach (var cell in cells)
                {
                    var key = Encoding.UTF8.GetString(System.Convert.FromBase64String(cell.Attribute("column").Value));
                    var value = Double.Parse(Encoding.UTF8.GetString(System.Convert.FromBase64String(cell.Value)));

                    if (key.Contains("Perf"))
                    {
                        string tmp = key.Split(new char[] { ':', })[1];
                        dic[tmp] = (int)value;
                    }
                }
            }

            IList<DateValue> retentions = new List<DateValue>();
            foreach (string key in dic.Keys.OrderBy(c => c, new DayComparer()))
            {
                retentions.Add(new DateValue() { Date = key, Value = dic[key] });
            }

            return retentions;
        }

        [OperationContract]
        [WebGet]
        public IList<string> GetArlOption()
        {
            string url = @"http://10.172.85.68:20550/sporequest/*/arl/";
            IList<string> retentions = new List<string>();
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "Get";
            request.ContentType = "application/json";
            // request.Accept = "text/json";
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                XDocument doc = XDocument.Load(response.GetResponseStream());
                var rows = doc.Root.Descendants("Row");

                foreach (var row in rows)
                {
                    var key = Encoding.UTF8.GetString(System.Convert.FromBase64String(row.Attribute("key").Value));
                    retentions.Add(key);
                }

            }
            return retentions;
        }

        [OperationContract]
        [WebGet]
        public IList<DataArl> GetArlFeature(string feature)
        {
            Dictionary<string, int> dics = new Dictionary<string, int>();
            Dictionary<string, float> dicc = new Dictionary<string, float>();
            var supportMax = 0;
            var supportMaxKey = "";

            string url = @"http://10.172.85.68:20550/sporequest/" + feature + "/arl/";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "Get";
            request.ContentType = "application/json";
            // request.Accept = "text/json";
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                XDocument doc = XDocument.Load(response.GetResponseStream());
                var rows = doc.Root.Descendants("Row");
                var cells = doc.Root.Descendants("Cell");

                foreach (var row in rows)
                {
                    var key = Encoding.UTF8.GetString(System.Convert.FromBase64String(row.Attribute("key").Value));
                }

                foreach (var cell in cells)
                {
                    var key = Encoding.UTF8.GetString(System.Convert.FromBase64String(cell.Attribute("column").Value));
                    var value = Double.Parse(Encoding.UTF8.GetString(System.Convert.FromBase64String(cell.Value)));
                    int b = key.IndexOf(":");
                    int e = key.Length;
                    key = key.Substring(b + 1, e - 4);
                    //key = key.Replace(",", "<br />");
                    if (key.Contains("_confidence"))
                    {
                        string tmp = key.Substring(0, key.LastIndexOf("_"));
                        dicc[tmp] = (float)value;
                    }
                    if (key.Contains("_support"))
                    {
                        string tmp = key.Substring(0, key.LastIndexOf("_"));
                        dics[tmp] = (int)value;
                        if (supportMax < dics[tmp])
                        {
                            supportMax = dics[tmp];
                            supportMaxKey = tmp;
                        }
                    }
                }
            }

            if (supportMax == 0)
                return null;
            IList<DataArl> retentions = new List<DataArl>();
            foreach (string key in dicc.Keys)
            {
                if (dics[key] > 10 && dicc[key] > 0.1 && (float)(1.0 * dics[key] / supportMax) > 0.1)
                    retentions.Add(new DataArl() { DataValue = key, SupportValue = dics[key], ConfidenceValue = dicc[key] });
            }
            return retentions;
        }

        [OperationContract]
        [WebGet]
        public IList<DataArl> GetTotalArlFeature()
        {
            Dictionary<string, int> dics = new Dictionary<string, int>();
            Dictionary<string, float> dicc = new Dictionary<string, float>();

            string url = @"http://10.172.85.68:20550/sporequest/*/arl/";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "Get";
            request.ContentType = "application/json";
            // request.Accept = "text/json";
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                XDocument doc = XDocument.Load(response.GetResponseStream());

                var cells = doc.Root.Descendants("Cell");

                foreach (var cell in cells)
                {
                    var key = Encoding.UTF8.GetString(System.Convert.FromBase64String(cell.Attribute("column").Value));
                    var value = Double.Parse(Encoding.UTF8.GetString(System.Convert.FromBase64String(cell.Value)));
                    int b = key.IndexOf(":");
                    int e = key.Length;
                    key = key.Substring(b + 1, e - 4);
                    if (key.Contains("_confidence"))
                    {
                        string tmp = key.Substring(0, key.LastIndexOf("_"));
                        dicc[tmp] = (float)value;
                    }
                    if (key.Contains("_support"))
                    {
                        string tmp = key.Substring(0, key.LastIndexOf("_"));
                        dics[tmp] = (int)value;
                    }
                }
            }

            IList<DataArl> retentions = new List<DataArl>();
            foreach (string key in dicc.Keys)
            {
                if (dics.ContainsKey(key))
                {
                    if (dicc[key] > 0.1)
                        retentions.Add(new DataArl() { DataValue = key, SupportValue = dics[key], ConfidenceValue = dicc[key] });
                    /*
                    if (dics[key] > 20000)
                    {
                        if (dicc[key] > 0.7)
                        {
                            retentions.Add(new DataArl() { DataValue = key, SupportValue = dics[key], ConfidenceValue = dicc[key] });
                        }
                    }
                     */
                }
            }

            return retentions;
        }





        class WeekComparer : IComparer<string>
        {
            public int Compare(string X, string Y)
            {
                var xParts = X.Split(new char[] { '-' });
                var yParts = Y.Split(new char[] { '-' });

                if (Int32.Parse(xParts[0]) > Int32.Parse(yParts[0]))
                {
                    return 1;
                }
                else if (Int32.Parse(xParts[0]) < Int32.Parse(yParts[0]))
                {
                    return -1;
                }
                else
                {
                    if (Int32.Parse(xParts[1]) > Int32.Parse(yParts[1]))
                    {
                        return 1;
                    }
                    else if (Int32.Parse(xParts[1]) < Int32.Parse(yParts[1]))
                    {
                        return -1;
                    }
                    return 0;

                }
            }
        }

        class DayComparer : IComparer<string>
        {
            public int Compare(string X, string Y)
            {
                var xParts = X.Split(new char[] { '-' });
                var yParts = Y.Split(new char[] { '-' });

                if (Int32.Parse(xParts[0]) > Int32.Parse(yParts[0]))
                {
                    return 1;
                }
                else if (Int32.Parse(xParts[0]) < Int32.Parse(yParts[0]))
                {
                    return -1;
                }
                else
                {
                    if (Int32.Parse(xParts[1]) > Int32.Parse(yParts[1]))
                    {
                        return 1;
                    }
                    else if (Int32.Parse(xParts[1]) < Int32.Parse(yParts[1]))
                    {
                        return -1;
                    }
                    else
                    {
                        var xTemp = xParts[2].Substring(0, 1);
                        var yTemp = yParts[2].Substring(0, 1);
                        if (Int32.Parse(xTemp) > Int32.Parse(yTemp))
                        {
                            return 1;
                        }
                        else if (Int32.Parse(xTemp) < Int32.Parse(yTemp))
                        {
                            return -1;
                        }
                        return 0;
                    }
                }
            }
        }

        [DataContract]
        public class DateValue
        {
            [DataMember]
            public string Date
            {
                get;
                set;
            }

            [DataMember]
            public double Value
            {
                get;
                set;
            }
        }

        [DataContract]
        public class DataArl
        {
            [DataMember]
            public string DataValue
            {
                get;
                set;
            }

            [DataMember]
            public int SupportValue
            {
                get;
                set;
            }

            [DataMember]
            public float ConfidenceValue
            {
                get;
                set;
            }
        }

        // Add more operations here and mark them with [OperationContract]
    }
}
