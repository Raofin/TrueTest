using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OPS.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Auth");

            migrationBuilder.EnsureSchema(
                name: "User");

            migrationBuilder.EnsureSchema(
                name: "Core");

            migrationBuilder.EnsureSchema(
                name: "Enum");

            migrationBuilder.EnsureSchema(
                name: "Exam");

            migrationBuilder.EnsureSchema(
                name: "Submit");

            migrationBuilder.CreateTable(
                name: "Difficulties",
                schema: "Enum",
                columns: table => new
                {
                    DifficultyId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DifficultyName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Difficulties", x => x.DifficultyId);
                });

            migrationBuilder.CreateTable(
                name: "Examinations",
                schema: "Exam",
                columns: table => new
                {
                    ExaminationId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DescriptionMarkdown = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OpensAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    ClosesAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    Duration = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getutcdate())"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Examinations", x => x.ExaminationId);
                });

            migrationBuilder.CreateTable(
                name: "LogEvents",
                schema: "Core",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MessageTemplate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Level = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TimeStamp = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getutcdate())"),
                    Exception = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Properties = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogEvents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProgLanguages",
                schema: "Enum",
                columns: table => new
                {
                    ProgLanguageId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Language = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProgLanguages", x => x.ProgLanguageId);
                });

            migrationBuilder.CreateTable(
                name: "QuestionTypes",
                schema: "Enum",
                columns: table => new
                {
                    QuestionTypeId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionTypes", x => x.QuestionTypeId);
                });

            migrationBuilder.CreateTable(
                name: "RoleTypes",
                schema: "Enum",
                columns: table => new
                {
                    RoleTypeId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleTypes", x => x.RoleTypeId);
                });

            migrationBuilder.CreateTable(
                name: "SocialTypes",
                schema: "Enum",
                columns: table => new
                {
                    SocialTypeId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlatformName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SocialTypes", x => x.SocialTypeId);
                });

            migrationBuilder.CreateTable(
                name: "Questions",
                schema: "Exam",
                columns: table => new
                {
                    QuestionId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StatementMarkdown = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Score = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getutcdate())"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    ExaminationId = table.Column<long>(type: "bigint", nullable: false),
                    DifficultyId = table.Column<long>(type: "bigint", nullable: false),
                    QuestionTypeId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questions", x => x.QuestionId);
                    table.ForeignKey(
                        name: "FK_Questions_Difficulties_DifficultyId",
                        column: x => x.DifficultyId,
                        principalSchema: "Enum",
                        principalTable: "Difficulties",
                        principalColumn: "DifficultyId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Questions_Examinations_ExaminationId",
                        column: x => x.ExaminationId,
                        principalSchema: "Exam",
                        principalTable: "Examinations",
                        principalColumn: "ExaminationId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Questions_QuestionTypes_QuestionTypeId",
                        column: x => x.QuestionTypeId,
                        principalSchema: "Enum",
                        principalTable: "QuestionTypes",
                        principalColumn: "QuestionTypeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SocialLinks",
                schema: "User",
                columns: table => new
                {
                    SocialLinkId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Link = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    SocialTypeId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SocialLinks", x => x.SocialLinkId);
                    table.ForeignKey(
                        name: "FK_SocialLinks_SocialTypes_SocialTypeId",
                        column: x => x.SocialTypeId,
                        principalSchema: "Enum",
                        principalTable: "SocialTypes",
                        principalColumn: "SocialTypeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "McqOptions",
                schema: "Exam",
                columns: table => new
                {
                    McqOptionId = table.Column<long>(type: "bigint", nullable: false),
                    QuestionId = table.Column<long>(type: "bigint", nullable: false),
                    OptionMarkdown = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_McqOptions", x => x.McqOptionId);
                    table.ForeignKey(
                        name: "FK_McqOptions_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalSchema: "Exam",
                        principalTable: "Questions",
                        principalColumn: "QuestionId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TestCases",
                schema: "Exam",
                columns: table => new
                {
                    TestCaseId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Input = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Output = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QuestionId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestCases", x => x.TestCaseId);
                    table.ForeignKey(
                        name: "FK_TestCases_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalSchema: "Exam",
                        principalTable: "Questions",
                        principalColumn: "QuestionId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "McqAnswers",
                schema: "Exam",
                columns: table => new
                {
                    McqAnswerId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuestionId = table.Column<long>(type: "bigint", nullable: false),
                    McqOptionId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_McqAnswers", x => x.McqAnswerId);
                    table.ForeignKey(
                        name: "FK_McqAnswers_McqOptions_McqOptionId",
                        column: x => x.McqOptionId,
                        principalSchema: "Exam",
                        principalTable: "McqOptions",
                        principalColumn: "McqOptionId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_McqAnswers_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalSchema: "Exam",
                        principalTable: "Questions",
                        principalColumn: "QuestionId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AccountRoles",
                schema: "Auth",
                columns: table => new
                {
                    AccountId = table.Column<long>(type: "bigint", nullable: false),
                    RoleTypeId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountRoles", x => new { x.AccountId, x.RoleTypeId });
                    table.ForeignKey(
                        name: "FK_AccountRoles_RoleTypes_RoleTypeId",
                        column: x => x.RoleTypeId,
                        principalSchema: "Enum",
                        principalTable: "RoleTypes",
                        principalColumn: "RoleTypeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Accounts",
                schema: "Auth",
                columns: table => new
                {
                    AccountId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Salt = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    IsVerified = table.Column<bool>(type: "bit", nullable: false),
                    CloudFileId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getutcdate())"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.AccountId);
                });

            migrationBuilder.CreateTable(
                name: "AccountSocials",
                schema: "User",
                columns: table => new
                {
                    SocialLinkId = table.Column<long>(type: "bigint", nullable: false),
                    AccountId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountSocials", x => x.SocialLinkId);
                    table.ForeignKey(
                        name: "FK_AccountSocials_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalSchema: "Auth",
                        principalTable: "Accounts",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AccountSocials_SocialLinks_SocialLinkId",
                        column: x => x.SocialLinkId,
                        principalSchema: "User",
                        principalTable: "SocialLinks",
                        principalColumn: "SocialLinkId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CloudFiles",
                schema: "Core",
                columns: table => new
                {
                    CloudFileId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ContentType = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Size = table.Column<long>(type: "bigint", nullable: false),
                    Link = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getutcdate())"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    AccountId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CloudFiles", x => x.CloudFileId);
                    table.ForeignKey(
                        name: "FK_CloudFiles_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalSchema: "Auth",
                        principalTable: "Accounts",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ExamCandidates",
                schema: "Exam",
                columns: table => new
                {
                    ExamCandidateId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CandidateEmail = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    AccountId = table.Column<long>(type: "bigint", nullable: true),
                    ExaminationId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExamCandidates", x => x.ExamCandidateId);
                    table.ForeignKey(
                        name: "FK_ExamCandidates_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalSchema: "Auth",
                        principalTable: "Accounts",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExamCandidates_Examinations_ExaminationId",
                        column: x => x.ExaminationId,
                        principalSchema: "Exam",
                        principalTable: "Examinations",
                        principalColumn: "ExaminationId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "McqSubmissions",
                schema: "Submit",
                columns: table => new
                {
                    McqSubmissionId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getutcdate())"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    AccountId = table.Column<long>(type: "bigint", nullable: false),
                    QuestionId = table.Column<long>(type: "bigint", nullable: false),
                    McqOptionId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_McqSubmissions", x => x.McqSubmissionId);
                    table.ForeignKey(
                        name: "FK_McqSubmissions_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalSchema: "Auth",
                        principalTable: "Accounts",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_McqSubmissions_McqOptions_McqOptionId",
                        column: x => x.McqOptionId,
                        principalSchema: "Exam",
                        principalTable: "McqOptions",
                        principalColumn: "McqOptionId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_McqSubmissions_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalSchema: "Exam",
                        principalTable: "Questions",
                        principalColumn: "QuestionId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Otps",
                schema: "Auth",
                columns: table => new
                {
                    OtpId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(dateadd(minute,(5),getutcdate()))"),
                    Attempts = table.Column<int>(type: "int", nullable: false),
                    AccountId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Otps", x => x.OtpId);
                    table.ForeignKey(
                        name: "FK_Otps_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalSchema: "Auth",
                        principalTable: "Accounts",
                        principalColumn: "AccountId");
                });

            migrationBuilder.CreateTable(
                name: "ProblemSubmissions",
                schema: "Submit",
                columns: table => new
                {
                    ProblemSubmissionId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Attempts = table.Column<int>(type: "int", nullable: false),
                    Score = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getutcdate())"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    AccountId = table.Column<long>(type: "bigint", nullable: false),
                    QuestionId = table.Column<long>(type: "bigint", nullable: false),
                    ProgLanguagesId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProblemSubmissions", x => x.ProblemSubmissionId);
                    table.ForeignKey(
                        name: "FK_ProblemSubmissions_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalSchema: "Auth",
                        principalTable: "Accounts",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProblemSubmissions_ProgLanguages_ProgLanguagesId",
                        column: x => x.ProgLanguagesId,
                        principalSchema: "Enum",
                        principalTable: "ProgLanguages",
                        principalColumn: "ProgLanguageId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProblemSubmissions_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalSchema: "Exam",
                        principalTable: "Questions",
                        principalColumn: "QuestionId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Profiles",
                schema: "User",
                columns: table => new
                {
                    ProfileId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    BioMarkdown = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InstituteName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    AccountId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profiles", x => x.ProfileId);
                    table.ForeignKey(
                        name: "FK_Profiles_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalSchema: "Auth",
                        principalTable: "Accounts",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WrittenSubmissions",
                schema: "Submit",
                columns: table => new
                {
                    WrittenSubmissionId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Answer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Score = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getutcdate())"),
                    QuestionId = table.Column<long>(type: "bigint", nullable: false),
                    AccountId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WrittenSubmissions", x => x.WrittenSubmissionId);
                    table.ForeignKey(
                        name: "FK_WrittenSubmissions_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalSchema: "Auth",
                        principalTable: "Accounts",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WrittenSubmissions_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalSchema: "Exam",
                        principalTable: "Questions",
                        principalColumn: "QuestionId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FlaggedSubmissions",
                schema: "Exam",
                columns: table => new
                {
                    FlaggedSolutionId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReasonMarkdown = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getutcdate())"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    ProblemSubmissionId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlaggedSubmissions", x => x.FlaggedSolutionId);
                    table.ForeignKey(
                        name: "FK_FlaggedSubmissions_ProblemSubmissions_ProblemSubmissionId",
                        column: x => x.ProblemSubmissionId,
                        principalSchema: "Submit",
                        principalTable: "ProblemSubmissions",
                        principalColumn: "ProblemSubmissionId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccountRoles_RoleTypeId",
                schema: "Auth",
                table: "AccountRoles",
                column: "RoleTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_CloudFileId",
                schema: "Auth",
                table: "Accounts",
                column: "CloudFileId");

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_Email",
                schema: "Auth",
                table: "Accounts",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_Username",
                schema: "Auth",
                table: "Accounts",
                column: "Username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AccountSocials_AccountId",
                schema: "User",
                table: "AccountSocials",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_CloudFiles_AccountId",
                schema: "Core",
                table: "CloudFiles",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Difficulties_DifficultyName",
                schema: "Enum",
                table: "Difficulties",
                column: "DifficultyName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ExamCandidates_AccountId",
                schema: "Exam",
                table: "ExamCandidates",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_ExamCandidates_ExaminationId",
                schema: "Exam",
                table: "ExamCandidates",
                column: "ExaminationId");

            migrationBuilder.CreateIndex(
                name: "IX_FlaggedSubmissions_ProblemSubmissionId",
                schema: "Exam",
                table: "FlaggedSubmissions",
                column: "ProblemSubmissionId");

            migrationBuilder.CreateIndex(
                name: "IX_McqAnswers_McqOptionId",
                schema: "Exam",
                table: "McqAnswers",
                column: "McqOptionId");

            migrationBuilder.CreateIndex(
                name: "IX_McqAnswers_QuestionId",
                schema: "Exam",
                table: "McqAnswers",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_McqOptions_QuestionId",
                schema: "Exam",
                table: "McqOptions",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_McqSubmissions_AccountId",
                schema: "Submit",
                table: "McqSubmissions",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_McqSubmissions_McqOptionId",
                schema: "Submit",
                table: "McqSubmissions",
                column: "McqOptionId");

            migrationBuilder.CreateIndex(
                name: "IX_McqSubmissions_QuestionId",
                schema: "Submit",
                table: "McqSubmissions",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_Otps_AccountId",
                schema: "Auth",
                table: "Otps",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_ProblemSubmissions_AccountId",
                schema: "Submit",
                table: "ProblemSubmissions",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_ProblemSubmissions_ProgLanguagesId",
                schema: "Submit",
                table: "ProblemSubmissions",
                column: "ProgLanguagesId");

            migrationBuilder.CreateIndex(
                name: "IX_ProblemSubmissions_QuestionId",
                schema: "Submit",
                table: "ProblemSubmissions",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_Profiles_AccountId",
                schema: "User",
                table: "Profiles",
                column: "AccountId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProgLanguages_Language",
                schema: "Enum",
                table: "ProgLanguages",
                column: "Language",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Questions_DifficultyId",
                schema: "Exam",
                table: "Questions",
                column: "DifficultyId");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_ExaminationId",
                schema: "Exam",
                table: "Questions",
                column: "ExaminationId");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_QuestionTypeId",
                schema: "Exam",
                table: "Questions",
                column: "QuestionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionTypes_Type",
                schema: "Enum",
                table: "QuestionTypes",
                column: "Type",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RoleTypes_RoleName",
                schema: "Enum",
                table: "RoleTypes",
                column: "RoleName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SocialLinks_SocialTypeId",
                schema: "User",
                table: "SocialLinks",
                column: "SocialTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_SocialTypes_PlatformName",
                schema: "Enum",
                table: "SocialTypes",
                column: "PlatformName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TestCases_QuestionId",
                schema: "Exam",
                table: "TestCases",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_WrittenSubmissions_AccountId",
                schema: "Submit",
                table: "WrittenSubmissions",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_WrittenSubmissions_QuestionId",
                schema: "Submit",
                table: "WrittenSubmissions",
                column: "QuestionId");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountRoles_Accounts_AccountId",
                schema: "Auth",
                table: "AccountRoles",
                column: "AccountId",
                principalSchema: "Auth",
                principalTable: "Accounts",
                principalColumn: "AccountId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Accounts_CloudFiles_CloudFileId",
                schema: "Auth",
                table: "Accounts",
                column: "CloudFileId",
                principalSchema: "Core",
                principalTable: "CloudFiles",
                principalColumn: "CloudFileId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CloudFiles_Accounts_AccountId",
                schema: "Core",
                table: "CloudFiles");

            migrationBuilder.DropTable(
                name: "AccountRoles",
                schema: "Auth");

            migrationBuilder.DropTable(
                name: "AccountSocials",
                schema: "User");

            migrationBuilder.DropTable(
                name: "ExamCandidates",
                schema: "Exam");

            migrationBuilder.DropTable(
                name: "FlaggedSubmissions",
                schema: "Exam");

            migrationBuilder.DropTable(
                name: "LogEvents",
                schema: "Core");

            migrationBuilder.DropTable(
                name: "McqAnswers",
                schema: "Exam");

            migrationBuilder.DropTable(
                name: "McqSubmissions",
                schema: "Submit");

            migrationBuilder.DropTable(
                name: "Otps",
                schema: "Auth");

            migrationBuilder.DropTable(
                name: "Profiles",
                schema: "User");

            migrationBuilder.DropTable(
                name: "TestCases",
                schema: "Exam");

            migrationBuilder.DropTable(
                name: "WrittenSubmissions",
                schema: "Submit");

            migrationBuilder.DropTable(
                name: "RoleTypes",
                schema: "Enum");

            migrationBuilder.DropTable(
                name: "SocialLinks",
                schema: "User");

            migrationBuilder.DropTable(
                name: "ProblemSubmissions",
                schema: "Submit");

            migrationBuilder.DropTable(
                name: "McqOptions",
                schema: "Exam");

            migrationBuilder.DropTable(
                name: "SocialTypes",
                schema: "Enum");

            migrationBuilder.DropTable(
                name: "ProgLanguages",
                schema: "Enum");

            migrationBuilder.DropTable(
                name: "Questions",
                schema: "Exam");

            migrationBuilder.DropTable(
                name: "Difficulties",
                schema: "Enum");

            migrationBuilder.DropTable(
                name: "Examinations",
                schema: "Exam");

            migrationBuilder.DropTable(
                name: "QuestionTypes",
                schema: "Enum");

            migrationBuilder.DropTable(
                name: "Accounts",
                schema: "Auth");

            migrationBuilder.DropTable(
                name: "CloudFiles",
                schema: "Core");
        }
    }
}
