// Copyright (c) 2019, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.CodeDom.Compiler;
using SectorDirector.Core.FormatModels.Common;

namespace SectorDirector.Core.FormatModels.Udmf.Parsing.PigletVersion
{
    [GeneratedCodeAttribute("DataModelGenerator", "1.0.0.0")]
    public static partial class UdmfParser
    {
        static partial void SetGlobalAssignments(MapData map, UdmfSyntaxTree tree)
        {
            tree.GetValueFor("NameSpace").SetRequiredString(value => map.NameSpace = value, "MapData", "NameSpace");
            tree.GetValueFor("Comment").SetOptionalString(value => map.Comment = value, "MapData", "Comment");
        }
        static partial void SetBlocks(MapData map, UdmfSyntaxTree tree)
        {
            foreach (var block in tree.Blocks)
            {
                switch(block.Name.ToLower())
                {
                    case "linedef":
                        map.LineDefs.Add(ParseLineDef(block));
                        break;
                    case "sidedef":
                        map.SideDefs.Add(ParseSideDef(block));
                        break;
                    case "vertex":
                        map.Vertices.Add(ParseVertex(block));
                        break;
                    case "sector":
                        map.Sectors.Add(ParseSector(block));
                        break;
                    case "thing":
                        map.Things.Add(ParseThing(block));
                        break;
                }
            }
        }
        public static LineDef ParseLineDef(IHaveAssignments block)
        {
            var parsedBlock = new LineDef();
            block.GetValueFor("Id").SetOptionalInteger(value => parsedBlock.Id = value, "LineDef", "Id");
            block.GetValueFor("V1").SetRequiredInteger(value => parsedBlock.V1 = value, "LineDef", "V1");
            block.GetValueFor("V2").SetRequiredInteger(value => parsedBlock.V2 = value, "LineDef", "V2");
            block.GetValueFor("Blocking").SetOptionalBoolean(value => parsedBlock.Blocking = value, "LineDef", "Blocking");
            block.GetValueFor("BlockMonsters").SetOptionalBoolean(value => parsedBlock.BlockMonsters = value, "LineDef", "BlockMonsters");
            block.GetValueFor("TwoSided").SetOptionalBoolean(value => parsedBlock.TwoSided = value, "LineDef", "TwoSided");
            block.GetValueFor("DontPegTop").SetOptionalBoolean(value => parsedBlock.DontPegTop = value, "LineDef", "DontPegTop");
            block.GetValueFor("DontPegBottom").SetOptionalBoolean(value => parsedBlock.DontPegBottom = value, "LineDef", "DontPegBottom");
            block.GetValueFor("Secret").SetOptionalBoolean(value => parsedBlock.Secret = value, "LineDef", "Secret");
            block.GetValueFor("BlockSound").SetOptionalBoolean(value => parsedBlock.BlockSound = value, "LineDef", "BlockSound");
            block.GetValueFor("DontDraw").SetOptionalBoolean(value => parsedBlock.DontDraw = value, "LineDef", "DontDraw");
            block.GetValueFor("Mapped").SetOptionalBoolean(value => parsedBlock.Mapped = value, "LineDef", "Mapped");
            block.GetValueFor("Special").SetOptionalInteger(value => parsedBlock.Special = value, "LineDef", "Special");
            block.GetValueFor("Arg0").SetOptionalInteger(value => parsedBlock.Arg0 = value, "LineDef", "Arg0");
            block.GetValueFor("Arg1").SetOptionalInteger(value => parsedBlock.Arg1 = value, "LineDef", "Arg1");
            block.GetValueFor("Arg2").SetOptionalInteger(value => parsedBlock.Arg2 = value, "LineDef", "Arg2");
            block.GetValueFor("Arg3").SetOptionalInteger(value => parsedBlock.Arg3 = value, "LineDef", "Arg3");
            block.GetValueFor("Arg4").SetOptionalInteger(value => parsedBlock.Arg4 = value, "LineDef", "Arg4");
            block.GetValueFor("SideFront").SetRequiredInteger(value => parsedBlock.SideFront = value, "LineDef", "SideFront");
            block.GetValueFor("SideBack").SetOptionalInteger(value => parsedBlock.SideBack = value, "LineDef", "SideBack");
            block.GetValueFor("Comment").SetOptionalString(value => parsedBlock.Comment = value, "LineDef", "Comment");
            return parsedBlock;
        }
        public static SideDef ParseSideDef(IHaveAssignments block)
        {
            var parsedBlock = new SideDef();
            block.GetValueFor("OffsetX").SetOptionalInteger(value => parsedBlock.OffsetX = value, "SideDef", "OffsetX");
            block.GetValueFor("OffsetY").SetOptionalInteger(value => parsedBlock.OffsetY = value, "SideDef", "OffsetY");
            block.GetValueFor("TextureTop").SetOptionalString(value => parsedBlock.TextureTop = value, "SideDef", "TextureTop");
            block.GetValueFor("TextureBottom").SetOptionalString(value => parsedBlock.TextureBottom = value, "SideDef", "TextureBottom");
            block.GetValueFor("TextureMiddle").SetOptionalString(value => parsedBlock.TextureMiddle = value, "SideDef", "TextureMiddle");
            block.GetValueFor("Sector").SetRequiredInteger(value => parsedBlock.Sector = value, "SideDef", "Sector");
            block.GetValueFor("Comment").SetOptionalString(value => parsedBlock.Comment = value, "SideDef", "Comment");
            return parsedBlock;
        }
        public static Vertex ParseVertex(IHaveAssignments block)
        {
            var parsedBlock = new Vertex();
            block.GetValueFor("X").SetRequiredDouble(value => parsedBlock.X = value, "Vertex", "X");
            block.GetValueFor("Y").SetRequiredDouble(value => parsedBlock.Y = value, "Vertex", "Y");
            block.GetValueFor("Comment").SetOptionalString(value => parsedBlock.Comment = value, "Vertex", "Comment");
            return parsedBlock;
        }
        public static Sector ParseSector(IHaveAssignments block)
        {
            var parsedBlock = new Sector();
            block.GetValueFor("HeightFloor").SetOptionalInteger(value => parsedBlock.HeightFloor = value, "Sector", "HeightFloor");
            block.GetValueFor("HeightCeiling").SetOptionalInteger(value => parsedBlock.HeightCeiling = value, "Sector", "HeightCeiling");
            block.GetValueFor("TextureFloor").SetRequiredString(value => parsedBlock.TextureFloor = value, "Sector", "TextureFloor");
            block.GetValueFor("TextureCeiling").SetRequiredString(value => parsedBlock.TextureCeiling = value, "Sector", "TextureCeiling");
            block.GetValueFor("LightLevel").SetOptionalInteger(value => parsedBlock.LightLevel = value, "Sector", "LightLevel");
            block.GetValueFor("Special").SetOptionalInteger(value => parsedBlock.Special = value, "Sector", "Special");
            block.GetValueFor("Id").SetOptionalInteger(value => parsedBlock.Id = value, "Sector", "Id");
            block.GetValueFor("Comment").SetOptionalString(value => parsedBlock.Comment = value, "Sector", "Comment");
            return parsedBlock;
        }
        public static Thing ParseThing(IHaveAssignments block)
        {
            var parsedBlock = new Thing();
            block.GetValueFor("Id").SetOptionalInteger(value => parsedBlock.Id = value, "Thing", "Id");
            block.GetValueFor("X").SetRequiredDouble(value => parsedBlock.X = value, "Thing", "X");
            block.GetValueFor("Y").SetRequiredDouble(value => parsedBlock.Y = value, "Thing", "Y");
            block.GetValueFor("Height").SetOptionalDouble(value => parsedBlock.Height = value, "Thing", "Height");
            block.GetValueFor("Angle").SetOptionalInteger(value => parsedBlock.Angle = value, "Thing", "Angle");
            block.GetValueFor("Type").SetRequiredInteger(value => parsedBlock.Type = value, "Thing", "Type");
            block.GetValueFor("Skill1").SetOptionalBoolean(value => parsedBlock.Skill1 = value, "Thing", "Skill1");
            block.GetValueFor("Skill2").SetOptionalBoolean(value => parsedBlock.Skill2 = value, "Thing", "Skill2");
            block.GetValueFor("Skill3").SetOptionalBoolean(value => parsedBlock.Skill3 = value, "Thing", "Skill3");
            block.GetValueFor("Skill4").SetOptionalBoolean(value => parsedBlock.Skill4 = value, "Thing", "Skill4");
            block.GetValueFor("Skill5").SetOptionalBoolean(value => parsedBlock.Skill5 = value, "Thing", "Skill5");
            block.GetValueFor("Ambush").SetOptionalBoolean(value => parsedBlock.Ambush = value, "Thing", "Ambush");
            block.GetValueFor("Single").SetOptionalBoolean(value => parsedBlock.Single = value, "Thing", "Single");
            block.GetValueFor("Dm").SetOptionalBoolean(value => parsedBlock.Dm = value, "Thing", "Dm");
            block.GetValueFor("Coop").SetOptionalBoolean(value => parsedBlock.Coop = value, "Thing", "Coop");
            block.GetValueFor("Comment").SetOptionalString(value => parsedBlock.Comment = value, "Thing", "Comment");
            return parsedBlock;
        }
    }
}
