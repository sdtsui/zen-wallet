﻿using System;
using System.Diagnostics;
using System.Text;
using BlockChain.Data;

namespace Miner
{
	public static class MinerTrace
	{
		static TraceSource _Trace = new TraceSource("Miner");

		internal static void Information(string info)
		{
			_Trace.TraceEvent(TraceEventType.Information, 0, info);
		}

		internal static void Error(string msg, Exception ex)
		{
			_Trace.TraceEvent(TraceEventType.Error, 0, msg + " " + Infrastructure.Utils.ExceptionToString(ex));
		}

		internal static void Verbose(string str)
		{
			_Trace.TraceEvent(TraceEventType.Verbose, 0, str);
		}
	}
}