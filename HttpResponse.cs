using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace StreamSocketHttpServer
{
    /// <summary>
    /// Class representing a http response.
    /// Code from http://www.codeproject.com/Articles/17071/Sample-HTTP-Server-Skeleton-in-C,
    /// with some modifications.
    /// </summary>
    public class HttpResponse
    {
        /// <summary>
        /// Name of the http server that is sent with every response.
        /// </summary>
        private const string ServerName = "SimpleCsHttpServer";

        /// <summary>
        /// Dictionary with the http strings for the status codes.
        /// </summary>
        private static readonly MyDictionary<int, String> ResponseStatusStrings = new MyDictionary<int, string>
            {
                {200, "200 Ok"},
                {201, "201 Created"},
                {202, "202 Accepted"},
                {204, "204 No Content"},
                {301, "301 Moved Permanently"},
                {302, "302 Redirection"},
                {304, "304 Not Modified"},
                {400, "400 Bad Request"},
                {401, "401 Unauthorized"},
                {403, "403 Forbidden"},
                {404, "404 Not Found"},
                {500, "500 Internal Server Error"},
                {501, "501 Not Implemented"},
                {502, "502 Bad Gateway"},
                {503, "503 Service Unavailable"}
            };

        public HttpStatusCode StatusCode { get; set; }
        public string Version { get; set; }
        public string ContentType { get; set; }
        public long ContentLength { get; set; }
        public MyDictionary<string, string> Headers { get; private set; }
        public byte[] BodyData { get; set; }

        /// <summary>
        /// Creates and returns a default http response object for the given request.
        /// </summary>
        /// <param name="httpRequest">the request the created response object is based on</param>
        public static HttpResponse CreateDefaultHttpResponse(HttpRequest httpRequest)
        {
            Debug.Assert(httpRequest != null);

            var httpResponse = new HttpResponse();

            httpResponse.Version = "HTTP/1.1";

            if (httpRequest.IsValid)
                httpResponse.StatusCode = HttpStatusCode.BadRequest;
            else
            httpResponse.StatusCode = HttpStatusCode.Ok;

            httpResponse.Headers = new MyDictionary<String, String>();
            httpResponse.Headers.Add("Server", ServerName);
            httpResponse.Headers.Add("Date", DateTime.Now.ToString("r"));

            return httpResponse;
        }

        /// <summary>
        /// Write this http response to the given stream.
        /// </summary>
        /// <param name="outputStream">stream to write the response to</param>
        /// <returns></returns>
        public async Task Send(Stream outputStream)
        {
            Debug.Assert(outputStream != null);

            if(!String.IsNullOrEmpty(this.ContentType))
                this.Headers.Add("Content-Type", this.ContentType);

            if (this.ContentLength != 0)
                this.Headers.Add("Content-Length", this.ContentLength.ToString());

            string headersString = this.Version + " " + ResponseStatusStrings[(int)this.StatusCode] + "\n";

            foreach (var header in this.Headers)
            {
                headersString += header.Key + ": " + header.Value + "\n";
            }

            headersString += "\n";
            byte[] bHeadersString = Encoding.UTF8.GetBytes(headersString);

            // Send headers	
            await outputStream.WriteAsync(bHeadersString, 0, bHeadersString.Length);

            // Send body
            if (this.BodyData != null)
                await outputStream.WriteAsync(this.BodyData, 0, this.BodyData.Length);

            await outputStream.FlushAsync();
        }
    }
}