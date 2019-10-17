using ORMi;
using System;

namespace ORMi.Sample.Models
{
    [WMIClass(Name = "Win32_FolderRedirectionHealth", Namespace = "root\\CimV2")]
    public class FolderRedirectionHealth
    {
        public string OfflineFileNameFolderGUID { get; set; }
        public bool Redirected { get; set; }
        public bool OfflineAccessEnabled { get; set; }
        public DateTime LastSuccessfulSyncTime { get; set; }
        public DateTime LastSyncTime { get; set; }
        public byte LastSyncStatus { get; set; }
        public byte HealthStatus { get; set; }
    }
}
