using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TradeTweet
{
    class TwittwerService
    {
        public bool Connected { get { return User != null; } }
        public User User { get; private set; } 
        string oauth_token = "";
        string oauth_token_secret = "";

        // oauth implementation details
        const string oauth_consumer_key = "wjuqr6fFh08Mbk5cGg0h3nc70";
        const string oauth_consumer_secret = "neCI38D28I2ar7yxalV0y6itpAtNGTEig3dhGKwuuOkouKXIyb";
        const string oauth_version = "1.0";
        const string oauth_signature_method = "HMAC-SHA1";

        public TwittwerService(string token = "", string secret_token = "")
        {
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(secret_token)) return;

            oauth_token = token;
            oauth_token_secret = secret_token;

            GetCredentials();
        }

        Dictionary<string, string> SendPOSTRequest(Twitt twitt, string resource_url, string media = "", string token = "")
        {
            // unique request details
            var oauth_nonce = Convert.ToBase64String(
                new ASCIIEncoding().GetBytes(DateTime.Now.Ticks.ToString()));

            var timeSpan = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            var oauth_timestamp = Convert.ToInt64(timeSpan.TotalSeconds).ToString();

            string headerMemberName = (token != "") ? "oauth_verifier" : "status";
            var oauth_token = (token == "") ? this.oauth_token : token;

            string baseString = null;

            if (twitt.Media == null)
            {

                string baseFormat = "oauth_consumer_key={0}&oauth_nonce={1}&oauth_signature_method={2}" +
                            "&oauth_timestamp={3}&oauth_token={4}&oauth_version={5}&" + headerMemberName + "={6}";


                baseString = string.Format(baseFormat,
                                                oauth_consumer_key,
                                                oauth_nonce,
                                                oauth_signature_method,
                                                oauth_timestamp,
                                                oauth_token,
                                                oauth_version,
                                                Uri.EscapeDataString(twitt.Text)
                                                );

            }
            else
            {
                // create oauth signature
                var baseFormat = "oauth_consumer_key={0}&oauth_nonce={1}&oauth_signature_method={2}" +
                                "&oauth_timestamp={3}&oauth_token={4}&oauth_version={5}";

                baseString = string.Format(baseFormat,
                                            oauth_consumer_key,
                                            oauth_nonce,
                                            oauth_signature_method,
                                            oauth_timestamp,
                                            oauth_token,
                                            oauth_version
                                            );

                if (!string.IsNullOrEmpty(media))
                    baseString = "media_ids=" + Uri.EscapeDataString(media) + "&" + baseString + "&place_id=NoInput&status=" + Uri.EscapeDataString(twitt.Text);
            }


            baseString = string.Concat("POST&", Uri.EscapeDataString(resource_url), "&", Uri.EscapeDataString(baseString));

            var compositeKey = string.Concat(Uri.EscapeDataString(oauth_consumer_secret),
                                    "&", Uri.EscapeDataString(oauth_token_secret));

            string oauth_signature;
            using (System.Security.Cryptography.HMACSHA1 hasher = new System.Security.Cryptography.HMACSHA1(ASCIIEncoding.ASCII.GetBytes(compositeKey)))
            {
                oauth_signature = Convert.ToBase64String(
                    hasher.ComputeHash(ASCIIEncoding.ASCII.GetBytes(baseString)));
            }

            string authHeader = null;

            if (twitt.Media == null)
            {
                // create the request header
                var headerFormat = "OAuth oauth_consumer_key=\"{0}\", oauth_nonce=\"{1}\", " +
                                   "oauth_signature_method=\"{2}\", oauth_timestamp=\"{3}\", " +
                                   "oauth_token=\"{4}\", oauth_version=\"{5}\", " +
                                   "oauth_signature=\"{6}\", " + headerMemberName + "=\"{7}\"";

                authHeader = string.Format(headerFormat,
                                        Uri.EscapeDataString(oauth_consumer_key),
                                        Uri.EscapeDataString(oauth_nonce),
                                        Uri.EscapeDataString(oauth_signature_method),
                                        Uri.EscapeDataString(oauth_timestamp),
                                        Uri.EscapeDataString(oauth_token),
                                        Uri.EscapeDataString(oauth_version),
                                        Uri.EscapeDataString(oauth_signature),
                                        Uri.EscapeDataString(twitt.Text)
                                );

                //if (!string.IsNullOrEmpty(media))
                //    authHeader += $"media_ids=\"{media}\"";
            }
            else
            {
                // create the request header
                var headerFormat = "OAuth oauth_consumer_key=\"{0}\", oauth_nonce=\"{1}\", " +
                                   "oauth_signature=\"{2}\", oauth_signature_method=\"{3}\", " +
                                   "oauth_timestamp=\"{4}\", oauth_token=\"{5}\", " +
                                   "oauth_version=\"{6}\"";

                authHeader = string.Format(headerFormat,
                                        Uri.EscapeDataString(oauth_consumer_key),
                                        Uri.EscapeDataString(oauth_nonce),
                                        Uri.EscapeDataString(oauth_signature),
                                        Uri.EscapeDataString(oauth_signature_method),
                                        Uri.EscapeDataString(oauth_timestamp),
                                        Uri.EscapeDataString(oauth_token),
                                        Uri.EscapeDataString(oauth_version)
                                );

                //if (!string.IsNullOrEmpty(media))
                //    authHeader = authHeader + ", media_ids=\"" + Uri.EscapeDataString(media) + "\", place_id=\"NoInput\", status=\"" + Uri.EscapeDataString(twitt.Text)+"\"";
            }

            HttpWebRequest request = null;

            if (twitt.Media == null)
            {
                request = (HttpWebRequest)WebRequest.Create(resource_url);
                request.Headers.Add("Authorization", authHeader);
                request.Method = "POST";


                // make the request
                string postBody = "status=" + Uri.EscapeDataString(twitt.Text);

                ServicePointManager.Expect100Continue = false;

                request.ContentType = "application/x-www-form-urlencoded";
                using (System.IO.Stream stream = request.GetRequestStream())
                {
                    byte[] content = ASCIIEncoding.ASCII.GetBytes(postBody);
                    stream.Write(content, 0, content.Length);
                }
            }
            else
            {

                if (string.IsNullOrEmpty(media))
                {
                    Encoding EncodingAlgorithm = Encoding.GetEncoding("iso-8859-1");
                    string Boundary = DateTime.Now.Ticks.ToString("x");
                    string StartBoundary = string.Format("--{0}\r\n", Boundary);
                    string EndBoundary = string.Format("\r\n--{0}--\r\n", Boundary);
                    string sContentType = "multipart/form-data;boundary=" + Boundary;
                    string contentDisposition = "Content-Disposition: form-data; ";
                    string contenName = "name=\"media\";\r\n ";
                    string contentType = "\r\nContent-Type: application/octet-stream\r\n\r\n";
                    string Data = EncodingAlgorithm.GetString(twitt.Media, 0, twitt.Media.Length);
                    var contents = new System.Text.StringBuilder();
                    contents.Append(String.Format("{0}{1}{2}{3}{4}", StartBoundary, contentDisposition, contenName, contentType, Data));
                    contents.Append(EndBoundary);
                    byte[] Content = EncodingAlgorithm.GetBytes(contents.ToString());

                    request = (HttpWebRequest)WebRequest.Create(resource_url);
                    request.Method = "POST";
                    request.AllowWriteStreamBuffering = true;
                    request.Headers.Add("Authorization", authHeader);
                    request.ContentType = sContentType;
                    request.ContentLength = Content.Length;

                    Stream dataStream = request.GetRequestStream();
                    dataStream.Write(Content, 0, Content.Length);
                    dataStream.Close();
                }
                else
                {
                    request = (HttpWebRequest)WebRequest.Create(resource_url);
                    request.Method = "POST";
                    request.ContentType = "application/x-www-form-urlencoded; charset=utf-8";
                    request.UseDefaultCredentials = true;
                    request.Headers.Add("Authorization", authHeader);
                    request.UserAgent = "LINQ-To-Twitter/3.0";
                    request.Headers.Add("ExpectContinue", "true");
                    request.Headers.Add("CacheControl", "no-cache");
                    request.Date = DateTime.UtcNow;

                    // make the request
                    string postBody = "status=" + Uri.EscapeDataString(twitt.Text) + "&place_id=NoInput&media_ids=" + media;
                    byte[] content = ASCIIEncoding.UTF8.GetBytes(postBody);

                    request.ContentLength = content.Length;
                    ServicePointManager.Expect100Continue = false;

                    using (System.IO.Stream stream = request.GetRequestStream())
                    {

                        stream.Write(content, 0, content.Length);
                    }

                }
            }

            Dictionary<string, string> responseItems = new Dictionary<string, string>();

            try
            {
                var response = (HttpWebResponse)request.GetResponse();

                using (var reader = new System.IO.StreamReader(response.GetResponseStream(), ASCIIEncoding.ASCII))
                {
                    string responseText = reader.ReadToEnd();
                    string[] tokens = responseText.Split(new char[] { '&', '=' });

                    if (tokens.Length % 2 == 0) // token's response
                    {
                        for (int i = 0; i < tokens.Length; i += 2)
                        {
                            responseItems[tokens[i]] = tokens[i + 1];
                        }
                    }
                    else // twitts response
                    {
                        responseItems[twitt.Text] = responseText;
                    }
                }
            }
            catch (Exception ex)
            {
                responseItems["Error"] = ex.Message;
            }

            return responseItems;
        }

        void GetCredentials()
        {
            var resource_url = "https://api.twitter.com/1.1/account/verify_credentials.json";

            // unique request details
            var oauth_nonce = Convert.ToBase64String(
                new ASCIIEncoding().GetBytes(DateTime.Now.Ticks.ToString()));

            var timeSpan = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            var oauth_timestamp = Convert.ToInt64(timeSpan.TotalSeconds).ToString();

            string baseString = null;


            string baseFormat = "oauth_consumer_key={0}&oauth_nonce={1}&oauth_signature_method={2}" +
                            "&oauth_timestamp={3}&oauth_token={4}&oauth_version={5}";


            baseString = string.Format(baseFormat,
                                                oauth_consumer_key,
                                                oauth_nonce,
                                                oauth_signature_method,
                                                oauth_timestamp,
                                                oauth_token,
                                                oauth_version
                                                );



            baseString = string.Concat("GET&", Uri.EscapeDataString(resource_url), "&", Uri.EscapeDataString(baseString));

            var compositeKey = string.Concat(Uri.EscapeDataString(oauth_consumer_secret),
                                    "&", Uri.EscapeDataString(oauth_token_secret));

            string oauth_signature;

            using (System.Security.Cryptography.HMACSHA1 hasher = new System.Security.Cryptography.HMACSHA1(ASCIIEncoding.ASCII.GetBytes(compositeKey)))
            {
                oauth_signature = Convert.ToBase64String(
                    hasher.ComputeHash(ASCIIEncoding.ASCII.GetBytes(baseString)));
            }

            string authHeader = null;


            // create the request header
            var headerFormat = "OAuth oauth_consumer_key=\"{0}\", oauth_nonce=\"{1}\", " +
                               "oauth_signature=\"{2}\", oauth_signature_method=\"{3}\", " +
                               "oauth_timestamp=\"{4}\", oauth_token=\"{5}\", " +
                               "oauth_version=\"{6}\"";

            authHeader = string.Format(headerFormat,
                                    Uri.EscapeDataString(oauth_consumer_key),
                                    Uri.EscapeDataString(oauth_nonce),
                                    Uri.EscapeDataString(oauth_signature),
                                    Uri.EscapeDataString(oauth_signature_method),
                                    Uri.EscapeDataString(oauth_timestamp),
                                    Uri.EscapeDataString(oauth_token),
                                    Uri.EscapeDataString(oauth_version)
                            );


            HttpWebRequest request = null;


            request = (HttpWebRequest)WebRequest.Create(resource_url);
            request.Headers.Add("Authorization", authHeader);
            request.Method = "GET";
            request.AutomaticDecompression = DecompressionMethods.GZip;

            ServicePointManager.Expect100Continue = false;
            request.KeepAlive = true;

            try
            {
                var response = (HttpWebResponse)request.GetResponse();

                using (var reader = new System.IO.StreamReader(response.GetResponseStream(), ASCIIEncoding.ASCII))
                {
                    string responseText = reader.ReadToEnd();

                    User = JsonConvert.DeserializeObject<User>(responseText);

                    HttpWebRequest req = (HttpWebRequest)WebRequest.Create(User.profile_image_url_https);
                    var resp = (HttpWebResponse)req.GetResponse();

                    User.avatar = System.Drawing.Image.FromStream(resp.GetResponseStream());
                }
            }
            catch (Exception ex)
            {
                
            }
        }

        public Response Connect()
        {
            Response resp = new Response();

            var request_url = "https://api.twitter.com/oauth/request_token";
            Twitt empty = new Twitt() { Text = "", Media = null };
            var response = SendPOSTRequest(empty, request_url);

            if (response.Keys.Contains("Error"))
            {
                resp.Failed = true;
                resp.Text = response["Error"];
                return resp;
            }

            // oauth token
            this.oauth_token = response["oauth_token"];
            this.oauth_token_secret = response["oauth_token_secret"];

            string verifier_url = "https://api.twitter.com/oauth/authorize?oauth_token=" + oauth_token;
            System.Diagnostics.Process.Start(verifier_url);

            return resp;
        }

        public Response SetToken(string oauth_verifier)
        {
            Response resp = new Response();

            if (oauth_verifier == null)
            {
                resp.Failed = true;
                resp.Text = "Bad PIN code!";
                Disconnect();
                return resp;
            }

            Twitt verify = new Twitt() { Text = oauth_verifier, Media = null };
            string access_url = "https://api.twitter.com/oauth/access_token";

            var response = SendPOSTRequest(verify, access_url, null, oauth_token);

            if (response.Keys.Contains("Error"))
            {
                resp.Failed = true;
                resp.Text = response["Error"];
                return resp;
            }

            // access_token_secret
            this.oauth_token = response["oauth_token"];
            this.oauth_token_secret = response["oauth_token_secret"];

            GetCredentials();

            return resp;
        }

        public async Task<string> SendTweetAsync(Twitt twitt, string mediaIds, CancellationToken ct)
        {
            return await Task.Factory.StartNew(() =>
            {

                if (!Connected) return "Service is not verified!";

                var resource_url = "https://api.twitter.com/1.1/statuses/update.json";

                var res = SendPOSTRequest(twitt, resource_url, mediaIds);

                return res[twitt.Text];
            }, ct);
        }

        public async Task<string> SendImageAsync(System.Drawing.Image img, CancellationToken ct)
        {
            return await Task.Factory.StartNew(() =>
            {

                if (!Connected) return "Service is not verified!";

                var resource_url = "https://upload.twitter.com/1.1/media/upload.json";

                byte[] bytes = null;

                using (var ms = new MemoryStream())
                {
                    img.Save(ms, img.RawFormat);
                    bytes = ms.ToArray();
                }

                var key = DateTime.UtcNow.ToBinary().ToString();

                Twitt twitt = new Twitt() { Media = bytes, Text = key };

                var res = SendPOSTRequest(twitt, resource_url);

                return res[key];
            }, ct);
        }

        HttpWebRequest GetStreamRequest()
        {
            HttpWebRequest request = null;
            string resource_url = "https://userstream.twitter.com/1.1/user.json";

            // unique request details
            var oauth_nonce = Convert.ToBase64String(
                new ASCIIEncoding().GetBytes(DateTime.Now.Ticks.ToString()));

            var timeSpan = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            var oauth_timestamp = Convert.ToInt64(timeSpan.TotalSeconds).ToString();

            string baseString = null;

            string baseFormat = "oauth_consumer_key={0}&oauth_nonce={1}&oauth_signature_method={2}" +
                        "&oauth_timestamp={3}&oauth_token={4}&oauth_version={5}";


            baseString = string.Format(baseFormat,
                                            oauth_consumer_key,
                                            oauth_nonce,
                                            oauth_signature_method,
                                            oauth_timestamp,
                                            oauth_token,
                                            oauth_version
                                            );



            baseString = string.Concat("POST&", Uri.EscapeDataString(resource_url), "&", Uri.EscapeDataString(baseString));

            var compositeKey = string.Concat(Uri.EscapeDataString(oauth_consumer_secret),
                                    "&", Uri.EscapeDataString(oauth_token_secret));

            string oauth_signature;
            using (System.Security.Cryptography.HMACSHA1 hasher = new System.Security.Cryptography.HMACSHA1(ASCIIEncoding.ASCII.GetBytes(compositeKey)))
            {
                oauth_signature = Convert.ToBase64String(
                    hasher.ComputeHash(ASCIIEncoding.ASCII.GetBytes(baseString)));
            }

            string authHeader = null;

            // create the request header
            var headerFormat = "OAuth oauth_consumer_key=\"{0}\", oauth_nonce=\"{1}\", " +
                               "oauth_signature_method=\"{2}\", oauth_timestamp=\"{3}\", " +
                               "oauth_token=\"{4}\", oauth_version=\"{5}\", " +
                               "oauth_signature=\"{6}\"";

            authHeader = string.Format(headerFormat,
                                    Uri.EscapeDataString(oauth_consumer_key),
                                    Uri.EscapeDataString(oauth_nonce),
                                    Uri.EscapeDataString(oauth_signature_method),
                                    Uri.EscapeDataString(oauth_timestamp),
                                    Uri.EscapeDataString(oauth_token),
                                    Uri.EscapeDataString(oauth_version),
                                    Uri.EscapeDataString(oauth_signature)
                            );

            request = (HttpWebRequest)WebRequest.Create(resource_url);

            request.Proxy = null;

            request.Headers.Add("Authorization", authHeader);
            request.Method = "POST";

            ServicePointManager.Expect100Continue = false;

            request.ContentType = "application/x-www-form-urlencoded";


            return request;
        }

        public event EventHandler OnNewEvent = delegate { };

        public async void EventEngine(Action<string> StreamingCallbackAsync = null)
        {
            var request = GetStreamRequest();
            request.Timeout = Timeout.Infinite;

            var res = await request.GetResponseAsync();

            Stream stream = res.GetResponseStream();

            const int CarriageReturn = 0x0D;
            const int LineFeed = 0x0A;

            var memStr = new MemoryStream();
            byte[] readByte;

            while (stream.CanRead)
            {
                readByte = new byte[1];
                await stream.ReadAsync(readByte, 0, 1);
                byte nextByte = readByte.SingleOrDefault();

                //if (nextByte == -1) break;

                if (nextByte != CarriageReturn && nextByte != LineFeed)
                    memStr.WriteByte(nextByte);

                if (nextByte == LineFeed)
                {
                    int byteCount = (int)memStr.Length;
                    byte[] tweetBytes = new byte[byteCount];

                    memStr.Position = 0;
                    await memStr.ReadAsync(tweetBytes, 0, byteCount).ConfigureAwait(false);

                    string tweet = Encoding.UTF8.GetString(tweetBytes, 0, byteCount);

                    if (tweet.Length > 0)
                        OnNewEvent(tweet, EventArgs.Empty);

                    StreamingCallbackAsync(tweet);

                    memStr.Dispose();
                    memStr = new MemoryStream();
                }
            }
        }

        public void Disconnect()
        {
            oauth_token = "";
            oauth_token_secret = "";
            User = null;
        }
    }

    class User
    {
        public string screen_name;
        public System.Drawing.Image avatar;
        public string profile_image_url_https;
    }

    class Twitt
    {
        public string Text;
        public byte[] Media;
    }

    class Response
    {
        public bool Failed { get; set; }
        public string Text { get; set; }
    }
}
