using Microsoft.EntityFrameworkCore.Migrations;

namespace Spotify.Migrations
{
    public partial class migr : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Artists",
                columns: table => new
                {
                    artistId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    artistName = table.Column<string>(fixedLength: true, maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Artists", x => x.artistId);
                });

            migrationBuilder.CreateTable(
                name: "Listeners",
                columns: table => new
                {
                    listenerId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    listenerName = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Listeners", x => x.listenerId);
                });

            migrationBuilder.CreateTable(
                name: "Songs",
                columns: table => new
                {
                    songId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    songName = table.Column<string>(fixedLength: true, maxLength: 50, nullable: true),
                    Path = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Songs", x => x.songId);
                });

            migrationBuilder.CreateTable(
                name: "Albums",
                columns: table => new
                {
                    albumId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    albumName = table.Column<string>(fixedLength: true, maxLength: 50, nullable: true),
                    artistId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Albums", x => x.albumId);
                    table.ForeignKey(
                        name: "FK_Albums_Artists",
                        column: x => x.artistId,
                        principalTable: "Artists",
                        principalColumn: "artistId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Playlists",
                columns: table => new
                {
                    playlistId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    playlistName = table.Column<string>(fixedLength: true, maxLength: 50, nullable: true),
                    listenerId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Playlists", x => x.playlistId);
                    table.ForeignKey(
                        name: "FK_Playlists_Listeners",
                        column: x => x.listenerId,
                        principalTable: "Listeners",
                        principalColumn: "listenerId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ArtistsSongs",
                columns: table => new
                {
                    ASId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    artistId = table.Column<int>(nullable: true),
                    songId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtistsSongs", x => x.ASId);
                    table.ForeignKey(
                        name: "FK_ArtistsSongs_Artists",
                        column: x => x.artistId,
                        principalTable: "Artists",
                        principalColumn: "artistId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ArtistsSongs_Songs",
                        column: x => x.songId,
                        principalTable: "Songs",
                        principalColumn: "songId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AlbumsSongs",
                columns: table => new
                {
                    ASid = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    albumId = table.Column<int>(nullable: true),
                    songId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlbumsSongs", x => x.ASid);
                    table.ForeignKey(
                        name: "FK_AlbumsSongs_Albums",
                        column: x => x.albumId,
                        principalTable: "Albums",
                        principalColumn: "albumId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AlbumsSongs_Songs",
                        column: x => x.songId,
                        principalTable: "Songs",
                        principalColumn: "songId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PlaylistsSongs",
                columns: table => new
                {
                    PSId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    playlistId = table.Column<int>(nullable: true),
                    songId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlaylistsSongs", x => x.PSId);
                    table.ForeignKey(
                        name: "FK_PlaylistsSongs_Playlists",
                        column: x => x.playlistId,
                        principalTable: "Playlists",
                        principalColumn: "playlistId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlaylistsSongs_Songs",
                        column: x => x.songId,
                        principalTable: "Songs",
                        principalColumn: "songId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Albums_artistId",
                table: "Albums",
                column: "artistId");

            migrationBuilder.CreateIndex(
                name: "IX_AlbumsSongs_albumId",
                table: "AlbumsSongs",
                column: "albumId");

            migrationBuilder.CreateIndex(
                name: "IX_AlbumsSongs_songId",
                table: "AlbumsSongs",
                column: "songId");

            migrationBuilder.CreateIndex(
                name: "IX_ArtistsSongs_artistId",
                table: "ArtistsSongs",
                column: "artistId");

            migrationBuilder.CreateIndex(
                name: "IX_ArtistsSongs_songId",
                table: "ArtistsSongs",
                column: "songId");

            migrationBuilder.CreateIndex(
                name: "IX_Playlists_listenerId",
                table: "Playlists",
                column: "listenerId");

            migrationBuilder.CreateIndex(
                name: "IX_PlaylistsSongs_playlistId",
                table: "PlaylistsSongs",
                column: "playlistId");

            migrationBuilder.CreateIndex(
                name: "IX_PlaylistsSongs_songId",
                table: "PlaylistsSongs",
                column: "songId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AlbumsSongs");

            migrationBuilder.DropTable(
                name: "ArtistsSongs");

            migrationBuilder.DropTable(
                name: "PlaylistsSongs");

            migrationBuilder.DropTable(
                name: "Albums");

            migrationBuilder.DropTable(
                name: "Playlists");

            migrationBuilder.DropTable(
                name: "Songs");

            migrationBuilder.DropTable(
                name: "Artists");

            migrationBuilder.DropTable(
                name: "Listeners");
        }
    }
}
