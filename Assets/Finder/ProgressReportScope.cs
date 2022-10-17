using System;
using System.Threading;
using UnityEditor;

namespace Finder
{
	public class ProgressReportScope : IDisposable
	{
		private int _totalSteps;
		private int _currentStep;
		private readonly int _id;

		public ProgressReportScope(string name)
		{
			_id = Progress.Start(name);
			Progress.Report(_id, 0);
		}

		public void SetTotalSteps(int value)
		{
			_totalSteps = value;
		}

		public void Increment()
		{
			Interlocked.Increment(ref _currentStep);
			Progress.Report(_id, _currentStep, _totalSteps);
		}

		public void Dispose()
		{
			Progress.Remove(_id);
		}
	}
}