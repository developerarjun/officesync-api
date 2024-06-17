using MediatR;

namespace OfficeSync.Application.Common.Interfaces
{
    public interface IEventDispatcherService
    {
        void QueueNotification(INotification notification);
        void ClearQueue();
        Task Dispatch(CancellationToken cancellationToken);
    }
}
