using System;

namespace BuildMonitor.Domain
{
    public interface IProject
    {
        Guid Id { get; }
        string Name { get; }
    }

    public class Project : IProject
    {
        public string Name { get; set; }
        public Guid Id { get; set; }
    }
}