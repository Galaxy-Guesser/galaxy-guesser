using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GalaxyGuesserApi.Models;

namespace GalaxyGuesserApi.Services.Interfaces
{
     public interface IQuestionService
    {
        Task<QuestionResponse> GetNextQuestionForSessionAsync(int sessionId);
        Task<List<OptionResponse>> GetOptionsByQuestionIdAsync(int questionId);
        Task<AnswerResponse> GetCorrectAnswerAsync(int questionId);
        Task<int> GetQuestionCountForCategory(int categoryId);
    }
}