﻿using AcBlog.Sdk;
using AcBlog.Tools.Sdk.Commands;
using AcBlog.Tools.Sdk.Models;
using AcBlog.Tools.Sdk.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StardustDL.Extensions.FileProviders;
using System;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Hosting;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace AcBlog.Tools.Sdk
{
    class Program
    {
        static async Task<int> Main(string[] args)
        {
            var rootCommand = new RootCommand("AcBlog SDK for command-line.");

            rootCommand.AddCommand(new InitCommand().Build());
            rootCommand.AddCommand(new RemoteCommand().Build());
            rootCommand.AddCommand(new NewCommand().Build());
            rootCommand.AddCommand(new ListCommand().Build());
            rootCommand.AddCommand(new PushCommand().Build());
            rootCommand.AddCommand(new CleanCommand().Build());

            var parser = new CommandLineBuilder(rootCommand).UseDefaults()
                .UseHost(args =>
                {
                    var host = Host.CreateDefaultBuilder();

                    host.ConfigureAppConfiguration((context, config) =>
                    {
                        config.AddJsonFile(Workspace.OptionPath,
                            optional: true,
                            reloadOnChange: true);
                        config.AddJsonFile(Workspace.DBPath,
                            optional: true,
                            reloadOnChange: true);
                        config.AddEnvironmentVariables("ACBLOG_");
                        config.AddCommandLine(args);
                    });
                    host.ConfigureLogging(log =>
                    {
                        log.AddFilter("Microsoft.Hosting.Lifetime", LogLevel.Warning);
                        log.AddFilter((category, level) =>
                        {
                            if (category.StartsWith("System."))
                                return level >= LogLevel.Warning;
                            return level >= LogLevel.Information;
                        });
                    });

                    return host;
                }, host =>
                {
                    host.ConfigureServices((context, services) =>
                    {
                        services.AddHttpClient();
                        services.AddOptions()
                            .Configure<WorkspaceOption>(context.Configuration.GetSection("acblog"))
                            .Configure<DB>(context.Configuration.GetSection("db"));
                        services.AddSingleton((services) =>
                        {
                            return new LocalBlogService(Environment.CurrentDirectory, new PhysicalFileProvider(Environment.CurrentDirectory).AsFileProvider());
                        });
                        services.AddSingleton<Workspace>();
                    });
                })
                .Build();
#if DEBUG
            while (true)
            {
                Console.Write("> ");
                string str = Console.ReadLine();
                try
                {
                    await parser.InvokeAsync(str);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
#else
            return await parser.InvokeAsync(args);
#endif
        }
    }
}
