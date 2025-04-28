using OPS.Domain.Enums;

namespace OPS.Domain.Constants;

public static class Permissions
{
    public const string ManageAccounts = "ManageAccounts";
    public const string ViewExams = "ViewExams";
    public const string ManageExams = "ManageExams";
    public const string ManageQuestions = "ManageQuestions";

    public const string ViewSubmissions = "ViewSubmissions";
    public const string ReviewSubmission = "ReviewSubmission";

    public const string ManageOwnProfile = "ManageOwnProfile";
    public const string AccessOwnExams = "AccessOwnExams";
    public const string SubmitAnswers = "SubmitAnswers";
    public const string RunCode = "RunCode";

    public static readonly Dictionary<RoleType, string[]> ByRole = new()
    {
        [RoleType.Admin] =
        [
            ManageAccounts,
            ViewExams,
            ManageExams,
            ManageQuestions,
            ViewSubmissions,
            ReviewSubmission
        ],
        [RoleType.Moderator] =
        [
            ViewExams,
            ManageQuestions,
            ViewSubmissions,
            ReviewSubmission
        ],
        [RoleType.Candidate] =
        [
            AccessOwnExams,
            SubmitAnswers,
            RunCode,
            ManageOwnProfile
        ]
    };
}