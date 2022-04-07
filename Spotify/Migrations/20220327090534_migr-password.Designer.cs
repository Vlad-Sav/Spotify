﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Spotify.Models;

namespace Spotify.Migrations
{
    [DbContext(typeof(SpotifyFuncContext))]
    [Migration("20220327090534_migr-password")]
    partial class migrpassword
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.21")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Spotify.Models.Albums", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("albumId")
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AlbumName")
                        .HasColumnName("albumName")
                        .HasColumnType("nchar(50)")
                        .IsFixedLength(true)
                        .HasMaxLength(50);

                    b.Property<int?>("ArtistId")
                        .HasColumnName("artistId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ArtistId");

                    b.ToTable("Albums");
                });

            modelBuilder.Entity("Spotify.Models.AlbumsSongs", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ASid")
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("AlbumId")
                        .HasColumnName("albumId")
                        .HasColumnType("int");

                    b.Property<int?>("SongId")
                        .HasColumnName("songId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AlbumId");

                    b.HasIndex("SongId");

                    b.ToTable("AlbumsSongs");
                });

            modelBuilder.Entity("Spotify.Models.Artists", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("artistId")
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ArtistName")
                        .HasColumnName("artistName")
                        .HasColumnType("nchar(50)")
                        .IsFixedLength(true)
                        .HasMaxLength(50);

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Artists");
                });

            modelBuilder.Entity("Spotify.Models.ArtistsSongs", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ASId")
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("ArtistId")
                        .HasColumnName("artistId")
                        .HasColumnType("int");

                    b.Property<int?>("SongId")
                        .HasColumnName("songId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ArtistId");

                    b.HasIndex("SongId");

                    b.ToTable("ArtistsSongs");
                });

            modelBuilder.Entity("Spotify.Models.Listeners", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("listenerId")
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ListenerName")
                        .HasColumnName("listenerName")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.ToTable("Listeners");
                });

            modelBuilder.Entity("Spotify.Models.Playlists", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("playlistId")
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("ListenerId")
                        .HasColumnName("listenerId")
                        .HasColumnType("int");

                    b.Property<string>("PlaylistName")
                        .HasColumnName("playlistName")
                        .HasColumnType("nchar(50)")
                        .IsFixedLength(true)
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.HasIndex("ListenerId");

                    b.ToTable("Playlists");
                });

            modelBuilder.Entity("Spotify.Models.PlaylistsSongs", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("PSId")
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("PlaylistId")
                        .HasColumnName("playlistId")
                        .HasColumnType("int");

                    b.Property<int?>("SongId")
                        .HasColumnName("songId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PlaylistId");

                    b.HasIndex("SongId");

                    b.ToTable("PlaylistsSongs");
                });

            modelBuilder.Entity("Spotify.Models.Songs", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("songId")
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Path")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SongName")
                        .HasColumnName("songName")
                        .HasColumnType("nchar(50)")
                        .IsFixedLength(true)
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.ToTable("Songs");
                });

            modelBuilder.Entity("Spotify.Models.Albums", b =>
                {
                    b.HasOne("Spotify.Models.Artists", "Artist")
                        .WithMany("Albums")
                        .HasForeignKey("ArtistId")
                        .HasConstraintName("FK_Albums_Artists");
                });

            modelBuilder.Entity("Spotify.Models.AlbumsSongs", b =>
                {
                    b.HasOne("Spotify.Models.Albums", "Album")
                        .WithMany("AlbumsSongs")
                        .HasForeignKey("AlbumId")
                        .HasConstraintName("FK_AlbumsSongs_Albums");

                    b.HasOne("Spotify.Models.Songs", "Song")
                        .WithMany("AlbumsSongs")
                        .HasForeignKey("SongId")
                        .HasConstraintName("FK_AlbumsSongs_Songs");
                });

            modelBuilder.Entity("Spotify.Models.ArtistsSongs", b =>
                {
                    b.HasOne("Spotify.Models.Artists", "Artist")
                        .WithMany("ArtistsSongs")
                        .HasForeignKey("ArtistId")
                        .HasConstraintName("FK_ArtistsSongs_Artists");

                    b.HasOne("Spotify.Models.Songs", "Song")
                        .WithMany("ArtistsSongs")
                        .HasForeignKey("SongId")
                        .HasConstraintName("FK_ArtistsSongs_Songs");
                });

            modelBuilder.Entity("Spotify.Models.Playlists", b =>
                {
                    b.HasOne("Spotify.Models.Listeners", "Listener")
                        .WithMany("Playlists")
                        .HasForeignKey("ListenerId")
                        .HasConstraintName("FK_Playlists_Listeners");
                });

            modelBuilder.Entity("Spotify.Models.PlaylistsSongs", b =>
                {
                    b.HasOne("Spotify.Models.Playlists", "Playlist")
                        .WithMany("PlaylistsSongs")
                        .HasForeignKey("PlaylistId")
                        .HasConstraintName("FK_PlaylistsSongs_Playlists");

                    b.HasOne("Spotify.Models.Songs", "Song")
                        .WithMany("PlaylistsSongs")
                        .HasForeignKey("SongId")
                        .HasConstraintName("FK_PlaylistsSongs_Songs");
                });
#pragma warning restore 612, 618
        }
    }
}
