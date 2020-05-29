﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cw5.Middleware
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private const string FILENAME = "LogFile.txt";
        private string FILEPATH;

        public LoggingMiddleware(RequestDelegate next)
        {
            FILEPATH = System.IO.Directory.GetCurrentDirectory() + "\\" + FILENAME;
            _next = next;
        }

        public async Task InvokeAsync (HttpContext context)
        {
            context.Request.EnableBuffering();
            if(context.Request!=null)
            {
                string path = context.Request.Path; //api/enrollments
                string method = context.Request.Method;//GET, POST
                string queryString = context.Request.QueryString.ToString();
                string bodyStr = "";

                using(var reader=new StreamReader(context.Request.Body, Encoding.UTF8, true, 1024,true))
                {
                    bodyStr = await reader.ReadToEndAsync();
                    context.Request.Body.Position = 0;
                }
                // do implementacji zapis do pliku 
                using (StreamWriter writer = File.AppendText(FILEPATH))
                {
                    writer.WriteLine("REQUEST timestamp:" + System.DateTime.Now.ToString("yyyyMMddhhmmss"));
                    writer.WriteLine("PATH: " + path);
                    writer.WriteLine("METHOD: " + method);
                    writer.WriteLine("QUERY: " + queryString);
                    writer.WriteLine("BODY: " + bodyStr);
                }
            }


            if (_next!=null) await _next(context);
        }
    }
}
