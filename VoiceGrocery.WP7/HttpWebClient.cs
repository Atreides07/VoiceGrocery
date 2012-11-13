using System;
using System.IO;
using System.Net;

namespace VoiceGrocery.WP7
{
    public class HttpWebClient
    {
        private string url;
        private byte[] content;
        private Action<string> resultAction;

        public HttpWebClient()
        {

        }

        public void Post(string url, byte[] content, Action<string> result)
        {
            this.url = url;
            this.content = content;
            this.resultAction = result;

            var request = WebRequest.Create(new Uri(url, UriKind.Absolute));

            request.Method = "POST";
            request.BeginGetRequestStream(RequestStream, request);
        }

        private void RequestStream(IAsyncResult ar)
        {
            var request = ar.AsyncState as WebRequest;
            var stream = request.EndGetRequestStream(ar);

            stream.Write(content, 0, content.Length);
            stream.Close();
            request.BeginGetResponse(Response, request);
        }

        private void Response(IAsyncResult ar)
        {
            try
            {
                var request = ar.AsyncState as WebRequest;
                var response = request.EndGetResponse(ar);

                var result = new StreamReader(response.GetResponseStream()).ReadToEnd();
                resultAction.Invoke(result);
            }
            catch (Exception)
            {
            }
        }
    }
}