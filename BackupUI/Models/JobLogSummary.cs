using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureServerBackup.Models
{
	public class JobLogSummary
	{
		public string JobName { get; set; } = string.Empty;
		public int TotalActivities { get; set; }
		public DateTime LastActivity { get; set; }
		public int SuccessCount { get; set; }
		public int WarningCount { get; set; }
		public int ErrorCount { get; set; }
		public int InfoCount { get; set; }
	}
}
