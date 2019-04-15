// Copyright (c) 2017, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

namespace UdmfParserGenerator.Utilities
{
    public static class StringExtensions
    {
        public static string ToCamelCase(this string name) => char.ToLowerInvariant(name[0]) + name.Substring(1);
        public static string ToPascalCase(this string name) => char.ToUpperInvariant(name[0]) + name.Substring(1);
        public static string ToPluralPascalCase(this string name) => name.ToPascalCase() + "s";
        public static string ToFieldName(this string name) => "_" + name.ToCamelCase();
    }
}