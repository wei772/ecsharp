using System;
using System.Collections.Generic;
using System.Text;
using Loyc.Essentials;
using Loyc.Utilities;
using Loyc.Threading;

namespace Loyc.CompilerCore
{
	public delegate void CompilerOutputWriter(Symbol msgType, SourcePos pos, Symbol msgId, [Localizable] string message, params object[] args);

	/// <summary>
	/// CompilerOutput is a simple message-output facility that can provide message 
	/// translation at the same time. It is a singleton (static class) but doesn't
	/// output the messages itself; it just passes the messages to the attached
	/// CompilerOutputWriter.
	/// </summary><remarks>
	/// In Loyc, you can use the Error, Warning, Note, Detail and Verbose classes
	/// to output messages that have the same msgType as the class name. For example,
	/// <c>Warning.Write(position, message)</c> is equivalent to 
	/// <c>Message.Write(:Warning, position, message)</c>.
	/// 
	/// TODO: consider how to support "warning levels" and "verbose levels" so user
	/// can hide all messages below a specified priority.
	/// </remarks>
	public class CompilerOutput
	{
		static ThreadLocalVariable<CompilerOutputWriter> Writer = new ThreadLocalVariable<CompilerOutputWriter>(ConsoleCompilerOutputWriter.Write);

		public static void Write(Symbol msgType, SourcePos pos, [Localizable] string message) { Write(msgType, pos, message, (object[])null); }
		public static void Write(Symbol msgType, SourcePos pos, [Localizable] string message, params object[] args)
		{
			Writer.Value(msgType, pos, null, message, args);
		}
		public static void Write(Symbol msgType, SourcePos pos, Symbol msgId) { Write(msgType, pos, msgId, (object[])null); }
		public static void Write(Symbol msgType, SourcePos pos, Symbol msgId, params object[] args)
		{
			Writer.Value(msgType, pos, msgId, null, args);
		}
		public static void Write(CompilerMsg msg)
		{
			Writer.Value(msg.Type, msg.Position, null, msg.MessageAlone, msg.MessageArgs);
		}
	}
	public class Warning
	{
		public static readonly Symbol MsgType = GSymbol.Get("Warning");

		public static void Write(SourcePos pos, [Localizable] string message, params object[] args)
			{ CompilerOutput.Write(MsgType, pos, message, args); }
		public static void Write(SourcePos pos, Symbol msgId, params object[] args)
			{ CompilerOutput.Write(MsgType, pos, msgId, args); }
	}
	public class Error
	{
		public static readonly Symbol MsgType = GSymbol.Get("Error");

		public static void Write(SourcePos pos, [Localizable] string message, params object[] args)
			{ CompilerOutput.Write(MsgType, pos, message, args); }
		public static void Write(SourcePos pos, Symbol msgId, params object[] args)
			{ CompilerOutput.Write(MsgType, pos, msgId, args); }
	}
	public class Note
	{
		public static readonly Symbol MsgType = GSymbol.Get("Note");

		public static void Write(SourcePos pos, [Localizable] string message, params object[] args)
			{ CompilerOutput.Write(MsgType, pos, message, args); }
		public static void Write(SourcePos pos, Symbol msgId, params object[] args)
			{ CompilerOutput.Write(MsgType, pos, msgId, args); }
	}
	public class Detail
	{
		public static readonly Symbol MsgType = GSymbol.Get("Detail");

		public static void Write(SourcePos pos, [Localizable] string message, params object[] args)
			{ CompilerOutput.Write(MsgType, pos, message, args); }
		public static void Write(SourcePos pos, Symbol msgId, params object[] args)
			{ CompilerOutput.Write(MsgType, pos, msgId, args); }
	}
	public class Verbose
	{
		public static readonly Symbol MsgType = GSymbol.Get("Verbose");

		public static void Write(SourcePos pos, [Localizable] string message, params object[] args)
			{ CompilerOutput.Write(MsgType, pos, message, args); }
		public static void Write(SourcePos pos, Symbol msgId, params object[] args)
			{ CompilerOutput.Write(MsgType, pos, msgId, args); }
	}

	public class ConsoleCompilerOutputWriter : ConsoleOutputWriter
	{
		public static void Write(Symbol msgType, SourcePos pos, Symbol msgId, [Localizable] string message, params object[] args)
		{
			string result = Localize.From(msgId, message, args);
			string typeText;
			ConsoleColor oldColor = Console.ForegroundColor;
			Console.ForegroundColor = PickColor(msgType, out typeText);
			if (typeText == null)
				Console.WriteLine("{0}: {1}", pos, result);
			else
				Console.WriteLine("{0}: {1}: {2}", pos, typeText, result);
			Console.ForegroundColor = oldColor;
		}
	}
}
