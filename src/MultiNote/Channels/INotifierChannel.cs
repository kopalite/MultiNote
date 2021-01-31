using System.Threading.Tasks;

namespace MultiNote.Channels
{
    public interface INotifierChannel
    {
        Task InfoAsync(params string[] messages);

        Task AlertAsync(params string[] messages);
    }
}
