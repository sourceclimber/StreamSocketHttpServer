# StreamSocketHttpServer
A simple HTTP server based on StreamSocketListener.


## Usage
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

### Credits
For the HTTP request parser and the the response object is used the code from [Sample HTTP Server Skeleton in C#](http://www.codeproject.com/Articles/17071/Sample-HTTP-Server-Skeleton-in-C) from CodeProject.com.
