﻿using AcBlog.Client.WebAssembly.Interops;
using AcBlog.Client.WebAssembly.Models;
using AcBlog.Sdk;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using System.Web;
using AcBlog.Client.WebAssembly.Shared;

namespace AcBlog.Client.WebAssembly.Pages.Notes
{
    public class BaseNotePage : BasePage
    {
        protected override string Title
        {
            get => base.Title; set
            {
                if (string.IsNullOrEmpty(value))
                    value = $"Notes";
                else
                    value = $"{value} - Notes";
                base.Title = value;
            }
        }
    }
}
