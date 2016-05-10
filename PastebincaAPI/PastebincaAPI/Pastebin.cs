using System.ComponentModel;
using System.IO;
using System.Net;
using System.Net.Mime;
using System.Text;

namespace PastebincaAPI
{
    /// <summary>
    /// This Library was created by Thomas 'Dotty' Michelot
    /// .  .  .  thomas.michelot@epita.fr
    /// </summary>
    /// <contact>
    /// thomas.michelot@epita.fr
    /// </contact>
    public class Pastebin
    {
        #region Attributes and Getters/Setters

        private string pastebinca_url = @"http://pastebin.ca/";
        private string pastebinca_post_url = @"http://pastebin.ca/quiet-paste.php";
        private string pastebinca_apikeyurl = @"http://pastebin.ca/apikey.php";
        private ASCIIEncoding encoding = new ASCIIEncoding();
        private string apikey = string.Empty;
        private string postData = string.Empty;
        private byte[] data;
        private HttpWebRequest myRequest;
        private string webResponse;

        public string Response
        {
            get { return this.webResponse; }
            private set { this.webResponse = value; }
        }

        #endregion

        #region Constructor and Public Methods

        /// <summary>
        /// Create a new Pastebin object and get a new apikey
        /// </summary>
        public Pastebin()
        {
            GetApiKey();
        }

        /// <summary>
        /// Send the Paste with all the data you gave
        /// </summary>
        /// <param name="content">The body of your Paste</param>
        /// <param name="name">The title of your Paste</param>
        /// <param name="description">The Description of your Paste</param>
        /// <returns>The URL to reach your Paste</returns>
        public string SendPast(string content, string name, string description, PasteContentType type, Expiration expiration)
        {
            string returnurl = this.pastebinca_url;
            SetData(content, name, description,type,expiration);
            SetRequest();
            SendForm();
            returnurl += this.Response.Split(':')[1];
            return returnurl;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Get a new API Key form pastebin.ca
        /// </summary>
        private string GetApiKey()
        {
            var req = (HttpWebRequest) WebRequest.Create(this.pastebinca_apikeyurl);
            var rep = req.GetResponse();
            var repStream = rep.GetResponseStream();
            var repReader = new StreamReader(repStream);
            this.apikey = repReader.ReadToEnd();
            repReader.Close();
            repStream.Close();
            rep.Close();
            return this.apikey;
        }

        /// <summary>
        /// Set the postData ready and encodes it to data
        /// </summary>
        /// <param name="content">The content of your Paste</param>
        /// <param name="name">The name of your Paste</param>
        /// <param name="description">The descriptin of your Paste</param>
        /// <param name="type">The type of the content of your Paste</param>
        /// <param name="expiration">The time your Paste will last</param>
        private void SetData(string content, string name, string description, PasteContentType type, Expiration expiration)
        {
            this.postData = "api=" + apikey;
            this.postData += "&content=" + content;
            this.postData += "&name=" + name;
            this.postData += "&description=" + description;
            this.postData += "&type=" + (int)type;
            this.postData += "&expiry=" + expiration.ToDescriptionString();
            this.data = this.encoding.GetBytes(postData);
        }

        /// <summary>
        /// Setup the HttWebRequest with the data to be submitted
        /// </summary>
        private void SetRequest()
        {
            this.myRequest = (HttpWebRequest) WebRequest.Create(this.pastebinca_post_url);
            myRequest.Method = WebRequestMethods.Http.Post;
            myRequest.ContentType = new ContentType("application/x-www-form-urlencoded").MediaType;
            myRequest.ContentLength = this.data.Length;
        }

        /// <summary>
        /// Send the HttpWebRequests with the data
        /// </summary>
        private void SendForm()
        {
            var newStream = this.myRequest.GetRequestStream();
            newStream.Write(data, 0, data.Length);
            newStream.Close();
            GetResponse();
        }

        /// <summary>
        /// Get the response after the data was submitted
        /// </summary>
        /// <returns>The Raw data (e.g: SUCCESS:1234567)</returns>
        private string GetResponse()
        {
            var response = this.myRequest.GetResponse();
            var responseStream = response.GetResponseStream();
            var responseReader = new StreamReader(responseStream);
            var result = responseReader.ReadToEnd();
            this.Response = result;
            responseReader.Close();
            responseStream.Close();
            response.Close();
            return result;
        }

        #endregion
    }

    /// <summary>
    /// This is the enum containing all the types that you can use
    /// </summary>
    /// used as follow in the code
    /// <code>int pasteType = (int)PasteContentType.Raw;</code>
    public enum PasteContentType
    {
        Raw = 1,
        AsteriskConfiguration = 2,
        C = 3,
        CPP=4,
        PHP = 5,
        Perl = 6,
        Java = 7,
        VisualBasic = 8,
        CSharp=9,
        Ruby = 10,
        Python = 11,
        Pascal = 12,
        mIRCScript = 13,
        PL_I = 14,
        XML = 15,
        SQLStatement = 16,
        SchemeSource = 17,
        ActionScript = 18,
        AdaSource = 19,
        ApacheConfiguration = 20,
        AssemblyNASM = 21,
        ASP = 22,
        Bash = 23,
        CSS = 24,
        Delphi = 25,
        HTML4Dot0Strict = 26,
        JavaScript = 27,
        LISP = 28,
        Lua = 29,
        MicroprocessorASM = 30,
        ObjectiveC = 31,
        VisualBasicDotNET = 32
    }

    /// <summary>
    /// This is the enum containing all the differents durations for your Paste
    /// </summary>
    /// used as follow in the code
    /// <code>string expiration = Expiration.Never.ToDescriptionString();</code>
    public enum Expiration {
        [Description("Never")]
        Never,
        [Description("5 minutes")]
        min5,
        [Description("10 minutes")]
        min10,
        [Description("15 minutes")]
        min15,
        [Description("30 minutes")]
        min30,
        [Description("45 minutes")]
        min45,
        [Description("1 hour")]
        hour1,
        [Description("2 hours")]
        hour2,
        [Description("4 hours")]
        hour4,
        [Description("8 hours")]
        hour8,
        [Description("12 hours")]
        hour12,
        [Description("1 day")]
        day1,
        [Description("2 days")]
        day2,
        [Description("3 days")]
        day3,
        [Description("1 week")]
        week1,
        [Description("2 weeks")]
        week2,
        [Description("3 weeks")]
        week3,
        [Description("1 month")]
        month1,
        [Description("2 months")]
        month2,
        [Description("3 months")]
        month3,
        [Description("4 months")]
        month4,
        [Description("5 months")]
        month5,
        [Description("6 months")]
        month6,
        [Description("1 year")]
        year1
    }

    /// <summary>
    /// Get my Enum Expiration to the correct string description
    /// </summary>
    /// To see the original post: https://goo.gl/m1K2GL
    public static class ExpirationExtention
    {
        public static string ToDescriptionString(this Expiration val)
        {
            DescriptionAttribute[] attributes = (DescriptionAttribute[])val.GetType().GetField(val.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : string.Empty;
        }
    }

}