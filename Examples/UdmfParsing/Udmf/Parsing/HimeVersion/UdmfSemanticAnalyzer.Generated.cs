// Copyright (c) 2019, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.CodeDom.Compiler;
using System.Linq;
using Hime.Redist;
using UdmfParsing.Common;

namespace UdmfParsing.Udmf.Parsing.HimeVersion
{
    [GeneratedCode("UdmfParserGenerator", "1.0.0.0")]
    public static partial class UdmfSemanticAnalyzer
    {
        static partial void ProcessGlobalExpression(MapData map, ASTNode assignment)
        {
            var id = GetAssignmentIdentifier(assignment);
            switch (id.ToLower())
            {
                case "namespace":
                    map.NameSpace = ReadString(assignment, "MapData.NameSpace");
                    break;
                case "comment":
                    map.Comment = ReadString(assignment, "MapData.Comment");
                    break;
                default:
                    map.UnknownProperties.Add(new UnknownProperty(id, ReadRawValue(assignment)));
                    break;
            }
        }

        static partial void ProcessBlock(MapData map, ASTNode block)
        {
            var blockName = new Identifier(block.Children[0].Value);
            switch (blockName.ToLower())
            {
                case "linedef":
                    map.LineDefs.Add(ProcessLineDef(block));
                    break;
                case "sidedef":
                    map.SideDefs.Add(ProcessSideDef(block));
                    break;
                case "vertex":
                    map.Vertices.Add(ProcessVertex(block));
                    break;
                case "sector":
                    map.Sectors.Add(ProcessSector(block));
                    break;
                case "thing":
                    map.Things.Add(ProcessThing(block));
                    break;
                default:
                    map.UnknownBlocks.Add(ProcessUnknownBlock(blockName, block));
                    break;
            }
        }

        static LineDef ProcessLineDef(ASTNode block)
        {
            var lineDef = new LineDef();
            foreach (var assignment in block.Children.Skip(1))
            {
                var id = GetAssignmentIdentifier(assignment);
                switch (id.ToLower())
                {
                    case "id":
                        lineDef.Id = ReadInt(assignment, "LineDef.Id");
                        break;
                    case "v1":
                        lineDef.V1 = ReadInt(assignment, "LineDef.V1");
                        break;
                    case "v2":
                        lineDef.V2 = ReadInt(assignment, "LineDef.V2");
                        break;
                    case "blocking":
                        lineDef.Blocking = ReadBool(assignment, "LineDef.Blocking");
                        break;
                    case "blockmonsters":
                        lineDef.BlockMonsters = ReadBool(assignment, "LineDef.BlockMonsters");
                        break;
                    case "twosided":
                        lineDef.TwoSided = ReadBool(assignment, "LineDef.TwoSided");
                        break;
                    case "dontpegtop":
                        lineDef.DontPegTop = ReadBool(assignment, "LineDef.DontPegTop");
                        break;
                    case "dontpegbottom":
                        lineDef.DontPegBottom = ReadBool(assignment, "LineDef.DontPegBottom");
                        break;
                    case "secret":
                        lineDef.Secret = ReadBool(assignment, "LineDef.Secret");
                        break;
                    case "blocksound":
                        lineDef.BlockSound = ReadBool(assignment, "LineDef.BlockSound");
                        break;
                    case "dontdraw":
                        lineDef.DontDraw = ReadBool(assignment, "LineDef.DontDraw");
                        break;
                    case "mapped":
                        lineDef.Mapped = ReadBool(assignment, "LineDef.Mapped");
                        break;
                    case "special":
                        lineDef.Special = ReadInt(assignment, "LineDef.Special");
                        break;
                    case "arg0":
                        lineDef.Arg0 = ReadInt(assignment, "LineDef.Arg0");
                        break;
                    case "arg1":
                        lineDef.Arg1 = ReadInt(assignment, "LineDef.Arg1");
                        break;
                    case "arg2":
                        lineDef.Arg2 = ReadInt(assignment, "LineDef.Arg2");
                        break;
                    case "arg3":
                        lineDef.Arg3 = ReadInt(assignment, "LineDef.Arg3");
                        break;
                    case "arg4":
                        lineDef.Arg4 = ReadInt(assignment, "LineDef.Arg4");
                        break;
                    case "sidefront":
                        lineDef.SideFront = ReadInt(assignment, "LineDef.SideFront");
                        break;
                    case "sideback":
                        lineDef.SideBack = ReadInt(assignment, "LineDef.SideBack");
                        break;
                    case "comment":
                        lineDef.Comment = ReadString(assignment, "LineDef.Comment");
                        break;
                    default:
                        lineDef.UnknownProperties.Add(new UnknownProperty(id, ReadRawValue(assignment)));
                        break;
                }
            }
            return lineDef;
        }

        static SideDef ProcessSideDef(ASTNode block)
        {
            var sideDef = new SideDef();
            foreach (var assignment in block.Children.Skip(1))
            {
                var id = GetAssignmentIdentifier(assignment);
                switch (id.ToLower())
                {
                    case "offsetx":
                        sideDef.OffsetX = ReadInt(assignment, "SideDef.OffsetX");
                        break;
                    case "offsety":
                        sideDef.OffsetY = ReadInt(assignment, "SideDef.OffsetY");
                        break;
                    case "texturetop":
                        sideDef.TextureTop = ReadString(assignment, "SideDef.TextureTop");
                        break;
                    case "texturebottom":
                        sideDef.TextureBottom = ReadString(assignment, "SideDef.TextureBottom");
                        break;
                    case "texturemiddle":
                        sideDef.TextureMiddle = ReadString(assignment, "SideDef.TextureMiddle");
                        break;
                    case "sector":
                        sideDef.Sector = ReadInt(assignment, "SideDef.Sector");
                        break;
                    case "comment":
                        sideDef.Comment = ReadString(assignment, "SideDef.Comment");
                        break;
                    default:
                        sideDef.UnknownProperties.Add(new UnknownProperty(id, ReadRawValue(assignment)));
                        break;
                }
            }
            return sideDef;
        }

        static Vertex ProcessVertex(ASTNode block)
        {
            var vertex = new Vertex();
            foreach (var assignment in block.Children.Skip(1))
            {
                var id = GetAssignmentIdentifier(assignment);
                switch (id.ToLower())
                {
                    case "x":
                        vertex.X = ReadDouble(assignment, "Vertex.X");
                        break;
                    case "y":
                        vertex.Y = ReadDouble(assignment, "Vertex.Y");
                        break;
                    case "comment":
                        vertex.Comment = ReadString(assignment, "Vertex.Comment");
                        break;
                    default:
                        vertex.UnknownProperties.Add(new UnknownProperty(id, ReadRawValue(assignment)));
                        break;
                }
            }
            return vertex;
        }

        static Sector ProcessSector(ASTNode block)
        {
            var sector = new Sector();
            foreach (var assignment in block.Children.Skip(1))
            {
                var id = GetAssignmentIdentifier(assignment);
                switch (id.ToLower())
                {
                    case "heightfloor":
                        sector.HeightFloor = ReadInt(assignment, "Sector.HeightFloor");
                        break;
                    case "heightceiling":
                        sector.HeightCeiling = ReadInt(assignment, "Sector.HeightCeiling");
                        break;
                    case "texturefloor":
                        sector.TextureFloor = ReadString(assignment, "Sector.TextureFloor");
                        break;
                    case "textureceiling":
                        sector.TextureCeiling = ReadString(assignment, "Sector.TextureCeiling");
                        break;
                    case "lightlevel":
                        sector.LightLevel = ReadInt(assignment, "Sector.LightLevel");
                        break;
                    case "special":
                        sector.Special = ReadInt(assignment, "Sector.Special");
                        break;
                    case "id":
                        sector.Id = ReadInt(assignment, "Sector.Id");
                        break;
                    case "comment":
                        sector.Comment = ReadString(assignment, "Sector.Comment");
                        break;
                    default:
                        sector.UnknownProperties.Add(new UnknownProperty(id, ReadRawValue(assignment)));
                        break;
                }
            }
            return sector;
        }

        static Thing ProcessThing(ASTNode block)
        {
            var thing = new Thing();
            foreach (var assignment in block.Children.Skip(1))
            {
                var id = GetAssignmentIdentifier(assignment);
                switch (id.ToLower())
                {
                    case "id":
                        thing.Id = ReadInt(assignment, "Thing.Id");
                        break;
                    case "x":
                        thing.X = ReadDouble(assignment, "Thing.X");
                        break;
                    case "y":
                        thing.Y = ReadDouble(assignment, "Thing.Y");
                        break;
                    case "height":
                        thing.Height = ReadDouble(assignment, "Thing.Height");
                        break;
                    case "angle":
                        thing.Angle = ReadInt(assignment, "Thing.Angle");
                        break;
                    case "type":
                        thing.Type = ReadInt(assignment, "Thing.Type");
                        break;
                    case "skill1":
                        thing.Skill1 = ReadBool(assignment, "Thing.Skill1");
                        break;
                    case "skill2":
                        thing.Skill2 = ReadBool(assignment, "Thing.Skill2");
                        break;
                    case "skill3":
                        thing.Skill3 = ReadBool(assignment, "Thing.Skill3");
                        break;
                    case "skill4":
                        thing.Skill4 = ReadBool(assignment, "Thing.Skill4");
                        break;
                    case "skill5":
                        thing.Skill5 = ReadBool(assignment, "Thing.Skill5");
                        break;
                    case "ambush":
                        thing.Ambush = ReadBool(assignment, "Thing.Ambush");
                        break;
                    case "single":
                        thing.Single = ReadBool(assignment, "Thing.Single");
                        break;
                    case "dm":
                        thing.Dm = ReadBool(assignment, "Thing.Dm");
                        break;
                    case "coop":
                        thing.Coop = ReadBool(assignment, "Thing.Coop");
                        break;
                    case "comment":
                        thing.Comment = ReadString(assignment, "Thing.Comment");
                        break;
                    default:
                        thing.UnknownProperties.Add(new UnknownProperty(id, ReadRawValue(assignment)));
                        break;
                }
            }
            return thing;
        }

    }
}
