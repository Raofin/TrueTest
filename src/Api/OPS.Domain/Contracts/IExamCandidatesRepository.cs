using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OPS.Domain.Entities.Exam;

namespace OPS.Domain.Contracts
{
    public interface IExamCandidatesRepository : IRepository<ExamCandidate>
    {
        Task<List<ExamCandidate>> GetExamCandidateAsync(CancellationToken cancellationToken);
        Task<List<ExamCandidate>> GetExamCandidateByAccountAsync(Guid id, CancellationToken cancellationToken);
    }
}
