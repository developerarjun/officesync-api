using MediatR;
using OfficeSync.Domain.Interfaces;

namespace OfficeSync.Application.Common.Events
{
    public class UpdatedEvent : INotification
    {
        public UpdatedEvent(IUpdatedEvent entity)
        {
            Entity = entity;
        }

        private object Entity { get; }

        public T GetEntity<T>() where T : class
        {
            if (Entity is T entity)
            {
                return entity;
            }
            return null;
        }
    }
}
