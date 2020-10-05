﻿using Microsoft.Extensions.DependencyInjection;
using AcBlog.UI.Components.Loading;
using AcBlog.UI.Components.Markdown;
using AcBlog.UI.Components.Slides;
using AcBlog.UI.Components.AntDesigns;
using AcBlog.Extensions;
using AcBlog.Client.WebAssembly.Models;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace AcBlog.Client.WebAssembly
{
    public static class ServiceExtensions
    {
        public static void AddUIComponents(this IServiceCollection services)
        {
            services.AddExtensions()
                .AddExtension<ClientUIComponent>()
                .AddExtension<LoadingUIComponent>()
                .AddExtension<MarkdownUIComponent>()
                .AddExtension<SlidesUIComponent>()
                .AddExtension<AntDesignUIComponent>();
        }

        public static void AddClientAuthorization(this IServiceCollection services, IdentityProvider identityProvider)
        {
            if (identityProvider.Enable)
            {
                services.AddApiAuthorization(options =>
                {
                    options.ProviderOptions.ConfigurationEndpoint = identityProvider.Endpoint;
                    options.AuthenticationPaths.RemoteProfilePath = identityProvider.RemoteProfilePath;
                    options.AuthenticationPaths.RemoteRegisterPath = identityProvider.RemoteRegisterPath;
                });
            }
            else
            {
                services.AddAuthorizationCore();
                services.AddSingleton<AuthenticationStateProvider, EmptyAuthenticationStateProvider>();
                services.AddSingleton<SignOutSessionStateManager>();
            }
        }
    }
}
