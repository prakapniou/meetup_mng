using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MeetupManager.Core.EF.Migrations
{
    /// <inheritdoc />
    public partial class AddRelationshipManyToMany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_sponsors",
                table: "sponsors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_speakers",
                table: "speakers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_meetups",
                table: "meetups");

            migrationBuilder.RenameTable(
                name: "sponsors",
                newName: "Sponsors");

            migrationBuilder.RenameTable(
                name: "speakers",
                newName: "Speakers");

            migrationBuilder.RenameTable(
                name: "meetups",
                newName: "Meetups");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Sponsors",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Sponsors",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Speakers",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Speakers",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "topic",
                table: "Meetups",
                newName: "Topic");

            migrationBuilder.RenameColumn(
                name: "spending",
                table: "Meetups",
                newName: "Spending");

            migrationBuilder.RenameColumn(
                name: "schedule",
                table: "Meetups",
                newName: "Schedule");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Meetups",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "Meetups",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "address",
                table: "Meetups",
                newName: "Address");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Meetups",
                newName: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Sponsors",
                table: "Sponsors",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Speakers",
                table: "Speakers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Meetups",
                table: "Meetups",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "MeetupSpeaker",
                columns: table => new
                {
                    MeetupId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SpeakersId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeetupSpeaker", x => new { x.MeetupId, x.SpeakersId });
                    table.ForeignKey(
                        name: "FK_MeetupSpeaker_Meetups_MeetupId",
                        column: x => x.MeetupId,
                        principalTable: "Meetups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MeetupSpeaker_Speakers_SpeakersId",
                        column: x => x.SpeakersId,
                        principalTable: "Speakers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MeetupSponsor",
                columns: table => new
                {
                    MeetupId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SponsorsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeetupSponsor", x => new { x.MeetupId, x.SponsorsId });
                    table.ForeignKey(
                        name: "FK_MeetupSponsor_Meetups_MeetupId",
                        column: x => x.MeetupId,
                        principalTable: "Meetups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MeetupSponsor_Sponsors_SponsorsId",
                        column: x => x.SponsorsId,
                        principalTable: "Sponsors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MeetupSpeaker_SpeakersId",
                table: "MeetupSpeaker",
                column: "SpeakersId");

            migrationBuilder.CreateIndex(
                name: "IX_MeetupSponsor_SponsorsId",
                table: "MeetupSponsor",
                column: "SponsorsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MeetupSpeaker");

            migrationBuilder.DropTable(
                name: "MeetupSponsor");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Sponsors",
                table: "Sponsors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Speakers",
                table: "Speakers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Meetups",
                table: "Meetups");

            migrationBuilder.RenameTable(
                name: "Sponsors",
                newName: "sponsors");

            migrationBuilder.RenameTable(
                name: "Speakers",
                newName: "speakers");

            migrationBuilder.RenameTable(
                name: "Meetups",
                newName: "meetups");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "sponsors",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "sponsors",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "speakers",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "speakers",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Topic",
                table: "meetups",
                newName: "topic");

            migrationBuilder.RenameColumn(
                name: "Spending",
                table: "meetups",
                newName: "spending");

            migrationBuilder.RenameColumn(
                name: "Schedule",
                table: "meetups",
                newName: "schedule");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "meetups",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "meetups",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "Address",
                table: "meetups",
                newName: "address");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "meetups",
                newName: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_sponsors",
                table: "sponsors",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_speakers",
                table: "speakers",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_meetups",
                table: "meetups",
                column: "id");
        }
    }
}
