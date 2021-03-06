// Generated from UseSequenceExpressions.ecs by LeMP custom tool. LeMP version: 1.9.0.0
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
using System.Diagnostics;
using Loyc;
using Loyc.Syntax;
using Loyc.Collections;
using Loyc.Ecs;
using S = Loyc.Syntax.CodeSymbols;
namespace LeMP
{
	partial class StandardMacros
	{
		static readonly Symbol __numrunSequence = (Symbol) "#runSequence";
		static readonly Symbol _useSequenceExpressionsIsRunning = (Symbol) "#useSequenceExpressionsIsRunning";
		[LexicalMacro("#runSequence { Stmts; };", "Allows #runSequence at brace-scope without the use of #useSequenceExpressions", "#runSequence", Mode = MacroMode.Passive)]
		public static LNode runSequence(LNode node, IMacroContext context)
		{
			if (context.Parent.Calls(S.Braces))
				return node.With(S.Splice, MaybeRemoveNoOpFromRunSeq(node.Args));
			if (!context.ScopedProperties.ContainsKey(_useSequenceExpressionsIsRunning))
				Reject(context, node, "#useSequenceExpressions is required to make #runSequence work");
			return null;
		}
		public static VList<LNode> MaybeRemoveNoOpFromRunSeq(VList<LNode> runSeq)
		{
			if (runSeq.Count > 1 && runSeq.Last.IsId)
				return runSeq.WithoutLast(1);
			return runSeq;
		}
		[LexicalMacro("#useSequenceExpressions; ... if (Foo.Bar()::b.Baz != null) b.Baz.Method(); ...", "Enables the use of variable-declaration and #runSequence expressions, including the quick-binding operator `::` and the `with` expression, in the code that follows." + "Technically this allows any executable code in an expression context, such as while and for-loops, " + "but its name comes from the fact that it is usually used to allow variable declarations. " + "#useSequenceExpressions expects to be used in a declaration context, " + "e.g. at class or namespace level, not within a function.", "#useSequenceExpressions", Mode = MacroMode.NoReprocessing)]
		public static LNode useSequenceExpressions(LNode node, IMacroContext context)
		{
			var tmp_0 = context.GetArgsAndBody(true);
			var args = tmp_0.Item1;
			var body = tmp_0.Item2;
			if (args.Count > 0)
				context.Write(Severity.Error, node[1], "#useSequenceExpressions does not support arguments.");
			{
				context.ScopedProperties[_useSequenceExpressionsIsRunning] = G.BoxedTrue;
				try {
					;
					body = context.PreProcess(body);
				} finally {
					context.ScopedProperties.Remove(_useSequenceExpressionsIsRunning);
				}
			}
			var ers = new EliminateRunSequences(context);
			return ers.EliminateSequenceExpressions(body, true).AsLNode(S.Splice);
		}
		class EliminateRunSequences
		{
			static readonly LNode _trivia_pure = LNode.Id("#trivia_pure");
			static readonly LNode _trivia_isTmpVar = LNode.Id("#trivia_isTmpVar");
			public IMacroContext Context;
			public EliminateRunSequences(IMacroContext context)
			{
				Context = context;
			}
			LNode[] _arrayOf1 = new LNode[1];
			public VList<LNode> EliminateSequenceExpressions(VList<LNode> stmts, bool isDeclContext)
			{
				return stmts.SmartSelectMany(stmt => {
					LNode result = EliminateSequenceExpressions(stmt, isDeclContext);
					if (result != stmt) {
						VList<LNode> results;
						if (result.Calls(__numrunSequence)) {
							results = MaybeRemoveNoOpFromRunSeq(result.Args);
							return results;
						}
					}
					_arrayOf1[0] = result;
					return _arrayOf1;
				});
			}
			public LNode EliminateSequenceExpressions(LNode stmt, bool isDeclContext)
			{
				LNode retType, name, argList, bases, body, initValue;
				if (EcsValidators.SpaceDefinitionKind(stmt, out name, out bases, out body) != null) {
					return body == null ? stmt : stmt.WithArgChanged(2, EliminateSequenceExpressions(body, true));
				} else if (EcsValidators.MethodDefinitionKind(stmt, out retType, out name, out argList, out body, true) != null) {
					return body == null ? stmt : stmt.WithArgChanged(3, EliminateSequenceExpressionsInLambdaExpr(body, retType));
				} else if (EcsValidators.IsPropertyDefinition(stmt, out retType, out name, out argList, out body, out initValue)) {
					stmt = stmt.WithArgChanged(3, body.WithArgs(part => {
						if (part.ArgCount == 1 && part[0].Calls(S.Braces))
							part = part.WithArgChanged(0, EliminateSequenceExpressions(part[0], false));
						return part;
					}));
					if (initValue != null) {
						var initMethod = EliminateRunSeqFromInitializer(retType, name, ref initValue);
						if (initMethod != null) {
							stmt = stmt.WithArgChanged(4, initValue);
							return LNode.Call((Symbol) "#runSequence", LNode.List(stmt, initMethod));
						}
					}
					return stmt;
				} else if (stmt.Calls(CodeSymbols.Braces)) {
					return stmt.WithArgs(EliminateSequenceExpressions(stmt.Args, isDeclContext));
				} else if (!isDeclContext) {
					return EliminateSequenceExpressionsInExecStmt(stmt);
				} else if (stmt.CallsMin(S.Var, 2)) {
					var results = new List<LNode> { 
						stmt
					};
					var vars = stmt.Args;
					var varType = vars[0];
					for (int i = 1; i < vars.Count; i++) {
						var @var = vars[i];
						if (@var.Calls(CodeSymbols.Assign, 2) && (name = @var.Args[0]) != null && (initValue = @var.Args[1]) != null) {
							var initMethod = EliminateRunSeqFromInitializer(varType, name, ref initValue);
							if (initMethod != null) {
								results.Add(initMethod);
								vars[i] = vars[i].WithArgChanged(1, initValue);
							}
						}
					}
					if (results.Count > 1) {
						results[0] = stmt.WithArgs(vars);
						return LNode.List(results).AsLNode(__numrunSequence);
					}
					return stmt;
				} else
					return stmt;
			}
			LNode EliminateSequenceExpressionsInLambdaExpr(LNode expr, LNode retType)
			{
				var stmt = EliminateSequenceExpressions(expr, false);
				if (stmt.Calls(__numrunSequence)) {
					stmt = stmt.WithTarget(S.Braces);
					if (!retType.IsIdNamed(S.Void)) {
						if (retType.IsIdNamed(S.Missing) && stmt.Args.Last.IsCall)
							Context.Write(Severity.Warning, expr, "This lambda must be converted to a braced block, but in LeMP it's not possible to tell whether the return keyword is needed. The output assumes `return` is required.");
						stmt = stmt.WithArgChanged(stmt.Args.Count - 1, LNode.Call(CodeSymbols.Return, LNode.List(stmt.Args.Last)));
					}
				}
				return stmt;
			}
			LNode EliminateSequenceExpressionsInExecStmt(LNode stmt)
			{
				{
					LNode block, collection, cond, init, initValue, loopVar, name, tmp_1, tmp_2, type;
					VList<LNode> attrs, incs, inits;
					if (stmt.Calls(CodeSymbols.Braces))
						return stmt.WithArgs(EliminateSequenceExpressions(stmt.Args, false));
					else if (stmt.CallsMin(CodeSymbols.If, 1) || stmt.Calls(CodeSymbols.UsingStmt, 2) || stmt.Calls(CodeSymbols.Lock, 2) || stmt.Calls(CodeSymbols.Switch, 2))
						return ProcessBlockCallStmt(stmt, 1);
					else if ((attrs = stmt.Attrs).IsEmpty | true && stmt.Calls(CodeSymbols.Fixed, 2) && (init = stmt.Args[0]) != null && (block = stmt.Args[1]) != null) {
						init = EliminateSequenceExpressionsInExecStmt(init);
						block = EliminateSequenceExpressionsInChildStmt(block);
						if (init.CallsMin(__numrunSequence, 1)) {
							return LNode.Call(LNode.List(attrs), CodeSymbols.Braces, LNode.List().AddRange(init.Args.WithoutLast(1)).Add(LNode.Call(CodeSymbols.Fixed, LNode.List(init.Args.Last, block)))).SetStyle(NodeStyle.Statement);
						} else
							return stmt.WithArgChanged(1, block);
					} else if ((attrs = stmt.Attrs).IsEmpty | true && stmt.Calls(CodeSymbols.While, 2) && (cond = stmt.Args[0]) != null && (block = stmt.Args[1]) != null) {
						cond = BubbleUpBlocks(cond);
						block = EliminateSequenceExpressionsInChildStmt(block);
						if (cond.CallsMin(__numrunSequence, 1)) {
							return LNode.Call(LNode.List(attrs), CodeSymbols.For, LNode.List(LNode.Call(CodeSymbols.AltList), LNode.Missing, LNode.Call(CodeSymbols.AltList), LNode.Call(CodeSymbols.Braces, LNode.List().AddRange(cond.Args.WithoutLast(1)).Add(LNode.Call(CodeSymbols.If, LNode.List(cond.Args.Last, block, LNode.Call(CodeSymbols.Break))))).SetStyle(NodeStyle.Statement)));
						} else
							return stmt.WithArgChanged(1, block);
					} else if ((attrs = stmt.Attrs).IsEmpty | true && stmt.Calls(CodeSymbols.DoWhile, 2) && (block = stmt.Args[0]) != null && (cond = stmt.Args[1]) != null) {
						block = EliminateSequenceExpressionsInChildStmt(block);
						cond = BubbleUpBlocks(cond);
						if (cond.CallsMin(__numrunSequence, 1)) {
							var continue_N = F.Id(NextTempName(Context, "continue_"));
							var bodyStmts = block.AsList(S.Braces);
							bodyStmts.AddRange(cond.Args.WithoutLast(1));
							bodyStmts.Add(LNode.Call(CodeSymbols.Assign, LNode.List(continue_N, cond.Args.Last)).SetStyle(NodeStyle.Operator));
							return LNode.Call(LNode.List(attrs), CodeSymbols.For, LNode.List(LNode.Call(CodeSymbols.AltList, LNode.List(LNode.Call(CodeSymbols.Var, LNode.List(LNode.Id(CodeSymbols.Bool), LNode.Call(CodeSymbols.Assign, LNode.List(continue_N, LNode.Literal(true))))))), continue_N, LNode.Call(CodeSymbols.AltList), LNode.Call(CodeSymbols.Braces, LNode.List(bodyStmts)).SetStyle(NodeStyle.Statement)));
						} else
							return stmt.WithArgChanged(0, block);
					} else if ((attrs = stmt.Attrs).IsEmpty | true && stmt.Calls(CodeSymbols.For, 4) && stmt.Args[0].Calls(CodeSymbols.AltList) && (cond = stmt.Args[1]) != null && stmt.Args[2].Calls(CodeSymbols.AltList) && (block = stmt.Args[3]) != null) {
						inits = stmt.Args[0].Args;
						incs = stmt.Args[2].Args;
						return ESEInForLoop(stmt, attrs, inits, cond, incs, block);
					} else if ((attrs = stmt.Attrs).IsEmpty | true && stmt.Calls(CodeSymbols.ForEach, 3) && (tmp_1 = stmt.Args[0]) != null && tmp_1.Calls(CodeSymbols.Var, 2) && (type = tmp_1.Args[0]) != null && (loopVar = tmp_1.Args[1]) != null && (collection = stmt.Args[1]) != null && (block = stmt.Args[2]) != null) {
						block = EliminateSequenceExpressionsInChildStmt(block);
						collection = BubbleUpBlocks(collection);
						if (collection.CallsMin(__numrunSequence, 1)) {
							return LNode.Call(LNode.List(attrs), CodeSymbols.Braces, LNode.List().AddRange(collection.Args.WithoutLast(1)).Add(LNode.Call(CodeSymbols.ForEach, LNode.List(LNode.Call(CodeSymbols.Var, LNode.List(type, loopVar)), collection.Args.Last, block)))).SetStyle(NodeStyle.Statement);
						} else {
							return stmt.WithArgChanged(stmt.Args.Count - 1, block);
						}
					} else if ((attrs = stmt.Attrs).IsEmpty | true && stmt.Calls(CodeSymbols.Var, 2) && (type = stmt.Args[0]) != null && (tmp_2 = stmt.Args[1]) != null && tmp_2.Calls(CodeSymbols.Assign, 2) && (name = tmp_2.Args[0]) != null && (initValue = tmp_2.Args[1]) != null) {
						var initValue_apos = BubbleUpBlocks(initValue);
						if (initValue_apos != initValue) {
							{
								LNode last;
								VList<LNode> stmts;
								if (initValue_apos.CallsMin((Symbol) "#runSequence", 1) && (last = initValue_apos.Args[initValue_apos.Args.Count - 1]) != null) {
									stmts = initValue_apos.Args.WithoutLast(1);
									return LNode.Call((Symbol) "#runSequence", LNode.List().AddRange(stmts).Add(LNode.Call(LNode.List(attrs), CodeSymbols.Var, LNode.List(type, LNode.Call(CodeSymbols.Assign, LNode.List(name, last))))));
								} else
									return LNode.Call(LNode.List(attrs), CodeSymbols.Var, LNode.List(type, LNode.Call(CodeSymbols.Assign, LNode.List(name, initValue_apos))));
							}
						}
					} else if (stmt.CallsMin(S.Try, 2)) {
						return ESEInTryStmt(stmt);
					} else if (stmt.HasSpecialName && stmt.ArgCount >= 1 && stmt.Args.Last.Calls(S.Braces)) {
						return ProcessBlockCallStmt(stmt, stmt.ArgCount - 1);
					} else {
						return BubbleUpBlocks(stmt, stmtContext: true);
					}
				}
				return stmt;
			}
			LNode ESEInForLoop(LNode stmt, VList<LNode> attrs, VList<LNode> init, LNode cond, VList<LNode> inc, LNode block)
			{
				var preInit = VList<LNode>.Empty;
				var init_apos = init.SmartSelect(init1 => {
					init1 = EliminateSequenceExpressionsInExecStmt(init1);
					if (init1.CallsMin(__numrunSequence, 1)) {
						preInit.AddRange(init1.Args.WithoutLast(1));
						return init1.Args.Last;
					}
					return init1;
				});
				var cond_apos = BubbleUpBlocks(cond);
				var inc_apos = inc.SmartSelectMany(inc1 => {
					inc1 = BubbleUpBlocks(inc1);
					return inc1.AsList(__numrunSequence);
				});
				block = EliminateSequenceExpressionsInChildStmt(block);
				if (init_apos != init || cond_apos != cond || inc_apos != inc) {
					init = init_apos;
					if (inc_apos != inc) {
						var blockStmts = block.AsList(S.Braces).AddRange(inc_apos);
						block = blockStmts.AsLNode(S.Braces);
						inc = LNode.List();
					}
					if (cond_apos.CallsMin(__numrunSequence, 1)) {
						var preCond = cond_apos.Args.WithoutLast(1);
						cond = cond_apos.Args.Last;
						stmt = LNode.Call(CodeSymbols.For, LNode.List(LNode.Call(CodeSymbols.AltList, LNode.List(init)), LNode.Missing, LNode.Call(CodeSymbols.AltList, LNode.List(inc)), LNode.Call(CodeSymbols.Braces, LNode.List().AddRange(preCond).Add(LNode.Call(CodeSymbols.If, LNode.List(cond, block, LNode.Call(CodeSymbols.Break))))).SetStyle(NodeStyle.Statement)));
					} else {
						stmt = LNode.Call(LNode.List(attrs), CodeSymbols.For, LNode.List(LNode.Call(CodeSymbols.AltList, LNode.List(init)), cond, LNode.Call(CodeSymbols.AltList, LNode.List(inc)), block));
					}
					if (preInit.Count != 0) {
						stmt = LNode.Call(CodeSymbols.Braces, LNode.List().AddRange(preInit).Add(stmt)).SetStyle(NodeStyle.Statement);
					}
					return stmt;
				} else {
					return stmt.WithArgChanged(3, block);
				}
			}
			LNode ESEInTryStmt(LNode stmt)
			{
				var args = stmt.Args.ToWList();
				args[0] = EliminateSequenceExpressionsInChildStmt(args[0]);
				for (int i = 1; i < args.Count; i++) {
					var part = args[i];
					if (part.Calls(S.Finally, 1) || part.Calls(S.Catch, 3)) {
						int lasti = part.ArgCount - 1;
						args[i] = part.WithArgChanged(lasti, EliminateSequenceExpressionsInChildStmt(part.Args[lasti]));
					}
				}
				return stmt.WithArgs(args.ToVList());
			}
			LNode ProcessBlockCallStmt(LNode stmt, int childStmtsStartAt)
			{
				List<LNode> childStmts = stmt.Args.Slice(childStmtsStartAt).ToList();
				LNode partialStmt = stmt.WithArgs(stmt.Args.First(childStmtsStartAt));
				VList<LNode> advanceSequence;
				if (ProcessBlockCallStmt2(ref partialStmt, out advanceSequence, childStmts)) {
					stmt = partialStmt.PlusArgs(childStmts);
					if (advanceSequence.Count != 0)
						return LNode.Call(CodeSymbols.Braces, LNode.List().AddRange(advanceSequence).Add(stmt)).SetStyle(NodeStyle.Statement);
					return stmt;
				} else
					return stmt;
			}
			bool ProcessBlockCallStmt2(ref LNode partialStmt, out VList<LNode> advanceSequence, List<LNode> childStmts)
			{
				bool childChanged = false;
				for (int i = 0; i < childStmts.Count; i++) {
					var oldChild = childStmts[i];
					childStmts[i] = EliminateSequenceExpressionsInChildStmt(oldChild);
					childChanged |= (oldChild != childStmts[i]);
				}
				var BubbleUp_GeneralCall2_3 = BubbleUp_GeneralCall2(partialStmt);
				advanceSequence = BubbleUp_GeneralCall2_3.Item1;
				partialStmt = BubbleUp_GeneralCall2_3.Item2;
				return childChanged || !advanceSequence.IsEmpty;
			}
			LNode EliminateSequenceExpressionsInChildStmt(LNode stmt)
			{
				stmt = EliminateSequenceExpressionsInExecStmt(stmt);
				if (stmt.Calls(__numrunSequence))
					return stmt.With(S.Braces, MaybeRemoveNoOpFromRunSeq(stmt.Args));
				return stmt;
			}
			LNode EliminateRunSeqFromInitializer(LNode retType, LNode fieldName, ref LNode expr)
			{
				expr = BubbleUpBlocks(expr);
				if (expr.CallsMin(__numrunSequence, 1)) {
					var statements = expr.Args.WithoutLast(1);
					var finalResult = expr.Args.Last;
					LNode methodName = F.Id(KeyNameComponentOf(fieldName).Name + "_initializer");
					expr = LNode.Call(methodName);
					return LNode.Call(LNode.List(LNode.Id(CodeSymbols.Static)), CodeSymbols.Fn, LNode.List(retType, methodName, LNode.Call(CodeSymbols.AltList), LNode.Call(CodeSymbols.Braces, LNode.List().AddRange(statements).Add(LNode.Call(CodeSymbols.Return, LNode.List(finalResult)))).SetStyle(NodeStyle.Statement)));
				} else
					return null;
			}
			LNode BubbleUpBlocks(LNode expr, bool stmtContext = false)
			{
				if (!expr.IsCall)
					return expr;
				LNode result = null;
				if (!stmtContext) {
					{
						LNode tmp_4, value, varName, varType;
						VList<LNode> attrs;
						if (expr.Calls(CodeSymbols.Braces)) {
							Context.Write(Severity.Warning, expr, "A braced block is not supported directly within an expression. Did you mean to use `#runSequence {...}`?");
							result = expr;
						} else if ((attrs = expr.Attrs).IsEmpty | true && attrs.NodeNamed(S.Out) != null && expr.Calls(CodeSymbols.Var, 2) && (varType = expr.Args[0]) != null && (varName = expr.Args[1]) != null && varName.IsId) {
							if (varType.IsIdNamed(S.Missing))
								Context.Write(Severity.Error, expr, "#useSequenceExpressions: the data type of this variable declaration cannot be inferred and must be stated explicitly.");
							result = LNode.Call(LNode.List(_trivia_pure), (Symbol) "#runSequence", LNode.List(expr.WithoutAttrNamed(S.Out), varName.PlusAttrs(LNode.List(LNode.Id(CodeSymbols.Out)))));
						} else if ((attrs = expr.Attrs).IsEmpty | true && expr.Calls(CodeSymbols.Var, 2) && (varType = expr.Args[0]) != null && (tmp_4 = expr.Args[1]) != null && tmp_4.Calls(CodeSymbols.Assign, 2) && (varName = tmp_4.Args[0]) != null && (value = tmp_4.Args[1]) != null)
							if (stmtContext)
								result = expr;
							else
								result = ConvertVarDeclToRunSequence(attrs, varType, varName, value);
					}
				}
				if (result == null) {
					{
						LNode args, code, value, varName;
						VList<LNode> attrs;
						if ((attrs = expr.Attrs).IsEmpty | true && expr.Calls(CodeSymbols.ColonColon, 2) && (value = expr.Args[0]) != null && IsQuickBindLhs(value) && (varName = expr.Args[1]) != null && varName.IsId)
							result = ConvertVarDeclToRunSequence(attrs, F.Missing, varName, value);
						else if ((attrs = expr.Attrs).IsEmpty | true && expr.Calls(CodeSymbols.Lambda, 2) && (args = expr.Args[0]) != null && (code = expr.Args[1]) != null)
							result = expr.WithArgChanged(1, EliminateSequenceExpressionsInLambdaExpr(code, F.Missing));
						else if (expr.Calls(__numrunSequence))
							result = expr;
						else
							result = BubbleUp_GeneralCall(expr);
					}
				}
				if (result.Calls(__numrunSequence))
					return result.WithArgs(EliminateSequenceExpressions(result.Args, false));
				else
					return result;
			}
			LNode BubbleUp_GeneralCall(LNode expr)
			{
				var BubbleUp_GeneralCall2_5 = BubbleUp_GeneralCall2(expr);
				var combinedSequence = BubbleUp_GeneralCall2_5.Item1;
				expr = BubbleUp_GeneralCall2_5.Item2;
				if (combinedSequence.Count != 0)
					return LNode.Call((Symbol) "#runSequence", LNode.List().AddRange(combinedSequence).Add(expr));
				else
					return expr;
			}
			Pair<VList<LNode>,LNode> BubbleUp_GeneralCall2(LNode expr)
			{
				var target = expr.Target;
				var args = expr.Args;
				var combinedSequence = LNode.List();
				target = BubbleUpBlocks(target);
				if (target.CallsMin(__numrunSequence, 1)) {
					combinedSequence = target.Args.WithoutLast(1);
					expr = expr.WithTarget(target.Args.Last);
				}
				var isAssignment = EcsValidators.IsAssignmentOperator(expr.Name);
				if (isAssignment) {
					LNode lhs = BubbleUpBlocks(expr.Args[0]);
					LNode rhs = BubbleUpBlocks(expr.Args[1]);
					args = LNode.List(lhs, rhs);
				} else {
					args = args.SmartSelect(arg => BubbleUpBlocks(arg));
				}
				int lastRunSeq = args.LastIndexWhere(a => a.CallsMin(__numrunSequence, 1));
				if (lastRunSeq >= 0) {
					int lastRunSeqImpure = args.First(lastRunSeq + 1).LastIndexWhere(a => a.CallsMin(__numrunSequence, 1) && a.AttrNamed(_trivia_pure.Name) == null);
					if (lastRunSeq > 0 && (args.Count == 2 && (target.IsIdNamed(S.And) || target.IsIdNamed(S.Or)) || args.Count == 3 && target.IsIdNamed(S.QuestionMark))) {
						Context.Write(Severity.Error, target, "#useSequenceExpressions is not designed to support sequences or variable declarations on the right-hand side of the `&&`, `||` or `?` operators. The generated code will be incorrect.");
					}
					var argsW = args.ToList();
					for (int i = 0; i <= lastRunSeq; i++) {
						LNode arg = argsW[i];
						if (!arg.IsLiteral) {
							if (arg.CallsMin(__numrunSequence, 1)) {
								combinedSequence.AddRange(arg.Args.WithoutLast(1));
								argsW[i] = arg = arg.Args.Last;
							}
							if (i < lastRunSeqImpure) {
								if (i == 0 && (expr.CallsMin(S.IndexBracks, 1) || expr.CallsMin(S.NullIndexBracks, 1))) {
								} else {
									if (isAssignment || arg.Attrs.Any(a => a.IsIdNamed(S.Ref) || a.IsIdNamed(S.Out)))
										argsW[i] = MaybeCreateTemporaryForLValue(arg, ref combinedSequence);
									else {
										LNode tmpVarName, tmpVarDecl = TempVarDecl(Context, arg, out tmpVarName);
										combinedSequence.Add(tmpVarDecl);
										argsW[i] = tmpVarName.PlusAttr(_trivia_isTmpVar);
									}
								}
							}
						}
					}
					expr = expr.WithArgs(LNode.List(argsW));
				}
				return Pair.Create(combinedSequence, expr);
			}
			LNode MaybeCreateTemporaryForLValue(LNode expr, ref VList<LNode> stmtSequence)
			{
				{
					LNode _, lhs;
					if (expr.Calls(CodeSymbols.Dot, 2) && (lhs = expr.Args[0]) != null || expr.CallsMin(CodeSymbols.Of, 1) && (lhs = expr.Args[0]) != null)
						return expr.WithArgChanged(0, MaybeCreateTemporaryForLValue(lhs, ref stmtSequence));
					else if ((_ = expr) != null && !_.IsCall)
						return expr;
					else {
						var args = expr.Args.ToWList();
						int i = 0;
						if (expr.CallsMin(S.IndexBracks, 1) || expr.CallsMin(S.NullIndexBracks, 1)) {
							i = 1;
						}
						for (; i < args.Count; i++) {
							if (!args[i].IsLiteral && !args[i].Attrs.Contains(_trivia_isTmpVar)) {
								LNode tmpVarName;
								stmtSequence.Add(TempVarDecl(Context, args[i], out tmpVarName));
								args[i] = tmpVarName.PlusAttr(_trivia_isTmpVar);
							}
						}
						return expr.WithArgs(args.ToVList());
					}
				}
			}
			LNode ConvertVarDeclToRunSequence(VList<LNode> attrs, LNode varType, LNode varName, LNode initValue)
			{
				initValue = BubbleUpBlocks(initValue);
				varType = varType ?? F.Missing;
				LNode @ref;
				attrs = attrs.WithoutNodeNamed(S.Ref, out @ref);
				var varName_apos = varName.PlusAttr(_trivia_isTmpVar);
				if (@ref != null)
					varName_apos = varName_apos.PlusAttr(@ref);
				{
					LNode resultValue;
					VList<LNode> stmts;
					if (initValue.CallsMin((Symbol) "#runSequence", 1) && (resultValue = initValue.Args[initValue.Args.Count - 1]) != null) {
						stmts = initValue.Args.WithoutLast(1);
						var newVarDecl = LNode.Call(LNode.List(attrs), CodeSymbols.Var, LNode.List(varType, LNode.Call(CodeSymbols.Assign, LNode.List(varName, resultValue))));
						return initValue.WithArgs(stmts.Add(newVarDecl).Add(varName_apos));
					} else {
						var newVarDecl = LNode.Call(LNode.List(attrs), CodeSymbols.Var, LNode.List(varType, LNode.Call(CodeSymbols.Assign, LNode.List(varName, initValue))));
						return LNode.Call((Symbol) "#runSequence", LNode.List(newVarDecl, varName_apos));
					}
				}
			}
			static bool IsQuickBindLhs(LNode value)
			{
				if (!value.IsId)
					return true;
				return char.IsUpper(value.Name.Name.TryGet(0, '\0'));
			}
		}
	}
}
