
using MediatR;
using Tienda.Application.Features.Streamers.Vms;

namespace Tienda.Application.Features.Streamers.Queries.GetStreamerListByUsername
{
    public class GetStreamerListQuery : IRequest<List<StreamersVm>>
    {
        public string?  Username { get; set; }

        public GetStreamerListQuery(string? username)
        {
            Username = username ?? throw new ArgumentNullException(nameof(username));
        }
    }
}