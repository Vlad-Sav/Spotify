using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Spotify.Models
{
    public partial class SpotifyFuncContext : DbContext
    {
        public SpotifyFuncContext()
        {
        }

        public SpotifyFuncContext(DbContextOptions<SpotifyFuncContext> options)
            : base(options)
        {
            Database.Migrate();
        }

        public virtual DbSet<Albums> Albums { get; set; }
        public virtual DbSet<AlbumsSongs> AlbumsSongs { get; set; }
        public virtual DbSet<Artists> Artists { get; set; }
        public virtual DbSet<ArtistsSongs> ArtistsSongs { get; set; }
        public virtual DbSet<Listeners> Listeners { get; set; }
        public virtual DbSet<Playlists> Playlists { get; set; }
        public virtual DbSet<PlaylistsSongs> PlaylistsSongs { get; set; }
        public virtual DbSet<Songs> Songs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {

                optionsBuilder.UseSqlServer("Server=LAPTOP-AS3UV8CS\\SQLEXPRESS;Database=SpotifyF;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Albums>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).HasColumnName("albumId");

                entity.Property(e => e.AlbumName)
                    .HasColumnName("albumName")
                    .HasMaxLength(50)
                    .IsFixedLength();

                entity.Property(e => e.ArtistId).HasColumnName("artistId");

                entity.HasOne(d => d.Artist)
                    .WithMany(p => p.Albums)
                    .HasForeignKey(d => d.ArtistId)
                    .HasConstraintName("FK_Albums_Artists");
            });

            modelBuilder.Entity<AlbumsSongs>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).HasColumnName("ASid");

                entity.Property(e => e.AlbumId).HasColumnName("albumId");

                entity.Property(e => e.SongId).HasColumnName("songId");

                entity.HasOne(d => d.Album)
                    .WithMany(p => p.AlbumsSongs)
                    .HasForeignKey(d => d.AlbumId)
                    .HasConstraintName("FK_AlbumsSongs_Albums");

                entity.HasOne(d => d.Song)
                    .WithMany(p => p.AlbumsSongs)
                    .HasForeignKey(d => d.SongId)
                    .HasConstraintName("FK_AlbumsSongs_Songs");
            });

            modelBuilder.Entity<Artists>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).HasColumnName("artistId");

                entity.Property(e => e.ArtistName)
                    .HasColumnName("artistName")
                    .HasMaxLength(50)
                    .IsFixedLength();
            });

            modelBuilder.Entity<ArtistsSongs>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).HasColumnName("ASId");

                entity.Property(e => e.ArtistId).HasColumnName("artistId");

                entity.Property(e => e.SongId).HasColumnName("songId");

                entity.HasOne(d => d.Artist)
                    .WithMany(p => p.ArtistsSongs)
                    .HasForeignKey(d => d.ArtistId)
                    .HasConstraintName("FK_ArtistsSongs_Artists");

                entity.HasOne(d => d.Song)
                    .WithMany(p => p.ArtistsSongs)
                    .HasForeignKey(d => d.SongId)
                    .HasConstraintName("FK_ArtistsSongs_Songs");
            });

            modelBuilder.Entity<Listeners>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).HasColumnName("listenerId");

                entity.Property(e => e.ListenerName)
                    .HasColumnName("listenerName")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Playlists>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).HasColumnName("playlistId");

                entity.Property(e => e.ListenerId).HasColumnName("listenerId");

                entity.Property(e => e.PlaylistName)
                    .HasColumnName("playlistName")
                    .HasMaxLength(50)
                    .IsFixedLength();

                entity.HasOne(d => d.Listener)
                    .WithMany(p => p.Playlists)
                    .HasForeignKey(d => d.ListenerId)
                    .HasConstraintName("FK_Playlists_Listeners");
            });

            modelBuilder.Entity<PlaylistsSongs>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).HasColumnName("PSId");

                entity.Property(e => e.PlaylistId).HasColumnName("playlistId");

                entity.Property(e => e.SongId).HasColumnName("songId");

                entity.HasOne(d => d.Playlist)
                    .WithMany(p => p.PlaylistsSongs)
                    .HasForeignKey(d => d.PlaylistId)
                    .HasConstraintName("FK_PlaylistsSongs_Playlists");

                entity.HasOne(d => d.Song)
                    .WithMany(p => p.PlaylistsSongs)
                    .HasForeignKey(d => d.SongId)
                    .HasConstraintName("FK_PlaylistsSongs_Songs");
            });

            modelBuilder.Entity<Songs>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).HasColumnName("songId");

                entity.Property(e => e.SongName)
                    .HasColumnName("songName")
                    .HasMaxLength(50)
                    .IsFixedLength();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
