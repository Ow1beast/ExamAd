namespace Exam
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class ExamContext : DbContext
    { 
        public ExamContext() : base("name=ExamContext") { }

        public DbSet<User> Users { get; set; }
    }
}