using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GalaxyGuesserApi.Models;

namespace GalaxyGuesserApi.Services.Interfaces
{
     public interface IQuestionService
    {
        Task<int> GetQuestionCountForCategory(int categoryId);
    }
}