
using System.Threading.Tasks;
using Refit;

namespace Project.Api
{
    [Headers("User-Agent: :request:")]
    interface INewsApi
    {
        [Get("/news")]
        Task<ApiNewsResponse> GetNews();
    }
}