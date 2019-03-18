using System;

namespace TwitchToolkit
{
    public class ScheduledJob
    {
        public int MinutesTillExpire;
        public Func<object, bool> Job;
        public object Product;

        public ScheduledJob(int length, Func<object, bool> job, object product)
        {
            MinutesTillExpire = length;
            Job = job;
            Product = product;
        }

        public void RunJob()
        {
            Job(Product);
        }

        public void Decrement()
        {
            if (MinutesTillExpire > 0)
            {
                MinutesTillExpire--;
            }
        }
    }
}