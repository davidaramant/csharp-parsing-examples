﻿// Copyright (c) 2019, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Generic;
using System.Linq;
using UdmfParsing.Udmf;

namespace UdmfParsing.Wad
{
    public static class WadLoader
    {
        public static List<MapData> LoadUsingPidgin(string path)
        {
            var maps = new List<MapData>();

            using (var wad = WadReader.Read(path))
            {
                maps.AddRange(
                    wad.GetMapNames().Select(name => MapData.LoadFromUsingPidgin(wad.GetMapStream(name))));
            }

            return maps;
        }

        public static List<MapData> LoadUsingCustomWithPidgin(string path)
        {
            var maps = new List<MapData>();

            using (var wad = WadReader.Read(path))
            {
                maps.AddRange(
                    wad.GetMapNames().Select(name => MapData.LoadFromUsingCustomWithPidgin(wad.GetMapStream(name))));
            }

            return maps;
        }

        public static List<MapData> LoadUsingTotallyCustom(string path)
        {
            var maps = new List<MapData>();

            using (var wad = WadReader.Read(path))
            {
                maps.AddRange(
                    wad.GetMapNames().Select(name => MapData.LoadFromUsingTotallyCustom(wad.GetMapStream(name))));
            }

            return maps;
        }

        public static List<MapData> LoadUsingSuperpower(string path)
        {
            var maps = new List<MapData>();

            using (var wad = WadReader.Read(path))
            {
                maps.AddRange(
                    wad.GetMapNames().Select(name => MapData.LoadFromUsingSuperpower(wad.GetMapStream(name))));
            }

            return maps;
        }

        public static List<MapData> LoadUsingPiglet(string path)
        {
            var maps = new List<MapData>();

            using (var wad = WadReader.Read(path))
            {
                maps.AddRange(
                    wad.GetMapNames().Select(name => MapData.LoadFromUsingPiglet(wad.GetMapStream(name))));
            }

            return maps;
        }
    }
}