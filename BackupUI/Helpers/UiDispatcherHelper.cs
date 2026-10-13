using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SecureServerBackup.Helpers
{
	public static class UiDispatcherHelper
	{
		private static SynchronizationContext? _context;

		/// <summary>Call once, on the UI thread, after the first form/control has been created.</summary>
		public static void Initialize()
		{
			_context = SynchronizationContext.Current;
		}

		/// <summary>Runs the action on the UI thread and waits for it (same as Dispatcher.Invoke).</summary>
		public static void Invoke(Action action)
		{
			if (_context == null || SynchronizationContext.Current == _context)
			{
				action();                       // already on the UI thread (or not initialised)
				return;
			}

			_context.Send(_ => action(), null);
		}

		/// <summary>Runs the action on the UI thread without waiting (same as Dispatcher.BeginInvoke).</summary>
		public static void BeginInvoke(Action action)
		{
			if (_context == null)
			{
				action();
				return;
			}

			_context.Post(_ => action(), null);
		}
	}
}
