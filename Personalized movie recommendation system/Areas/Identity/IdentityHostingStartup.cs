using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Personalized_movie_recommendation_system.Data;
using Personalized_movie_recommendation_system.Models;

[assembly: HostingStartup(typeof(Personalized_movie_recommendation_system.Areas.Identity.IdentityHostingStartup))]
namespace Personalized_movie_recommendation_system.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
            });
        }
    }
}