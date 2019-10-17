using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORMi.Sample.Models
{
    [WMIClass(Name = "Win32_UserProfile", Namespace = "root\\CimV2")]
    public class UserProfile
    {
        public string SID { get; set; }
        public string LocalPath { get; set; }
        public bool Loaded { get; set; }
        [WMIProperty("refCount")]
        public uint RefCount { get; set; }
        public bool Special { get; set; }
        public bool RoamingConfigured { get; set; }
        public string RoamingPath { get; set; }
        public bool RoamingPreference { get; set; }
        public uint Status { get; set; }
        public DateTime LastUseTime { get; set; }
        public DateTime LastDownloadTime { get; set; }
        public DateTime LastUploadTime { get; set; }
        public byte HealthStatus { get; set; }
        public DateTime LastAttemptedProfileDownloadTime { get; set; }
        public DateTime LastAttemptedProfileUploadTime { get; set; }
        public DateTime LastBackgroundRegistryUploadTime { get; set; }
        public FolderRedirectionHealth AppDataRoaming { get; set; }
        public FolderRedirectionHealth Desktop { get; set; }
        public FolderRedirectionHealth StartMenu { get; set; }
        public FolderRedirectionHealth Documents { get; set; }
        public FolderRedirectionHealth Pictures { get; set; }
        public FolderRedirectionHealth Music { get; set; }
        public FolderRedirectionHealth Videos { get; set; }
        public FolderRedirectionHealth Favorites { get; set; }
        public FolderRedirectionHealth Contacts { get; set; }
        public FolderRedirectionHealth Downloads { get; set; }
        public FolderRedirectionHealth Links { get; set; }
        public FolderRedirectionHealth Searches { get; set; }
        public FolderRedirectionHealth SavedGames { get; set; }
    }
}
