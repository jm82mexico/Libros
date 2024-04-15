using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Tienda.Application.Features.Streamers.Vms;

namespace Tienda.Application.Features.Streamers.Queries.GetStreamerListByUrl
{
    public class GetStreamerListByUrlQuery : IRequest<List<StreamersVm>>
    {
        public string? Url { get; set; }

        public GetStreamerListByUrlQuery(string url)
        {
            Url = url ?? throw new ArgumentNullException(nameof(url));
        }
    }
}