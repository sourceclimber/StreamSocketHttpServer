using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Networking.Sockets;


namespace StreamSocketHttpServer
{
    /// <summary>
    /// Implementation of a simple HTTP server.
    /// How to use: Inherit from this class and override the abstract HandleRequest() method.
    /// </summary>
    public abstract class HttpServer : IDisposable
    {
        private StreamSocketListener socketListener;
        private readonly int port;

        protected HttpServer(int port)
        {
            this.port = port;
            IgnoreExceptions = true;
        }

        public bool IgnoreExceptions { get; set; }

        public async void Start()
        {
            socketListener = new StreamSocketListener();
            socketListener.ConnectionReceived += OnConnectionReceived;

            await socketListener.BindServiceNameAsync(port.ToString());
        }

        public void Stop()
        {
            Dispose();
        }

        private void OnConnectionReceived(StreamSocketListener sender, StreamSocketListenerConnectionReceivedEventArgs args)
        {
            ProcessRequestAsync(args.Socket);
        }

        /// <summary>
        /// Process an incomming rquest.
        /// </summary>
        /// <param name="socket">the socket associated with the request</param>
        private async void ProcessRequestAsync(StreamSocket socket)
        {
            try
            {
                using (var inputStream = socket.InputStream.AsStreamForRead())
                using (var outputStream = socket.OutputStream.AsStreamForWrite())
                {
                    var request = await HttpRequest.ParseFromStream(inputStream);   //Read the http request

                    var response = HttpResponse.CreateDefaultHttpResponse(request); //Create the default response object for this request

                    if(request.IsValid)
                        await HandleRequest(request, response);     //call the abstract method if the request is valid
                    
                    await response.Send(outputStream);
                }
            }
            catch (Exception)
            {
                if (!IgnoreExceptions)  //Only throw exceptions if they are enabled
                    throw;
            }
            finally
            {
                socket.Dispose();   //This call is important: This closes the connection to the client.
            }
        }

        /// <summary>
        /// Abstract method that gets called for every incomming request.
        /// Override this method with the specific behavior of the http server.
        /// This method should be implemented asynchronously.
        /// </summary>
        /// <param name="request">the request object for this request</param>
        /// <param name="response">the response object for this request</param>
        public abstract Task HandleRequest(HttpRequest request, HttpResponse response);

        public void Dispose()
        {
            if (socketListener != null)
            {
                socketListener.ConnectionReceived -= OnConnectionReceived;
                socketListener.Dispose();
                socketListener = null;
            }
        } 
    }
}
