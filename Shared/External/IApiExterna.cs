using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.External
{
    public interface IApiExterna
    {
        Task<string> GetApiDataAsync(string url);
    }
}
