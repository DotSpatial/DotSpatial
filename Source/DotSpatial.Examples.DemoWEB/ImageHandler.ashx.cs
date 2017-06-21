using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DemoWEB
{
    /// <summary>
    /// Descrizione di riepilogo per ImageHandler1
    /// </summary>
    public class ImageHandler1 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string[] cmd = context.Request.Params["ID"].ToString().Split('?');

            string ID = cmd[0];

            //context.Response.BufferOutput = true;

            byte[] buffer = (byte[])HttpContext.Current.Cache.Get(ID);

            context.Response.OutputStream.Write(buffer, 0, buffer.Length);

            context.Response.Flush();

            return;

        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}