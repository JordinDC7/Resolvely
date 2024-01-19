using Sabio.Data.Providers;
using Sabio.Models;
using Sabio.Models.Domain.Podcasts;
using Sabio.Models.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Services.Interfaces
{
    public interface IPodcastService
    {
        int AddPodcast(PodcastAddRequest model, int userId);

        Paged<Podcasts> SelectAllPagination(int pageIndex, int pageSize);

        Paged<Podcasts> SelectByCreatedBy(int pageIndex, int pageSize, int createdByIdQuery);
        void PodcastsDelete(int id);
       Paged<Podcasts> SearchPagination(int pageIndex, int pageSize, string query);
         void Update(PodcastUpdateRequest model, int userId);

        

    }
}

