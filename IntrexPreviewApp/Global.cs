using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntrexPreviewApp
{
    public static class Global
    {
        public static string plcIp { get; set; }
        public static string vncLogin { get; set; }

        public static string cogIp { get; set; }
        public static string cogPort { get; set; }


        public static void LoadSettings()
        {
            var appSettings = ConfigurationManager.AppSettings;

            if (appSettings.Count != 0)
            {
                plcIp = appSettings["PlcIp"];
                vncLogin = appSettings["VncLogin"];

                cogIp = appSettings["CameraIp"];
            }
        }
    }
}
