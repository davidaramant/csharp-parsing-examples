# csharp-parsing-examples
Some examples of parsing for a talk.

Written in .NET Core.

## `StockyardReportParsing`

Parsing a stockyard report (as in, cows) using [Sprache](https://github.com/sprache/Sprache).  The file format (which is completely real) is very context heavy and would be miserable to parse using a traditional parser generator.

The project is itself an NUnit test project and is completely self contained.

## Universal Doom Map Format (UDMF)

This is a set of projects used to parse [UDMF](https://doomwiki.org/wiki/UDMF).  Most of the code was ported over from [Sector Director](https://github.com/davidaramant/sector-director) which has some pros and cons...  The project structure is a bit wonky, plus SC uses the 3-clause BSD license so all the license headers are wrong (oops).  I wrote it though and I totally claim that everything here is dual-licensed as MIT also!

On the plus side, getting everything out of Sector Director allows for a much better playground to compare different parsers instead of trying to deal with multiple branches.  Sector Director will only ever use one parser framework.

Project descriptions:

### `UdmfParserGenerator`

EXE project to run the various parser generators and create the UDMF model.  The [Hime](https://cenotelie.fr/projects/hime/) parser generator assumes that Hime is "installed" in a folder at the base of the repo (just unzip the package into a folder called "Hime").  It also assumes that Java is installed.  **Note that running this project is optional** - this is only useful if (like me) you want to change something.

### `UdmfParsing`

Library project that holds the model and the guts of the various parsers.  This is pretty messy, but stuff is relatively cleanly separated into different directories.  Parsers implemented:

* [Piglet](https://github.com/Dervall/Piglet) - Only uses the Piglet lexer.  The parser/AST is handwritten and is terrible.
* [Superpower](https://github.com/datalust/superpower)
* [Pidgin](https://github.com/benjamin-hodgson/Pidgin)
* [Hime](https://cenotelie.fr/projects/hime/) - There's an issue with the grammar that makes it massively slow.  Well, probably more than one, but the one I know for sure is "`translation_unit -> global_expr+;`".  See [this issue](https://bitbucket.org/cenotelie/hime/issues/63/net-really-slow-parsing) I reported on the project Bitbucket page.


### `UdmfParsingBenchmarks`

A [BenchmarkDotNet](https://benchmarkdotnet.org/) project pitting the various parsers against each other.  Hime doesn't get to play in the longer benchmarks since the speed is so abysmal.

### `UdmfParsingTests`

An NUnit test library.