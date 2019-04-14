# csharp-parsing-examples
Some examples of parsing for a talk.

Written in .NET Core.

## `StockyardReportParsing`

Parsing a stockyard report (as in, cows) using [Sprache](https://github.com/sprache/Sprache).  The file format (which is completely real) is very context heavy and would be miserable to parse using a traditional parser generator.

The project is itself an NUnit test project and is completely self contained.

## Universal Doom Map Format (UDMF)

This is a set of projects used to parse [UDMF](https://doomwiki.org/wiki/UDMF).  Most of the code was ported over from [Sector Director](https://github.com/davidaramant/sector-director) which has some pros and cons...  The namespaces are all wrong since they're based on the Sector Director project structure, plus SC uses the 2-clause BSD license so all the license headers are wrong (oops).  I wrote it though and I totally claim that everything here is dual-licensed as MIT also.

On the plus side, getting everything out of Sector Director allows for a much better playground to compare different parsers instead of trying to deal with multiple branches.  Sector Director will only ever use one parser framework.

Project descriptions:

### `UdmfParserGenerator`

EXE project to run the various parser generators and create the UDMF model.

### `UdmfParsing`

Library project that holds the model and the guts of the various parsers.

### `UdmfParsingBenchmarks`

A [BenchmarkDotNet](https://benchmarkdotnet.org/) project pitting the various parsers against each other.

### `UdmfParsingTests`

An NUnit testing library.