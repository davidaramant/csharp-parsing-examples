// Copyright (c) 2019, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.Reflection;

namespace SectorDirector.DataModelGenerator.Utilities
{
    public static class CurrentLibraryInfo
    {
        private static readonly Lazy<(string Name, Version Version)> _info = 
            new Lazy<(string Name, Version Version)>(() =>
            {
                var assemblyName = Assembly.GetAssembly(typeof(CurrentLibraryInfo)).GetName();

                return (Name: assemblyName.Name, Version: assemblyName.Version);
            });

        public static string Name => _info.Value.Name;
        public static Version Version => _info.Value.Version;
    }
}