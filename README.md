## Corvida.API.Canny Library 

Client implementation for the [Corvida Canny Edge Detection API](https://www.corvida.eu/watermark).

#### Target Framework
.NET Standard 2.0
#### Dependencies
- System.Drawing.Common (>= 4.7.0)
- System.Text.Json (>= 4.7.0)
### How to use
#### Create API client instance
```csharp
CannyClient client = new CannyClient("a6efde02-2aca-423a-90a0-fe4fe713d3e7"); //Replace with your key
```
The client is thread safe and can be used the whole application lifetime.
#### Make Request
```csharp
Image image = Image.FromFile("./img/demo.jpg");
using (Image cannyImage = (await client.CannyRequest(image, 250, 150)))
{
    // Do something with the api processed image
}
```
Incorrect requests will throw a BadRequestException with a error code and a descrption of the cause.

#### Request Parameters
```csharp
client.CannyRequest(System.Drawing.Image image)
client.CannyRequest(System.Drawing.Image image, double threshold1, double threshold2)
client.CannyRequest(System.Drawing.Image image, double threshold1, double threshold2, int apertureSize, bool l2Gradient)
```

## Corvida.API.Demo
Demo console application for Corvida Canny API Client. A sample image for the API call is in the /img subdirectory included.

#### Target Framework
.NET Core 3.1

#### How to use
1. Register or sign in at [https://corvida.eu](https://corvida.eu)
2. Generate new Canny API key in the [API Key Management](https://corvida.eu/apikey)
3. Replace the "YourApiKeyHere" string in the Main function with your new API key
4. Run console application
5. Check console output to get the path of the image with applied edge detection

#### Information
For simplicity, the request is done with .Result from the synchronous main method.

In a context in which an asynchronous call is possible, the request should always be executed with an await.
