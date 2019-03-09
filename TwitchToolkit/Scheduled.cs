using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TwitchToolkit
{   
    public class Scheduled
    {
        private List<ScheduledJob> jobs;

        public Scheduled()
        {
            jobs = new List<ScheduledJob>();
        }

        public void CheckAllJobs()
        {
            List<ScheduledJob> jobstodecrement = jobs.Where(k => k.MinutesTillExpire > 0).ToList();
            foreach(ScheduledJob job in jobstodecrement)
            {
                job.Decrement();
            }
            List<ScheduledJob> jobstorun = jobs.Where(k => k.MinutesTillExpire == 0).ToList();
            foreach(ScheduledJob job in jobstorun)
            {
                job.RunJob();
                jobs = jobs.Where(l => l != job).ToList();
            }
        }

        public void AddNewJob(ScheduledJob job)
        {
            this.jobs.Add(job);
        }
    }
}
