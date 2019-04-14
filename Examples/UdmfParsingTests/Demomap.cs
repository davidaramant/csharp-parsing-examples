// Copyright (c) 2018, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using SectorDirector.Core.FormatModels.Udmf;

namespace UdmfParsingTests
{
    public sealed class DemoMap
    {
        public static MapData Create()
        {
            return new MapData
            {
                NameSpace = "Doom",
                Vertices =
                {
                    new Vertex // 0
                    {
                        X = 0,
                        Y = 0,
                    },

                    new Vertex // 1
                    {
                        X = 0,
                        Y = 256,
                    },

                    new Vertex // 2
                    {
                        X = 0,
                        Y = 512,
                    },

                    new Vertex // 3
                    {
                        X = 96,
                        Y = 512,
                    },

                    new Vertex // 4
                    {
                        X = 256,
                        Y = 256,
                    },

                    new Vertex // 5
                    {
                        X = 256,
                        Y = 0,
                    },

                    new Vertex // 6
                    {
                        X = 256,
                        Y = 512,
                    },

                    new Vertex // 7
                    {
                        X = 160,
                        Y = 512,
                    },

                    new Vertex // 8
                    {
                        X = 96,
                        Y = 528,
                    },

                    new Vertex // 9
                    {
                        X = 160,
                        Y = 528,
                    },
                },
                LineDefs =
                {
                    new LineDef // 0
                    {
                        V1 = 0,
                        V2 = 1,
                        SideFront = 0,
                        Blocking = true,
                    },

                    new LineDef // 1
                    {
                        V1 = 2,
                        V2 = 3,
                        SideFront = 1,
                        Blocking = true,
                    },

                    new LineDef // 2
                    {
                        V1 = 4,
                        V2 = 5,
                        SideFront = 2,
                        Blocking = true,
                    },

                    new LineDef // 3
                    {
                        V1 = 5,
                        V2 = 0,
                        SideFront = 3,
                        Blocking = true,
                    },

                    new LineDef // 4
                    {
                        V1 = 1,
                        V2 = 2,
                        SideFront = 4,
                        Blocking = true,
                    },

                    new LineDef // 5
                    {
                        V1 = 1,
                        V2 = 4,
                        SideFront = 5,
                        SideBack = 6,
                        TwoSided = true,
                    },

                    new LineDef // 6
                    {
                        V1 = 6,
                        V2 = 4,
                        SideFront = 7,
                        Blocking = true,
                    },

                    new LineDef // 7
                    {
                        V1 = 7,
                        V2 = 6,
                        SideFront = 8,
                        Blocking = true,
                    },

                    new LineDef // 8
                    {
                        V1 = 8,
                        V2 = 9,
                        SideFront = 9,
                        Special = 11,
                        Blocking = true,
                    },

                    new LineDef // 9
                    {
                        V1 = 9,
                        V2 = 7,
                        SideFront = 10,
                        Blocking = true,
                    },

                    new LineDef // 10
                    {
                        V1 = 3,
                        V2 = 7,
                        SideFront = 11,
                        SideBack = 12,
                        TwoSided = true,
                        DontPegTop = true,
                        DontPegBottom = true,
                    },

                    new LineDef // 11
                    {
                        V1 = 3,
                        V2 = 8,
                        SideFront = 13,
                        Blocking = true,
                    },
                },
                SideDefs =
                {
                    new SideDef // 0
                    {
                    Sector = 0,
                    TextureMiddle = "STARTAN1",
                    },

                    new SideDef // 1
                    {
                        Sector = 1,
                        TextureMiddle = "STARTAN1",
                    },

                    new SideDef // 2
                    {
                        Sector = 0,
                        TextureMiddle = "STARTAN1",
                    },

                    new SideDef // 3
                    {
                        Sector = 0,
                        TextureMiddle = "STARTAN1",
                    },

                    new SideDef // 4
                    {
                        Sector = 1,
                        TextureMiddle = "STARTAN1",
                    },

                    new SideDef // 5
                    {
                        Sector = 0,
                        TextureTop = "STEPTOP",
                        TextureBottom = "STEPTOP",
                    },

                    new SideDef // 6
                    {
                        Sector = 1,
                    },

                    new SideDef // 7
                    {
                        Sector = 1,
                        TextureMiddle = "STARTAN1",
                    },

                    new SideDef // 8
                    {
                        Sector = 1,
                        OffsetX = 160,
                        TextureMiddle = "STARTAN1",
                    },

                    new SideDef // 9
                    {
                        Sector = 2,
                        TextureMiddle = "SW1COMM",
                    },

                    new SideDef // 10
                    {
                        Sector = 2,
                        TextureMiddle = "SHAWN2",
                    },

                    new SideDef // 11
                    {
                        Sector = 1,
                        OffsetX = 96,
                        TextureTop = "STARTAN1",
                        TextureBottom = "STARTAN1",
                    },

                    new SideDef // 12
                    {
                        Sector = 2,
                    },

                    new SideDef // 13
                    {
                        Sector = 2,
                        TextureMiddle = "SHAWN2",
                    },
                },
                Sectors =
                {
                    new Sector
                    {
                        HeightFloor = 0,
                        HeightCeiling = 128,
                        TextureFloor = "FLOOR0_1",
                        TextureCeiling = "CEIL1_1",
                        LightLevel = 192,
                    },
                    new Sector
                    {
                        HeightFloor = 16,
                        HeightCeiling = 112,
                        TextureFloor = "FLOOR0_1",
                        TextureCeiling = "CEIL1_1",
                        LightLevel = 160,
                    },
                    new Sector
                    {
                        HeightFloor = 32,
                        HeightCeiling = 96,
                        TextureFloor = "FLAT23",
                        TextureCeiling = "FLAT23",
                        LightLevel = 255,
                    },
                },
                Things =
                {
                    new Thing
                    {
                        X = 128,
                        Y = 64,
                        Angle = 90,
                        Type = 1,
                        Skill1 = true,
                        Skill2 = true,
                        Skill3 = true,
                        Skill4 = true,
                        Skill5 = true,
                        Single = true,
                        Dm = true,
                        Coop = true,
                    }
                }
            };
        }
    }
}