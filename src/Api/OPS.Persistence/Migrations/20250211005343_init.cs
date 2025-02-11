using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OPS.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Auth");

            migrationBuilder.EnsureSchema(
                name: "Core");

            migrationBuilder.EnsureSchema(
                name: "Enum");

            migrationBuilder.EnsureSchema(
                name: "Exam");

            migrationBuilder.EnsureSchema(
                name: "Submit");

            migrationBuilder.EnsureSchema(
                name: "User");

            migrationBuilder.CreateTable(
                name: "CloudFiles",
                schema: "Core",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ContentType = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Size = table.Column<long>(type: "bigint", nullable: false),
                    Link = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "DateTime", nullable: false, defaultValueSql: "GetUtcDate()"),
                    UpdatedAt = table.Column<DateTime>(type: "DateTime", nullable: false, defaultValueSql: "GetUtcDate()"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CloudFiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Difficulties",
                schema: "Enum",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DifficultyName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "DateTime", nullable: false, defaultValueSql: "GetUtcDate()"),
                    UpdatedAt = table.Column<DateTime>(type: "DateTime", nullable: false, defaultValueSql: "GetUtcDate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Difficulties", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Examinations",
                schema: "Exam",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DescriptionMarkdown = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DurationMinutes = table.Column<int>(type: "int", nullable: false),
                    OpensAt = table.Column<DateTime>(type: "DateTime", nullable: false),
                    ClosesAt = table.Column<DateTime>(type: "DateTime", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "DateTime", nullable: false, defaultValueSql: "GetUtcDate()"),
                    UpdatedAt = table.Column<DateTime>(type: "DateTime", nullable: false, defaultValueSql: "GetUtcDate()"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Examinations", x => x.Id);
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
                    TimeStamp = table.Column<DateTime>(type: "DateTime", nullable: false, defaultValueSql: "(GetUtcDate())"),
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
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Language = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "DateTime", nullable: false, defaultValueSql: "GetUtcDate()"),
                    UpdatedAt = table.Column<DateTime>(type: "DateTime", nullable: false, defaultValueSql: "GetUtcDate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProgLanguages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "QuestionTypes",
                schema: "Enum",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "DateTime", nullable: false, defaultValueSql: "GetUtcDate()"),
                    UpdatedAt = table.Column<DateTime>(type: "DateTime", nullable: false, defaultValueSql: "GetUtcDate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RoleTypes",
                schema: "Enum",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "DateTime", nullable: false, defaultValueSql: "GetUtcDate()"),
                    UpdatedAt = table.Column<DateTime>(type: "DateTime", nullable: false, defaultValueSql: "GetUtcDate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SocialTypes",
                schema: "Enum",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PlatformName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "DateTime", nullable: false, defaultValueSql: "GetUtcDate()"),
                    UpdatedAt = table.Column<DateTime>(type: "DateTime", nullable: false, defaultValueSql: "GetUtcDate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SocialTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Accounts",
                schema: "Auth",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Salt = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    IsVerified = table.Column<bool>(type: "bit", nullable: false),
                    CloudFileId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "DateTime", nullable: false, defaultValueSql: "GetUtcDate()"),
                    UpdatedAt = table.Column<DateTime>(type: "DateTime", nullable: false, defaultValueSql: "GetUtcDate()"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Accounts_CloudFiles_CloudFileId",
                        column: x => x.CloudFileId,
                        principalSchema: "Core",
                        principalTable: "CloudFiles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Questions",
                schema: "Exam",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StatementMarkdown = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Score = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    ExaminationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DifficultyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuestionTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "DateTime", nullable: false, defaultValueSql: "GetUtcDate()"),
                    UpdatedAt = table.Column<DateTime>(type: "DateTime", nullable: false, defaultValueSql: "GetUtcDate()"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Questions_Difficulties_DifficultyId",
                        column: x => x.DifficultyId,
                        principalSchema: "Enum",
                        principalTable: "Difficulties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Questions_Examinations_ExaminationId",
                        column: x => x.ExaminationId,
                        principalSchema: "Exam",
                        principalTable: "Examinations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Questions_QuestionTypes_QuestionTypeId",
                        column: x => x.QuestionTypeId,
                        principalSchema: "Enum",
                        principalTable: "QuestionTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SocialLinks",
                schema: "User",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Link = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "DateTime", nullable: false, defaultValueSql: "GetUtcDate()"),
                    UpdatedAt = table.Column<DateTime>(type: "DateTime", nullable: false, defaultValueSql: "GetUtcDate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SocialLinks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SocialLinks_SocialTypes_Id",
                        column: x => x.Id,
                        principalSchema: "Enum",
                        principalTable: "SocialTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AccountRoles",
                schema: "Auth",
                columns: table => new
                {
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountRoles", x => new { x.AccountId, x.RoleTypeId });
                    table.ForeignKey(
                        name: "FK_AccountRoles_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalSchema: "Auth",
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AccountRoles_RoleTypes_RoleTypeId",
                        column: x => x.RoleTypeId,
                        principalSchema: "Enum",
                        principalTable: "RoleTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ExamCandidates",
                schema: "Exam",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CandidateEmail = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ExaminationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "DateTime", nullable: false, defaultValueSql: "GetUtcDate()"),
                    UpdatedAt = table.Column<DateTime>(type: "DateTime", nullable: false, defaultValueSql: "GetUtcDate()"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExamCandidates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExamCandidates_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalSchema: "Auth",
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExamCandidates_Examinations_ExaminationId",
                        column: x => x.ExaminationId,
                        principalSchema: "Exam",
                        principalTable: "Examinations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Otps",
                schema: "Auth",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "DateTime", nullable: false, defaultValueSql: "(DateAdd(minute, (5), GetUtcDate()))"),
                    Attempts = table.Column<int>(type: "int", nullable: false),
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "DateTime", nullable: false, defaultValueSql: "GetUtcDate()"),
                    UpdatedAt = table.Column<DateTime>(type: "DateTime", nullable: false, defaultValueSql: "GetUtcDate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Otps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Otps_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalSchema: "Auth",
                        principalTable: "Accounts",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Profiles",
                schema: "User",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    BioMarkdown = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InstituteName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ImageFileId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "DateTime", nullable: false, defaultValueSql: "GetUtcDate()"),
                    UpdatedAt = table.Column<DateTime>(type: "DateTime", nullable: false, defaultValueSql: "GetUtcDate()"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Profiles_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalSchema: "Auth",
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Profiles_CloudFiles_ImageFileId",
                        column: x => x.ImageFileId,
                        principalSchema: "Core",
                        principalTable: "CloudFiles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "McqOptions",
                schema: "Exam",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OptionMarkdown = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QuestionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "DateTime", nullable: false, defaultValueSql: "GetUtcDate()"),
                    UpdatedAt = table.Column<DateTime>(type: "DateTime", nullable: false, defaultValueSql: "GetUtcDate()"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_McqOptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_McqOptions_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalSchema: "Exam",
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProblemSubmissions",
                schema: "Submit",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Attempts = table.Column<int>(type: "int", nullable: false),
                    Score = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuestionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProgLanguagesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "DateTime", nullable: false, defaultValueSql: "GetUtcDate()"),
                    UpdatedAt = table.Column<DateTime>(type: "DateTime", nullable: false, defaultValueSql: "GetUtcDate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProblemSubmissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProblemSubmissions_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalSchema: "Auth",
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProblemSubmissions_ProgLanguages_ProgLanguagesId",
                        column: x => x.ProgLanguagesId,
                        principalSchema: "Enum",
                        principalTable: "ProgLanguages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProblemSubmissions_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalSchema: "Exam",
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TestCases",
                schema: "Exam",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Input = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Output = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QuestionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "DateTime", nullable: false, defaultValueSql: "GetUtcDate()"),
                    UpdatedAt = table.Column<DateTime>(type: "DateTime", nullable: false, defaultValueSql: "GetUtcDate()"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestCases", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TestCases_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalSchema: "Exam",
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WrittenSubmissions",
                schema: "Submit",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Answer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Score = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    QuestionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "DateTime", nullable: false, defaultValueSql: "GetUtcDate()"),
                    UpdatedAt = table.Column<DateTime>(type: "DateTime", nullable: false, defaultValueSql: "GetUtcDate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WrittenSubmissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WrittenSubmissions_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalSchema: "Auth",
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WrittenSubmissions_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalSchema: "Exam",
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProfileSocials",
                schema: "User",
                columns: table => new
                {
                    ProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SocialLinkId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfileSocials", x => new { x.ProfileId, x.SocialLinkId });
                    table.ForeignKey(
                        name: "FK_ProfileSocials_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalSchema: "User",
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProfileSocials_SocialLinks_SocialLinkId",
                        column: x => x.SocialLinkId,
                        principalSchema: "User",
                        principalTable: "SocialLinks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "McqAnswers",
                schema: "Exam",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuestionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    McqOptionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_McqAnswers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_McqAnswers_McqOptions_McqOptionId",
                        column: x => x.McqOptionId,
                        principalSchema: "Exam",
                        principalTable: "McqOptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_McqAnswers_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalSchema: "Exam",
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "McqSubmissions",
                schema: "Submit",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuestionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    McqOptionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "DateTime", nullable: false, defaultValueSql: "GetUtcDate()"),
                    UpdatedAt = table.Column<DateTime>(type: "DateTime", nullable: false, defaultValueSql: "GetUtcDate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_McqSubmissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_McqSubmissions_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalSchema: "Auth",
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_McqSubmissions_McqOptions_McqOptionId",
                        column: x => x.McqOptionId,
                        principalSchema: "Exam",
                        principalTable: "McqOptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_McqSubmissions_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalSchema: "Exam",
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FlaggedSubmissions",
                schema: "Submit",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReasonMarkdown = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProblemSubmissionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "DateTime", nullable: false, defaultValueSql: "GetUtcDate()"),
                    UpdatedAt = table.Column<DateTime>(type: "DateTime", nullable: false, defaultValueSql: "GetUtcDate()"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlaggedSubmissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FlaggedSubmissions_ProblemSubmissions_ProblemSubmissionId",
                        column: x => x.ProblemSubmissionId,
                        principalSchema: "Submit",
                        principalTable: "ProblemSubmissions",
                        principalColumn: "Id",
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
                name: "IX_Accounts_IsActive",
                schema: "Auth",
                table: "Accounts",
                column: "IsActive",
                filter: "[IsActive] = 1");

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_IsDeleted",
                schema: "Auth",
                table: "Accounts",
                column: "IsDeleted",
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_Username",
                schema: "Auth",
                table: "Accounts",
                column: "Username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CloudFiles_IsActive",
                schema: "Core",
                table: "CloudFiles",
                column: "IsActive",
                filter: "[IsActive] = 1");

            migrationBuilder.CreateIndex(
                name: "IX_CloudFiles_IsDeleted",
                schema: "Core",
                table: "CloudFiles",
                column: "IsDeleted",
                filter: "[IsDeleted] = 0");

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
                name: "IX_ExamCandidates_IsActive",
                schema: "Exam",
                table: "ExamCandidates",
                column: "IsActive",
                filter: "[IsActive] = 1");

            migrationBuilder.CreateIndex(
                name: "IX_ExamCandidates_IsDeleted",
                schema: "Exam",
                table: "ExamCandidates",
                column: "IsDeleted",
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Examinations_IsActive",
                schema: "Exam",
                table: "Examinations",
                column: "IsActive",
                filter: "[IsActive] = 1");

            migrationBuilder.CreateIndex(
                name: "IX_Examinations_IsDeleted",
                schema: "Exam",
                table: "Examinations",
                column: "IsDeleted",
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_FlaggedSubmissions_IsActive",
                schema: "Submit",
                table: "FlaggedSubmissions",
                column: "IsActive",
                filter: "[IsActive] = 1");

            migrationBuilder.CreateIndex(
                name: "IX_FlaggedSubmissions_IsDeleted",
                schema: "Submit",
                table: "FlaggedSubmissions",
                column: "IsDeleted",
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_FlaggedSubmissions_ProblemSubmissionId",
                schema: "Submit",
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
                name: "IX_Profiles_ImageFileId",
                schema: "User",
                table: "Profiles",
                column: "ImageFileId",
                unique: true,
                filter: "[ImageFileId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Profiles_IsActive",
                schema: "User",
                table: "Profiles",
                column: "IsActive",
                filter: "[IsActive] = 1");

            migrationBuilder.CreateIndex(
                name: "IX_Profiles_IsDeleted",
                schema: "User",
                table: "Profiles",
                column: "IsDeleted",
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_ProfileSocials_SocialLinkId",
                schema: "User",
                table: "ProfileSocials",
                column: "SocialLinkId");

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
                name: "IX_Questions_IsActive",
                schema: "Exam",
                table: "Questions",
                column: "IsActive",
                filter: "[IsActive] = 1");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_IsDeleted",
                schema: "Exam",
                table: "Questions",
                column: "IsDeleted",
                filter: "[IsDeleted] = 0");

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
                name: "IX_SocialTypes_PlatformName",
                schema: "Enum",
                table: "SocialTypes",
                column: "PlatformName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TestCases_IsActive",
                schema: "Exam",
                table: "TestCases",
                column: "IsActive",
                filter: "[IsActive] = 1");

            migrationBuilder.CreateIndex(
                name: "IX_TestCases_IsDeleted",
                schema: "Exam",
                table: "TestCases",
                column: "IsDeleted",
                filter: "[IsDeleted] = 0");

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountRoles",
                schema: "Auth");

            migrationBuilder.DropTable(
                name: "ExamCandidates",
                schema: "Exam");

            migrationBuilder.DropTable(
                name: "FlaggedSubmissions",
                schema: "Submit");

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
                name: "ProfileSocials",
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
                name: "ProblemSubmissions",
                schema: "Submit");

            migrationBuilder.DropTable(
                name: "McqOptions",
                schema: "Exam");

            migrationBuilder.DropTable(
                name: "Profiles",
                schema: "User");

            migrationBuilder.DropTable(
                name: "SocialLinks",
                schema: "User");

            migrationBuilder.DropTable(
                name: "ProgLanguages",
                schema: "Enum");

            migrationBuilder.DropTable(
                name: "Questions",
                schema: "Exam");

            migrationBuilder.DropTable(
                name: "Accounts",
                schema: "Auth");

            migrationBuilder.DropTable(
                name: "SocialTypes",
                schema: "Enum");

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
                name: "CloudFiles",
                schema: "Core");
        }
    }
}
