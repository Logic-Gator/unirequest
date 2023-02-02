# UniRequest

UnityWebRequest wrapper to make Http Requests easier and straightforward in Unity. Easily build requests using method chaining.

Can be installed as a unity package with this git url.

```C#
// To start a new request just use UniRequest.(Method)
// Then you can method chain to modify the request before sending it
var result = await UniRequest.Get("https://github.com")
  .SetAuthorizationHeader("AuthorizationType", "Value")
  .SetHeader("CustomKey", "You can set custom headers")
  // You can easily set what download handler will you use, this will download a string in the result
  .DownloadText()
  .SendAsUniTask(); // Finish by sending the request, compatible with UniTask

Debug.Log(result.Status); // Result object contains many useful data like the status itself
Debug.Log(result.Data);   // Or the data, in this case a string that was set by DownloadText()

// We can upload jsons, form data or even multipart form data
// We can also download json responses
var response = await UniRequest.Post("url")
  .UploadWithJsonNet(new RegisterUserRequest("admin", "123456"))
  .DownloadWithJsonNet(default(RegisterUserResponse))
  .SendAsTask()
  // Errors are optionally thrown, so we can choose to handle errors with exceptions
  // or with the error object itself
  .ThrowOnError()
  // We can also extract the data itself if we don't want to do result.Data
  .GetData();
  
Debug.Log(response.userId);


var unityWebRequest = UnityWebRequest.Post("Test", "data");

unityWebRequest.AsUniRequest() // You can transform Unity's UnityWebRequests to UniRequests
  SetJsonContentTypeHeader() // And chain UniRequest builders
  // You can also set a custom sender for mocking or adding behaviour like authentication
  SendUsing(TaskRequestSender.Sender);
```
