// Generated from EcsLexerGrammar.les by LeMP custom tool. LeMP version: 1.9.5.0
// Note: you can give command-line arguments to the tool via 'Custom Tool Namespace':
// --no-out-header       Suppress this message
// --verbose             Allow verbose messages (shown by VS as 'warnings')
// --timeout=X           Abort processing thread after X seconds (default: 10)
// --macros=FileName.dll Load macros from FileName.dll, path relative to this file 
// Use #importMacros to use macros in a given namespace, e.g. #importMacros(Loyc.LLPG);
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Loyc;
using Loyc.Syntax;
using Loyc.Syntax.Lexing;

namespace Loyc.Ecs.Parser
{
	using TT = TokenType;
	using S = CodeSymbols;

	public partial class EcsLexer
	{
		new void Newline() {
			base.Newline();
			_allowPPAt = InputPosition;
			_value = WhitespaceTag.Value;
		}
		void OtherContextualKeyword()
		{
			_parseNeeded = _verbatim = false;
			_type = TT.ContextualKeyword;
			ParseIdValue(0, false);
		}
		// Keywords --------------------------------------------------------------
		bool AllowPP { get {
				return _startPosition == _allowPPAt;
			} }
	
		static readonly Symbol _var = GSymbol.Get("var");
		static readonly Symbol _dynamic = GSymbol.Get("dynamic");
		static readonly Symbol _trait = GSymbol.Get("trait");
		static readonly Symbol _alias = GSymbol.Get("alias");
		static readonly Symbol _assembly = GSymbol.Get("assembly");
		static readonly Symbol _module = GSymbol.Get("module");
		static readonly Symbol _await = GSymbol.Get("await");
	
		static readonly Symbol _where = GSymbol.Get("where");
		static readonly Symbol _when = GSymbol.Get("when");
		static readonly Symbol _select = GSymbol.Get("select");
		static readonly Symbol _from = GSymbol.Get("from");
		static readonly Symbol _join = GSymbol.Get("join");
		static readonly Symbol _on = GSymbol.Get("on");
		static readonly Symbol _equals = GSymbol.Get("equals");
		static readonly Symbol _into = GSymbol.Get("into");
		static readonly Symbol _let = GSymbol.Get("let");
		static readonly Symbol _orderby = GSymbol.Get("orderby");
		static readonly Symbol _ascending = GSymbol.Get("ascending");
		static readonly Symbol _descending = GSymbol.Get("descending");
		static readonly Symbol _group = GSymbol.Get("group");
		static readonly Symbol _by = GSymbol.Get("by");
		// This is used by EcsParser
		internal static readonly HashSet<object> LinqKeywords = new HashSet<object> { 
			_where, _select, _from, _join, _on, _equals, _into, _let, _orderby, _ascending, _descending, _group, _by
		};
	
	
		void DotIndent()
		{
			int la0, la1;
			Check(_startPosition == _lineStartAt, "_startPosition == _lineStartAt");
			#line 30 "EcsLexerGrammar.les"
			int startPosition = InputPosition;
			#line default
			Match('.');
			Match('\t', ' ');
			// Line 31: ([\t ])*
			for (;;) {
				la0 = LA0;
				if (la0 == '\t' || la0 == ' ')
					Skip();
				else
					break;
			}
			// Line 31: ([.] [\t ] ([\t ])*)*
			for (;;) {
				la0 = LA0;
				if (la0 == '.') {
					la1 = LA(1);
					if (la1 == '\t' || la1 == ' ') {
						Skip();
						Skip();
						// Line 31: ([\t ])*
						for (;;) {
							la0 = LA0;
							if (la0 == '\t' || la0 == ' ')
								Skip();
							else
								break;
						}
					} else
						break;
				} else
					break;
			}
			#line 33 "EcsLexerGrammar.les"
			_indentLevel = MeasureIndent(_indent = CharSource.Slice(startPosition, InputPosition - startPosition));
			#line default
		}
	
		new void Spaces()
		{
			int la0;
			#line 38 "EcsLexerGrammar.les"
			int startPosition = InputPosition;
			#line default
			Match('\t', ' ');
			// Line 39: ([\t ])*
			for (;;) {
				la0 = LA0;
				if (la0 == '\t' || la0 == ' ')
					Skip();
				else
					break;
			}
			#line 41 "EcsLexerGrammar.les"
			if ((_allowPPAt == startPosition)) {
				_allowPPAt = InputPosition;
			}
			#line 44 "EcsLexerGrammar.les"
			if ((_lineStartAt == startPosition)) {
				_indentLevel = MeasureIndent(_indent = CharSource.Slice(startPosition, InputPosition - startPosition));
			}
			#line default
		}
	
		void UTF_BOM()
		{
			Match('﻿');
			#line 52 "EcsLexerGrammar.les"
			if ((_lineStartAt == _startPosition)) {
				_lineStartAt = InputPosition;
			}
			#line 55 "EcsLexerGrammar.les"
			_value = WhitespaceTag.Value;
			#line default
		}
	
		void SLComment()
		{
			int la0;
			Skip();
			Skip();
			// Line 59: ([^\$\n\r])*
			for (;;) {
				la0 = LA0;
				if (!(la0 == -1 || la0 == '\n' || la0 == '\r'))
					Skip();
				else
					break;
			}
			#line 60 "EcsLexerGrammar.les"
			_value = WhitespaceTag.Value;
			#line default
		}
	
		void MLComment()
		{
			int la1;
			Skip();
			Skip();
			// Line 64: nongreedy( &{AllowNestedComments} MLComment / Newline / [^\$] )*
			for (;;) {
				switch (LA0) {
				case '*':
					{
						la1 = LA(1);
						if (la1 == -1 || la1 == '/')
							goto stop;
						else
							Skip();
					}
					break;
				case -1:
					goto stop;
				case '/':
					{
						if (AllowNestedComments) {
							la1 = LA(1);
							if (la1 == '*')
								MLComment();
							else
								Skip();
						} else
							Skip();
					}
					break;
				case '\n': case '\r':
					Newline();
					break;
				default:
					Skip();
					break;
				}
			}
		stop:;
			Match('*');
			Match('/');
			#line 65 "EcsLexerGrammar.les"
			_value = WhitespaceTag.Value;
			#line default
		}
	
	
		// Numbers ---------------------------------------------------------------
		void DecDigits()
		{
			int la0, la1;
			MatchRange('0', '9');
			// Line 70: ([0-9])*
			for (;;) {
				la0 = LA0;
				if (la0 >= '0' && la0 <= '9')
					Skip();
				else
					break;
			}
			// Line 70: ([_] [0-9] ([0-9])*)*
			for (;;) {
				la0 = LA0;
				if (la0 == '_') {
					la1 = LA(1);
					if (la1 >= '0' && la1 <= '9') {
						Skip();
						Skip();
						// Line 70: ([0-9])*
						for (;;) {
							la0 = LA0;
							if (la0 >= '0' && la0 <= '9')
								Skip();
							else
								break;
						}
					} else
						break;
				} else
					break;
			}
		}
		static readonly HashSet<int> HexDigit_set0 = NewSetOfRanges('0', '9', 'A', 'F', 'a', 'f');
	
		void HexDigit()
		{
			Match(HexDigit_set0);
		}
		bool Scan_HexDigit()
		{
			if (!TryMatch(HexDigit_set0))
				return false;
			return true;
		}
	
		void HexDigits()
		{
			int la0, la1;
			HexDigit();
			// Line 72: greedy(HexDigit)*
			for (;;) {
				la0 = LA0;
				if (HexDigit_set0.Contains(la0))
					HexDigit();
				else
					break;
			}
			// Line 72: greedy([_] HexDigit (HexDigit)*)*
			for (;;) {
				la0 = LA0;
				if (la0 == '_') {
					la1 = LA(1);
					if (HexDigit_set0.Contains(la1)) {
						Skip();
						HexDigit();
						// Line 72: (HexDigit)*
						for (;;) {
							la0 = LA0;
							if (HexDigit_set0.Contains(la0))
								HexDigit();
							else
								break;
						}
					} else
						break;
				} else
					break;
			}
		}
		bool Scan_HexDigits()
		{
			int la0, la1;
			if (!Scan_HexDigit())
				return false;
			// Line 72: greedy(HexDigit)*
			for (;;) {
				la0 = LA0;
				if (HexDigit_set0.Contains(la0)){
					if (!Scan_HexDigit())
						return false;}
				else
					break;
			}
			// Line 72: greedy([_] HexDigit (HexDigit)*)*
			for (;;) {
				la0 = LA0;
				if (la0 == '_') {
					la1 = LA(1);
					if (HexDigit_set0.Contains(la1)) {
						if (!TryMatch('_'))
							return false;
						if (!Scan_HexDigit())
							return false;
						// Line 72: (HexDigit)*
						for (;;) {
							la0 = LA0;
							if (HexDigit_set0.Contains(la0)){
								if (!Scan_HexDigit())
									return false;}
							else
								break;
						}
					} else
						break;
				} else
					break;
			}
			return true;
		}
	
		void BinDigits()
		{
			int la0;
			Match('0', '1');
			// Line 73: ([01])*
			for (;;) {
				la0 = LA0;
				if (la0 >= '0' && la0 <= '1')
					Skip();
				else
					break;
			}
			// Line 73: ([_] [01] ([01])*)*
			for (;;) {
				la0 = LA0;
				if (la0 == '_') {
					Skip();
					Match('0', '1');
					// Line 73: ([01])*
					for (;;) {
						la0 = LA0;
						if (la0 >= '0' && la0 <= '1')
							Skip();
						else
							break;
					}
				} else
					break;
			}
		}
	
		void DecNumber()
		{
			int la0, la1;
			#line 75 "EcsLexerGrammar.les"
			_numberBase = 10;
			#line default
			// Line 76: ([.] DecDigits | DecDigits ([.] DecDigits)?)
			la0 = LA0;
			if (la0 == '.') {
				Skip();
				DecDigits();
				#line 76 "EcsLexerGrammar.les"
				_isFloat = true;
				#line default
			} else {
				DecDigits();
				// Line 77: ([.] DecDigits)?
				la0 = LA0;
				if (la0 == '.') {
					la1 = LA(1);
					if (la1 >= '0' && la1 <= '9') {
						#line 77 "EcsLexerGrammar.les"
						_isFloat = true;
						#line default
						Skip();
						DecDigits();
					}
				}
			}
			// Line 79: ([Ee] ([+\-])? DecDigits)?
			la0 = LA0;
			if (la0 == 'E' || la0 == 'e') {
				la1 = LA(1);
				if (la1 == '+' || la1 == '-' || la1 >= '0' && la1 <= '9') {
					#line 79 "EcsLexerGrammar.les"
					_isFloat = true;
					#line default
					Skip();
					// Line 79: ([+\-])?
					la0 = LA0;
					if (la0 == '+' || la0 == '-')
						Skip();
					DecDigits();
				}
			}
		}
	
		void HexNumber()
		{
			int la0, la1;
			Skip();
			Skip();
			#line 82 "EcsLexerGrammar.les"
			_numberBase = 16;
			#line default
			// Line 83: (HexDigits)?
			la0 = LA0;
			if (HexDigit_set0.Contains(la0))
				HexDigits();
			// Line 85: ([.] &(([0-9] / HexDigits [Pp] [+\-0-9])) HexDigits)?
			la0 = LA0;
			if (la0 == '.') {
				la1 = LA(1);
				if (HexDigit_set0.Contains(la1)) {
					if (Try_HexNumber_Test0(1)) {
						Skip();
						#line 86 "EcsLexerGrammar.les"
						_isFloat = true;
						#line default
						HexDigits();
					}
				}
			}
			// Line 87: ([Pp] ([+\-])? DecDigits)?
			la0 = LA0;
			if (la0 == 'P' || la0 == 'p') {
				la1 = LA(1);
				if (la1 == '+' || la1 == '-' || la1 >= '0' && la1 <= '9') {
					#line 87 "EcsLexerGrammar.les"
					_isFloat = true;
					#line default
					Skip();
					// Line 87: ([+\-])?
					la0 = LA0;
					if (la0 == '+' || la0 == '-')
						Skip();
					DecDigits();
				}
			}
		}
	
		void BinNumber()
		{
			int la0, la1;
			Skip();
			Skip();
			#line 90 "EcsLexerGrammar.les"
			_numberBase = 2;
			#line default
			// Line 91: ([.] DecDigits | DecDigits ([.] DecDigits)?)
			la0 = LA0;
			if (la0 == '.') {
				Skip();
				DecDigits();
				#line 91 "EcsLexerGrammar.les"
				_isFloat = true;
				#line default
			} else {
				DecDigits();
				// Line 92: ([.] DecDigits)?
				la0 = LA0;
				if (la0 == '.') {
					la1 = LA(1);
					if (la1 >= '0' && la1 <= '9') {
						#line 92 "EcsLexerGrammar.les"
						_isFloat = true;
						#line default
						Skip();
						DecDigits();
					}
				}
			}
			// Line 94: ([Pp] ([+\-])? DecDigits)?
			la0 = LA0;
			if (la0 == 'P' || la0 == 'p') {
				la1 = LA(1);
				if (la1 == '+' || la1 == '-' || la1 >= '0' && la1 <= '9') {
					#line 94 "EcsLexerGrammar.les"
					_isFloat = true;
					#line default
					Skip();
					// Line 94: ([+\-])?
					la0 = LA0;
					if (la0 == '+' || la0 == '-')
						Skip();
					DecDigits();
				}
			}
		}
	
		void Number()
		{
			int la0;
			#line 97 "EcsLexerGrammar.les"
			_isFloat = false;
			#line 97 "EcsLexerGrammar.les"
			_typeSuffix = null;
			#line default
			// Line 98: ( HexNumber / BinNumber / DecNumber )
			la0 = LA0;
			if (la0 == '0') {
				switch (LA(1)) {
				case 'X': case 'x':
					HexNumber();
					break;
				case 'B': case 'b':
					BinNumber();
					break;
				default:
					DecNumber();
					break;
				}
			} else
				DecNumber();
			// Line 99: ( [Ff] | [Dd] | [Mm] | [Ll] ([Uu])? | [Uu] ([Ll])? )?
			switch (LA0) {
			case 'F': case 'f':
				{
					Skip();
					#line 99 "EcsLexerGrammar.les"
					_typeSuffix = _F;
					#line 99 "EcsLexerGrammar.les"
					_isFloat = true;
					#line default
				}
				break;
			case 'D': case 'd':
				{
					Skip();
					#line 100 "EcsLexerGrammar.les"
					_typeSuffix = _D;
					#line 100 "EcsLexerGrammar.les"
					_isFloat = true;
					#line default
				}
				break;
			case 'M': case 'm':
				{
					Skip();
					#line 101 "EcsLexerGrammar.les"
					_typeSuffix = _M;
					#line 101 "EcsLexerGrammar.les"
					_isFloat = true;
					#line default
				}
				break;
			case 'L': case 'l':
				{
					Skip();
					#line 103 "EcsLexerGrammar.les"
					_typeSuffix = _L;
					#line default
					// Line 103: ([Uu])?
					la0 = LA0;
					if (la0 == 'U' || la0 == 'u') {
						Skip();
						#line 103 "EcsLexerGrammar.les"
						_typeSuffix = _UL;
						#line default
					}
				}
				break;
			case 'U': case 'u':
				{
					Skip();
					#line 104 "EcsLexerGrammar.les"
					_typeSuffix = _U;
					#line default
					// Line 104: ([Ll])?
					la0 = LA0;
					if (la0 == 'L' || la0 == 'l') {
						Skip();
						#line 104 "EcsLexerGrammar.les"
						_typeSuffix = _UL;
						#line default
					}
				}
				break;
			}
			#line 106 "EcsLexerGrammar.les"
			ParseNumberValue();
			#line default
		}
	
	
		// Strings ---------------------------------------------------------------
		void SQString()
		{
			int la0;
			_parseNeeded = false;
			_verbatim = false;
			Skip();
			// Line 114: ([\\] [^\$] | [^\$\n\r'\\])*
			for (;;) {
				la0 = LA0;
				if (la0 == '\\') {
					Skip();
					MatchExcept();
					#line 114 "EcsLexerGrammar.les"
					_parseNeeded = true;
					#line default
				} else if (!(la0 == -1 || la0 == '\n' || la0 == '\r' || la0 == '\''))
					Skip();
				else
					break;
			}
			Match('\'');
			#line 115 "EcsLexerGrammar.les"
			ParseSQStringValue();
			#line default
		}
	
		void DQString()
		{
			int la0, la1;
			_parseNeeded = false;
			_verbatim = false;
			// Line 120: (["] ([\\] [^\$] | [^\$\n\r"\\])* ["] | [@] ["] (["] ["] / [^\$"])* ["])
			la0 = LA0;
			if (la0 == '"') {
				Skip();
				// Line 120: ([\\] [^\$] | [^\$\n\r"\\])*
				for (;;) {
					la0 = LA0;
					if (la0 == '\\') {
						Skip();
						MatchExcept();
						#line 120 "EcsLexerGrammar.les"
						_parseNeeded = true;
						#line default
					} else if (!(la0 == -1 || la0 == '\n' || la0 == '\r' || la0 == '"'))
						Skip();
					else
						break;
				}
				Match('"');
			} else {
				#line 121 "EcsLexerGrammar.les"
				_verbatim = true;
				#line 121 "EcsLexerGrammar.les"
				_style = NodeStyle.VerbatimStringLiteral;
				#line default
				Match('@');
				Match('"');
				// Line 122: (["] ["] / [^\$"])*
				for (;;) {
					la0 = LA0;
					if (la0 == '"') {
						la1 = LA(1);
						if (la1 == '"') {
							Skip();
							Skip();
							#line 122 "EcsLexerGrammar.les"
							_parseNeeded = true;
							#line default
						} else
							break;
					} else if (la0 != -1)
						Skip();
					else
						break;
				}
				Match('"');
			}
			#line 123 "EcsLexerGrammar.les"
			ParseStringValue();
			#line default
		}
	
		void TQString()
		{
			int la0, la1, la2;
			#line 127 "EcsLexerGrammar.les"
			_parseNeeded = true;
			#line default
			// Line 128: (["] ["] ["] nongreedy([^\$])* ["] ["] ["] | ['] ['] ['] nongreedy([^\$])* ['] ['] ['])
			la0 = LA0;
			if (la0 == '"') {
				Skip();
				Match('"');
				Match('"');
				// Line 128: nongreedy([^\$])*
				for (;;) {
					la0 = LA0;
					if (la0 == '"') {
						la1 = LA(1);
						if (la1 == '"') {
							la2 = LA(2);
							if (la2 == -1 || la2 == '"')
								break;
							else
								Skip();
						} else if (la1 == -1)
							break;
						else
							Skip();
					} else if (la0 == -1)
						break;
					else
						Skip();
				}
				Match('"');
				Match('"');
				Match('"');
				#line 128 "EcsLexerGrammar.les"
				_style = NodeStyle.TDQStringLiteral;
				#line default
			} else {
				Match('\'');
				Match('\'');
				Match('\'');
				// Line 129: nongreedy([^\$])*
				for (;;) {
					la0 = LA0;
					if (la0 == '\'') {
						la1 = LA(1);
						if (la1 == '\'') {
							la2 = LA(2);
							if (la2 == -1 || la2 == '\'')
								break;
							else
								Skip();
						} else if (la1 == -1)
							break;
						else
							Skip();
					} else if (la0 == -1)
						break;
					else
						Skip();
				}
				Match('\'');
				Match('\'');
				Match('\'');
				#line 129 "EcsLexerGrammar.les"
				_style = NodeStyle.TQStringLiteral;
				#line default
			}
			#line 131 "EcsLexerGrammar.les"
			ParseStringValue();
			#line default
		}
	
		//@[private] rule BQStringV @{ {_verbatim=true;}
		//	'`' ("``" {_parseNeeded = true;} | ~('`'|'\r'|'\n'))* '`'
		//};
		void BQStringN()
		{
			int la0;
			#line 136 "EcsLexerGrammar.les"
			_verbatim = false;
			#line default
			Skip();
			// Line 137: ([\\] [^\$] | [^\$\n\r\\`])*
			for (;;) {
				la0 = LA0;
				if (la0 == '\\') {
					Skip();
					#line 137 "EcsLexerGrammar.les"
					_parseNeeded = true;
					#line default
					MatchExcept();
				} else if (!(la0 == -1 || la0 == '\n' || la0 == '\r' || la0 == '`'))
					Skip();
				else
					break;
			}
			Match('`');
		}
	
		void BQString()
		{
			#line 140 "EcsLexerGrammar.les"
			_parseNeeded = false;
			#line default
			BQStringN();
			#line 142 "EcsLexerGrammar.les"
			ParseBQStringValue();
			#line default
		}
	
	
		// Identifiers and Symbols -----------------------------------------------
		void IdStartChar()
		{
			Skip();
		}
	
		void IdUniLetter()
		{
			int la0, la1;
			// Line 150: ( &{@char.IsLetter(LA0->@char)} (128..65532) | [\\] [u] HexDigit HexDigit HexDigit HexDigit | [\\] [U] HexDigit HexDigit HexDigit HexDigit HexDigit HexDigit HexDigit HexDigit )
			la0 = LA0;
			if (la0 >= 128 && la0 <= 65532) {
				Check(char.IsLetter((char) LA0), "@char.IsLetter(LA0->@char)");
				Skip();
			} else {
				la1 = LA(1);
				if (la1 == 'u') {
					Match('\\');
					Skip();
					HexDigit();
					HexDigit();
					HexDigit();
					HexDigit();
					#line 151 "EcsLexerGrammar.les"
					_parseNeeded = true;
					#line default
				} else {
					Match('\\');
					Match('U');
					HexDigit();
					HexDigit();
					HexDigit();
					HexDigit();
					HexDigit();
					HexDigit();
					HexDigit();
					HexDigit();
					#line 152 "EcsLexerGrammar.les"
					_parseNeeded = true;
					#line default
				}
			}
		}
	
		void IdContChars()
		{
			int la0, la1, la2, la3, la4, la5, la6, la7, la8;
			// Line 155: ( [#'0-9] | IdStartChar | IdUniLetter )*
			for (;;) {
				la0 = LA0;
				if (la0 == '#' || la0 == '\'' || la0 >= '0' && la0 <= '9')
					Skip();
				else if (la0 >= 'A' && la0 <= 'Z' || la0 == '_' || la0 >= 'a' && la0 <= 'z')
					IdStartChar();
				else if (la0 >= 128 && la0 <= 65532)
					IdUniLetter();
				else if (la0 == '\\') {
					la1 = LA(1);
					if (la1 == 'u') {
						la2 = LA(2);
						if (HexDigit_set0.Contains(la2)) {
							la3 = LA(3);
							if (HexDigit_set0.Contains(la3)) {
								la4 = LA(4);
								if (HexDigit_set0.Contains(la4)) {
									la5 = LA(5);
									if (HexDigit_set0.Contains(la5))
										IdUniLetter();
									else
										break;
								} else
									break;
							} else
								break;
						} else
							break;
					} else if (la1 == 'U') {
						la2 = LA(2);
						if (HexDigit_set0.Contains(la2)) {
							la3 = LA(3);
							if (HexDigit_set0.Contains(la3)) {
								la4 = LA(4);
								if (HexDigit_set0.Contains(la4)) {
									la5 = LA(5);
									if (HexDigit_set0.Contains(la5)) {
										la6 = LA(6);
										if (HexDigit_set0.Contains(la6)) {
											la7 = LA(7);
											if (HexDigit_set0.Contains(la7)) {
												la8 = LA(8);
												if (HexDigit_set0.Contains(la8))
													IdUniLetter();
												else
													break;
											} else
												break;
										} else
											break;
									} else
										break;
								} else
									break;
							} else
								break;
						} else
							break;
					} else
						break;
				} else
					break;
			}
		}
	
		void NormalId()
		{
			int la0;
			// Line 156: (IdStartChar | IdUniLetter)
			la0 = LA0;
			if (la0 >= 'A' && la0 <= 'Z' || la0 == '_' || la0 >= 'a' && la0 <= 'z')
				IdStartChar();
			else
				IdUniLetter();
			IdContChars();
		}
	
		void HashId()
		{
			Skip();
			IdContChars();
		}
	
	
		void CommentStart()
		{
			Match('/');
			Match('*', '/');
		}
		static readonly HashSet<int> FancyId_set0 = NewSetOfRanges('!', '!', '#', '\'', '*', '+', '-', ':', '<', '?', 'A', 'Z', '^', '_', 'a', 'z', '|', '|', '~', '~');
	
		bool FancyId()
		{
			int la0, la1, la2, la3, la4, la5, la6, la7, la8;
			// Line 162: (BQStringN | (IdUniLetter / LettersOrPunc) (IdUniLetter / LettersOrPunc)*)
			la0 = LA0;
			if (la0 == '`') {
				BQStringN();
				#line 162 "EcsLexerGrammar.les"
				return true;
				#line default
			} else {
				// Line 163: (IdUniLetter / LettersOrPunc)
				la0 = LA0;
				if (la0 >= 128 && la0 <= 65532)
					IdUniLetter();
				else if (la0 == '\\') {
					la1 = LA(1);
					if (la1 == 117) {
						la2 = LA(2);
						if (HexDigit_set0.Contains(la2)) {
							la3 = LA(3);
							if (HexDigit_set0.Contains(la3)) {
								la4 = LA(4);
								if (HexDigit_set0.Contains(la4)) {
									la5 = LA(5);
									if (HexDigit_set0.Contains(la5))
										IdUniLetter();
									else
										LettersOrPunc();
								} else
									LettersOrPunc();
							} else
								LettersOrPunc();
						} else
							LettersOrPunc();
					} else if (la1 == 85) {
						la2 = LA(2);
						if (HexDigit_set0.Contains(la2)) {
							la3 = LA(3);
							if (HexDigit_set0.Contains(la3)) {
								la4 = LA(4);
								if (HexDigit_set0.Contains(la4)) {
									la5 = LA(5);
									if (HexDigit_set0.Contains(la5)) {
										la6 = LA(6);
										if (HexDigit_set0.Contains(la6)) {
											la7 = LA(7);
											if (HexDigit_set0.Contains(la7)) {
												la8 = LA(8);
												if (HexDigit_set0.Contains(la8))
													IdUniLetter();
												else
													LettersOrPunc();
											} else
												LettersOrPunc();
										} else
											LettersOrPunc();
									} else
										LettersOrPunc();
								} else
									LettersOrPunc();
							} else
								LettersOrPunc();
						} else
							LettersOrPunc();
					} else
						LettersOrPunc();
				} else
					LettersOrPunc();
				// Line 163: (IdUniLetter / LettersOrPunc)*
				for (;;) {
					la0 = LA0;
					if (la0 >= 128 && la0 <= 65532)
						IdUniLetter();
					else if (la0 == '\\') {
						la1 = LA(1);
						if (la1 == 117) {
							la2 = LA(2);
							if (HexDigit_set0.Contains(la2)) {
								la3 = LA(3);
								if (HexDigit_set0.Contains(la3)) {
									la4 = LA(4);
									if (HexDigit_set0.Contains(la4)) {
										la5 = LA(5);
										if (HexDigit_set0.Contains(la5))
											IdUniLetter();
										else
											LettersOrPunc();
									} else
										LettersOrPunc();
								} else
									LettersOrPunc();
							} else
								LettersOrPunc();
						} else if (la1 == 85) {
							la2 = LA(2);
							if (HexDigit_set0.Contains(la2)) {
								la3 = LA(3);
								if (HexDigit_set0.Contains(la3)) {
									la4 = LA(4);
									if (HexDigit_set0.Contains(la4)) {
										la5 = LA(5);
										if (HexDigit_set0.Contains(la5)) {
											la6 = LA(6);
											if (HexDigit_set0.Contains(la6)) {
												la7 = LA(7);
												if (HexDigit_set0.Contains(la7)) {
													la8 = LA(8);
													if (HexDigit_set0.Contains(la8))
														IdUniLetter();
													else
														LettersOrPunc();
												} else
													LettersOrPunc();
											} else
												LettersOrPunc();
										} else
											LettersOrPunc();
									} else
										LettersOrPunc();
								} else
									LettersOrPunc();
							} else
								LettersOrPunc();
						} else
							LettersOrPunc();
					} else if (FancyId_set0.Contains(la0))
						LettersOrPunc();
					else
						break;
				}
				#line 163 "EcsLexerGrammar.les"
				return false;
				#line default
			}
		}
	
		static readonly HashSet<int> Symbol_set0 = NewSetOfRanges('A', 'Z', '_', '_', 'a', 'z', 128, 65532);
	
		void Symbol()
		{
			int la0, la1;
			_parseNeeded = _verbatim = false;
			bool isBQ = false;
			Skip();
			Skip();
			// Line 169: (NormalId / FancyId)
			la0 = LA0;
			if (Symbol_set0.Contains(la0))
				NormalId();
			else if (la0 == 92) {
				la1 = LA(1);
				if (la1 == 85 || la1 == 117)
					NormalId();
				else
					isBQ = FancyId();
			} else
				isBQ = FancyId();
			#line 170 "EcsLexerGrammar.les"
			ParseSymbolValue(isBQ);
			#line default
		}
		static readonly HashSet<int> Id_set0 = NewSetOfRanges('A', 'Z', '\\', '\\', '_', '_', 'a', 'z', 128, 65532);
	
		// detect completeness of \uABCD
		void Id()
		{
			int la0, la1, la2, la3, la4, la5, la6;
			_parseNeeded = _verbatim = false;
			bool isBQ = false;
			int skipAt = 0;
			// Line 177: ( default NormalId | HashId | [@] (NormalId / FancyId) )
			la0 = LA0;
			if (Id_set0.Contains(la0))
				NormalId();
			else if (la0 == '#')
				HashId();
			else if (la0 == '@') {
				Skip();
				// Line 179: (NormalId / FancyId)
				la0 = LA0;
				if (Symbol_set0.Contains(la0))
					NormalId();
				else if (la0 == 92) {
					la1 = LA(1);
					if (la1 == 117) {
						la2 = LA(2);
						if (HexDigit_set0.Contains(la2)) {
							la3 = LA(3);
							if (HexDigit_set0.Contains(la3)) {
								la4 = LA(4);
								if (HexDigit_set0.Contains(la4)) {
									la5 = LA(5);
									if (HexDigit_set0.Contains(la5))
										NormalId();
									else
										isBQ = FancyId();
								} else
									isBQ = FancyId();
							} else
								isBQ = FancyId();
						} else
							isBQ = FancyId();
					} else if (la1 == 85) {
						la2 = LA(2);
						if (HexDigit_set0.Contains(la2)) {
							la3 = LA(3);
							if (HexDigit_set0.Contains(la3)) {
								la4 = LA(4);
								if (HexDigit_set0.Contains(la4)) {
									la5 = LA(5);
									if (HexDigit_set0.Contains(la5)) {
										la6 = LA(6);
										if (HexDigit_set0.Contains(la6))
											NormalId();
										else
											isBQ = FancyId();
									} else
										isBQ = FancyId();
								} else
									isBQ = FancyId();
							} else
								isBQ = FancyId();
						} else
							isBQ = FancyId();
					} else
						isBQ = FancyId();
				} else
					isBQ = FancyId();
				#line 179 "EcsLexerGrammar.les"
				skipAt = 1;
				#line default
			} else
				NormalId();
			#line 180 "EcsLexerGrammar.les"
			ParseIdValue(skipAt, isBQ);
			#line default
		}
		static readonly HashSet<int> LettersOrPunc_set0 = NewSetOfRanges('!', '!', '#', '\'', '*', '+', '-', ':', '<', '?', 'A', 'Z', '\\', '\\', '^', '_', 'a', 'z', '|', '|', '~', '~');
	
		void LettersOrPunc()
		{
			Match(LettersOrPunc_set0);
		}
	
	
		// Punctuation & operators -----------------------------------------------
		void Comma()
		{
			Skip();
			#line 193 "EcsLexerGrammar.les"
			_type = TT.Comma;
			#line 193 "EcsLexerGrammar.les"
			_value = S.Comma;
			#line default
		}
	
		void Semicolon()
		{
			Skip();
			#line 194 "EcsLexerGrammar.les"
			_type = TT.Semicolon;
			#line 194 "EcsLexerGrammar.les"
			_value = S.Semicolon;
			#line default
		}
	
		void At()
		{
			Skip();
			#line 195 "EcsLexerGrammar.les"
			_type = TT.At;
			#line 195 "EcsLexerGrammar.les"
			_value = S.AtSign;
			#line default
		}
	
	
		void Operator()
		{
			int la1, la2;
			// Line 198: ( ((((((((((((([.] [.] [.] / [.] [.] [<] / [.] [.] / [.]) | ([>] [>] [=] / [>] [=] / [>] / [<] [<] [=] / [<] [=] / [<])) | ([&] [&] / [&] [=] / [&])) | ([|] [|] / [|] [=] / [|])) | ([\^] [\^] / [\^] [=] / [\^])) | ([:] [=] / [=] [:] / [:] [:] / [:] / [=] [=] [>] / [=] [=] / [=] [>] / [=])) | ([!] [=] / [!])) | ([~] [=] / [~])) | ([*] [*] [=] / [*] [*] / [*] [=] / [*])) | ([/] [=] / [/])) | ([%] [=] / [%])) | ([+] [=] / [+] [+] / [+])) | ([\-] [>] / [\-] [=] / [\-] [\-] / [\-])) | ([?] [?] [=] / [?] [?] / [?] [.] / [?]) | [$] | [\\] )
			do {
				switch (LA0) {
				case '.':
					{
						la1 = LA(1);
						if (la1 == '.') {
							la2 = LA(2);
							if (la2 == '.') {
								Skip();
								Skip();
								Skip();
								#line 198 "EcsLexerGrammar.les"
								_type = TT.DotDot;
								#line 198 "EcsLexerGrammar.les"
								_value = S.DotDotDot;
								#line default
							} else if (la2 == '<') {
								Skip();
								Skip();
								Skip();
								#line 199 "EcsLexerGrammar.les"
								_type = TT.DotDot;
								#line 199 "EcsLexerGrammar.les"
								_value = S.DotDot;
								#line default
							} else {
								Skip();
								Skip();
								#line 200 "EcsLexerGrammar.les"
								_type = TT.DotDot;
								#line 200 "EcsLexerGrammar.les"
								_value = S.DotDot;
								#line default
							}
						} else {
							Skip();
							#line 201 "EcsLexerGrammar.les"
							_type = TT.Dot;
							#line 201 "EcsLexerGrammar.les"
							_value = S.Dot;
							#line default
						}
					}
					break;
				case '>':
					{
						la1 = LA(1);
						if (la1 == '>') {
							la2 = LA(2);
							if (la2 == '=') {
								Skip();
								Skip();
								Skip();
								#line 202 "EcsLexerGrammar.les"
								_type = TT.CompoundSet;
								#line 202 "EcsLexerGrammar.les"
								_value = S.ShrAssign;
								#line default
							} else
								goto match7;
						} else if (la1 == '=') {
							Skip();
							Skip();
							#line 203 "EcsLexerGrammar.les"
							_type = TT.LEGE;
							#line 203 "EcsLexerGrammar.les"
							_value = S.GE;
							#line default
						} else
							goto match7;
					}
					break;
				case '<':
					{
						la1 = LA(1);
						if (la1 == '<') {
							la2 = LA(2);
							if (la2 == '=') {
								Skip();
								Skip();
								Skip();
								#line 205 "EcsLexerGrammar.les"
								_type = TT.CompoundSet;
								#line 205 "EcsLexerGrammar.les"
								_value = S.ShlAssign;
								#line default
							} else
								goto match10;
						} else if (la1 == '=') {
							Skip();
							Skip();
							#line 206 "EcsLexerGrammar.les"
							_type = TT.LEGE;
							#line 206 "EcsLexerGrammar.les"
							_value = S.LE;
							#line default
						} else
							goto match10;
					}
					break;
				case '&':
					{
						la1 = LA(1);
						if (la1 == '&') {
							Skip();
							Skip();
							#line 208 "EcsLexerGrammar.les"
							_type = TT.And;
							#line 208 "EcsLexerGrammar.les"
							_value = S.And;
							#line default
						} else if (la1 == '=') {
							Skip();
							Skip();
							#line 209 "EcsLexerGrammar.les"
							_type = TT.CompoundSet;
							#line 209 "EcsLexerGrammar.les"
							_value = S.AndBitsAssign;
							#line default
						} else {
							Skip();
							#line 210 "EcsLexerGrammar.les"
							_type = TT.AndBits;
							#line 210 "EcsLexerGrammar.les"
							_value = S.AndBits;
							#line default
						}
					}
					break;
				case '|':
					{
						la1 = LA(1);
						if (la1 == '|') {
							Skip();
							Skip();
							#line 211 "EcsLexerGrammar.les"
							_type = TT.OrXor;
							#line 211 "EcsLexerGrammar.les"
							_value = S.Or;
							#line default
						} else if (la1 == '=') {
							Skip();
							Skip();
							#line 212 "EcsLexerGrammar.les"
							_type = TT.CompoundSet;
							#line 212 "EcsLexerGrammar.les"
							_value = S.OrBitsAssign;
							#line default
						} else {
							Skip();
							#line 213 "EcsLexerGrammar.les"
							_type = TT.OrBits;
							#line 213 "EcsLexerGrammar.les"
							_value = S.OrBits;
							#line default
						}
					}
					break;
				case '^':
					{
						la1 = LA(1);
						if (la1 == '^') {
							Skip();
							Skip();
							#line 214 "EcsLexerGrammar.les"
							_type = TT.OrXor;
							#line 214 "EcsLexerGrammar.les"
							_value = S.Xor;
							#line default
						} else if (la1 == '=') {
							Skip();
							Skip();
							#line 215 "EcsLexerGrammar.les"
							_type = TT.CompoundSet;
							#line 215 "EcsLexerGrammar.les"
							_value = S.XorBitsAssign;
							#line default
						} else {
							Skip();
							#line 216 "EcsLexerGrammar.les"
							_type = TT.XorBits;
							#line 216 "EcsLexerGrammar.les"
							_value = S.XorBits;
							#line default
						}
					}
					break;
				case ':':
					{
						la1 = LA(1);
						if (la1 == '=') {
							Skip();
							Skip();
							#line 217 "EcsLexerGrammar.les"
							_type = TT.CompoundSet;
							#line 217 "EcsLexerGrammar.les"
							_value = S.QuickBindAssign;
							#line default
						} else if (la1 == ':') {
							Skip();
							Skip();
							#line 219 "EcsLexerGrammar.les"
							_type = TT.ColonColon;
							#line 219 "EcsLexerGrammar.les"
							_value = S.ColonColon;
							#line default
						} else {
							Skip();
							#line 220 "EcsLexerGrammar.les"
							_type = TT.Colon;
							#line 220 "EcsLexerGrammar.les"
							_value = S.Colon;
							#line default
						}
					}
					break;
				case '=':
					{
						la1 = LA(1);
						if (la1 == ':') {
							Skip();
							Skip();
							#line 218 "EcsLexerGrammar.les"
							_type = TT.QuickBind;
							#line 218 "EcsLexerGrammar.les"
							_value = S.QuickBind;
							#line default
						} else if (la1 == '=') {
							la2 = LA(2);
							if (la2 == '>') {
								Skip();
								Skip();
								Skip();
								#line 221 "EcsLexerGrammar.les"
								_type = TT.Forward;
								#line 221 "EcsLexerGrammar.les"
								_value = S.Forward;
								#line default
							} else {
								Skip();
								Skip();
								#line 222 "EcsLexerGrammar.les"
								_type = TT.EqNeq;
								#line 222 "EcsLexerGrammar.les"
								_value = S.Eq;
								#line default
							}
						} else if (la1 == '>') {
							Skip();
							Skip();
							#line 223 "EcsLexerGrammar.les"
							_type = TT.LambdaArrow;
							#line 223 "EcsLexerGrammar.les"
							_value = S.Lambda;
							#line default
						} else {
							Skip();
							#line 224 "EcsLexerGrammar.les"
							_type = TT.Set;
							#line 224 "EcsLexerGrammar.les"
							_value = S.Assign;
							#line default
						}
					}
					break;
				case '!':
					{
						la1 = LA(1);
						if (la1 == '=') {
							Skip();
							Skip();
							#line 225 "EcsLexerGrammar.les"
							_type = TT.EqNeq;
							#line 225 "EcsLexerGrammar.les"
							_value = S.Neq;
							#line default
						} else {
							Skip();
							#line 226 "EcsLexerGrammar.les"
							_type = TT.Not;
							#line 226 "EcsLexerGrammar.les"
							_value = S.Not;
							#line default
						}
					}
					break;
				case '~':
					{
						la1 = LA(1);
						if (la1 == '=') {
							Skip();
							Skip();
							#line 227 "EcsLexerGrammar.les"
							_type = TT.CompoundSet;
							#line 227 "EcsLexerGrammar.les"
							_value = S.ConcatAssign;
							#line default
						} else {
							Skip();
							#line 228 "EcsLexerGrammar.les"
							_type = TT.NotBits;
							#line 228 "EcsLexerGrammar.les"
							_value = S.NotBits;
							#line default
						}
					}
					break;
				case '*':
					{
						la1 = LA(1);
						if (la1 == '*') {
							la2 = LA(2);
							if (la2 == '=') {
								Skip();
								Skip();
								Skip();
								#line 229 "EcsLexerGrammar.les"
								_type = TT.CompoundSet;
								#line 229 "EcsLexerGrammar.les"
								_value = S.ExpAssign;
								#line default
							} else {
								Skip();
								Skip();
								#line 230 "EcsLexerGrammar.les"
								_type = TT.Power;
								#line 230 "EcsLexerGrammar.les"
								_value = S.Exp;
								#line default
							}
						} else if (la1 == '=') {
							Skip();
							Skip();
							#line 231 "EcsLexerGrammar.les"
							_type = TT.CompoundSet;
							#line 231 "EcsLexerGrammar.les"
							_value = S.MulAssign;
							#line default
						} else {
							Skip();
							#line 232 "EcsLexerGrammar.les"
							_type = TT.Mul;
							#line 232 "EcsLexerGrammar.les"
							_value = S.Mul;
							#line default
						}
					}
					break;
				case '/':
					{
						la1 = LA(1);
						if (la1 == '=') {
							Skip();
							Skip();
							#line 233 "EcsLexerGrammar.les"
							_type = TT.CompoundSet;
							#line 233 "EcsLexerGrammar.les"
							_value = S.DivAssign;
							#line default
						} else {
							Skip();
							#line 234 "EcsLexerGrammar.les"
							_type = TT.DivMod;
							#line 234 "EcsLexerGrammar.les"
							_value = S.Div;
							#line default
						}
					}
					break;
				case '%':
					{
						la1 = LA(1);
						if (la1 == '=') {
							Skip();
							Skip();
							#line 235 "EcsLexerGrammar.les"
							_type = TT.CompoundSet;
							#line 235 "EcsLexerGrammar.les"
							_value = S.ModAssign;
							#line default
						} else {
							Skip();
							#line 236 "EcsLexerGrammar.les"
							_type = TT.DivMod;
							#line 236 "EcsLexerGrammar.les"
							_value = S.Mod;
							#line default
						}
					}
					break;
				case '+':
					{
						la1 = LA(1);
						if (la1 == '=') {
							Skip();
							Skip();
							#line 237 "EcsLexerGrammar.les"
							_type = TT.CompoundSet;
							#line 237 "EcsLexerGrammar.les"
							_value = S.AddAssign;
							#line default
						} else if (la1 == '+') {
							Skip();
							Skip();
							#line 238 "EcsLexerGrammar.les"
							_type = TT.IncDec;
							#line 238 "EcsLexerGrammar.les"
							_value = S.PreInc;
							#line default
						} else {
							Skip();
							#line 239 "EcsLexerGrammar.les"
							_type = TT.Add;
							#line 239 "EcsLexerGrammar.les"
							_value = S.Add;
							#line default
						}
					}
					break;
				case '-':
					{
						la1 = LA(1);
						if (la1 == '>') {
							Skip();
							Skip();
							#line 240 "EcsLexerGrammar.les"
							_type = TT.PtrArrow;
							#line 240 "EcsLexerGrammar.les"
							_value = S.PtrArrow;
							#line default
						} else if (la1 == '=') {
							Skip();
							Skip();
							#line 241 "EcsLexerGrammar.les"
							_type = TT.CompoundSet;
							#line 241 "EcsLexerGrammar.les"
							_value = S.SubAssign;
							#line default
						} else if (la1 == '-') {
							Skip();
							Skip();
							#line 242 "EcsLexerGrammar.les"
							_type = TT.IncDec;
							#line 242 "EcsLexerGrammar.les"
							_value = S.PreDec;
							#line default
						} else {
							Skip();
							#line 243 "EcsLexerGrammar.les"
							_type = TT.Sub;
							#line 243 "EcsLexerGrammar.les"
							_value = S.Sub;
							#line default
						}
					}
					break;
				case '?':
					{
						la1 = LA(1);
						if (la1 == '?') {
							la2 = LA(2);
							if (la2 == '=') {
								Skip();
								Skip();
								Skip();
								#line 244 "EcsLexerGrammar.les"
								_type = TT.CompoundSet;
								#line 244 "EcsLexerGrammar.les"
								_value = S.NullCoalesceAssign;
								#line default
							} else {
								Skip();
								Skip();
								#line 245 "EcsLexerGrammar.les"
								_type = TT.NullCoalesce;
								#line 245 "EcsLexerGrammar.les"
								_value = S.NullCoalesce;
								#line default
							}
						} else if (la1 == '.') {
							Skip();
							Skip();
							#line 246 "EcsLexerGrammar.les"
							_type = TT.NullDot;
							#line 246 "EcsLexerGrammar.les"
							_value = S.NullDot;
							#line default
						} else {
							Skip();
							#line 247 "EcsLexerGrammar.les"
							_type = TT.QuestionMark;
							#line 247 "EcsLexerGrammar.les"
							_value = S.QuestionMark;
							#line default
						}
					}
					break;
				case '$':
					{
						Skip();
						#line 248 "EcsLexerGrammar.les"
						_type = TT.Substitute;
						#line 248 "EcsLexerGrammar.les"
						_value = S.Substitute;
						#line default
					}
					break;
				default:
					{
						Match('\\');
						#line 249 "EcsLexerGrammar.les"
						_type = TT.Backslash;
						#line 249 "EcsLexerGrammar.les"
						_value = S.Backslash;
						#line default
					}
					break;
				}
				break;
			match7:
				{
					Skip();
					#line 204 "EcsLexerGrammar.les"
					_type = TT.GT;
					#line 204 "EcsLexerGrammar.les"
					_value = S.GT;
					#line default
				}
				break;
			match10:
				{
					Skip();
					#line 207 "EcsLexerGrammar.les"
					_type = TT.LT;
					#line 207 "EcsLexerGrammar.les"
					_value = S.LT;
					#line default
				}
			} while (false);
		}
	
	
		// Shebang ---------------------------------------------------------------
		void Shebang()
		{
			int la0;
			Skip();
			Skip();
			// Line 254: ([^\$\n\r])*
			for (;;) {
				la0 = LA0;
				if (!(la0 == -1 || la0 == '\n' || la0 == '\r'))
					Skip();
				else
					break;
			}
			// Line 254: (Newline)?
			la0 = LA0;
			if (la0 == '\n' || la0 == '\r')
				Newline();
		}
	
		static readonly HashSet<int> IdOrKeyword_set0 = NewSetOfRanges('#', '#', '0', '9', 'A', 'Z', '_', '_', 'a', 'z');
	
		void IdOrKeyword()
		{
			int la1, la2, la3, la4, la5, la6, la7, la8, la9, la10;
			// Line 262: ( [a] [b] [s] [t] [r] [a] [c] [t] EndId =>  / [a] [s] EndId =>  / [b] [a] [s] [e] EndId =>  / [b] [o] [o] [l] EndId =>  / [b] [r] [e] [a] [k] EndId =>  / [b] [y] [t] [e] EndId =>  / [c] [a] [s] [e] EndId =>  / [c] [a] [t] [c] [h] EndId =>  / [c] [h] [a] [r] EndId =>  / [c] [h] [e] [c] [k] [e] [d] EndId =>  / [c] [l] [a] [s] [s] EndId =>  / [c] [o] [n] [s] [t] EndId =>  / [c] [o] [n] [t] [i] [n] [u] [e] EndId =>  / [d] [e] [c] [i] [m] [a] [l] EndId =>  / [d] [e] [f] [a] [u] [l] [t] EndId =>  / [d] [e] [l] [e] [g] [a] [t] [e] EndId =>  / [d] [o] [u] [b] [l] [e] EndId =>  / [d] [o] EndId =>  / [e] [l] [s] [e] EndId =>  / [e] [n] [u] [m] EndId =>  / [e] [v] [e] [n] [t] EndId =>  / [e] [x] [p] [l] [i] [c] [i] [t] EndId =>  / [e] [x] [t] [e] [r] [n] EndId =>  / [f] [a] [l] [s] [e] EndId =>  / [f] [i] [n] [a] [l] [l] [y] EndId =>  / [f] [i] [x] [e] [d] EndId =>  / [f] [l] [o] [a] [t] EndId =>  / [f] [o] [r] [e] [a] [c] [h] EndId =>  / [f] [o] [r] EndId =>  / [g] [o] [t] [o] EndId =>  / [i] [f] EndId =>  / [i] [m] [p] [l] [i] [c] [i] [t] EndId =>  / [i] [n] [t] [e] [r] [f] [a] [c] [e] EndId =>  / [i] [n] [t] [e] [r] [n] [a] [l] EndId =>  / [i] [n] [t] EndId =>  / [i] [n] EndId =>  / [i] [s] EndId =>  / [l] [o] [c] [k] EndId =>  / [l] [o] [n] [g] EndId =>  / [n] [a] [m] [e] [s] [p] [a] [c] [e] EndId =>  / [n] [e] [w] EndId =>  / [n] [u] [l] [l] EndId =>  / [o] [b] [j] [e] [c] [t] EndId =>  / [o] [p] [e] [r] [a] [t] [o] [r] EndId =>  / [o] [u] [t] EndId =>  / [o] [v] [e] [r] [r] [i] [d] [e] EndId =>  / [p] [a] [r] [a] [m] [s] EndId =>  / [p] [r] [i] [v] [a] [t] [e] EndId =>  / [p] [r] [o] [t] [e] [c] [t] [e] [d] EndId =>  / [p] [u] [b] [l] [i] [c] EndId =>  / [r] [e] [a] [d] [o] [n] [l] [y] EndId =>  / [r] [e] [f] EndId =>  / [r] [e] [t] [u] [r] [n] EndId =>  / [s] [b] [y] [t] [e] EndId =>  / [s] [e] [a] [l] [e] [d] EndId =>  / [s] [h] [o] [r] [t] EndId =>  / [s] [i] [z] [e] [o] [f] EndId =>  / [s] [t] [a] [c] [k] [a] [l] [l] [o] [c] EndId =>  / [s] [t] [a] [t] [i] [c] EndId =>  / [s] [t] [r] [i] [n] [g] EndId =>  / [s] [t] [r] [u] [c] [t] EndId =>  / [s] [w] [i] [t] [c] [h] EndId =>  / [t] [h] [i] [s] EndId =>  / [t] [h] [r] [o] [w] EndId =>  / [t] [r] [u] [e] EndId =>  / [t] [r] [y] EndId =>  / [t] [y] [p] [e] [o] [f] EndId =>  / [u] [i] [n] [t] EndId =>  / [u] [l] [o] [n] [g] EndId =>  / [u] [n] [c] [h] [e] [c] [k] [e] [d] EndId =>  / [u] [n] [s] [a] [f] [e] EndId =>  / [u] [s] [h] [o] [r] [t] EndId =>  / [u] [s] [i] [n] [g] EndId =>  / [v] [i] [r] [t] [u] [a] [l] EndId =>  / [v] [o] [l] [a] [t] [i] [l] [e] EndId =>  / [v] [o] [i] [d] EndId =>  / [w] [h] [i] [l] [e] EndId =>  / &{AllowPP} [#] [i] [f] EndId =>  / &{AllowPP} [#] [e] [l] [s] [e] EndId =>  / &{AllowPP} [#] [e] [l] [i] [f] EndId =>  / &{AllowPP} [#] [e] [n] [d] [i] [f] EndId =>  / &{AllowPP} [#] [d] [e] [f] [i] [n] [e] EndId =>  / &{AllowPP} [#] [u] [n] [d] [e] [f] EndId =>  / &{AllowPP} [#] [p] [r] [a] [g] [m] [a] EndId =>  / &{AllowPP} [#] [l] [i] [n] [e] EndId =>  / &{AllowPP} [#] [e] [r] [r] [o] [r] EndId => RestOfPPLine / &{AllowPP} [#] [w] [a] [r] [n] [i] [n] [g] EndId => RestOfPPLine / &{AllowPP} [#] [n] [o] [t] [e] EndId => RestOfPPLine / &{AllowPP} [#] [r] [e] [g] [i] [o] [n] EndId => RestOfPPLine / &{AllowPP} [#] [e] [n] [d] [r] [e] [g] [i] [o] [n] EndId =>  / [v] [a] [r] EndId =>  / [d] [y] [n] [a] [m] [i] [c] EndId =>  / [t] [r] [a] [i] [t] EndId =>  / [a] [l] [i] [a] [s] EndId =>  / [a] [s] [s] [e] [m] [b] [l] [y] EndId =>  / [m] [o] [d] [u] [l] [e] EndId =>  / [f] [r] [o] [m] EndId =>  / [w] [h] [e] [r] [e] EndId =>  / [w] [h] [e] [n] EndId =>  / [s] [e] [l] [e] [c] [t] EndId =>  / [j] [o] [i] [n] EndId =>  / [o] [n] EndId =>  / [e] [q] [u] [a] [l] [s] EndId =>  / [i] [n] [t] [o] EndId =>  / [l] [e] [t] EndId =>  / [o] [r] [d] [e] [r] [b] [y] EndId =>  / [a] [s] [c] [e] [n] [d] [i] [n] [g] EndId =>  / [d] [e] [s] [c] [e] [n] [d] [i] [n] [g] EndId =>  / [g] [r] [o] [u] [p] EndId =>  / [b] [y] EndId =>  / [a] [w] [a] [i] [t] EndId =>  / Id )
			switch (LA0) {
			case 'a':
				{
					la1 = LA(1);
					if (la1 == 'b') {
						la2 = LA(2);
						if (la2 == 's') {
							la3 = LA(3);
							if (la3 == 't') {
								la4 = LA(4);
								if (la4 == 'r') {
									la5 = LA(5);
									if (la5 == 'a') {
										la6 = LA(6);
										if (la6 == 'c') {
											la7 = LA(7);
											if (la7 == 't') {
												la8 = LA(8);
												if (!IdOrKeyword_set0.Contains(la8)) {
													Skip();
													Skip();
													Skip();
													Skip();
													Skip();
													Skip();
													Skip();
													Skip();
													#line 262 "EcsLexerGrammar.les"
													_type = TT.AttrKeyword;
													#line 262 "EcsLexerGrammar.les"
													_value = S.Abstract;
													#line default
												} else
													Id();
											} else
												Id();
										} else
											Id();
									} else
										Id();
								} else
									Id();
							} else
								Id();
						} else
							Id();
					} else if (la1 == 's') {
						la2 = LA(2);
						if (!IdOrKeyword_set0.Contains(la2)) {
							Skip();
							Skip();
							#line 263 "EcsLexerGrammar.les"
							_type = TT.As;
							#line 263 "EcsLexerGrammar.les"
							_value = S.As;
							#line default
						} else if (la2 == 's') {
							la3 = LA(3);
							if (la3 == 'e') {
								la4 = LA(4);
								if (la4 == 'm') {
									la5 = LA(5);
									if (la5 == 'b') {
										la6 = LA(6);
										if (la6 == 'l') {
											la7 = LA(7);
											if (la7 == 'y') {
												la8 = LA(8);
												if (!IdOrKeyword_set0.Contains(la8)) {
													Skip();
													Skip();
													Skip();
													Skip();
													Skip();
													Skip();
													Skip();
													Skip();
													#line 356 "EcsLexerGrammar.les"
													_type = TT.ContextualKeyword;
													#line 356 "EcsLexerGrammar.les"
													_value = _assembly;
													#line default
												} else
													Id();
											} else
												Id();
										} else
											Id();
									} else
										Id();
								} else
									Id();
							} else
								Id();
						} else if (la2 == 'c') {
							la3 = LA(3);
							if (la3 == 'e') {
								la4 = LA(4);
								if (la4 == 'n') {
									la5 = LA(5);
									if (la5 == 'd') {
										la6 = LA(6);
										if (la6 == 'i') {
											la7 = LA(7);
											if (la7 == 'n') {
												la8 = LA(8);
												if (la8 == 'g') {
													la9 = LA(9);
													if (!IdOrKeyword_set0.Contains(la9)) {
														Skip();
														Skip();
														Skip();
														Skip();
														Skip();
														Skip();
														Skip();
														Skip();
														Skip();
														#line 368 "EcsLexerGrammar.les"
														OtherContextualKeyword();
														#line default
													} else
														Id();
												} else
													Id();
											} else
												Id();
										} else
											Id();
									} else
										Id();
								} else
									Id();
							} else
								Id();
						} else
							Id();
					} else if (la1 == 'l') {
						la2 = LA(2);
						if (la2 == 'i') {
							la3 = LA(3);
							if (la3 == 'a') {
								la4 = LA(4);
								if (la4 == 's') {
									la5 = LA(5);
									if (!IdOrKeyword_set0.Contains(la5)) {
										Skip();
										Skip();
										Skip();
										Skip();
										Skip();
										#line 355 "EcsLexerGrammar.les"
										_type = TT.ContextualKeyword;
										#line 355 "EcsLexerGrammar.les"
										_value = _alias;
										#line default
									} else
										Id();
								} else
									Id();
							} else
								Id();
						} else
							Id();
					} else if (la1 == 'w') {
						la2 = LA(2);
						if (la2 == 'a') {
							la3 = LA(3);
							if (la3 == 'i') {
								la4 = LA(4);
								if (la4 == 't') {
									la5 = LA(5);
									if (!IdOrKeyword_set0.Contains(la5)) {
										Skip();
										Skip();
										Skip();
										Skip();
										Skip();
										#line 372 "EcsLexerGrammar.les"
										_type = TT.ContextualKeyword;
										#line 372 "EcsLexerGrammar.les"
										_value = _await;
										#line default
									} else
										Id();
								} else
									Id();
							} else
								Id();
						} else
							Id();
					} else
						Id();
				}
				break;
			case 'b':
				{
					la1 = LA(1);
					if (la1 == 'a') {
						la2 = LA(2);
						if (la2 == 's') {
							la3 = LA(3);
							if (la3 == 'e') {
								la4 = LA(4);
								if (!IdOrKeyword_set0.Contains(la4)) {
									Skip();
									Skip();
									Skip();
									Skip();
									#line 264 "EcsLexerGrammar.les"
									_type = TT.Base;
									#line 264 "EcsLexerGrammar.les"
									_value = S.Base;
									#line default
								} else
									Id();
							} else
								Id();
						} else
							Id();
					} else if (la1 == 'o') {
						la2 = LA(2);
						if (la2 == 'o') {
							la3 = LA(3);
							if (la3 == 'l') {
								la4 = LA(4);
								if (!IdOrKeyword_set0.Contains(la4)) {
									Skip();
									Skip();
									Skip();
									Skip();
									#line 265 "EcsLexerGrammar.les"
									_type = TT.TypeKeyword;
									#line 265 "EcsLexerGrammar.les"
									_value = S.Bool;
									#line default
								} else
									Id();
							} else
								Id();
						} else
							Id();
					} else if (la1 == 'r') {
						la2 = LA(2);
						if (la2 == 'e') {
							la3 = LA(3);
							if (la3 == 'a') {
								la4 = LA(4);
								if (la4 == 'k') {
									la5 = LA(5);
									if (!IdOrKeyword_set0.Contains(la5)) {
										Skip();
										Skip();
										Skip();
										Skip();
										Skip();
										#line 266 "EcsLexerGrammar.les"
										_type = TT.Break;
										#line 266 "EcsLexerGrammar.les"
										_value = S.Break;
										#line default
									} else
										Id();
								} else
									Id();
							} else
								Id();
						} else
							Id();
					} else if (la1 == 'y') {
						la2 = LA(2);
						if (la2 == 't') {
							la3 = LA(3);
							if (la3 == 'e') {
								la4 = LA(4);
								if (!IdOrKeyword_set0.Contains(la4)) {
									Skip();
									Skip();
									Skip();
									Skip();
									#line 267 "EcsLexerGrammar.les"
									_type = TT.TypeKeyword;
									#line 267 "EcsLexerGrammar.les"
									_value = S.UInt8;
									#line default
								} else
									Id();
							} else
								Id();
						} else if (!IdOrKeyword_set0.Contains(la2)) {
							Skip();
							Skip();
							#line 371 "EcsLexerGrammar.les"
							OtherContextualKeyword();
							#line default
						} else
							Id();
					} else
						Id();
				}
				break;
			case 'c':
				{
					la1 = LA(1);
					if (la1 == 'a') {
						la2 = LA(2);
						if (la2 == 's') {
							la3 = LA(3);
							if (la3 == 'e') {
								la4 = LA(4);
								if (!IdOrKeyword_set0.Contains(la4)) {
									Skip();
									Skip();
									Skip();
									Skip();
									#line 268 "EcsLexerGrammar.les"
									_type = TT.Case;
									#line 268 "EcsLexerGrammar.les"
									_value = S.Case;
									#line default
								} else
									Id();
							} else
								Id();
						} else if (la2 == 't') {
							la3 = LA(3);
							if (la3 == 'c') {
								la4 = LA(4);
								if (la4 == 'h') {
									la5 = LA(5);
									if (!IdOrKeyword_set0.Contains(la5)) {
										Skip();
										Skip();
										Skip();
										Skip();
										Skip();
										#line 269 "EcsLexerGrammar.les"
										_type = TT.Catch;
										#line 269 "EcsLexerGrammar.les"
										_value = S.Catch;
										#line default
									} else
										Id();
								} else
									Id();
							} else
								Id();
						} else
							Id();
					} else if (la1 == 'h') {
						la2 = LA(2);
						if (la2 == 'a') {
							la3 = LA(3);
							if (la3 == 'r') {
								la4 = LA(4);
								if (!IdOrKeyword_set0.Contains(la4)) {
									Skip();
									Skip();
									Skip();
									Skip();
									#line 270 "EcsLexerGrammar.les"
									_type = TT.TypeKeyword;
									#line 270 "EcsLexerGrammar.les"
									_value = S.Char;
									#line default
								} else
									Id();
							} else
								Id();
						} else if (la2 == 'e') {
							la3 = LA(3);
							if (la3 == 'c') {
								la4 = LA(4);
								if (la4 == 'k') {
									la5 = LA(5);
									if (la5 == 'e') {
										la6 = LA(6);
										if (la6 == 'd') {
											la7 = LA(7);
											if (!IdOrKeyword_set0.Contains(la7)) {
												Skip();
												Skip();
												Skip();
												Skip();
												Skip();
												Skip();
												Skip();
												#line 271 "EcsLexerGrammar.les"
												_type = TT.Checked;
												#line 271 "EcsLexerGrammar.les"
												_value = S.Checked;
												#line default
											} else
												Id();
										} else
											Id();
									} else
										Id();
								} else
									Id();
							} else
								Id();
						} else
							Id();
					} else if (la1 == 'l') {
						la2 = LA(2);
						if (la2 == 'a') {
							la3 = LA(3);
							if (la3 == 's') {
								la4 = LA(4);
								if (la4 == 's') {
									la5 = LA(5);
									if (!IdOrKeyword_set0.Contains(la5)) {
										Skip();
										Skip();
										Skip();
										Skip();
										Skip();
										#line 272 "EcsLexerGrammar.les"
										_type = TT.Class;
										#line 272 "EcsLexerGrammar.les"
										_value = S.Class;
										#line default
									} else
										Id();
								} else
									Id();
							} else
								Id();
						} else
							Id();
					} else if (la1 == 'o') {
						la2 = LA(2);
						if (la2 == 'n') {
							la3 = LA(3);
							if (la3 == 's') {
								la4 = LA(4);
								if (la4 == 't') {
									la5 = LA(5);
									if (!IdOrKeyword_set0.Contains(la5)) {
										Skip();
										Skip();
										Skip();
										Skip();
										Skip();
										#line 273 "EcsLexerGrammar.les"
										_type = TT.AttrKeyword;
										#line 273 "EcsLexerGrammar.les"
										_value = S.Const;
										#line default
									} else
										Id();
								} else
									Id();
							} else if (la3 == 't') {
								la4 = LA(4);
								if (la4 == 'i') {
									la5 = LA(5);
									if (la5 == 'n') {
										la6 = LA(6);
										if (la6 == 'u') {
											la7 = LA(7);
											if (la7 == 'e') {
												la8 = LA(8);
												if (!IdOrKeyword_set0.Contains(la8)) {
													Skip();
													Skip();
													Skip();
													Skip();
													Skip();
													Skip();
													Skip();
													Skip();
													#line 274 "EcsLexerGrammar.les"
													_type = TT.Continue;
													#line 274 "EcsLexerGrammar.les"
													_value = S.Continue;
													#line default
												} else
													Id();
											} else
												Id();
										} else
											Id();
									} else
										Id();
								} else
									Id();
							} else
								Id();
						} else
							Id();
					} else
						Id();
				}
				break;
			case 'd':
				{
					la1 = LA(1);
					if (la1 == 'e') {
						la2 = LA(2);
						if (la2 == 'c') {
							la3 = LA(3);
							if (la3 == 'i') {
								la4 = LA(4);
								if (la4 == 'm') {
									la5 = LA(5);
									if (la5 == 'a') {
										la6 = LA(6);
										if (la6 == 'l') {
											la7 = LA(7);
											if (!IdOrKeyword_set0.Contains(la7)) {
												Skip();
												Skip();
												Skip();
												Skip();
												Skip();
												Skip();
												Skip();
												#line 275 "EcsLexerGrammar.les"
												_type = TT.TypeKeyword;
												#line 275 "EcsLexerGrammar.les"
												_value = S.Decimal;
												#line default
											} else
												Id();
										} else
											Id();
									} else
										Id();
								} else
									Id();
							} else
								Id();
						} else if (la2 == 'f') {
							la3 = LA(3);
							if (la3 == 'a') {
								la4 = LA(4);
								if (la4 == 'u') {
									la5 = LA(5);
									if (la5 == 'l') {
										la6 = LA(6);
										if (la6 == 't') {
											la7 = LA(7);
											if (!IdOrKeyword_set0.Contains(la7)) {
												Skip();
												Skip();
												Skip();
												Skip();
												Skip();
												Skip();
												Skip();
												#line 276 "EcsLexerGrammar.les"
												_type = TT.Default;
												#line 276 "EcsLexerGrammar.les"
												_value = S.Default;
												#line default
											} else
												Id();
										} else
											Id();
									} else
										Id();
								} else
									Id();
							} else
								Id();
						} else if (la2 == 'l') {
							la3 = LA(3);
							if (la3 == 'e') {
								la4 = LA(4);
								if (la4 == 'g') {
									la5 = LA(5);
									if (la5 == 'a') {
										la6 = LA(6);
										if (la6 == 't') {
											la7 = LA(7);
											if (la7 == 'e') {
												la8 = LA(8);
												if (!IdOrKeyword_set0.Contains(la8)) {
													Skip();
													Skip();
													Skip();
													Skip();
													Skip();
													Skip();
													Skip();
													Skip();
													#line 277 "EcsLexerGrammar.les"
													_type = TT.Delegate;
													#line 277 "EcsLexerGrammar.les"
													_value = S.Delegate;
													#line default
												} else
													Id();
											} else
												Id();
										} else
											Id();
									} else
										Id();
								} else
									Id();
							} else
								Id();
						} else if (la2 == 's') {
							la3 = LA(3);
							if (la3 == 'c') {
								la4 = LA(4);
								if (la4 == 'e') {
									la5 = LA(5);
									if (la5 == 'n') {
										la6 = LA(6);
										if (la6 == 'd') {
											la7 = LA(7);
											if (la7 == 'i') {
												la8 = LA(8);
												if (la8 == 'n') {
													la9 = LA(9);
													if (la9 == 'g') {
														la10 = LA(10);
														if (!IdOrKeyword_set0.Contains(la10)) {
															Skip();
															Skip();
															Skip();
															Skip();
															Skip();
															Skip();
															Skip();
															Skip();
															Skip();
															Skip();
															#line 369 "EcsLexerGrammar.les"
															OtherContextualKeyword();
															#line default
														} else
															Id();
													} else
														Id();
												} else
													Id();
											} else
												Id();
										} else
											Id();
									} else
										Id();
								} else
									Id();
							} else
								Id();
						} else
							Id();
					} else if (la1 == 'o') {
						la2 = LA(2);
						if (la2 == 'u') {
							la3 = LA(3);
							if (la3 == 'b') {
								la4 = LA(4);
								if (la4 == 'l') {
									la5 = LA(5);
									if (la5 == 'e') {
										la6 = LA(6);
										if (!IdOrKeyword_set0.Contains(la6)) {
											Skip();
											Skip();
											Skip();
											Skip();
											Skip();
											Skip();
											#line 278 "EcsLexerGrammar.les"
											_type = TT.TypeKeyword;
											#line 278 "EcsLexerGrammar.les"
											_value = S.Double;
											#line default
										} else
											Id();
									} else
										Id();
								} else
									Id();
							} else
								Id();
						} else if (!IdOrKeyword_set0.Contains(la2)) {
							Skip();
							Skip();
							#line 279 "EcsLexerGrammar.les"
							_type = TT.Do;
							#line 279 "EcsLexerGrammar.les"
							_value = S.Do;
							#line default
						} else
							Id();
					} else if (la1 == 'y') {
						la2 = LA(2);
						if (la2 == 'n') {
							la3 = LA(3);
							if (la3 == 'a') {
								la4 = LA(4);
								if (la4 == 'm') {
									la5 = LA(5);
									if (la5 == 'i') {
										la6 = LA(6);
										if (la6 == 'c') {
											la7 = LA(7);
											if (!IdOrKeyword_set0.Contains(la7)) {
												Skip();
												Skip();
												Skip();
												Skip();
												Skip();
												Skip();
												Skip();
												#line 353 "EcsLexerGrammar.les"
												_type = TT.ContextualKeyword;
												#line 353 "EcsLexerGrammar.les"
												_value = _dynamic;
												#line default
											} else
												Id();
										} else
											Id();
									} else
										Id();
								} else
									Id();
							} else
								Id();
						} else
							Id();
					} else
						Id();
				}
				break;
			case 'e':
				{
					switch (LA(1)) {
					case 'l':
						{
							la2 = LA(2);
							if (la2 == 's') {
								la3 = LA(3);
								if (la3 == 'e') {
									la4 = LA(4);
									if (!IdOrKeyword_set0.Contains(la4)) {
										Skip();
										Skip();
										Skip();
										Skip();
										#line 280 "EcsLexerGrammar.les"
										_type = TT.Else;
										#line 280 "EcsLexerGrammar.les"
										_value = S.Else;
										#line default
									} else
										Id();
								} else
									Id();
							} else
								Id();
						}
						break;
					case 'n':
						{
							la2 = LA(2);
							if (la2 == 'u') {
								la3 = LA(3);
								if (la3 == 'm') {
									la4 = LA(4);
									if (!IdOrKeyword_set0.Contains(la4)) {
										Skip();
										Skip();
										Skip();
										Skip();
										#line 281 "EcsLexerGrammar.les"
										_type = TT.Enum;
										#line 281 "EcsLexerGrammar.les"
										_value = S.Enum;
										#line default
									} else
										Id();
								} else
									Id();
							} else
								Id();
						}
						break;
					case 'v':
						{
							la2 = LA(2);
							if (la2 == 'e') {
								la3 = LA(3);
								if (la3 == 'n') {
									la4 = LA(4);
									if (la4 == 't') {
										la5 = LA(5);
										if (!IdOrKeyword_set0.Contains(la5)) {
											Skip();
											Skip();
											Skip();
											Skip();
											Skip();
											#line 282 "EcsLexerGrammar.les"
											_type = TT.Event;
											#line 282 "EcsLexerGrammar.les"
											_value = S.Event;
											#line default
										} else
											Id();
									} else
										Id();
								} else
									Id();
							} else
								Id();
						}
						break;
					case 'x':
						{
							la2 = LA(2);
							if (la2 == 'p') {
								la3 = LA(3);
								if (la3 == 'l') {
									la4 = LA(4);
									if (la4 == 'i') {
										la5 = LA(5);
										if (la5 == 'c') {
											la6 = LA(6);
											if (la6 == 'i') {
												la7 = LA(7);
												if (la7 == 't') {
													la8 = LA(8);
													if (!IdOrKeyword_set0.Contains(la8)) {
														Skip();
														Skip();
														Skip();
														Skip();
														Skip();
														Skip();
														Skip();
														Skip();
														#line 283 "EcsLexerGrammar.les"
														_type = TT.AttrKeyword;
														#line 283 "EcsLexerGrammar.les"
														_value = S.Explicit;
														#line default
													} else
														Id();
												} else
													Id();
											} else
												Id();
										} else
											Id();
									} else
										Id();
								} else
									Id();
							} else if (la2 == 't') {
								la3 = LA(3);
								if (la3 == 'e') {
									la4 = LA(4);
									if (la4 == 'r') {
										la5 = LA(5);
										if (la5 == 'n') {
											la6 = LA(6);
											if (!IdOrKeyword_set0.Contains(la6)) {
												Skip();
												Skip();
												Skip();
												Skip();
												Skip();
												Skip();
												#line 284 "EcsLexerGrammar.les"
												_type = TT.AttrKeyword;
												#line 284 "EcsLexerGrammar.les"
												_value = S.Extern;
												#line default
											} else
												Id();
										} else
											Id();
									} else
										Id();
								} else
									Id();
							} else
								Id();
						}
						break;
					case 'q':
						{
							la2 = LA(2);
							if (la2 == 'u') {
								la3 = LA(3);
								if (la3 == 'a') {
									la4 = LA(4);
									if (la4 == 'l') {
										la5 = LA(5);
										if (la5 == 's') {
											la6 = LA(6);
											if (!IdOrKeyword_set0.Contains(la6)) {
												Skip();
												Skip();
												Skip();
												Skip();
												Skip();
												Skip();
												#line 364 "EcsLexerGrammar.les"
												OtherContextualKeyword();
												#line default
											} else
												Id();
										} else
											Id();
									} else
										Id();
								} else
									Id();
							} else
								Id();
						}
						break;
					default:
						Id();
						break;
					}
				}
				break;
			case 'f':
				{
					switch (LA(1)) {
					case 'a':
						{
							la2 = LA(2);
							if (la2 == 'l') {
								la3 = LA(3);
								if (la3 == 's') {
									la4 = LA(4);
									if (la4 == 'e') {
										la5 = LA(5);
										if (!IdOrKeyword_set0.Contains(la5)) {
											Skip();
											Skip();
											Skip();
											Skip();
											Skip();
											#line 285 "EcsLexerGrammar.les"
											_type = TT.Literal;
											#line 285 "EcsLexerGrammar.les"
											_value = G.BoxedFalse;
											#line default
										} else
											Id();
									} else
										Id();
								} else
									Id();
							} else
								Id();
						}
						break;
					case 'i':
						{
							la2 = LA(2);
							if (la2 == 'n') {
								la3 = LA(3);
								if (la3 == 'a') {
									la4 = LA(4);
									if (la4 == 'l') {
										la5 = LA(5);
										if (la5 == 'l') {
											la6 = LA(6);
											if (la6 == 'y') {
												la7 = LA(7);
												if (!IdOrKeyword_set0.Contains(la7)) {
													Skip();
													Skip();
													Skip();
													Skip();
													Skip();
													Skip();
													Skip();
													#line 286 "EcsLexerGrammar.les"
													_type = TT.Finally;
													#line 286 "EcsLexerGrammar.les"
													_value = S.Finally;
													#line default
												} else
													Id();
											} else
												Id();
										} else
											Id();
									} else
										Id();
								} else
									Id();
							} else if (la2 == 'x') {
								la3 = LA(3);
								if (la3 == 'e') {
									la4 = LA(4);
									if (la4 == 'd') {
										la5 = LA(5);
										if (!IdOrKeyword_set0.Contains(la5)) {
											Skip();
											Skip();
											Skip();
											Skip();
											Skip();
											#line 287 "EcsLexerGrammar.les"
											_type = TT.Fixed;
											#line 287 "EcsLexerGrammar.les"
											_value = S.Fixed;
											#line default
										} else
											Id();
									} else
										Id();
								} else
									Id();
							} else
								Id();
						}
						break;
					case 'l':
						{
							la2 = LA(2);
							if (la2 == 'o') {
								la3 = LA(3);
								if (la3 == 'a') {
									la4 = LA(4);
									if (la4 == 't') {
										la5 = LA(5);
										if (!IdOrKeyword_set0.Contains(la5)) {
											Skip();
											Skip();
											Skip();
											Skip();
											Skip();
											#line 288 "EcsLexerGrammar.les"
											_type = TT.TypeKeyword;
											#line 288 "EcsLexerGrammar.les"
											_value = S.Single;
											#line default
										} else
											Id();
									} else
										Id();
								} else
									Id();
							} else
								Id();
						}
						break;
					case 'o':
						{
							la2 = LA(2);
							if (la2 == 'r') {
								la3 = LA(3);
								if (la3 == 'e') {
									la4 = LA(4);
									if (la4 == 'a') {
										la5 = LA(5);
										if (la5 == 'c') {
											la6 = LA(6);
											if (la6 == 'h') {
												la7 = LA(7);
												if (!IdOrKeyword_set0.Contains(la7)) {
													Skip();
													Skip();
													Skip();
													Skip();
													Skip();
													Skip();
													Skip();
													#line 289 "EcsLexerGrammar.les"
													_type = TT.Foreach;
													#line 289 "EcsLexerGrammar.les"
													_value = S.ForEach;
													#line default
												} else
													Id();
											} else
												Id();
										} else
											Id();
									} else
										Id();
								} else if (!IdOrKeyword_set0.Contains(la3)) {
									Skip();
									Skip();
									Skip();
									#line 290 "EcsLexerGrammar.les"
									_type = TT.For;
									#line 290 "EcsLexerGrammar.les"
									_value = S.For;
									#line default
								} else
									Id();
							} else
								Id();
						}
						break;
					case 'r':
						{
							la2 = LA(2);
							if (la2 == 'o') {
								la3 = LA(3);
								if (la3 == 'm') {
									la4 = LA(4);
									if (!IdOrKeyword_set0.Contains(la4)) {
										Skip();
										Skip();
										Skip();
										Skip();
										#line 358 "EcsLexerGrammar.les"
										_type = TT.ContextualKeyword;
										#line 358 "EcsLexerGrammar.les"
										_value = _from;
										#line default
									} else
										Id();
								} else
									Id();
							} else
								Id();
						}
						break;
					default:
						Id();
						break;
					}
				}
				break;
			case 'g':
				{
					la1 = LA(1);
					if (la1 == 'o') {
						la2 = LA(2);
						if (la2 == 't') {
							la3 = LA(3);
							if (la3 == 'o') {
								la4 = LA(4);
								if (!IdOrKeyword_set0.Contains(la4)) {
									Skip();
									Skip();
									Skip();
									Skip();
									#line 291 "EcsLexerGrammar.les"
									_type = TT.Goto;
									#line 291 "EcsLexerGrammar.les"
									_value = S.Goto;
									#line default
								} else
									Id();
							} else
								Id();
						} else
							Id();
					} else if (la1 == 'r') {
						la2 = LA(2);
						if (la2 == 'o') {
							la3 = LA(3);
							if (la3 == 'u') {
								la4 = LA(4);
								if (la4 == 'p') {
									la5 = LA(5);
									if (!IdOrKeyword_set0.Contains(la5)) {
										Skip();
										Skip();
										Skip();
										Skip();
										Skip();
										#line 370 "EcsLexerGrammar.les"
										OtherContextualKeyword();
										#line default
									} else
										Id();
								} else
									Id();
							} else
								Id();
						} else
							Id();
					} else
						Id();
				}
				break;
			case 'i':
				{
					la1 = LA(1);
					if (la1 == 'f') {
						la2 = LA(2);
						if (!IdOrKeyword_set0.Contains(la2)) {
							Skip();
							Skip();
							#line 292 "EcsLexerGrammar.les"
							_type = TT.If;
							#line 292 "EcsLexerGrammar.les"
							_value = S.If;
							#line default
						} else
							Id();
					} else if (la1 == 'm') {
						la2 = LA(2);
						if (la2 == 'p') {
							la3 = LA(3);
							if (la3 == 'l') {
								la4 = LA(4);
								if (la4 == 'i') {
									la5 = LA(5);
									if (la5 == 'c') {
										la6 = LA(6);
										if (la6 == 'i') {
											la7 = LA(7);
											if (la7 == 't') {
												la8 = LA(8);
												if (!IdOrKeyword_set0.Contains(la8)) {
													Skip();
													Skip();
													Skip();
													Skip();
													Skip();
													Skip();
													Skip();
													Skip();
													#line 293 "EcsLexerGrammar.les"
													_type = TT.AttrKeyword;
													#line 293 "EcsLexerGrammar.les"
													_value = S.Implicit;
													#line default
												} else
													Id();
											} else
												Id();
										} else
											Id();
									} else
										Id();
								} else
									Id();
							} else
								Id();
						} else
							Id();
					} else if (la1 == 'n') {
						la2 = LA(2);
						if (la2 == 't') {
							la3 = LA(3);
							if (la3 == 'e') {
								la4 = LA(4);
								if (la4 == 'r') {
									la5 = LA(5);
									if (la5 == 'f') {
										la6 = LA(6);
										if (la6 == 'a') {
											la7 = LA(7);
											if (la7 == 'c') {
												la8 = LA(8);
												if (la8 == 'e') {
													la9 = LA(9);
													if (!IdOrKeyword_set0.Contains(la9)) {
														Skip();
														Skip();
														Skip();
														Skip();
														Skip();
														Skip();
														Skip();
														Skip();
														Skip();
														#line 294 "EcsLexerGrammar.les"
														_type = TT.Interface;
														#line 294 "EcsLexerGrammar.les"
														_value = S.Interface;
														#line default
													} else
														Id();
												} else
													Id();
											} else
												Id();
										} else
											Id();
									} else if (la5 == 'n') {
										la6 = LA(6);
										if (la6 == 'a') {
											la7 = LA(7);
											if (la7 == 'l') {
												la8 = LA(8);
												if (!IdOrKeyword_set0.Contains(la8)) {
													Skip();
													Skip();
													Skip();
													Skip();
													Skip();
													Skip();
													Skip();
													Skip();
													#line 295 "EcsLexerGrammar.les"
													_type = TT.AttrKeyword;
													#line 295 "EcsLexerGrammar.les"
													_value = S.Internal;
													#line default
												} else
													Id();
											} else
												Id();
										} else
											Id();
									} else
										Id();
								} else
									Id();
							} else if (!IdOrKeyword_set0.Contains(la3)) {
								Skip();
								Skip();
								Skip();
								#line 296 "EcsLexerGrammar.les"
								_type = TT.TypeKeyword;
								#line 296 "EcsLexerGrammar.les"
								_value = S.Int32;
								#line default
							} else if (la3 == 'o') {
								la4 = LA(4);
								if (!IdOrKeyword_set0.Contains(la4)) {
									Skip();
									Skip();
									Skip();
									Skip();
									#line 365 "EcsLexerGrammar.les"
									OtherContextualKeyword();
									#line default
								} else
									Id();
							} else
								Id();
						} else if (!IdOrKeyword_set0.Contains(la2)) {
							Skip();
							Skip();
							#line 297 "EcsLexerGrammar.les"
							_type = TT.In;
							#line 297 "EcsLexerGrammar.les"
							_value = S.In;
							#line default
						} else
							Id();
					} else if (la1 == 's') {
						la2 = LA(2);
						if (!IdOrKeyword_set0.Contains(la2)) {
							Skip();
							Skip();
							#line 298 "EcsLexerGrammar.les"
							_type = TT.Is;
							#line 298 "EcsLexerGrammar.les"
							_value = S.Is;
							#line default
						} else
							Id();
					} else
						Id();
				}
				break;
			case 'l':
				{
					la1 = LA(1);
					if (la1 == 'o') {
						la2 = LA(2);
						if (la2 == 'c') {
							la3 = LA(3);
							if (la3 == 'k') {
								la4 = LA(4);
								if (!IdOrKeyword_set0.Contains(la4)) {
									Skip();
									Skip();
									Skip();
									Skip();
									#line 299 "EcsLexerGrammar.les"
									_type = TT.Lock;
									#line 299 "EcsLexerGrammar.les"
									_value = S.Lock;
									#line default
								} else
									Id();
							} else
								Id();
						} else if (la2 == 'n') {
							la3 = LA(3);
							if (la3 == 'g') {
								la4 = LA(4);
								if (!IdOrKeyword_set0.Contains(la4)) {
									Skip();
									Skip();
									Skip();
									Skip();
									#line 300 "EcsLexerGrammar.les"
									_type = TT.TypeKeyword;
									#line 300 "EcsLexerGrammar.les"
									_value = S.Int64;
									#line default
								} else
									Id();
							} else
								Id();
						} else
							Id();
					} else if (la1 == 'e') {
						la2 = LA(2);
						if (la2 == 't') {
							la3 = LA(3);
							if (!IdOrKeyword_set0.Contains(la3)) {
								Skip();
								Skip();
								Skip();
								#line 366 "EcsLexerGrammar.les"
								OtherContextualKeyword();
								#line default
							} else
								Id();
						} else
							Id();
					} else
						Id();
				}
				break;
			case 'n':
				{
					la1 = LA(1);
					if (la1 == 'a') {
						la2 = LA(2);
						if (la2 == 'm') {
							la3 = LA(3);
							if (la3 == 'e') {
								la4 = LA(4);
								if (la4 == 's') {
									la5 = LA(5);
									if (la5 == 'p') {
										la6 = LA(6);
										if (la6 == 'a') {
											la7 = LA(7);
											if (la7 == 'c') {
												la8 = LA(8);
												if (la8 == 'e') {
													la9 = LA(9);
													if (!IdOrKeyword_set0.Contains(la9)) {
														Skip();
														Skip();
														Skip();
														Skip();
														Skip();
														Skip();
														Skip();
														Skip();
														Skip();
														#line 301 "EcsLexerGrammar.les"
														_type = TT.Namespace;
														#line 301 "EcsLexerGrammar.les"
														_value = S.Namespace;
														#line default
													} else
														Id();
												} else
													Id();
											} else
												Id();
										} else
											Id();
									} else
										Id();
								} else
									Id();
							} else
								Id();
						} else
							Id();
					} else if (la1 == 'e') {
						la2 = LA(2);
						if (la2 == 'w') {
							la3 = LA(3);
							if (!IdOrKeyword_set0.Contains(la3)) {
								Skip();
								Skip();
								Skip();
								#line 302 "EcsLexerGrammar.les"
								_type = TT.New;
								#line 302 "EcsLexerGrammar.les"
								_value = S.New;
								#line default
							} else
								Id();
						} else
							Id();
					} else if (la1 == 'u') {
						la2 = LA(2);
						if (la2 == 'l') {
							la3 = LA(3);
							if (la3 == 'l') {
								la4 = LA(4);
								if (!IdOrKeyword_set0.Contains(la4)) {
									Skip();
									Skip();
									Skip();
									Skip();
									#line 303 "EcsLexerGrammar.les"
									_type = TT.Literal;
									#line 303 "EcsLexerGrammar.les"
									_value = null;
									#line default
								} else
									Id();
							} else
								Id();
						} else
							Id();
					} else
						Id();
				}
				break;
			case 'o':
				{
					switch (LA(1)) {
					case 'b':
						{
							la2 = LA(2);
							if (la2 == 'j') {
								la3 = LA(3);
								if (la3 == 'e') {
									la4 = LA(4);
									if (la4 == 'c') {
										la5 = LA(5);
										if (la5 == 't') {
											la6 = LA(6);
											if (!IdOrKeyword_set0.Contains(la6)) {
												Skip();
												Skip();
												Skip();
												Skip();
												Skip();
												Skip();
												#line 304 "EcsLexerGrammar.les"
												_type = TT.TypeKeyword;
												#line 304 "EcsLexerGrammar.les"
												_value = S.Object;
												#line default
											} else
												Id();
										} else
											Id();
									} else
										Id();
								} else
									Id();
							} else
								Id();
						}
						break;
					case 'p':
						{
							la2 = LA(2);
							if (la2 == 'e') {
								la3 = LA(3);
								if (la3 == 'r') {
									la4 = LA(4);
									if (la4 == 'a') {
										la5 = LA(5);
										if (la5 == 't') {
											la6 = LA(6);
											if (la6 == 'o') {
												la7 = LA(7);
												if (la7 == 'r') {
													la8 = LA(8);
													if (!IdOrKeyword_set0.Contains(la8)) {
														Skip();
														Skip();
														Skip();
														Skip();
														Skip();
														Skip();
														Skip();
														Skip();
														#line 305 "EcsLexerGrammar.les"
														_type = TT.Operator;
														#line 305 "EcsLexerGrammar.les"
														_value = S.Operator;
														#line default
													} else
														Id();
												} else
													Id();
											} else
												Id();
										} else
											Id();
									} else
										Id();
								} else
									Id();
							} else
								Id();
						}
						break;
					case 'u':
						{
							la2 = LA(2);
							if (la2 == 't') {
								la3 = LA(3);
								if (!IdOrKeyword_set0.Contains(la3)) {
									Skip();
									Skip();
									Skip();
									#line 306 "EcsLexerGrammar.les"
									_type = TT.AttrKeyword;
									#line 306 "EcsLexerGrammar.les"
									_value = S.Out;
									#line default
								} else
									Id();
							} else
								Id();
						}
						break;
					case 'v':
						{
							la2 = LA(2);
							if (la2 == 'e') {
								la3 = LA(3);
								if (la3 == 'r') {
									la4 = LA(4);
									if (la4 == 'r') {
										la5 = LA(5);
										if (la5 == 'i') {
											la6 = LA(6);
											if (la6 == 'd') {
												la7 = LA(7);
												if (la7 == 'e') {
													la8 = LA(8);
													if (!IdOrKeyword_set0.Contains(la8)) {
														Skip();
														Skip();
														Skip();
														Skip();
														Skip();
														Skip();
														Skip();
														Skip();
														#line 307 "EcsLexerGrammar.les"
														_type = TT.AttrKeyword;
														#line 307 "EcsLexerGrammar.les"
														_value = S.Override;
														#line default
													} else
														Id();
												} else
													Id();
											} else
												Id();
										} else
											Id();
									} else
										Id();
								} else
									Id();
							} else
								Id();
						}
						break;
					case 'n':
						{
							la2 = LA(2);
							if (!IdOrKeyword_set0.Contains(la2)) {
								Skip();
								Skip();
								#line 363 "EcsLexerGrammar.les"
								OtherContextualKeyword();
								#line default
							} else
								Id();
						}
						break;
					case 'r':
						{
							la2 = LA(2);
							if (la2 == 'd') {
								la3 = LA(3);
								if (la3 == 'e') {
									la4 = LA(4);
									if (la4 == 'r') {
										la5 = LA(5);
										if (la5 == 'b') {
											la6 = LA(6);
											if (la6 == 'y') {
												la7 = LA(7);
												if (!IdOrKeyword_set0.Contains(la7)) {
													Skip();
													Skip();
													Skip();
													Skip();
													Skip();
													Skip();
													Skip();
													#line 367 "EcsLexerGrammar.les"
													OtherContextualKeyword();
													#line default
												} else
													Id();
											} else
												Id();
										} else
											Id();
									} else
										Id();
								} else
									Id();
							} else
								Id();
						}
						break;
					default:
						Id();
						break;
					}
				}
				break;
			case 'p':
				{
					la1 = LA(1);
					if (la1 == 'a') {
						la2 = LA(2);
						if (la2 == 'r') {
							la3 = LA(3);
							if (la3 == 'a') {
								la4 = LA(4);
								if (la4 == 'm') {
									la5 = LA(5);
									if (la5 == 's') {
										la6 = LA(6);
										if (!IdOrKeyword_set0.Contains(la6)) {
											Skip();
											Skip();
											Skip();
											Skip();
											Skip();
											Skip();
											#line 308 "EcsLexerGrammar.les"
											_type = TT.AttrKeyword;
											#line 308 "EcsLexerGrammar.les"
											_value = S.Params;
											#line default
										} else
											Id();
									} else
										Id();
								} else
									Id();
							} else
								Id();
						} else
							Id();
					} else if (la1 == 'r') {
						la2 = LA(2);
						if (la2 == 'i') {
							la3 = LA(3);
							if (la3 == 'v') {
								la4 = LA(4);
								if (la4 == 'a') {
									la5 = LA(5);
									if (la5 == 't') {
										la6 = LA(6);
										if (la6 == 'e') {
											la7 = LA(7);
											if (!IdOrKeyword_set0.Contains(la7)) {
												Skip();
												Skip();
												Skip();
												Skip();
												Skip();
												Skip();
												Skip();
												#line 309 "EcsLexerGrammar.les"
												_type = TT.AttrKeyword;
												#line 309 "EcsLexerGrammar.les"
												_value = S.Private;
												#line default
											} else
												Id();
										} else
											Id();
									} else
										Id();
								} else
									Id();
							} else
								Id();
						} else if (la2 == 'o') {
							la3 = LA(3);
							if (la3 == 't') {
								la4 = LA(4);
								if (la4 == 'e') {
									la5 = LA(5);
									if (la5 == 'c') {
										la6 = LA(6);
										if (la6 == 't') {
											la7 = LA(7);
											if (la7 == 'e') {
												la8 = LA(8);
												if (la8 == 'd') {
													la9 = LA(9);
													if (!IdOrKeyword_set0.Contains(la9)) {
														Skip();
														Skip();
														Skip();
														Skip();
														Skip();
														Skip();
														Skip();
														Skip();
														Skip();
														#line 310 "EcsLexerGrammar.les"
														_type = TT.AttrKeyword;
														#line 310 "EcsLexerGrammar.les"
														_value = S.Protected;
														#line default
													} else
														Id();
												} else
													Id();
											} else
												Id();
										} else
											Id();
									} else
										Id();
								} else
									Id();
							} else
								Id();
						} else
							Id();
					} else if (la1 == 'u') {
						la2 = LA(2);
						if (la2 == 'b') {
							la3 = LA(3);
							if (la3 == 'l') {
								la4 = LA(4);
								if (la4 == 'i') {
									la5 = LA(5);
									if (la5 == 'c') {
										la6 = LA(6);
										if (!IdOrKeyword_set0.Contains(la6)) {
											Skip();
											Skip();
											Skip();
											Skip();
											Skip();
											Skip();
											#line 311 "EcsLexerGrammar.les"
											_type = TT.AttrKeyword;
											#line 311 "EcsLexerGrammar.les"
											_value = S.Public;
											#line default
										} else
											Id();
									} else
										Id();
								} else
									Id();
							} else
								Id();
						} else
							Id();
					} else
						Id();
				}
				break;
			case 'r':
				{
					la1 = LA(1);
					if (la1 == 'e') {
						la2 = LA(2);
						if (la2 == 'a') {
							la3 = LA(3);
							if (la3 == 'd') {
								la4 = LA(4);
								if (la4 == 'o') {
									la5 = LA(5);
									if (la5 == 'n') {
										la6 = LA(6);
										if (la6 == 'l') {
											la7 = LA(7);
											if (la7 == 'y') {
												la8 = LA(8);
												if (!IdOrKeyword_set0.Contains(la8)) {
													Skip();
													Skip();
													Skip();
													Skip();
													Skip();
													Skip();
													Skip();
													Skip();
													#line 312 "EcsLexerGrammar.les"
													_type = TT.AttrKeyword;
													#line 312 "EcsLexerGrammar.les"
													_value = S.Readonly;
													#line default
												} else
													Id();
											} else
												Id();
										} else
											Id();
									} else
										Id();
								} else
									Id();
							} else
								Id();
						} else if (la2 == 'f') {
							la3 = LA(3);
							if (!IdOrKeyword_set0.Contains(la3)) {
								Skip();
								Skip();
								Skip();
								#line 313 "EcsLexerGrammar.les"
								_type = TT.AttrKeyword;
								#line 313 "EcsLexerGrammar.les"
								_value = S.Ref;
								#line default
							} else
								Id();
						} else if (la2 == 't') {
							la3 = LA(3);
							if (la3 == 'u') {
								la4 = LA(4);
								if (la4 == 'r') {
									la5 = LA(5);
									if (la5 == 'n') {
										la6 = LA(6);
										if (!IdOrKeyword_set0.Contains(la6)) {
											Skip();
											Skip();
											Skip();
											Skip();
											Skip();
											Skip();
											#line 314 "EcsLexerGrammar.les"
											_type = TT.Return;
											#line 314 "EcsLexerGrammar.les"
											_value = S.Return;
											#line default
										} else
											Id();
									} else
										Id();
								} else
									Id();
							} else
								Id();
						} else
							Id();
					} else
						Id();
				}
				break;
			case 's':
				{
					switch (LA(1)) {
					case 'b':
						{
							la2 = LA(2);
							if (la2 == 'y') {
								la3 = LA(3);
								if (la3 == 't') {
									la4 = LA(4);
									if (la4 == 'e') {
										la5 = LA(5);
										if (!IdOrKeyword_set0.Contains(la5)) {
											Skip();
											Skip();
											Skip();
											Skip();
											Skip();
											#line 315 "EcsLexerGrammar.les"
											_type = TT.TypeKeyword;
											#line 315 "EcsLexerGrammar.les"
											_value = S.Int8;
											#line default
										} else
											Id();
									} else
										Id();
								} else
									Id();
							} else
								Id();
						}
						break;
					case 'e':
						{
							la2 = LA(2);
							if (la2 == 'a') {
								la3 = LA(3);
								if (la3 == 'l') {
									la4 = LA(4);
									if (la4 == 'e') {
										la5 = LA(5);
										if (la5 == 'd') {
											la6 = LA(6);
											if (!IdOrKeyword_set0.Contains(la6)) {
												Skip();
												Skip();
												Skip();
												Skip();
												Skip();
												Skip();
												#line 316 "EcsLexerGrammar.les"
												_type = TT.AttrKeyword;
												#line 316 "EcsLexerGrammar.les"
												_value = S.Sealed;
												#line default
											} else
												Id();
										} else
											Id();
									} else
										Id();
								} else
									Id();
							} else if (la2 == 'l') {
								la3 = LA(3);
								if (la3 == 'e') {
									la4 = LA(4);
									if (la4 == 'c') {
										la5 = LA(5);
										if (la5 == 't') {
											la6 = LA(6);
											if (!IdOrKeyword_set0.Contains(la6)) {
												Skip();
												Skip();
												Skip();
												Skip();
												Skip();
												Skip();
												#line 361 "EcsLexerGrammar.les"
												_type = TT.ContextualKeyword;
												#line 361 "EcsLexerGrammar.les"
												_value = _select;
												#line default
											} else
												Id();
										} else
											Id();
									} else
										Id();
								} else
									Id();
							} else
								Id();
						}
						break;
					case 'h':
						{
							la2 = LA(2);
							if (la2 == 'o') {
								la3 = LA(3);
								if (la3 == 'r') {
									la4 = LA(4);
									if (la4 == 't') {
										la5 = LA(5);
										if (!IdOrKeyword_set0.Contains(la5)) {
											Skip();
											Skip();
											Skip();
											Skip();
											Skip();
											#line 317 "EcsLexerGrammar.les"
											_type = TT.TypeKeyword;
											#line 317 "EcsLexerGrammar.les"
											_value = S.Int16;
											#line default
										} else
											Id();
									} else
										Id();
								} else
									Id();
							} else
								Id();
						}
						break;
					case 'i':
						{
							la2 = LA(2);
							if (la2 == 'z') {
								la3 = LA(3);
								if (la3 == 'e') {
									la4 = LA(4);
									if (la4 == 'o') {
										la5 = LA(5);
										if (la5 == 'f') {
											la6 = LA(6);
											if (!IdOrKeyword_set0.Contains(la6)) {
												Skip();
												Skip();
												Skip();
												Skip();
												Skip();
												Skip();
												#line 318 "EcsLexerGrammar.les"
												_type = TT.Sizeof;
												#line 318 "EcsLexerGrammar.les"
												_value = S.Sizeof;
												#line default
											} else
												Id();
										} else
											Id();
									} else
										Id();
								} else
									Id();
							} else
								Id();
						}
						break;
					case 't':
						{
							la2 = LA(2);
							if (la2 == 'a') {
								la3 = LA(3);
								if (la3 == 'c') {
									la4 = LA(4);
									if (la4 == 'k') {
										la5 = LA(5);
										if (la5 == 'a') {
											la6 = LA(6);
											if (la6 == 'l') {
												la7 = LA(7);
												if (la7 == 'l') {
													la8 = LA(8);
													if (la8 == 'o') {
														la9 = LA(9);
														if (la9 == 'c') {
															la10 = LA(10);
															if (!IdOrKeyword_set0.Contains(la10)) {
																Skip();
																Skip();
																Skip();
																Skip();
																Skip();
																Skip();
																Skip();
																Skip();
																Skip();
																Skip();
																#line 319 "EcsLexerGrammar.les"
																_type = TT.Stackalloc;
																#line 319 "EcsLexerGrammar.les"
																_value = S.StackAlloc;
																#line default
															} else
																Id();
														} else
															Id();
													} else
														Id();
												} else
													Id();
											} else
												Id();
										} else
											Id();
									} else
										Id();
								} else if (la3 == 't') {
									la4 = LA(4);
									if (la4 == 'i') {
										la5 = LA(5);
										if (la5 == 'c') {
											la6 = LA(6);
											if (!IdOrKeyword_set0.Contains(la6)) {
												Skip();
												Skip();
												Skip();
												Skip();
												Skip();
												Skip();
												#line 320 "EcsLexerGrammar.les"
												_type = TT.AttrKeyword;
												#line 320 "EcsLexerGrammar.les"
												_value = S.Static;
												#line default
											} else
												Id();
										} else
											Id();
									} else
										Id();
								} else
									Id();
							} else if (la2 == 'r') {
								la3 = LA(3);
								if (la3 == 'i') {
									la4 = LA(4);
									if (la4 == 'n') {
										la5 = LA(5);
										if (la5 == 'g') {
											la6 = LA(6);
											if (!IdOrKeyword_set0.Contains(la6)) {
												Skip();
												Skip();
												Skip();
												Skip();
												Skip();
												Skip();
												#line 321 "EcsLexerGrammar.les"
												_type = TT.TypeKeyword;
												#line 321 "EcsLexerGrammar.les"
												_value = S.String;
												#line default
											} else
												Id();
										} else
											Id();
									} else
										Id();
								} else if (la3 == 'u') {
									la4 = LA(4);
									if (la4 == 'c') {
										la5 = LA(5);
										if (la5 == 't') {
											la6 = LA(6);
											if (!IdOrKeyword_set0.Contains(la6)) {
												Skip();
												Skip();
												Skip();
												Skip();
												Skip();
												Skip();
												#line 322 "EcsLexerGrammar.les"
												_type = TT.Struct;
												#line 322 "EcsLexerGrammar.les"
												_value = S.Struct;
												#line default
											} else
												Id();
										} else
											Id();
									} else
										Id();
								} else
									Id();
							} else
								Id();
						}
						break;
					case 'w':
						{
							la2 = LA(2);
							if (la2 == 'i') {
								la3 = LA(3);
								if (la3 == 't') {
									la4 = LA(4);
									if (la4 == 'c') {
										la5 = LA(5);
										if (la5 == 'h') {
											la6 = LA(6);
											if (!IdOrKeyword_set0.Contains(la6)) {
												Skip();
												Skip();
												Skip();
												Skip();
												Skip();
												Skip();
												#line 323 "EcsLexerGrammar.les"
												_type = TT.Switch;
												#line 323 "EcsLexerGrammar.les"
												_value = S.Switch;
												#line default
											} else
												Id();
										} else
											Id();
									} else
										Id();
								} else
									Id();
							} else
								Id();
						}
						break;
					default:
						Id();
						break;
					}
				}
				break;
			case 't':
				{
					la1 = LA(1);
					if (la1 == 'h') {
						la2 = LA(2);
						if (la2 == 'i') {
							la3 = LA(3);
							if (la3 == 's') {
								la4 = LA(4);
								if (!IdOrKeyword_set0.Contains(la4)) {
									Skip();
									Skip();
									Skip();
									Skip();
									#line 324 "EcsLexerGrammar.les"
									_type = TT.This;
									#line 324 "EcsLexerGrammar.les"
									_value = S.This;
									#line default
								} else
									Id();
							} else
								Id();
						} else if (la2 == 'r') {
							la3 = LA(3);
							if (la3 == 'o') {
								la4 = LA(4);
								if (la4 == 'w') {
									la5 = LA(5);
									if (!IdOrKeyword_set0.Contains(la5)) {
										Skip();
										Skip();
										Skip();
										Skip();
										Skip();
										#line 325 "EcsLexerGrammar.les"
										_type = TT.Throw;
										#line 325 "EcsLexerGrammar.les"
										_value = S.Throw;
										#line default
									} else
										Id();
								} else
									Id();
							} else
								Id();
						} else
							Id();
					} else if (la1 == 'r') {
						la2 = LA(2);
						if (la2 == 'u') {
							la3 = LA(3);
							if (la3 == 'e') {
								la4 = LA(4);
								if (!IdOrKeyword_set0.Contains(la4)) {
									Skip();
									Skip();
									Skip();
									Skip();
									#line 326 "EcsLexerGrammar.les"
									_type = TT.Literal;
									#line 326 "EcsLexerGrammar.les"
									_value = G.BoxedTrue;
									#line default
								} else
									Id();
							} else
								Id();
						} else if (la2 == 'y') {
							la3 = LA(3);
							if (!IdOrKeyword_set0.Contains(la3)) {
								Skip();
								Skip();
								Skip();
								#line 327 "EcsLexerGrammar.les"
								_type = TT.Try;
								#line 327 "EcsLexerGrammar.les"
								_value = S.Try;
								#line default
							} else
								Id();
						} else if (la2 == 'a') {
							la3 = LA(3);
							if (la3 == 'i') {
								la4 = LA(4);
								if (la4 == 't') {
									la5 = LA(5);
									if (!IdOrKeyword_set0.Contains(la5)) {
										Skip();
										Skip();
										Skip();
										Skip();
										Skip();
										#line 354 "EcsLexerGrammar.les"
										_type = TT.ContextualKeyword;
										#line 354 "EcsLexerGrammar.les"
										_value = _trait;
										#line default
									} else
										Id();
								} else
									Id();
							} else
								Id();
						} else
							Id();
					} else if (la1 == 'y') {
						la2 = LA(2);
						if (la2 == 'p') {
							la3 = LA(3);
							if (la3 == 'e') {
								la4 = LA(4);
								if (la4 == 'o') {
									la5 = LA(5);
									if (la5 == 'f') {
										la6 = LA(6);
										if (!IdOrKeyword_set0.Contains(la6)) {
											Skip();
											Skip();
											Skip();
											Skip();
											Skip();
											Skip();
											#line 328 "EcsLexerGrammar.les"
											_type = TT.Typeof;
											#line 328 "EcsLexerGrammar.les"
											_value = S.Typeof;
											#line default
										} else
											Id();
									} else
										Id();
								} else
									Id();
							} else
								Id();
						} else
							Id();
					} else
						Id();
				}
				break;
			case 'u':
				{
					la1 = LA(1);
					if (la1 == 'i') {
						la2 = LA(2);
						if (la2 == 'n') {
							la3 = LA(3);
							if (la3 == 't') {
								la4 = LA(4);
								if (!IdOrKeyword_set0.Contains(la4)) {
									Skip();
									Skip();
									Skip();
									Skip();
									#line 329 "EcsLexerGrammar.les"
									_type = TT.TypeKeyword;
									#line 329 "EcsLexerGrammar.les"
									_value = S.UInt32;
									#line default
								} else
									Id();
							} else
								Id();
						} else
							Id();
					} else if (la1 == 'l') {
						la2 = LA(2);
						if (la2 == 'o') {
							la3 = LA(3);
							if (la3 == 'n') {
								la4 = LA(4);
								if (la4 == 'g') {
									la5 = LA(5);
									if (!IdOrKeyword_set0.Contains(la5)) {
										Skip();
										Skip();
										Skip();
										Skip();
										Skip();
										#line 330 "EcsLexerGrammar.les"
										_type = TT.TypeKeyword;
										#line 330 "EcsLexerGrammar.les"
										_value = S.UInt64;
										#line default
									} else
										Id();
								} else
									Id();
							} else
								Id();
						} else
							Id();
					} else if (la1 == 'n') {
						la2 = LA(2);
						if (la2 == 'c') {
							la3 = LA(3);
							if (la3 == 'h') {
								la4 = LA(4);
								if (la4 == 'e') {
									la5 = LA(5);
									if (la5 == 'c') {
										la6 = LA(6);
										if (la6 == 'k') {
											la7 = LA(7);
											if (la7 == 'e') {
												la8 = LA(8);
												if (la8 == 'd') {
													la9 = LA(9);
													if (!IdOrKeyword_set0.Contains(la9)) {
														Skip();
														Skip();
														Skip();
														Skip();
														Skip();
														Skip();
														Skip();
														Skip();
														Skip();
														#line 331 "EcsLexerGrammar.les"
														_type = TT.Unchecked;
														#line 331 "EcsLexerGrammar.les"
														_value = S.Unchecked;
														#line default
													} else
														Id();
												} else
													Id();
											} else
												Id();
										} else
											Id();
									} else
										Id();
								} else
									Id();
							} else
								Id();
						} else if (la2 == 's') {
							la3 = LA(3);
							if (la3 == 'a') {
								la4 = LA(4);
								if (la4 == 'f') {
									la5 = LA(5);
									if (la5 == 'e') {
										la6 = LA(6);
										if (!IdOrKeyword_set0.Contains(la6)) {
											Skip();
											Skip();
											Skip();
											Skip();
											Skip();
											Skip();
											#line 332 "EcsLexerGrammar.les"
											_type = TT.AttrKeyword;
											#line 332 "EcsLexerGrammar.les"
											_value = S.Unsafe;
											#line default
										} else
											Id();
									} else
										Id();
								} else
									Id();
							} else
								Id();
						} else
							Id();
					} else if (la1 == 's') {
						la2 = LA(2);
						if (la2 == 'h') {
							la3 = LA(3);
							if (la3 == 'o') {
								la4 = LA(4);
								if (la4 == 'r') {
									la5 = LA(5);
									if (la5 == 't') {
										la6 = LA(6);
										if (!IdOrKeyword_set0.Contains(la6)) {
											Skip();
											Skip();
											Skip();
											Skip();
											Skip();
											Skip();
											#line 333 "EcsLexerGrammar.les"
											_type = TT.TypeKeyword;
											#line 333 "EcsLexerGrammar.les"
											_value = S.UInt16;
											#line default
										} else
											Id();
									} else
										Id();
								} else
									Id();
							} else
								Id();
						} else if (la2 == 'i') {
							la3 = LA(3);
							if (la3 == 'n') {
								la4 = LA(4);
								if (la4 == 'g') {
									la5 = LA(5);
									if (!IdOrKeyword_set0.Contains(la5)) {
										Skip();
										Skip();
										Skip();
										Skip();
										Skip();
										#line 334 "EcsLexerGrammar.les"
										_type = TT.Using;
										#line 334 "EcsLexerGrammar.les"
										_value = S.UsingStmt;
										#line default
									} else
										Id();
								} else
									Id();
							} else
								Id();
						} else
							Id();
					} else
						Id();
				}
				break;
			case 'v':
				{
					la1 = LA(1);
					if (la1 == 'i') {
						la2 = LA(2);
						if (la2 == 'r') {
							la3 = LA(3);
							if (la3 == 't') {
								la4 = LA(4);
								if (la4 == 'u') {
									la5 = LA(5);
									if (la5 == 'a') {
										la6 = LA(6);
										if (la6 == 'l') {
											la7 = LA(7);
											if (!IdOrKeyword_set0.Contains(la7)) {
												Skip();
												Skip();
												Skip();
												Skip();
												Skip();
												Skip();
												Skip();
												#line 335 "EcsLexerGrammar.les"
												_type = TT.AttrKeyword;
												#line 335 "EcsLexerGrammar.les"
												_value = S.Virtual;
												#line default
											} else
												Id();
										} else
											Id();
									} else
										Id();
								} else
									Id();
							} else
								Id();
						} else
							Id();
					} else if (la1 == 'o') {
						la2 = LA(2);
						if (la2 == 'l') {
							la3 = LA(3);
							if (la3 == 'a') {
								la4 = LA(4);
								if (la4 == 't') {
									la5 = LA(5);
									if (la5 == 'i') {
										la6 = LA(6);
										if (la6 == 'l') {
											la7 = LA(7);
											if (la7 == 'e') {
												la8 = LA(8);
												if (!IdOrKeyword_set0.Contains(la8)) {
													Skip();
													Skip();
													Skip();
													Skip();
													Skip();
													Skip();
													Skip();
													Skip();
													#line 336 "EcsLexerGrammar.les"
													_type = TT.AttrKeyword;
													#line 336 "EcsLexerGrammar.les"
													_value = S.Volatile;
													#line default
												} else
													Id();
											} else
												Id();
										} else
											Id();
									} else
										Id();
								} else
									Id();
							} else
								Id();
						} else if (la2 == 'i') {
							la3 = LA(3);
							if (la3 == 'd') {
								la4 = LA(4);
								if (!IdOrKeyword_set0.Contains(la4)) {
									Skip();
									Skip();
									Skip();
									Skip();
									#line 337 "EcsLexerGrammar.les"
									_type = TT.TypeKeyword;
									#line 337 "EcsLexerGrammar.les"
									_value = S.Void;
									#line default
								} else
									Id();
							} else
								Id();
						} else
							Id();
					} else if (la1 == 'a') {
						la2 = LA(2);
						if (la2 == 'r') {
							la3 = LA(3);
							if (!IdOrKeyword_set0.Contains(la3)) {
								Skip();
								Skip();
								Skip();
								#line 352 "EcsLexerGrammar.les"
								_type = TT.ContextualKeyword;
								#line 352 "EcsLexerGrammar.les"
								_value = _var;
								#line default
							} else
								Id();
						} else
							Id();
					} else
						Id();
				}
				break;
			case 'w':
				{
					la1 = LA(1);
					if (la1 == 'h') {
						la2 = LA(2);
						if (la2 == 'i') {
							la3 = LA(3);
							if (la3 == 'l') {
								la4 = LA(4);
								if (la4 == 'e') {
									la5 = LA(5);
									if (!IdOrKeyword_set0.Contains(la5)) {
										Skip();
										Skip();
										Skip();
										Skip();
										Skip();
										#line 338 "EcsLexerGrammar.les"
										_type = TT.While;
										#line 338 "EcsLexerGrammar.les"
										_value = S.While;
										#line default
									} else
										Id();
								} else
									Id();
							} else
								Id();
						} else if (la2 == 'e') {
							la3 = LA(3);
							if (la3 == 'r') {
								la4 = LA(4);
								if (la4 == 'e') {
									la5 = LA(5);
									if (!IdOrKeyword_set0.Contains(la5)) {
										Skip();
										Skip();
										Skip();
										Skip();
										Skip();
										#line 359 "EcsLexerGrammar.les"
										_type = TT.ContextualKeyword;
										#line 359 "EcsLexerGrammar.les"
										_value = _where;
										#line default
									} else
										Id();
								} else
									Id();
							} else if (la3 == 'n') {
								la4 = LA(4);
								if (!IdOrKeyword_set0.Contains(la4)) {
									Skip();
									Skip();
									Skip();
									Skip();
									#line 360 "EcsLexerGrammar.les"
									_type = TT.ContextualKeyword;
									#line 360 "EcsLexerGrammar.les"
									_value = _when;
									#line default
								} else
									Id();
							} else
								Id();
						} else
							Id();
					} else
						Id();
				}
				break;
			case '#':
				{
					if (AllowPP) {
						switch (LA(1)) {
						case 'i':
							{
								la2 = LA(2);
								if (la2 == 'f') {
									la3 = LA(3);
									if (!IdOrKeyword_set0.Contains(la3)) {
										Skip();
										Skip();
										Skip();
										#line 339 "EcsLexerGrammar.les"
										_type = TT.PPif;
										#line 339 "EcsLexerGrammar.les"
										_value = S.PPIf;
										#line default
									} else
										Id();
								} else
									Id();
							}
							break;
						case 'e':
							{
								la2 = LA(2);
								if (la2 == 'l') {
									la3 = LA(3);
									if (la3 == 's') {
										la4 = LA(4);
										if (la4 == 'e') {
											la5 = LA(5);
											if (!IdOrKeyword_set0.Contains(la5)) {
												Skip();
												Skip();
												Skip();
												Skip();
												Skip();
												#line 340 "EcsLexerGrammar.les"
												_type = TT.PPelse;
												#line 340 "EcsLexerGrammar.les"
												_value = S.PPElse;
												#line default
											} else
												Id();
										} else
											Id();
									} else if (la3 == 'i') {
										la4 = LA(4);
										if (la4 == 'f') {
											la5 = LA(5);
											if (!IdOrKeyword_set0.Contains(la5)) {
												Skip();
												Skip();
												Skip();
												Skip();
												Skip();
												#line 341 "EcsLexerGrammar.les"
												_type = TT.PPelif;
												#line 341 "EcsLexerGrammar.les"
												_value = S.PPElIf;
												#line default
											} else
												Id();
										} else
											Id();
									} else
										Id();
								} else if (la2 == 'n') {
									la3 = LA(3);
									if (la3 == 'd') {
										la4 = LA(4);
										if (la4 == 'i') {
											la5 = LA(5);
											if (la5 == 'f') {
												la6 = LA(6);
												if (!IdOrKeyword_set0.Contains(la6)) {
													Skip();
													Skip();
													Skip();
													Skip();
													Skip();
													Skip();
													#line 342 "EcsLexerGrammar.les"
													_type = TT.PPendif;
													#line 342 "EcsLexerGrammar.les"
													_value = S.PPEndIf;
													#line default
												} else
													Id();
											} else
												Id();
										} else if (la4 == 'r') {
											la5 = LA(5);
											if (la5 == 'e') {
												la6 = LA(6);
												if (la6 == 'g') {
													la7 = LA(7);
													if (la7 == 'i') {
														la8 = LA(8);
														if (la8 == 'o') {
															la9 = LA(9);
															if (la9 == 'n') {
																la10 = LA(10);
																if (!IdOrKeyword_set0.Contains(la10)) {
																	Skip();
																	Skip();
																	Skip();
																	Skip();
																	Skip();
																	Skip();
																	Skip();
																	Skip();
																	Skip();
																	Skip();
																	#line 351 "EcsLexerGrammar.les"
																	_type = TT.PPendregion;
																	#line 351 "EcsLexerGrammar.les"
																	_value = S.PPEndRegion;
																	#line default
																} else
																	Id();
															} else
																Id();
														} else
															Id();
													} else
														Id();
												} else
													Id();
											} else
												Id();
										} else
											Id();
									} else
										Id();
								} else if (la2 == 'r') {
									la3 = LA(3);
									if (la3 == 'r') {
										la4 = LA(4);
										if (la4 == 'o') {
											la5 = LA(5);
											if (la5 == 'r') {
												la6 = LA(6);
												if (!IdOrKeyword_set0.Contains(la6)) {
													Skip();
													Skip();
													Skip();
													Skip();
													Skip();
													Skip();
													#line 347 "EcsLexerGrammar.les"
													_type = TT.PPerror;
													#line default
													_value = RestOfPPLine();
												} else
													Id();
											} else
												Id();
										} else
											Id();
									} else
										Id();
								} else
									Id();
							}
							break;
						case 'd':
							{
								la2 = LA(2);
								if (la2 == 'e') {
									la3 = LA(3);
									if (la3 == 'f') {
										la4 = LA(4);
										if (la4 == 'i') {
											la5 = LA(5);
											if (la5 == 'n') {
												la6 = LA(6);
												if (la6 == 'e') {
													la7 = LA(7);
													if (!IdOrKeyword_set0.Contains(la7)) {
														Skip();
														Skip();
														Skip();
														Skip();
														Skip();
														Skip();
														Skip();
														#line 343 "EcsLexerGrammar.les"
														_type = TT.PPdefine;
														#line 343 "EcsLexerGrammar.les"
														_value = S.PPDefine;
														#line default
													} else
														Id();
												} else
													Id();
											} else
												Id();
										} else
											Id();
									} else
										Id();
								} else
									Id();
							}
							break;
						case 'u':
							{
								la2 = LA(2);
								if (la2 == 'n') {
									la3 = LA(3);
									if (la3 == 'd') {
										la4 = LA(4);
										if (la4 == 'e') {
											la5 = LA(5);
											if (la5 == 'f') {
												la6 = LA(6);
												if (!IdOrKeyword_set0.Contains(la6)) {
													Skip();
													Skip();
													Skip();
													Skip();
													Skip();
													Skip();
													#line 344 "EcsLexerGrammar.les"
													_type = TT.PPundef;
													#line 344 "EcsLexerGrammar.les"
													_value = S.PPUndef;
													#line default
												} else
													Id();
											} else
												Id();
										} else
											Id();
									} else
										Id();
								} else
									Id();
							}
							break;
						case 'p':
							{
								la2 = LA(2);
								if (la2 == 'r') {
									la3 = LA(3);
									if (la3 == 'a') {
										la4 = LA(4);
										if (la4 == 'g') {
											la5 = LA(5);
											if (la5 == 'm') {
												la6 = LA(6);
												if (la6 == 'a') {
													la7 = LA(7);
													if (!IdOrKeyword_set0.Contains(la7)) {
														Skip();
														Skip();
														Skip();
														Skip();
														Skip();
														Skip();
														Skip();
														#line 345 "EcsLexerGrammar.les"
														_type = TT.PPpragma;
														#line 345 "EcsLexerGrammar.les"
														_value = S.PPPragma;
														#line default
													} else
														Id();
												} else
													Id();
											} else
												Id();
										} else
											Id();
									} else
										Id();
								} else
									Id();
							}
							break;
						case 'l':
							{
								la2 = LA(2);
								if (la2 == 'i') {
									la3 = LA(3);
									if (la3 == 'n') {
										la4 = LA(4);
										if (la4 == 'e') {
											la5 = LA(5);
											if (!IdOrKeyword_set0.Contains(la5)) {
												Skip();
												Skip();
												Skip();
												Skip();
												Skip();
												#line 346 "EcsLexerGrammar.les"
												_type = TT.PPline;
												#line 346 "EcsLexerGrammar.les"
												_value = S.PPLine;
												#line default
											} else
												Id();
										} else
											Id();
									} else
										Id();
								} else
									Id();
							}
							break;
						case 'w':
							{
								la2 = LA(2);
								if (la2 == 'a') {
									la3 = LA(3);
									if (la3 == 'r') {
										la4 = LA(4);
										if (la4 == 'n') {
											la5 = LA(5);
											if (la5 == 'i') {
												la6 = LA(6);
												if (la6 == 'n') {
													la7 = LA(7);
													if (la7 == 'g') {
														la8 = LA(8);
														if (!IdOrKeyword_set0.Contains(la8)) {
															Skip();
															Skip();
															Skip();
															Skip();
															Skip();
															Skip();
															Skip();
															Skip();
															#line 348 "EcsLexerGrammar.les"
															_type = TT.PPwarning;
															#line default
															_value = RestOfPPLine();
														} else
															Id();
													} else
														Id();
												} else
													Id();
											} else
												Id();
										} else
											Id();
									} else
										Id();
								} else
									Id();
							}
							break;
						case 'n':
							{
								la2 = LA(2);
								if (la2 == 'o') {
									la3 = LA(3);
									if (la3 == 't') {
										la4 = LA(4);
										if (la4 == 'e') {
											la5 = LA(5);
											if (!IdOrKeyword_set0.Contains(la5)) {
												Skip();
												Skip();
												Skip();
												Skip();
												Skip();
												#line 349 "EcsLexerGrammar.les"
												_type = TT.PPnote;
												#line default
												_value = RestOfPPLine();
											} else
												Id();
										} else
											Id();
									} else
										Id();
								} else
									Id();
							}
							break;
						case 'r':
							{
								la2 = LA(2);
								if (la2 == 'e') {
									la3 = LA(3);
									if (la3 == 'g') {
										la4 = LA(4);
										if (la4 == 'i') {
											la5 = LA(5);
											if (la5 == 'o') {
												la6 = LA(6);
												if (la6 == 'n') {
													la7 = LA(7);
													if (!IdOrKeyword_set0.Contains(la7)) {
														Skip();
														Skip();
														Skip();
														Skip();
														Skip();
														Skip();
														Skip();
														#line 350 "EcsLexerGrammar.les"
														_type = TT.PPregion;
														#line default
														_value = RestOfPPLine();
													} else
														Id();
												} else
													Id();
											} else
												Id();
										} else
											Id();
									} else
										Id();
								} else
									Id();
							}
							break;
						default:
							Id();
							break;
						}
					} else
						Id();
				}
				break;
			case 'm':
				{
					la1 = LA(1);
					if (la1 == 'o') {
						la2 = LA(2);
						if (la2 == 'd') {
							la3 = LA(3);
							if (la3 == 'u') {
								la4 = LA(4);
								if (la4 == 'l') {
									la5 = LA(5);
									if (la5 == 'e') {
										la6 = LA(6);
										if (!IdOrKeyword_set0.Contains(la6)) {
											Skip();
											Skip();
											Skip();
											Skip();
											Skip();
											Skip();
											#line 357 "EcsLexerGrammar.les"
											_type = TT.ContextualKeyword;
											#line 357 "EcsLexerGrammar.les"
											_value = _module;
											#line default
										} else
											Id();
									} else
										Id();
								} else
									Id();
							} else
								Id();
						} else
							Id();
					} else
						Id();
				}
				break;
			case 'j':
				{
					la1 = LA(1);
					if (la1 == 'o') {
						la2 = LA(2);
						if (la2 == 'i') {
							la3 = LA(3);
							if (la3 == 'n') {
								la4 = LA(4);
								if (!IdOrKeyword_set0.Contains(la4)) {
									Skip();
									Skip();
									Skip();
									Skip();
									#line 362 "EcsLexerGrammar.les"
									OtherContextualKeyword();
									#line default
								} else
									Id();
							} else
								Id();
						} else
							Id();
					} else
						Id();
				}
				break;
			default:
				Id();
				break;
			}
		}
	
	
		string RestOfPPLine()
		{
			int la0;
			#line 404 "EcsLexerGrammar.les"
			int start = InputPosition;
			#line default
			// Line 405: ([^\$\n\r])*
			for (;;) {
				la0 = LA0;
				if (!(la0 == -1 || la0 == '\n' || la0 == '\r'))
					Skip();
				else
					break;
			}
			#line 406 "EcsLexerGrammar.les"
			return CharSource.Slice(start, InputPosition - start).ToString();
			#line default
		}
	
		static readonly HashSet<int> Token_set0 = NewSetOfRanges('!', '!', '#', '\'', '*', '+', '-', ':', '<', '?', 'A', 'Z', '\\', '\\', '^', 'z', '|', '|', '~', '~', 128, 65532);
		static readonly HashSet<int> Token_set1 = NewSetOfRanges('!', '!', '#', '\'', '*', '+', '-', ':', '<', '?', '^', '^', '|', '|', '~', '~');
	
		// Token -----------------------------------------------------------------
	
		void Token()
		{
			int la0, la1, la2;
			// Line 418: ( Newline / Number / SLComment / MLComment / &{InputPosition == 0} Shebang / Id => IdOrKeyword / TQString / SQString / DQString / BQString / Symbol / At / Operator / UTF_BOM | Comma | Semicolon | [(] | [)] | [[] | [\]] | [{] | [}] )
			do {
				la0 = LA0;
				switch (la0) {
				case '\n': case '\r':
					{
						#line 418 "EcsLexerGrammar.les"
						_type = TT.Newline;
						#line default
						Newline();
					}
					break;
				case '0':
					goto matchNumber;
				case '.':
					{
						la1 = LA(1);
						if (la1 >= '0' && la1 <= '9')
							goto matchNumber;
						else
							Operator();
					}
					break;
				case '1': case '2': case '3': case '4':
				case '5': case '6': case '7': case '8':
				case '9':
					goto matchNumber;
				case '/':
					{
						la1 = LA(1);
						if (la1 == '/') {
							#line 422 "EcsLexerGrammar.les"
							_type = TT.SLComment;
							#line default
							SLComment();
						} else if (la1 == '*') {
							la2 = LA(2);
							if (la2 != -1) {
								#line 423 "EcsLexerGrammar.les"
								_type = TT.MLComment;
								#line default
								MLComment();
							} else
								Operator();
						} else
							Operator();
					}
					break;
				case '#':
					{
						if (InputPosition == 0) {
							la1 = LA(1);
							if (la1 == '!') {
								#line 425 "EcsLexerGrammar.les"
								_type = TT.Shebang;
								#line default
								Shebang();
							} else
								goto match6;
						} else
							goto match6;
					}
					break;
				case '\\':
					{
						la1 = LA(1);
						if (la1 == 'U' || la1 == 'u') {
							la2 = LA(2);
							if (HexDigit_set0.Contains(la2))
								goto match6;
							else
								Operator();
						} else
							Operator();
					}
					break;
				case '@':
					{
						la1 = LA(1);
						if (Id_set0.Contains(la1))
							goto match6;
						else if (la1 == '`') {
							la2 = LA(2);
							if (!(la2 == -1 || la2 == '\n' || la2 == '\r'))
								goto match6;
							else
								goto matchAt;
						} else if (Token_set1.Contains(la1))
							goto match6;
						else if (la1 == '"') {
							la2 = LA(2);
							if (la2 != -1)
								goto matchDQString;
							else
								goto matchAt;
						} else if (la1 == '@') {
							la2 = LA(2);
							if (Token_set0.Contains(la2)) {
								#line 431 "EcsLexerGrammar.les"
								_type = TT.Literal;
								#line default
								Symbol();
							} else
								goto matchAt;
						} else
							goto matchAt;
					}
					break;
				case '"':
					{
						la1 = LA(1);
						if (la1 == '"') {
							la2 = LA(2);
							if (la2 == '"')
								goto matchTQString;
							else
								goto matchDQString;
						} else if (!(la1 == -1 || la1 == '\n' || la1 == '\r'))
							goto matchDQString;
						else
							goto error;
					}
				case '\'':
					{
						la1 = LA(1);
						if (la1 == '\'') {
							la2 = LA(2);
							if (la2 == '\'')
								goto matchTQString;
							else
								goto matchSQString;
						} else if (!(la1 == -1 || la1 == '\n' || la1 == '\r'))
							goto matchSQString;
						else
							goto error;
					}
				case '`':
					{
						#line 430 "EcsLexerGrammar.les"
						_type = TT.BQString;
						#line default
						BQString();
					}
					break;
				case '!': case '$': case '%': case '&':
				case '*': case '+': case '-': case ':':
				case '<': case '=': case '>': case '?':
				case '^': case '|': case '~':
					Operator();
					break;
				case ',':
					{
						#line 435 "EcsLexerGrammar.les"
						_type = TT.Comma;
						#line default
						Comma();
					}
					break;
				case ';':
					{
						#line 436 "EcsLexerGrammar.les"
						_type = TT.Semicolon;
						#line default
						Semicolon();
					}
					break;
				case '(':
					{
						#line 437 "EcsLexerGrammar.les"
						_type = TT.LParen;
						#line default
						Skip();
					}
					break;
				case ')':
					{
						#line 438 "EcsLexerGrammar.les"
						_type = TT.RParen;
						#line default
						Skip();
					}
					break;
				case '[':
					{
						#line 439 "EcsLexerGrammar.les"
						_type = TT.LBrack;
						#line default
						Skip();
					}
					break;
				case ']':
					{
						#line 440 "EcsLexerGrammar.les"
						_type = TT.RBrack;
						#line default
						Skip();
					}
					break;
				case '{':
					{
						#line 441 "EcsLexerGrammar.les"
						_type = TT.LBrace;
						#line default
						Skip();
					}
					break;
				case '}':
					{
						#line 442 "EcsLexerGrammar.les"
						_type = TT.RBrace;
						#line default
						Skip();
					}
					break;
				default:
					if (Symbol_set0.Contains(la0))
						goto match6;
					else
						goto error;
				}
				break;
			matchNumber:
				{
					#line 421 "EcsLexerGrammar.les"
					_type = TT.Literal;
					#line default
					Number();
				}
				break;
			match6:
				{
					#line 426 "EcsLexerGrammar.les"
					_type = TT.Id;
					#line default
					IdOrKeyword();
				}
				break;
			matchTQString:
				{
					#line 427 "EcsLexerGrammar.les"
					_type = TT.Literal;
					#line default
					TQString();
				}
				break;
			matchSQString:
				{
					#line 428 "EcsLexerGrammar.les"
					_type = TT.Literal;
					#line default
					SQString();
				}
				break;
			matchDQString:
				{
					#line 429 "EcsLexerGrammar.les"
					_type = TT.Literal;
					#line default
					DQString();
				}
				break;
			matchAt:
				{
					#line 432 "EcsLexerGrammar.les"
					_type = TT.At;
					#line default
					At();
				}
				break;
			error:
				{
					#line 443 "EcsLexerGrammar.les"
					_type = TT.Unknown;
					#line 443 "EcsLexerGrammar.les"
					Error(0, "Unrecognized token");
					#line default
					MatchExcept();
				}
			} while (false);
		}
		static readonly HashSet<int> HexNumber_Test0_set0 = NewSetOfRanges('+', '+', '-', '-', '0', '9');
	
		private bool Try_HexNumber_Test0(int lookaheadAmt) {
			using (new SavePosition(this, lookaheadAmt))
				return HexNumber_Test0();
		}
		private bool HexNumber_Test0()
		{
			int la0;
			// Line 85: ([0-9] / HexDigits [Pp] [+\-0-9])
			la0 = LA0;
			if (la0 >= '0' && la0 <= '9'){
				if (!TryMatchRange('0', '9'))
					return false;}
			else {
				if (!Scan_HexDigits())
					return false;
				if (!TryMatch('P', 'p'))
					return false;
				if (!TryMatch(HexNumber_Test0_set0))
					return false;
			}
			return true;
		}
	}
}
