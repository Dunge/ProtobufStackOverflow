using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProtoBuf.Grpc.Server;
using System;

namespace Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureServices((ctx, services) =>
                    {
                        services.AddCodeFirstGrpc(config =>
                        {
                            config.ResponseCompressionLevel = System.IO.Compression.CompressionLevel.Optimal;
                        });

                        services.AddGrpc(options =>
                        {
                            options.EnableDetailedErrors = true;
                        });

                        services.AddCodeFirstGrpcReflection();

                    });

                    webBuilder.ConfigureKestrel(serverOptions =>
                    {
                        serverOptions.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(30);
                        serverOptions.Limits.Http2.KeepAlivePingDelay = TimeSpan.FromSeconds(30);
                        serverOptions.Limits.Http2.KeepAlivePingTimeout = TimeSpan.FromSeconds(60);
                    });

                    webBuilder.UseStartup<Startup>();
                });
               
    }
}
