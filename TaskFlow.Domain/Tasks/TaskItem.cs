using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Domain.Common;

namespace TaskFlow.Domain.Tasks
{
    public class TaskItem : Entity
    {
        public string Title { get; private set; }
        public string? Description { get; private set; }
        public bool IsCompleted { get; private set; }
        public Guid UserId { get; private set; }
        public DateTime CreatedAt { get; private set; }

        private TaskItem() { }

        public TaskItem(string title, string? description, Guid userId)
        {
            if(string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Title cannot be empty.");

            Title = title;
            Id = Guid.NewGuid();
            Description = description;
            IsCompleted = false;
            UserId = userId;
            CreatedAt = DateTime.UtcNow;
        }

        public void UpdateDetails(string title, string? description)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Title cannot be empty.");
            Title = title;
            Description = description;
        }

        public void MarkAsCompleted()
        {
            IsCompleted = true;
        }

        public void MarkAsIncomplete()
        {
            IsCompleted = false;
        }
    }
}
