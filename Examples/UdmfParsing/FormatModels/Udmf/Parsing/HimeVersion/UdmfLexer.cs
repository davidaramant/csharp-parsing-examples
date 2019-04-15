/*
 * WARNING: this file has been generated by
 * Hime Parser Generator 3.4.0.0
 */
using System.Collections.Generic;
using System.IO;
using Hime.Redist;
using Hime.Redist.Lexer;

namespace SectorDirector.Core.FormatModels.Udmf.Parsing.HimeVersion
{
	/// <summary>
	/// Represents a lexer
	/// </summary>
	public class UdmfLexer : ContextFreeLexer
	{
		/// <summary>
		/// The automaton for this lexer
		/// </summary>
		private static readonly Automaton commonAutomaton = Automaton.Find(typeof(UdmfLexer), "UdmfLexer.bin");
		/// <summary>
		/// Contains the constant IDs for the terminals for this lexer
		/// </summary>
		public class ID
		{
			/// <summary>
			/// The unique identifier for terminal NEW_LINE
			/// </summary>
			public const int TerminalNewLine = 0x0003;
			/// <summary>
			/// The unique identifier for terminal COMMENT_LINE
			/// </summary>
			public const int TerminalCommentLine = 0x0004;
			/// <summary>
			/// The unique identifier for terminal COMMENT_BLOCK
			/// </summary>
			public const int TerminalCommentBlock = 0x0005;
			/// <summary>
			/// The unique identifier for terminal WHITE_SPACE
			/// </summary>
			public const int TerminalWhiteSpace = 0x0006;
			/// <summary>
			/// The unique identifier for terminal SEPARATOR
			/// </summary>
			public const int TerminalSeparator = 0x0007;
			/// <summary>
			/// The unique identifier for terminal IDENTIFIER
			/// </summary>
			public const int TerminalIdentifier = 0x0008;
			/// <summary>
			/// The unique identifier for terminal BOOLEAN
			/// </summary>
			public const int TerminalBoolean = 0x0009;
			/// <summary>
			/// The unique identifier for terminal QUOTED_STRING
			/// </summary>
			public const int TerminalQuotedString = 0x000A;
			/// <summary>
			/// The unique identifier for terminal INTEGER
			/// </summary>
			public const int TerminalInteger = 0x000B;
			/// <summary>
			/// The unique identifier for terminal FLOAT
			/// </summary>
			public const int TerminalFloat = 0x000C;
		}
		/// <summary>
		/// Contains the constant IDs for the contexts for this lexer
		/// </summary>
		public class Context
		{
			/// <summary>
			/// The unique identifier for the default context
			/// </summary>
			public const int Default = 0;
		}
		/// <summary>
		/// The collection of terminals matched by this lexer
		/// </summary>
		/// <remarks>
		/// The terminals are in an order consistent with the automaton,
		/// so that terminal indices in the automaton can be used to retrieve the terminals in this table
		/// </remarks>
		private static readonly Symbol[] terminals = {
			new Symbol(0x0001, "ε"),
			new Symbol(0x0002, "$"),
			new Symbol(0x0003, "NEW_LINE"),
			new Symbol(0x0004, "COMMENT_LINE"),
			new Symbol(0x0005, "COMMENT_BLOCK"),
			new Symbol(0x0006, "WHITE_SPACE"),
			new Symbol(0x0007, "SEPARATOR"),
			new Symbol(0x0008, "IDENTIFIER"),
			new Symbol(0x0009, "BOOLEAN"),
			new Symbol(0x000A, "QUOTED_STRING"),
			new Symbol(0x000B, "INTEGER"),
			new Symbol(0x000C, "FLOAT"),
			new Symbol(0x0012, "="),
			new Symbol(0x0013, ";"),
			new Symbol(0x0014, "{"),
			new Symbol(0x0016, "}") };
		/// <summary>
		/// Initializes a new instance of the lexer
		/// </summary>
		/// <param name="input">The lexer's input</param>
		public UdmfLexer(string input) : base(commonAutomaton, terminals, 0x0007, input) {}
		/// <summary>
		/// Initializes a new instance of the lexer
		/// </summary>
		/// <param name="input">The lexer's input</param>
		public UdmfLexer(TextReader input) : base(commonAutomaton, terminals, 0x0007, input) {}
	}
}
