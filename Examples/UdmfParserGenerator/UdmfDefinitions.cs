// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Generic;
using SectorDirector.DataModelGenerator.DefinitionModel;

namespace SectorDirector.DataModelGenerator
{
    public static class UdmfDefinitions
    {
        public static readonly IEnumerable<Block> Blocks = new Block[]
        {
            new Block("linedef",
                codeName:"LineDef",
                properties:new IProperty[]
                {
                    new Field("id",FieldType.Integer, defaultValue:-1),
                    
                    new Field("v1",FieldType.Integer),
                    new Field("v2",FieldType.Integer),

                    new Field("blocking",FieldType.Boolean, defaultValue:false),
                    new Field("blockMonsters",FieldType.Boolean, defaultValue:false),
                    new Field("twoSided",FieldType.Boolean, defaultValue:false),
                    new Field("dontPegTop",FieldType.Boolean, defaultValue:false),
                    new Field("dontPegBottom",FieldType.Boolean, defaultValue:false),
                    new Field("secret",FieldType.Boolean, defaultValue:false),
                    new Field("blockSound",FieldType.Boolean, defaultValue:false),
                    new Field("dontDraw",FieldType.Boolean, defaultValue:false),
                    new Field("mapped",FieldType.Boolean, defaultValue:false),

                    new Field("special",FieldType.Integer, defaultValue:0),
                    new Field("arg0",FieldType.Integer, defaultValue:0),
                    new Field("arg1",FieldType.Integer, defaultValue:0),
                    new Field("arg2",FieldType.Integer, defaultValue:0),
                    new Field("arg3",FieldType.Integer, defaultValue:0),
                    new Field("arg4",FieldType.Integer, defaultValue:0),

                    new Field("sideFront",FieldType.Integer),
                    new Field("sideBack",FieldType.Integer, defaultValue:-1),

                    new Field("comment",type:FieldType.String, defaultValue:string.Empty),
                    new UnknownPropertiesList()
                }),

            new Block("sidedef",
                codeName:"SideDef",
                properties:new IProperty[]
                {
                    new Field("offsetX",FieldType.Integer, defaultValue:0),
                    new Field("offsetY",FieldType.Integer, defaultValue:0),

                    new Field("textureTop",type:FieldType.String, defaultValue:"-"),
                    new Field("textureBottom",type:FieldType.String, defaultValue:"-"),
                    new Field("textureMiddle",type:FieldType.String, defaultValue:"-"),

                    new Field("sector",FieldType.Integer),

                    new Field("comment",type:FieldType.String, defaultValue:string.Empty),
                    new UnknownPropertiesList()
                }),

            new Block("vertex",
                properties:new IProperty[]
                {
                    new Field("x",FieldType.Float),
                    new Field("y",FieldType.Float),

                    new Field("comment",type:FieldType.String, defaultValue:string.Empty),
                    new UnknownPropertiesList()
                }),

            new Block("sector",
                properties:new IProperty[]
                {
                    new Field("heightFloor",FieldType.Integer, defaultValue:0),
                    new Field("heightCeiling",FieldType.Integer, defaultValue:0),

                    new Field("textureFloor",type:FieldType.String),
                    new Field("textureCeiling",type:FieldType.String),

                    new Field("lightLevel",FieldType.Integer, defaultValue:160),

                    new Field("special",FieldType.Integer, defaultValue:0),
                    new Field("id",FieldType.Integer, defaultValue:0),

                    new Field("comment",type:FieldType.String, defaultValue:string.Empty),
                    new UnknownPropertiesList()
                }),

            new Block("thing",
                properties:new IProperty[]
                {
                    new Field("id",FieldType.Integer, defaultValue:0),
                    new Field("x",FieldType.Float),
                    new Field("y",FieldType.Float),
                    new Field("height",FieldType.Float, defaultValue:0),
                    new Field("angle",FieldType.Integer, defaultValue:0),
                    new Field("type",FieldType.Integer),

                    new Field("skill1", FieldType.Boolean, defaultValue:false),
                    new Field("skill2", FieldType.Boolean, defaultValue:false),
                    new Field("skill3", FieldType.Boolean, defaultValue:false),
                    new Field("skill4", FieldType.Boolean, defaultValue:false),
                    new Field("skill5", FieldType.Boolean, defaultValue:false),
                    new Field("ambush", FieldType.Boolean, defaultValue:false),
                    new Field("single", FieldType.Boolean, defaultValue:false),
                    new Field("dm", FieldType.Boolean, defaultValue:false),
                    new Field("coop", FieldType.Boolean, defaultValue:false),

                    new Field("comment",type:FieldType.String, defaultValue:string.Empty),
                    new UnknownPropertiesList()
                }),

            new Block("mapData",
                isSubBlock:false,
                properties:new IProperty[]
                {
                    new Field("nameSpace", formatName:"namespace", type:FieldType.String),
                    new Field("comment", type:FieldType.String, defaultValue:string.Empty),

                    new BlockList("lineDef"),
                    new BlockList("sideDef"),
                    new BlockList("vertices",singularName:"vertex"),
                    new BlockList("sector"),
                    new BlockList("thing"),

                    new UnknownPropertiesList(),
                    new UnknownBlocksList(),
                }),
        };
    }
}