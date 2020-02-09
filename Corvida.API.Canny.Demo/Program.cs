using System;
using System.Drawing;

namespace Corvida.API.Canny.Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            String apiKey = "YourApiKeyHere";
            CannyClient client = new CannyClient(apiKey);
            try
            {
                using (Image watermarkedImage = client.CannyRequest(Image.FromFile("./img/demo.jpg"), 250, 150).Result)
                {
                    String path = System.IO.Directory.GetCurrentDirectory() + "/img/canny.jpg";
                    watermarkedImage.Save(path);
                    Console.WriteLine("Canny image: " + path);
                    Console.ReadLine();
                }
            }
            catch (BadRequestException ex)
            {
                Console.WriteLine("Could not execute request");
                Console.WriteLine(ex);
            }
        }
    }
}
