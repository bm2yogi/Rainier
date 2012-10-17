using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Nancy;
using Rainier.Properties;

namespace Rainier.Images
{
    public class ImagesModule : NancyModule
    {
        public static Random Random = new Random();

        public ImagesModule() : base("images")
        {
            Get[@"/"] = args => Response.FromStream(() =>
                                                        {
                                                            var stream = new MemoryStream();
                                                            RandomImage.Save(stream, ImageFormat.Jpeg);
                                                            stream.Position = 0;
                                                            return stream;
                                                        }, "image/jpg"
                                    );
        }

        private Bitmap RandomImage
        {
            get
            {
                var index = Random.Next(0, 10);

                return new []
                           {
                               Resources.comp,
                               Resources._137551881,
                               Resources._200366222_001,
                               Resources.dv166010,
                               Resources.sb10068251n_001,
                               Resources.BU000084,
                               Resources._109882640,
                               Resources._142955305,
                               Resources._200257682_001,
                               Resources._200355486_007,
                               Resources._99289653,
                           }[index];
            }
        }
    }
}