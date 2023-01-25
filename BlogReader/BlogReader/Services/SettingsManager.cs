using System;
using System.Collections.Generic;
using System.Text;

namespace BlogReader.Services
{
    public static class SettingsManager
    {
#if DEBUG
        public static string BaseURL = "https://127.0.0.1:7236/api";
#else
        public static string BaseURL = "https://blogreaderapi.azurewebsites.net/api";
#endif
    }
}
