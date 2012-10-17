using System;
using NUnit.Framework;
using Rainier.Images;
using Rainier.SecurityToken;
using Rainier.WebClient;
using Sahara;

namespace UnitTests
{
    [TestFixture]
    public class Getting_a_session_token
    {
        [Test]
        public void It_should_get_a_token()
        {
            var webClient = new RainierWebClient();
            var token = new SecurityTokenRepository(webClient).GetSecurityToken();

            token.ShouldNotBeNull();
            Console.WriteLine(token);
        }
    }

    [TestFixture]
    public class Searching_for_images
    {
        [Test]
        public void It_should_get_results()
        {
            var webClient = new RainierWebClient();
            var results =
                new GettyImageRepository(webClient, new SecurityTokenRepository(webClient)).GetImageData("water");
            results.ShouldNotBeNull();
        }
    }
}
