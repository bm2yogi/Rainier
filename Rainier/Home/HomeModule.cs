using System;
using Nancy;
using Rainier.Images;

namespace Rainier.Home
{
    public class HomeModule : NancyModule
    {
        private readonly IImageFactory _imageFactory;

        public HomeModule(IImageFactory imageFactory)
        {
            _imageFactory = imageFactory;

            Get[@"{height}/{width}"] = GetImageStream;

            Get[@"{query}/{height}/{width}"] = GetImageStream;

            Get[@"/"] = args => View["index.htm"];
        }

        private Func<dynamic, Response> GetImageStream
        {
            get { return args => FromStream(args); }
        }

        private Response FromStream(dynamic args)
        {
            return Response.FromStream(() => _imageFactory
                .GetImage(height: args.height, width: args.width, query: args.query),
                "image/jpg");
        }
    }
}