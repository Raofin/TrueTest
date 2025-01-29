using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OPS.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "exam");

            migrationBuilder.EnsureSchema(
                name: "enum");

            migrationBuilder.EnsureSchema(
                name: "usr");

            migrationBuilder.RenameTable(
                name: "UserDetails",
                schema: "core",
                newName: "UserDetails",
                newSchema: "usr");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                schema: "auth",
                table: "Users",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "OtpEmail",
                schema: "auth",
                table: "Users",
                type: "nvarchar(255)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CloudFiles",
                schema: "core",
                columns: table => new
                {
                    CloudFileId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ContentType = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Size = table.Column<long>(type: "bigint", nullable: false),
                    Link = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CloudFiles", x => x.CloudFileId);
                    table.ForeignKey(
                        name: "FK_CloudFiles_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "auth",
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "Examinations",
                schema: "exam",
                columns: table => new
                {
                    ExamId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OpensAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    ClosesAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    Duration = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Examinations", x => x.ExamId);
                });

            migrationBuilder.CreateTable(
                name: "LogEvents",
                schema: "core",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MessageTemplate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Level = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TimeStamp = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Exception = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Properties = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogEvents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "McqOptions",
                schema: "exam",
                columns: table => new
                {
                    McqOptionId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Optionn = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_McqOptions", x => x.McqOptionId);
                });

            migrationBuilder.CreateTable(
                name: "Otps",
                schema: "auth",
                columns: table => new
                {
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(dateadd(minute,(5),getdate()))"),
                    Attempts = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Otps", x => x.Email);
                });

            migrationBuilder.CreateTable(
                name: "ProgLanguages",
                schema: "enum",
                columns: table => new
                {
                    ProgLanguagesId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Language = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProgLanguages", x => x.ProgLanguagesId);
                });

            migrationBuilder.CreateTable(
                name: "QuestionTypes",
                schema: "enum",
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
                name: "Roles",
                schema: "enum",
                columns: table => new
                {
                    RoleId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "SocialPlatforms",
                schema: "enum",
                columns: table => new
                {
                    SocialPlatformId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlatformName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SocialPlatforms", x => x.SocialPlatformId);
                });

            migrationBuilder.CreateTable(
                name: "ExamCandidates",
                schema: "exam",
                columns: table => new
                {
                    ExamCandidateId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserEmail = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    ExamId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExamCandidates", x => x.ExamCandidateId);
                    table.ForeignKey(
                        name: "FK_ExamCandidates_Examinations_ExamId",
                        column: x => x.ExamId,
                        principalSchema: "exam",
                        principalTable: "Examinations",
                        principalColumn: "ExamId");
                });

            migrationBuilder.CreateTable(
                name: "Questions",
                schema: "exam",
                columns: table => new
                {
                    QuestionId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Statement = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Score = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    QuestionTypeId = table.Column<long>(type: "bigint", nullable: false),
                    ExamId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questions", x => x.QuestionId);
                    table.ForeignKey(
                        name: "FK_Questions_Examinations_ExamId",
                        column: x => x.ExamId,
                        principalSchema: "exam",
                        principalTable: "Examinations",
                        principalColumn: "ExamId");
                    table.ForeignKey(
                        name: "FK_Questions_QuestionTypes_QuestionTypeId",
                        column: x => x.QuestionTypeId,
                        principalSchema: "enum",
                        principalTable: "QuestionTypes",
                        principalColumn: "QuestionTypeId");
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                schema: "auth",
                columns: table => new
                {
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    RoleId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__UserRole__AF2760AD02D51B95", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK__UserRoles__RoleI__5070F446",
                        column: x => x.RoleId,
                        principalSchema: "enum",
                        principalTable: "Roles",
                        principalColumn: "RoleId");
                    table.ForeignKey(
                        name: "FK__UserRoles__UserI__4F7CD00D",
                        column: x => x.UserId,
                        principalSchema: "auth",
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "Socials",
                schema: "usr",
                columns: table => new
                {
                    SocialId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SocialPlatformId = table.Column<long>(type: "bigint", nullable: false),
                    Link = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Socials", x => x.SocialId);
                    table.ForeignKey(
                        name: "FK_Socials_SocialPlatforms_SocialPlatformId",
                        column: x => x.SocialPlatformId,
                        principalSchema: "enum",
                        principalTable: "SocialPlatforms",
                        principalColumn: "SocialPlatformId");
                });

            migrationBuilder.CreateTable(
                name: "McqAnswers",
                schema: "exam",
                columns: table => new
                {
                    QuestionId = table.Column<long>(type: "bigint", nullable: false),
                    McqOptionId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_McqAnswers", x => new { x.QuestionId, x.McqOptionId });
                    table.ForeignKey(
                        name: "FK_McqAnswers_McqOptions_McqOptionId",
                        column: x => x.McqOptionId,
                        principalSchema: "exam",
                        principalTable: "McqOptions",
                        principalColumn: "McqOptionId");
                    table.ForeignKey(
                        name: "FK_McqAnswers_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalSchema: "exam",
                        principalTable: "Questions",
                        principalColumn: "QuestionId");
                });

            migrationBuilder.CreateTable(
                name: "Problems",
                schema: "exam",
                columns: table => new
                {
                    ProblemId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DifficultyLevel = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    QuestionId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Problems", x => x.ProblemId);
                    table.ForeignKey(
                        name: "FK_Problems_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalSchema: "exam",
                        principalTable: "Questions",
                        principalColumn: "QuestionId");
                });

            migrationBuilder.CreateTable(
                name: "UserMcqAnswers",
                schema: "usr",
                columns: table => new
                {
                    UserMcqAnswerId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    QuestionId = table.Column<long>(type: "bigint", nullable: false),
                    McqOptionId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserMcqAnswers", x => x.UserMcqAnswerId);
                    table.ForeignKey(
                        name: "FK_UserMcqAnswers_McqOptions_McqOptionId",
                        column: x => x.McqOptionId,
                        principalSchema: "exam",
                        principalTable: "McqOptions",
                        principalColumn: "McqOptionId");
                    table.ForeignKey(
                        name: "FK_UserMcqAnswers_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalSchema: "exam",
                        principalTable: "Questions",
                        principalColumn: "QuestionId");
                    table.ForeignKey(
                        name: "FK_UserMcqAnswers_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "auth",
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "UserWrittenAnswers",
                schema: "usr",
                columns: table => new
                {
                    UserWrittenAnswerId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Answer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    QuestionId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserWrittenAnswers", x => x.UserWrittenAnswerId);
                    table.ForeignKey(
                        name: "FK_UserWrittenAnswers_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalSchema: "exam",
                        principalTable: "Questions",
                        principalColumn: "QuestionId");
                    table.ForeignKey(
                        name: "FK_UserWrittenAnswers_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "auth",
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserSocials",
                schema: "usr",
                columns: table => new
                {
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    SocialId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__UserSoci__A1F43B5D0C5D0F96", x => new { x.UserId, x.SocialId });
                    table.ForeignKey(
                        name: "FK__UserSocia__Socia__49C3F6B7",
                        column: x => x.SocialId,
                        principalSchema: "usr",
                        principalTable: "Socials",
                        principalColumn: "SocialId");
                    table.ForeignKey(
                        name: "FK__UserSocia__UserI__48CFD27E",
                        column: x => x.UserId,
                        principalSchema: "auth",
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "TestCases",
                schema: "exam",
                columns: table => new
                {
                    TestCaseId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Input = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Output = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProblemId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestCases", x => x.TestCaseId);
                    table.ForeignKey(
                        name: "FK_TestCases_Problems_ProblemId",
                        column: x => x.ProblemId,
                        principalSchema: "exam",
                        principalTable: "Problems",
                        principalColumn: "ProblemId");
                });

            migrationBuilder.CreateTable(
                name: "UserSolutions",
                schema: "usr",
                columns: table => new
                {
                    UserSolutionId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Language = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Attempts = table.Column<int>(type: "int", nullable: false),
                    Score = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    ProblemId = table.Column<long>(type: "bigint", nullable: false),
                    ProgLanguagesId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSolutions", x => x.UserSolutionId);
                    table.ForeignKey(
                        name: "FK_UserSolutions_Problems_ProblemId",
                        column: x => x.ProblemId,
                        principalSchema: "exam",
                        principalTable: "Problems",
                        principalColumn: "ProblemId");
                    table.ForeignKey(
                        name: "FK_UserSolutions_ProgLanguages_ProgLanguagesId",
                        column: x => x.ProgLanguagesId,
                        principalSchema: "enum",
                        principalTable: "ProgLanguages",
                        principalColumn: "ProgLanguagesId");
                    table.ForeignKey(
                        name: "FK_UserSolutions_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "auth",
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "FlaggedSolution",
                schema: "exam",
                columns: table => new
                {
                    FlaggedSolutionId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Reason = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    UserSolutionId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlaggedSolution", x => x.FlaggedSolutionId);
                    table.ForeignKey(
                        name: "FK_FlaggedSolution_UserSolutions_UserSolutionId",
                        column: x => x.UserSolutionId,
                        principalSchema: "usr",
                        principalTable: "UserSolutions",
                        principalColumn: "UserSolutionId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_CloudFileId",
                schema: "auth",
                table: "Users",
                column: "CloudFileId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_OtpEmail",
                schema: "auth",
                table: "Users",
                column: "OtpEmail");

            migrationBuilder.CreateIndex(
                name: "IX_CloudFiles_UserId",
                schema: "core",
                table: "CloudFiles",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ExamCandidates_ExamId",
                schema: "exam",
                table: "ExamCandidates",
                column: "ExamId");

            migrationBuilder.CreateIndex(
                name: "IX_FlaggedSolution_UserSolutionId",
                schema: "exam",
                table: "FlaggedSolution",
                column: "UserSolutionId");

            migrationBuilder.CreateIndex(
                name: "IX_McqAnswers_McqOptionId",
                schema: "exam",
                table: "McqAnswers",
                column: "McqOptionId");

            migrationBuilder.CreateIndex(
                name: "IX_Problems_QuestionId",
                schema: "exam",
                table: "Problems",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_ProgLanguages_Language",
                schema: "enum",
                table: "ProgLanguages",
                column: "Language",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Questions_ExamId",
                schema: "exam",
                table: "Questions",
                column: "ExamId");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_QuestionTypeId",
                schema: "exam",
                table: "Questions",
                column: "QuestionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_RoleName",
                schema: "enum",
                table: "Roles",
                column: "RoleName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SocialPlatforms_PlatformName",
                schema: "enum",
                table: "SocialPlatforms",
                column: "PlatformName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Socials_SocialPlatformId",
                schema: "usr",
                table: "Socials",
                column: "SocialPlatformId");

            migrationBuilder.CreateIndex(
                name: "IX_TestCases_ProblemId",
                schema: "exam",
                table: "TestCases",
                column: "ProblemId");

            migrationBuilder.CreateIndex(
                name: "IX_UserMcqAnswers_McqOptionId",
                schema: "usr",
                table: "UserMcqAnswers",
                column: "McqOptionId");

            migrationBuilder.CreateIndex(
                name: "IX_UserMcqAnswers_QuestionId",
                schema: "usr",
                table: "UserMcqAnswers",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_UserMcqAnswers_UserId",
                schema: "usr",
                table: "UserMcqAnswers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                schema: "auth",
                table: "UserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSocials_SocialId",
                schema: "usr",
                table: "UserSocials",
                column: "SocialId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSolutions_ProblemId",
                schema: "usr",
                table: "UserSolutions",
                column: "ProblemId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSolutions_ProgLanguagesId",
                schema: "usr",
                table: "UserSolutions",
                column: "ProgLanguagesId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSolutions_UserId",
                schema: "usr",
                table: "UserSolutions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserWrittenAnswers_QuestionId",
                schema: "usr",
                table: "UserWrittenAnswers",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_UserWrittenAnswers_UserId",
                schema: "usr",
                table: "UserWrittenAnswers",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_CloudFiles_CloudFileId",
                schema: "auth",
                table: "Users",
                column: "CloudFileId",
                principalSchema: "core",
                principalTable: "CloudFiles",
                principalColumn: "CloudFileId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Otps_OtpEmail",
                schema: "auth",
                table: "Users",
                column: "OtpEmail",
                principalSchema: "auth",
                principalTable: "Otps",
                principalColumn: "Email");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_CloudFiles_CloudFileId",
                schema: "auth",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Otps_OtpEmail",
                schema: "auth",
                table: "Users");

            migrationBuilder.DropTable(
                name: "CloudFiles",
                schema: "core");

            migrationBuilder.DropTable(
                name: "ExamCandidates",
                schema: "exam");

            migrationBuilder.DropTable(
                name: "FlaggedSolution",
                schema: "exam");

            migrationBuilder.DropTable(
                name: "LogEvents",
                schema: "core");

            migrationBuilder.DropTable(
                name: "McqAnswers",
                schema: "exam");

            migrationBuilder.DropTable(
                name: "Otps",
                schema: "auth");

            migrationBuilder.DropTable(
                name: "TestCases",
                schema: "exam");

            migrationBuilder.DropTable(
                name: "UserMcqAnswers",
                schema: "usr");

            migrationBuilder.DropTable(
                name: "UserRoles",
                schema: "auth");

            migrationBuilder.DropTable(
                name: "UserSocials",
                schema: "usr");

            migrationBuilder.DropTable(
                name: "UserWrittenAnswers",
                schema: "usr");

            migrationBuilder.DropTable(
                name: "UserSolutions",
                schema: "usr");

            migrationBuilder.DropTable(
                name: "McqOptions",
                schema: "exam");

            migrationBuilder.DropTable(
                name: "Roles",
                schema: "enum");

            migrationBuilder.DropTable(
                name: "Socials",
                schema: "usr");

            migrationBuilder.DropTable(
                name: "Problems",
                schema: "exam");

            migrationBuilder.DropTable(
                name: "ProgLanguages",
                schema: "enum");

            migrationBuilder.DropTable(
                name: "SocialPlatforms",
                schema: "enum");

            migrationBuilder.DropTable(
                name: "Questions",
                schema: "exam");

            migrationBuilder.DropTable(
                name: "Examinations",
                schema: "exam");

            migrationBuilder.DropTable(
                name: "QuestionTypes",
                schema: "enum");

            migrationBuilder.DropIndex(
                name: "IX_Users_CloudFileId",
                schema: "auth",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_OtpEmail",
                schema: "auth",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "OtpEmail",
                schema: "auth",
                table: "Users");

            migrationBuilder.RenameTable(
                name: "UserDetails",
                schema: "usr",
                newName: "UserDetails",
                newSchema: "core");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                schema: "auth",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit");
        }
    }
}
