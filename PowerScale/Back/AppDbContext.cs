using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using PowerScale.Back.Models;

namespace PowerScale.Back;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AdminBackup> AdminBackups { get; set; }

    public virtual DbSet<Akasualtype> Akasualtypes { get; set; }

    public virtual DbSet<Character> Characters { get; set; }

    public virtual DbSet<CharacterBackup> CharacterBackups { get; set; }

    public virtual DbSet<Characteristic> Characteristics { get; set; }

    public virtual DbSet<Dclass> Dclasses { get; set; }

    public virtual DbSet<Fight> Fights { get; set; }

    public virtual DbSet<Universe> Universes { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<ViewCharacterFull> ViewCharacterFulls { get; set; }

    public virtual DbSet<ViewFightsHistory> ViewFightsHistories { get; set; }

    public virtual DbSet<ViewUserRating> ViewUserRatings { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=postgres;Username=postgres;Password=siskabobra12334");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("pg_trgm");

        modelBuilder.Entity<AdminBackup>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("admin_backup");

            entity.Property(e => e.Nickname)
                .HasMaxLength(100)
                .HasColumnName("nickname");
        });

        modelBuilder.Entity<Akasualtype>(entity =>
        {
            entity.HasKey(e => e.UndyingId).HasName("akasualtype_pkey");

            entity.ToTable("akasualtype");

            entity.Property(e => e.UndyingId).HasColumnName("undying_id");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Tag)
                .HasMaxLength(50)
                .HasColumnName("tag");
            entity.Property(e => e.Type).HasColumnName("type");
        });

        modelBuilder.Entity<Character>(entity =>
        {
            entity.HasKey(e => e.CharacterId).HasName("character_pkey");

            entity.ToTable("character");

            entity.HasIndex(e => e.CharacteristicsId, "idx_character_characteristics_id");

            entity.HasIndex(e => e.DclassId, "idx_character_dclass_id");

            entity.HasIndex(e => e.Name, "idx_character_name_trgm")
                .HasMethod("gin")
                .HasOperators(new[] { "gin_trgm_ops" });

            entity.HasIndex(e => e.Powerscale, "idx_character_powerscale");

            entity.HasIndex(e => e.UniverseId, "idx_character_universe_id");

            entity.Property(e => e.CharacterId).HasColumnName("character_id");
            entity.Property(e => e.Age).HasColumnName("age");
            entity.Property(e => e.CharacteristicsId).HasColumnName("characteristics_id");
            entity.Property(e => e.DclassId).HasColumnName("dclass_id");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .HasColumnName("name");
            entity.Property(e => e.Powerscale).HasColumnName("powerscale");
            entity.Property(e => e.Skills).HasColumnName("skills");
            entity.Property(e => e.UniverseId).HasColumnName("universe_id");

            entity.HasOne(d => d.Characteristics).WithMany(p => p.Characters)
                .HasForeignKey(d => d.CharacteristicsId)
                .HasConstraintName("character_characteristics_id_fkey");

            entity.HasOne(d => d.Dclass).WithMany(p => p.Characters)
                .HasForeignKey(d => d.DclassId)
                .HasConstraintName("character_dclass_id_fkey");

            entity.HasOne(d => d.Universe).WithMany(p => p.Characters)
                .HasForeignKey(d => d.UniverseId)
                .HasConstraintName("character_universe_id_fkey");
        });

        modelBuilder.Entity<CharacterBackup>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("character_backup");

            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .HasColumnName("name");
            entity.Property(e => e.Powerscale).HasColumnName("powerscale");
        });

        modelBuilder.Entity<Characteristic>(entity =>
        {
            entity.HasKey(e => e.CharacteristicsId).HasName("characteristics_pkey");

            entity.ToTable("characteristics");

            entity.Property(e => e.CharacteristicsId).HasColumnName("characteristics_id");
            entity.Property(e => e.Battleiq).HasColumnName("battleiq");
            entity.Property(e => e.Iq).HasColumnName("iq");
            entity.Property(e => e.Speed).HasColumnName("speed");
            entity.Property(e => e.Strenght).HasColumnName("strenght");
        });

        modelBuilder.Entity<Dclass>(entity =>
        {
            entity.HasKey(e => e.DclassId).HasName("dclass_pkey");

            entity.ToTable("dclass");

            entity.HasIndex(e => e.UndyingId, "idx_dclass_undying_id");

            entity.Property(e => e.DclassId).HasColumnName("dclass_id");
            entity.Property(e => e.Durability).HasColumnName("durability");
            entity.Property(e => e.UndyingId).HasColumnName("undying_id");

            entity.HasOne(d => d.Undying).WithMany(p => p.Dclasses)
                .HasForeignKey(d => d.UndyingId)
                .HasConstraintName("dclass_undying_id_fkey");
        });

        modelBuilder.Entity<Fight>(entity =>
        {
            entity.HasKey(e => e.FightsId).HasName("fights_pkey");

            entity.ToTable("fights");

            entity.HasIndex(e => e.UserId, "idx_fights_user_id");

            entity.Property(e => e.FightsId).HasColumnName("fights_id");
            entity.Property(e => e.Character1Id).HasColumnName("character1_id");
            entity.Property(e => e.Character2Id).HasColumnName("character2_id");
            entity.Property(e => e.Fightexodus)
                .HasMaxLength(500)
                .HasColumnName("fightexodus");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Character1).WithMany(p => p.FightCharacter1s)
                .HasForeignKey(d => d.Character1Id)
                .HasConstraintName("fights_character1_id_fkey");

            entity.HasOne(d => d.Character2).WithMany(p => p.FightCharacter2s)
                .HasForeignKey(d => d.Character2Id)
                .HasConstraintName("fights_character2_id_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.Fights)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("fights_user_id_fkey");
        });

        modelBuilder.Entity<Universe>(entity =>
        {
            entity.HasKey(e => e.UniverseId).HasName("universe_pkey");

            entity.ToTable("universe");

            entity.Property(e => e.UniverseId).HasColumnName("universe_id");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Type)
                .HasMaxLength(50)
                .HasColumnName("type");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("users_pkey");

            entity.ToTable("users");

            entity.HasIndex(e => e.Email, "users_email_key").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Adminstatus)
                .HasDefaultValue(false)
                .HasColumnName("adminstatus");
            entity.Property(e => e.Email)
                .HasMaxLength(150)
                .HasColumnName("email");
            entity.Property(e => e.Nickname)
                .HasMaxLength(100)
                .HasColumnName("nickname");
            entity.Property(e => e.Rep)
                .HasDefaultValue(0)
                .HasColumnName("rep");
        });

        modelBuilder.Entity<ViewCharacterFull>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("view_character_full");

            entity.Property(e => e.Age).HasColumnName("age");
            entity.Property(e => e.AkasualTag)
                .HasMaxLength(50)
                .HasColumnName("akasual_tag");
            entity.Property(e => e.AkasualType).HasColumnName("akasual_type");
            entity.Property(e => e.Battleiq).HasColumnName("battleiq");
            entity.Property(e => e.CharacterId).HasColumnName("character_id");
            entity.Property(e => e.Durability).HasColumnName("durability");
            entity.Property(e => e.Iq).HasColumnName("iq");
            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .HasColumnName("name");
            entity.Property(e => e.Powerscale).HasColumnName("powerscale");
            entity.Property(e => e.Skills).HasColumnName("skills");
            entity.Property(e => e.Speed).HasColumnName("speed");
            entity.Property(e => e.Strenght).HasColumnName("strenght");
            entity.Property(e => e.Tier)
                .HasColumnType("character varying")
                .HasColumnName("tier");
            entity.Property(e => e.UniverseName)
                .HasMaxLength(100)
                .HasColumnName("universe_name");
            entity.Property(e => e.UniverseType)
                .HasMaxLength(50)
                .HasColumnName("universe_type");
        });

        modelBuilder.Entity<ViewFightsHistory>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("view_fights_history");

            entity.Property(e => e.Fighter1)
                .HasMaxLength(200)
                .HasColumnName("fighter1");
            entity.Property(e => e.Fighter2)
                .HasMaxLength(200)
                .HasColumnName("fighter2");
            entity.Property(e => e.Fightexodus)
                .HasMaxLength(500)
                .HasColumnName("fightexodus");
            entity.Property(e => e.FightsId).HasColumnName("fights_id");
            entity.Property(e => e.ProposedBy)
                .HasMaxLength(100)
                .HasColumnName("proposed_by");
            entity.Property(e => e.Winner)
                .HasColumnType("character varying")
                .HasColumnName("winner");
        });

        modelBuilder.Entity<ViewUserRating>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("view_user_rating");

            entity.Property(e => e.Adminstatus).HasColumnName("adminstatus");
            entity.Property(e => e.Nickname)
                .HasMaxLength(100)
                .HasColumnName("nickname");
            entity.Property(e => e.Rep).HasColumnName("rep");
            entity.Property(e => e.TotalFights).HasColumnName("total_fights");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.UserStatus).HasColumnName("user_status");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
