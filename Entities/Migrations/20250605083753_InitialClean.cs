using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    /// <inheritdoc />
    public partial class InitialClean : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FatherName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsSpecialist = table.Column<bool>(type: "bit", nullable: false),
                    SpecialistNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IDFK_Directory = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false),
                    Invalidated = table.Column<byte>(type: "tinyint", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedIp = table.Column<string>(type: "nvarchar(46)", maxLength: 46, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedIp = table.Column<string>(type: "nvarchar(46)", maxLength: 46, nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_AspNetUsers_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_AspNetUsers_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Auto_Directorates",
                columns: table => new
                {
                    IDPK_Directory = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DirectoryName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Invalidated = table.Column<byte>(type: "tinyint", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedIp = table.Column<string>(type: "nvarchar(46)", maxLength: 46, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedIp = table.Column<string>(type: "nvarchar(46)", maxLength: 46, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Auto_Directorates", x => x.IDPK_Directory);
                    table.ForeignKey(
                        name: "FK_Auto_Directorates_AspNetUsers_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Auto_Directorates_AspNetUsers_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Auto_FineRecipients",
                columns: table => new
                {
                    IDPK_FineRecipient = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IDFK_User = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FatherName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PersonalId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    PlateNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Invalidated = table.Column<byte>(type: "tinyint", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedIp = table.Column<string>(type: "nvarchar(46)", maxLength: 46, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedIp = table.Column<string>(type: "nvarchar(46)", maxLength: 46, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Auto_FineRecipients", x => x.IDPK_FineRecipient);
                    table.ForeignKey(
                        name: "FK_Auto_FineRecipients_AspNetUsers_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Auto_FineRecipients_AspNetUsers_IDFK_User",
                        column: x => x.IDFK_User,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Auto_FineRecipients_AspNetUsers_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Auto_Notifications",
                columns: table => new
                {
                    IDPK_Notification = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IDFK_Receiver = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Message = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    IsSeen = table.Column<bool>(type: "bit", nullable: false),
                    Type = table.Column<byte>(type: "tinyint", nullable: false),
                    Invalidated = table.Column<byte>(type: "tinyint", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedIp = table.Column<string>(type: "nvarchar(46)", maxLength: 46, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Auto_Notifications", x => x.IDPK_Notification);
                    table.ForeignKey(
                        name: "FK_Auto_Notifications_AspNetUsers_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Auto_Notifications_AspNetUsers_IDFK_Receiver",
                        column: x => x.IDFK_Receiver,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Auto_Vehicles",
                columns: table => new
                {
                    IDPK_Vehicle = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IDFK_Owner = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PlateNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Color = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SeatCount = table.Column<byte>(type: "tinyint", nullable: false),
                    DoorCount = table.Column<byte>(type: "tinyint", nullable: false),
                    ChassisNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Status = table.Column<byte>(type: "tinyint", nullable: false),
                    ApprovalComment = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Invalidated = table.Column<byte>(type: "tinyint", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedIp = table.Column<string>(type: "nvarchar(46)", maxLength: 46, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedIp = table.Column<string>(type: "nvarchar(46)", maxLength: 46, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Auto_Vehicles", x => x.IDPK_Vehicle);
                    table.ForeignKey(
                        name: "FK_Auto_Vehicles_AspNetUsers_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Auto_Vehicles_AspNetUsers_IDFK_Owner",
                        column: x => x.IDFK_Owner,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Auto_Vehicles_AspNetUsers_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Auto_Fines",
                columns: table => new
                {
                    IDPK_Fine = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IDFK_Vehicle = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IDFK_FineRecipient = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FineAmount = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    FineDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FineReason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Invalidated = table.Column<byte>(type: "tinyint", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedIp = table.Column<string>(type: "nvarchar(46)", maxLength: 46, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedIp = table.Column<string>(type: "nvarchar(46)", maxLength: 46, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Auto_Fines", x => x.IDPK_Fine);
                    table.ForeignKey(
                        name: "FK_Auto_Fines_AspNetUsers_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Auto_Fines_AspNetUsers_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Auto_Fines_Auto_FineRecipients_IDFK_FineRecipient",
                        column: x => x.IDFK_FineRecipient,
                        principalTable: "Auto_FineRecipients",
                        principalColumn: "IDPK_FineRecipient",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Auto_Fines_Auto_Vehicles_IDFK_Vehicle",
                        column: x => x.IDFK_Vehicle,
                        principalTable: "Auto_Vehicles",
                        principalColumn: "IDPK_Vehicle");
                });

            migrationBuilder.CreateTable(
                name: "Auto_InspectionRequests",
                columns: table => new
                {
                    IDPK_InspectionRequest = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IDFK_Vehicle = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IDFK_Directory = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RequestedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<byte>(type: "tinyint", nullable: false),
                    Invalidated = table.Column<byte>(type: "tinyint", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedIp = table.Column<string>(type: "nvarchar(46)", maxLength: 46, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedIp = table.Column<string>(type: "nvarchar(46)", maxLength: 46, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Auto_InspectionRequests", x => x.IDPK_InspectionRequest);
                    table.ForeignKey(
                        name: "FK_Auto_InspectionRequests_AspNetUsers_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Auto_InspectionRequests_AspNetUsers_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Auto_InspectionRequests_Auto_Directorates_IDFK_Directory",
                        column: x => x.IDFK_Directory,
                        principalTable: "Auto_Directorates",
                        principalColumn: "IDPK_Directory",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Auto_InspectionRequests_Auto_Vehicles_IDFK_Vehicle",
                        column: x => x.IDFK_Vehicle,
                        principalTable: "Auto_Vehicles",
                        principalColumn: "IDPK_Vehicle",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Auto_VehicleChangeRequests",
                columns: table => new
                {
                    IDPK_ChangeRequest = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IDFK_Vehicle = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IDFK_Requester = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RequestType = table.Column<byte>(type: "tinyint", nullable: false),
                    RequestDataJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CurrentDataSnapshotJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RequesterComment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<byte>(type: "tinyint", nullable: false),
                    AdminComment = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Invalidated = table.Column<byte>(type: "tinyint", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedIp = table.Column<string>(type: "nvarchar(46)", maxLength: 46, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedIp = table.Column<string>(type: "nvarchar(46)", maxLength: 46, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Auto_VehicleChangeRequests", x => x.IDPK_ChangeRequest);
                    table.ForeignKey(
                        name: "FK_Auto_VehicleChangeRequests_AspNetUsers_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Auto_VehicleChangeRequests_AspNetUsers_IDFK_Requester",
                        column: x => x.IDFK_Requester,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Auto_VehicleChangeRequests_AspNetUsers_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Auto_VehicleChangeRequests_Auto_Vehicles_IDFK_Vehicle",
                        column: x => x.IDFK_Vehicle,
                        principalTable: "Auto_Vehicles",
                        principalColumn: "IDPK_Vehicle",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Auto_Inspections",
                columns: table => new
                {
                    IDPK_Inspection = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IDFK_InspectionRequest = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IDFK_Specialist = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IsPassed = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Invalidated = table.Column<byte>(type: "tinyint", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedIp = table.Column<string>(type: "nvarchar(46)", maxLength: 46, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedIp = table.Column<string>(type: "nvarchar(46)", maxLength: 46, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Auto_Inspections", x => x.IDPK_Inspection);
                    table.ForeignKey(
                        name: "FK_Auto_Inspections_AspNetUsers_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Auto_Inspections_AspNetUsers_IDFK_Specialist",
                        column: x => x.IDFK_Specialist,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Auto_Inspections_AspNetUsers_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Auto_Inspections_Auto_InspectionRequests_IDFK_InspectionRequest",
                        column: x => x.IDFK_InspectionRequest,
                        principalTable: "Auto_InspectionRequests",
                        principalColumn: "IDPK_InspectionRequest",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Auto_InspectionDocs",
                columns: table => new
                {
                    IDPK_InspectionDoc = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IDFK_InspectionRequest = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DocumentName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    FileBase64 = table.Column<string>(type: "nvarchar(max)", maxLength: 7000000, nullable: false),
                    Invalidated = table.Column<byte>(type: "tinyint", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedIp = table.Column<string>(type: "nvarchar(46)", maxLength: 46, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedIp = table.Column<string>(type: "nvarchar(46)", maxLength: 46, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Auto_InspectionDocs", x => x.IDPK_InspectionDoc);
                    table.ForeignKey(
                        name: "FK_Auto_InspectionDocs_AspNetUsers_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Auto_InspectionDocs_AspNetUsers_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Auto_InspectionDocs_Auto_Inspections_IDFK_InspectionRequest",
                        column: x => x.IDFK_InspectionRequest,
                        principalTable: "Auto_Inspections",
                        principalColumn: "IDPK_Inspection",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_CreatedBy",
                table: "AspNetUsers",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_IDFK_Directory",
                table: "AspNetUsers",
                column: "IDFK_Directory");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ModifiedBy",
                table: "AspNetUsers",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Auto_Directorates_CreatedBy",
                table: "Auto_Directorates",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Auto_Directorates_DirectoryName",
                table: "Auto_Directorates",
                column: "DirectoryName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Auto_Directorates_ModifiedBy",
                table: "Auto_Directorates",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Auto_FineRecipients_CreatedBy",
                table: "Auto_FineRecipients",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Auto_FineRecipients_IDFK_User",
                table: "Auto_FineRecipients",
                column: "IDFK_User");

            migrationBuilder.CreateIndex(
                name: "IX_Auto_FineRecipients_ModifiedBy",
                table: "Auto_FineRecipients",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Auto_FineRecipients_PersonalId",
                table: "Auto_FineRecipients",
                column: "PersonalId",
                unique: true,
                filter: "[PersonalId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Auto_FineRecipients_PlateNumber",
                table: "Auto_FineRecipients",
                column: "PlateNumber");

            migrationBuilder.CreateIndex(
                name: "IX_Auto_Fines_CreatedBy",
                table: "Auto_Fines",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Auto_Fines_IDFK_FineRecipient",
                table: "Auto_Fines",
                column: "IDFK_FineRecipient");

            migrationBuilder.CreateIndex(
                name: "IX_Auto_Fines_IDFK_Vehicle",
                table: "Auto_Fines",
                column: "IDFK_Vehicle");

            migrationBuilder.CreateIndex(
                name: "IX_Auto_Fines_ModifiedBy",
                table: "Auto_Fines",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Auto_InspectionDocs_CreatedBy",
                table: "Auto_InspectionDocs",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Auto_InspectionDocs_IDFK_InspectionRequest",
                table: "Auto_InspectionDocs",
                column: "IDFK_InspectionRequest");

            migrationBuilder.CreateIndex(
                name: "IX_Auto_InspectionDocs_ModifiedBy",
                table: "Auto_InspectionDocs",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Auto_InspectionRequests_CreatedBy",
                table: "Auto_InspectionRequests",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Auto_InspectionRequests_IDFK_Directory",
                table: "Auto_InspectionRequests",
                column: "IDFK_Directory");

            migrationBuilder.CreateIndex(
                name: "IX_Auto_InspectionRequests_IDFK_Vehicle",
                table: "Auto_InspectionRequests",
                column: "IDFK_Vehicle");

            migrationBuilder.CreateIndex(
                name: "IX_Auto_InspectionRequests_ModifiedBy",
                table: "Auto_InspectionRequests",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Auto_Inspections_CreatedBy",
                table: "Auto_Inspections",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Auto_Inspections_IDFK_InspectionRequest",
                table: "Auto_Inspections",
                column: "IDFK_InspectionRequest");

            migrationBuilder.CreateIndex(
                name: "IX_Auto_Inspections_IDFK_Specialist",
                table: "Auto_Inspections",
                column: "IDFK_Specialist");

            migrationBuilder.CreateIndex(
                name: "IX_Auto_Inspections_ModifiedBy",
                table: "Auto_Inspections",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Auto_Notifications_CreatedBy",
                table: "Auto_Notifications",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Auto_Notifications_IDFK_Receiver",
                table: "Auto_Notifications",
                column: "IDFK_Receiver");

            migrationBuilder.CreateIndex(
                name: "IX_Auto_VehicleChangeRequests_CreatedBy",
                table: "Auto_VehicleChangeRequests",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Auto_VehicleChangeRequests_IDFK_Requester",
                table: "Auto_VehicleChangeRequests",
                column: "IDFK_Requester");

            migrationBuilder.CreateIndex(
                name: "IX_Auto_VehicleChangeRequests_IDFK_Vehicle",
                table: "Auto_VehicleChangeRequests",
                column: "IDFK_Vehicle");

            migrationBuilder.CreateIndex(
                name: "IX_Auto_VehicleChangeRequests_ModifiedBy",
                table: "Auto_VehicleChangeRequests",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Auto_Vehicles_ChassisNumber",
                table: "Auto_Vehicles",
                column: "ChassisNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Auto_Vehicles_CreatedBy",
                table: "Auto_Vehicles",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Auto_Vehicles_IDFK_Owner",
                table: "Auto_Vehicles",
                column: "IDFK_Owner");

            migrationBuilder.CreateIndex(
                name: "IX_Auto_Vehicles_ModifiedBy",
                table: "Auto_Vehicles",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Auto_Vehicles_PlateNumber",
                table: "Auto_Vehicles",
                column: "PlateNumber",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                table: "AspNetUserClaims",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                table: "AspNetUserLogins",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                table: "AspNetUserRoles",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Auto_Directorates_IDFK_Directory",
                table: "AspNetUsers",
                column: "IDFK_Directory",
                principalTable: "Auto_Directorates",
                principalColumn: "IDPK_Directory",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Auto_Directorates_AspNetUsers_CreatedBy",
                table: "Auto_Directorates");

            migrationBuilder.DropForeignKey(
                name: "FK_Auto_Directorates_AspNetUsers_ModifiedBy",
                table: "Auto_Directorates");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Auto_Fines");

            migrationBuilder.DropTable(
                name: "Auto_InspectionDocs");

            migrationBuilder.DropTable(
                name: "Auto_Notifications");

            migrationBuilder.DropTable(
                name: "Auto_VehicleChangeRequests");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Auto_FineRecipients");

            migrationBuilder.DropTable(
                name: "Auto_Inspections");

            migrationBuilder.DropTable(
                name: "Auto_InspectionRequests");

            migrationBuilder.DropTable(
                name: "Auto_Vehicles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Auto_Directorates");
        }
    }
}
