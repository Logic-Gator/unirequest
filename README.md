# UniRequest

UnityWebRequest wrapper to make Http Requests easier and straightforward in Unity.

```C#
// To start a new request just use UniRequest.(Method)
// Then you can chain to modify the request before sending it
var result = await UniRequest.Get("https://github.com")
  .SetAuthorizationHeader("AuthorizationType", "Value")
  .SetHeader("CustomKey", "You can set custom headers")
  .DownloadText() // You can easily set what download handler will you use, this will download a string in the result
  .SendAsUniTask(); // Finish by sending the request, compatible with UniTask
  
Debug.Log(result.Data);

var unityWebRequest = UnityWebRequest.Post("Test", "data");
unityWebRequest.AsUniRequest() // You can transform Unity's UnityWebRequests to UniRequests
  SetJsonContentTypeHeader() // And chain UniRequest builders
  SendUsing(TaskRequestSender.Sender); // You can also set a custom sender for mocking or adding behaviour like authentication
```
