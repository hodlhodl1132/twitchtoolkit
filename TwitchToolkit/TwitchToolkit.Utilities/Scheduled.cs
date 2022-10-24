using System.Collections.Generic;
using System.Linq;

namespace TwitchToolkit.Utilities;

public class Scheduled
{
	private List<ScheduledJob> jobs;

	public Scheduled()
	{
		jobs = new List<ScheduledJob>();
	}

	public void CheckAllJobs()
	{
		List<ScheduledJob> jobstodecrement = jobs.Where((ScheduledJob k) => k.MinutesTillExpire > 0).ToList();
		foreach (ScheduledJob job in jobstodecrement)
		{
			job.Decrement();
		}
		List<ScheduledJob> jobstorun = jobs.Where((ScheduledJob k) => k.MinutesTillExpire == 0).ToList();
		foreach (ScheduledJob job2 in jobstorun)
		{
			job2.RunJob();
			jobs = jobs.Where((ScheduledJob l) => l != job2).ToList();
		}
	}

	public void AddNewJob(ScheduledJob job)
	{
		jobs.Add(job);
	}
}
