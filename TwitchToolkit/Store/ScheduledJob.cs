using System;

namespace TwitchToolkit
{
    public class ScheduledJob
    {
        public int MinutesTillExpire;
        public Func<Product, bool> Job;
        public Product Product;

        public ScheduledJob(int length, Func<Product, bool> job, Product product)
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