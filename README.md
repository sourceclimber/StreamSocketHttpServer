# StreamSocketHttpServer
A simple HTTP server based on StreamSocketListener.

I used this implementation of an HTTP server for a Xamarin project including Windows 10 (Universal Platform) and Windows Phone 8.1, because the HttpListener class is not available for this platforms.
##Usage
Create a derived class from the HttpServer class and override the HandleRequest method.
```
class MusicServer : HttpServer
{
  /// <summary>
  /// This method handles each HTTP request to the server.
  /// </summary>
  public override async Task HandleRequest(HttpRequest request, HttpResponse response)
  {
    //Create the response depeding ond the request
  }
}
```
###Credits
For the HTTP request parser and the the response object is used the code from [Sample HTTP Server Skeleton in C#](http://www.codeproject.com/Articles/17071/Sample-HTTP-Server-Skeleton-in-C) from CodeProject.com.
