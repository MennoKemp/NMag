using Auxilia.Utilities;
using System;

namespace NMag.Persistence
{
    internal class Paths
    {
        public static string Root { get; } = new PathInfo(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "NMag").Create().FullPath;
        public static string Temp { get; } = new PathInfo(Root, "Temp").Create().FullPath;
    }
}
