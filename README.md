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

Library project that holds the model and the guts of the various parsers.  This is pretty messy, but stuff is relatively cleanly separated into different directories.  

#### Parsers Implemented

* [Piglet](https://github.com/Dervall/Piglet) - Only uses the Piglet lexer.  The parser/AST is handwritten and is terrible.
* [Superpower](https://github.com/datalust/superpower)
* [Pidgin](https://github.com/benjamin-hodgson/Pidgin)
* [Hime](https://cenotelie.fr/projects/hime/) - There's an issue with the grammar that makes it massively slow.  Well, probably more than one, but the one I know for sure is "`translation_unit -> global_expr+;`".  See [this issue](https://bitbucket.org/cenotelie/hime/issues/63/net-really-slow-parsing) I reported on the project Bitbucket page.
* Custom Lexer with Pidgin Parser
* Custom Lexer with Custom Parser

#### Benchmarks

The absolute times are only relevant for my laptop, but the relative times are interesting (all times are in seconds):

|Map(s)|Custom Lexer + Parser|Pidgin with Custom Lexer|Piglet|Pidgin, Take 2|Pidgin|Superpower|Hime|
|---|---:|---:|---:|---:|---:|---:|---:|
|Freedoom MAP28|0.1|0.3|0.4|0.9|1.8|3.0|18.5|
|All Freedoom Maps|1.1|2.7|3.6|7.8|12.0|26.3|600+|
|ZDCMP2|1.2|3.0|3.8|7.4|11.3|26.0|600+|

The first version of the Pidgin parser is only available if you dive into the history.  The speedup in the current ("take 2") version is that it has a unified "number" parser that does not have to backtrack when parsing integers & floating point numbers.

Maps:

* [Freedoom](https://doomwiki.org/wiki/Freedoom) MAP28 is an example of a large map.
* Freedoom itself (technically, "Freedoom: Phase 2") is composed of 32 maps.  This benchmark doesn't have as many trials as just the MAP28 loading, so the number is more variable.
* [ZDCMP2](https://doomwiki.org/wiki/The_ZDoom_Community_Map_Project_"Take_2") is a _gargantuan_ level (2.7 million lines!)

### `UdmfParsingBenchmarks`

A [BenchmarkDotNet](https://benchmarkdotnet.org/) project pitting the various parsers against each other.  Hime doesn't get to play in the longer benchmarks since the speed is so abysmal.

### `UdmfParsingTests`

An NUnit test library.