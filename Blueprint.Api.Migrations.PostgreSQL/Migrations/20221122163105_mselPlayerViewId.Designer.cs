/*
 Copyright 2022 Carnegie Mellon University. All Rights Reserved.
 Released under a MIT (SEI)-style license. See LICENSE.md in the project root for license information.
*/

// <auto-generated />
using System;
using Blueprint.Api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Blueprint.Api.Migrations.PostgreSQL.Migrations
{
    [DbContext(typeof(BlueprintContext))]
    [Migration("20221122163105_mselPlayerViewId")]
    partial class MselPlayerViewId
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.HasPostgresExtension(modelBuilder, "uuid-ossp");
            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Blueprint.Api.Data.Models.CardEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<Guid>("CreatedBy")
                        .HasColumnType("uuid")
                        .HasColumnName("created_by");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("date_created");

                    b.Property<DateTime?>("DateModified")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("date_modified");

                    b.Property<string>("Description")
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<Guid?>("GalleryId")
                        .HasColumnType("uuid")
                        .HasColumnName("gallery_id");

                    b.Property<int>("Inject")
                        .HasColumnType("integer")
                        .HasColumnName("inject");

                    b.Property<Guid?>("ModifiedBy")
                        .HasColumnType("uuid")
                        .HasColumnName("modified_by");

                    b.Property<int>("Move")
                        .HasColumnType("integer")
                        .HasColumnName("move");

                    b.Property<Guid>("MselId")
                        .HasColumnType("uuid")
                        .HasColumnName("msel_id");

                    b.Property<string>("Name")
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.HasKey("Id");

                    b.HasIndex("MselId");

                    b.ToTable("cards");
                });

            modelBuilder.Entity("Blueprint.Api.Data.Models.DataFieldEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<string>("CellMetadata")
                        .HasColumnType("text")
                        .HasColumnName("cell_metadata");

                    b.Property<string>("ColumnMetadata")
                        .HasColumnType("text")
                        .HasColumnName("column_metadata");

                    b.Property<Guid>("CreatedBy")
                        .HasColumnType("uuid")
                        .HasColumnName("created_by");

                    b.Property<int>("DataType")
                        .HasColumnType("integer")
                        .HasColumnName("data_type");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("date_created");

                    b.Property<DateTime?>("DateModified")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("date_modified");

                    b.Property<int>("DisplayOrder")
                        .HasColumnType("integer")
                        .HasColumnName("display_order");

                    b.Property<string>("GalleryArticleParameter")
                        .HasColumnType("text")
                        .HasColumnName("gallery_article_parameter");

                    b.Property<bool>("IsChosenFromList")
                        .HasColumnType("boolean")
                        .HasColumnName("is_chosen_from_list");

                    b.Property<bool>("IsInitiallyHidden")
                        .HasColumnType("boolean")
                        .HasColumnName("is_initially_hidden");

                    b.Property<bool>("IsOnlyShownToOwners")
                        .HasColumnType("boolean")
                        .HasColumnName("is_only_shown_to_owners");

                    b.Property<Guid?>("ModifiedBy")
                        .HasColumnType("uuid")
                        .HasColumnName("modified_by");

                    b.Property<Guid>("MselId")
                        .HasColumnType("uuid")
                        .HasColumnName("msel_id");

                    b.Property<string>("Name")
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.HasKey("Id");

                    b.HasIndex("MselId");

                    b.ToTable("data_fields");
                });

            modelBuilder.Entity("Blueprint.Api.Data.Models.DataOptionEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<Guid>("CreatedBy")
                        .HasColumnType("uuid")
                        .HasColumnName("created_by");

                    b.Property<Guid>("DataFieldId")
                        .HasColumnType("uuid")
                        .HasColumnName("data_field_id");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("date_created");

                    b.Property<DateTime?>("DateModified")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("date_modified");

                    b.Property<int>("DisplayOrder")
                        .HasColumnType("integer")
                        .HasColumnName("display_order");

                    b.Property<Guid?>("ModifiedBy")
                        .HasColumnType("uuid")
                        .HasColumnName("modified_by");

                    b.Property<string>("OptionName")
                        .HasColumnType("text")
                        .HasColumnName("option_name");

                    b.Property<string>("OptionValue")
                        .HasColumnType("text")
                        .HasColumnName("option_value");

                    b.HasKey("Id");

                    b.HasIndex("DataFieldId");

                    b.ToTable("data_options");
                });

            modelBuilder.Entity("Blueprint.Api.Data.Models.DataValueEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<string>("CellMetadata")
                        .HasColumnType("text")
                        .HasColumnName("cell_metadata");

                    b.Property<Guid>("CreatedBy")
                        .HasColumnType("uuid")
                        .HasColumnName("created_by");

                    b.Property<Guid>("DataFieldId")
                        .HasColumnType("uuid")
                        .HasColumnName("data_field_id");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("date_created");

                    b.Property<DateTime?>("DateModified")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("date_modified");

                    b.Property<Guid?>("ModifiedBy")
                        .HasColumnType("uuid")
                        .HasColumnName("modified_by");

                    b.Property<Guid>("ScenarioEventId")
                        .HasColumnType("uuid")
                        .HasColumnName("scenario_event_id");

                    b.Property<string>("Value")
                        .HasColumnType("text")
                        .HasColumnName("value");

                    b.HasKey("Id");

                    b.HasIndex("DataFieldId");

                    b.HasIndex("ScenarioEventId");

                    b.ToTable("data_values");
                });

            modelBuilder.Entity("Blueprint.Api.Data.Models.MoveEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<Guid>("CreatedBy")
                        .HasColumnType("uuid")
                        .HasColumnName("created_by");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("date_created");

                    b.Property<DateTime?>("DateModified")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("date_modified");

                    b.Property<string>("Description")
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<Guid?>("ModifiedBy")
                        .HasColumnType("uuid")
                        .HasColumnName("modified_by");

                    b.Property<int>("MoveNumber")
                        .HasColumnType("integer")
                        .HasColumnName("move_number");

                    b.Property<Guid>("MselId")
                        .HasColumnType("uuid")
                        .HasColumnName("msel_id");

                    b.HasKey("Id");

                    b.HasIndex("MselId", "MoveNumber")
                        .IsUnique();

                    b.ToTable("moves");
                });

            modelBuilder.Entity("Blueprint.Api.Data.Models.MselEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<Guid?>("CiteEvaluationId")
                        .HasColumnType("uuid")
                        .HasColumnName("cite_evaluation_id");

                    b.Property<Guid>("CreatedBy")
                        .HasColumnType("uuid")
                        .HasColumnName("created_by");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("date_created");

                    b.Property<DateTime?>("DateModified")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("date_modified");

                    b.Property<string>("Description")
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<Guid?>("GalleryCollectionId")
                        .HasColumnType("uuid")
                        .HasColumnName("gallery_collection_id");

                    b.Property<Guid?>("GalleryExhibitId")
                        .HasColumnType("uuid")
                        .HasColumnName("gallery_exhibit_id");

                    b.Property<string>("HeaderRowMetadata")
                        .HasColumnType("text")
                        .HasColumnName("header_row_metadata");

                    b.Property<bool>("IsTemplate")
                        .HasColumnType("boolean")
                        .HasColumnName("is_template");

                    b.Property<Guid?>("ModifiedBy")
                        .HasColumnType("uuid")
                        .HasColumnName("modified_by");

                    b.Property<string>("Name")
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<Guid?>("PlayerViewId")
                        .HasColumnType("uuid")
                        .HasColumnName("player_view_id");

                    b.Property<int>("Status")
                        .HasColumnType("integer")
                        .HasColumnName("status");

                    b.Property<Guid?>("SteamfitterScenarioId")
                        .HasColumnType("uuid")
                        .HasColumnName("steamfitter_scenario_id");

                    b.Property<bool>("UseCite")
                        .HasColumnType("boolean")
                        .HasColumnName("use_cite");

                    b.Property<bool>("UseGallery")
                        .HasColumnType("boolean")
                        .HasColumnName("use_gallery");

                    b.Property<bool>("UseSteamfitter")
                        .HasColumnType("boolean")
                        .HasColumnName("use_steamfitter");

                    b.HasKey("Id");

                    b.ToTable("msels");
                });

            modelBuilder.Entity("Blueprint.Api.Data.Models.MselTeamEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<Guid>("MselId")
                        .HasColumnType("uuid")
                        .HasColumnName("msel_id");

                    b.Property<Guid>("TeamId")
                        .HasColumnType("uuid")
                        .HasColumnName("team_id");

                    b.HasKey("Id");

                    b.HasIndex("MselId");

                    b.HasIndex("TeamId", "MselId")
                        .IsUnique();

                    b.ToTable("msel_teams");
                });

            modelBuilder.Entity("Blueprint.Api.Data.Models.OrganizationEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<Guid>("CreatedBy")
                        .HasColumnType("uuid")
                        .HasColumnName("created_by");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("date_created");

                    b.Property<DateTime?>("DateModified")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("date_modified");

                    b.Property<string>("Description")
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<bool>("IsTemplate")
                        .HasColumnType("boolean")
                        .HasColumnName("is_template");

                    b.Property<Guid?>("ModifiedBy")
                        .HasColumnType("uuid")
                        .HasColumnName("modified_by");

                    b.Property<Guid?>("MselId")
                        .HasColumnType("uuid")
                        .HasColumnName("msel_id");

                    b.Property<string>("Name")
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<string>("Summary")
                        .HasColumnType("text")
                        .HasColumnName("summary");

                    b.HasKey("Id");

                    b.HasIndex("MselId");

                    b.ToTable("organizations");
                });

            modelBuilder.Entity("Blueprint.Api.Data.Models.PermissionEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<Guid>("CreatedBy")
                        .HasColumnType("uuid")
                        .HasColumnName("created_by");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("date_created");

                    b.Property<DateTime?>("DateModified")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("date_modified");

                    b.Property<string>("Description")
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<string>("Key")
                        .HasColumnType("text")
                        .HasColumnName("key");

                    b.Property<Guid?>("ModifiedBy")
                        .HasColumnType("uuid")
                        .HasColumnName("modified_by");

                    b.Property<bool>("ReadOnly")
                        .HasColumnType("boolean")
                        .HasColumnName("read_only");

                    b.Property<string>("Value")
                        .HasColumnType("text")
                        .HasColumnName("value");

                    b.HasKey("Id");

                    b.HasIndex("Key", "Value")
                        .IsUnique();

                    b.ToTable("permissions");
                });

            modelBuilder.Entity("Blueprint.Api.Data.Models.ScenarioEventEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<Guid?>("AssignedTeamId")
                        .HasColumnType("uuid")
                        .HasColumnName("assigned_team_id");

                    b.Property<Guid>("CreatedBy")
                        .HasColumnType("uuid")
                        .HasColumnName("created_by");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("date_created");

                    b.Property<DateTime?>("DateModified")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("date_modified");

                    b.Property<Guid?>("ModifiedBy")
                        .HasColumnType("uuid")
                        .HasColumnName("modified_by");

                    b.Property<Guid>("MselId")
                        .HasColumnType("uuid")
                        .HasColumnName("msel_id");

                    b.Property<int>("RowIndex")
                        .HasColumnType("integer")
                        .HasColumnName("row_index");

                    b.Property<string>("RowMetadata")
                        .HasColumnType("text")
                        .HasColumnName("row_metadata");

                    b.Property<int>("Status")
                        .HasColumnType("integer")
                        .HasColumnName("status");

                    b.HasKey("Id");

                    b.HasIndex("AssignedTeamId");

                    b.HasIndex("MselId");

                    b.ToTable("scenario_events");
                });

            modelBuilder.Entity("Blueprint.Api.Data.Models.TeamEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<Guid>("CreatedBy")
                        .HasColumnType("uuid")
                        .HasColumnName("created_by");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("date_created");

                    b.Property<DateTime?>("DateModified")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("date_modified");

                    b.Property<Guid?>("ModifiedBy")
                        .HasColumnType("uuid")
                        .HasColumnName("modified_by");

                    b.Property<string>("Name")
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<string>("ShortName")
                        .HasColumnType("text")
                        .HasColumnName("short_name");

                    b.HasKey("Id");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.ToTable("teams");
                });

            modelBuilder.Entity("Blueprint.Api.Data.Models.TeamUserEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<Guid>("TeamId")
                        .HasColumnType("uuid")
                        .HasColumnName("team_id");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.HasKey("Id");

                    b.HasIndex("TeamId");

                    b.HasIndex("UserId", "TeamId")
                        .IsUnique();

                    b.ToTable("team_users");
                });

            modelBuilder.Entity("Blueprint.Api.Data.Models.UserEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<Guid>("CreatedBy")
                        .HasColumnType("uuid")
                        .HasColumnName("created_by");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("date_created");

                    b.Property<DateTime?>("DateModified")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("date_modified");

                    b.Property<Guid?>("ModifiedBy")
                        .HasColumnType("uuid")
                        .HasColumnName("modified_by");

                    b.Property<string>("Name")
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.HasKey("Id");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.ToTable("users");
                });

            modelBuilder.Entity("Blueprint.Api.Data.Models.UserMselRoleEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<Guid>("CreatedBy")
                        .HasColumnType("uuid")
                        .HasColumnName("created_by");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("date_created");

                    b.Property<DateTime?>("DateModified")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("date_modified");

                    b.Property<Guid?>("ModifiedBy")
                        .HasColumnType("uuid")
                        .HasColumnName("modified_by");

                    b.Property<Guid>("MselId")
                        .HasColumnType("uuid")
                        .HasColumnName("msel_id");

                    b.Property<int>("Role")
                        .HasColumnType("integer")
                        .HasColumnName("role");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.HasIndex("MselId", "UserId", "Role")
                        .IsUnique();

                    b.ToTable("user_msel_roles");
                });

            modelBuilder.Entity("Blueprint.Api.Data.Models.UserPermissionEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<Guid>("PermissionId")
                        .HasColumnType("uuid")
                        .HasColumnName("permission_id");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.HasKey("Id");

                    b.HasIndex("PermissionId");

                    b.HasIndex("UserId", "PermissionId")
                        .IsUnique();

                    b.ToTable("user_permissions");
                });

            modelBuilder.Entity("Blueprint.Api.Data.Models.CardEntity", b =>
                {
                    b.HasOne("Blueprint.Api.Data.Models.MselEntity", "Msel")
                        .WithMany("Cards")
                        .HasForeignKey("MselId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Msel");
                });

            modelBuilder.Entity("Blueprint.Api.Data.Models.DataFieldEntity", b =>
                {
                    b.HasOne("Blueprint.Api.Data.Models.MselEntity", "Msel")
                        .WithMany("DataFields")
                        .HasForeignKey("MselId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Msel");
                });

            modelBuilder.Entity("Blueprint.Api.Data.Models.DataOptionEntity", b =>
                {
                    b.HasOne("Blueprint.Api.Data.Models.DataFieldEntity", "DataField")
                        .WithMany("DataOptions")
                        .HasForeignKey("DataFieldId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DataField");
                });

            modelBuilder.Entity("Blueprint.Api.Data.Models.DataValueEntity", b =>
                {
                    b.HasOne("Blueprint.Api.Data.Models.DataFieldEntity", "DataField")
                        .WithMany()
                        .HasForeignKey("DataFieldId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Blueprint.Api.Data.Models.ScenarioEventEntity", "ScenarioEvent")
                        .WithMany("DataValues")
                        .HasForeignKey("ScenarioEventId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DataField");

                    b.Navigation("ScenarioEvent");
                });

            modelBuilder.Entity("Blueprint.Api.Data.Models.MoveEntity", b =>
                {
                    b.HasOne("Blueprint.Api.Data.Models.MselEntity", "Msel")
                        .WithMany("Moves")
                        .HasForeignKey("MselId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Msel");
                });

            modelBuilder.Entity("Blueprint.Api.Data.Models.MselTeamEntity", b =>
                {
                    b.HasOne("Blueprint.Api.Data.Models.MselEntity", "Msel")
                        .WithMany("MselTeams")
                        .HasForeignKey("MselId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Blueprint.Api.Data.Models.TeamEntity", "Team")
                        .WithMany("MselTeams")
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Msel");

                    b.Navigation("Team");
                });

            modelBuilder.Entity("Blueprint.Api.Data.Models.OrganizationEntity", b =>
                {
                    b.HasOne("Blueprint.Api.Data.Models.MselEntity", "Msel")
                        .WithMany("Organizations")
                        .HasForeignKey("MselId");

                    b.Navigation("Msel");
                });

            modelBuilder.Entity("Blueprint.Api.Data.Models.ScenarioEventEntity", b =>
                {
                    b.HasOne("Blueprint.Api.Data.Models.TeamEntity", "AssignedTeam")
                        .WithMany()
                        .HasForeignKey("AssignedTeamId");

                    b.HasOne("Blueprint.Api.Data.Models.MselEntity", "Msel")
                        .WithMany("ScenarioEvents")
                        .HasForeignKey("MselId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AssignedTeam");

                    b.Navigation("Msel");
                });

            modelBuilder.Entity("Blueprint.Api.Data.Models.TeamUserEntity", b =>
                {
                    b.HasOne("Blueprint.Api.Data.Models.TeamEntity", "Team")
                        .WithMany("TeamUsers")
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Blueprint.Api.Data.Models.UserEntity", "User")
                        .WithMany("TeamUsers")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Team");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Blueprint.Api.Data.Models.UserMselRoleEntity", b =>
                {
                    b.HasOne("Blueprint.Api.Data.Models.MselEntity", "Msel")
                        .WithMany("UserMselRoles")
                        .HasForeignKey("MselId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Blueprint.Api.Data.Models.UserEntity", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Msel");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Blueprint.Api.Data.Models.UserPermissionEntity", b =>
                {
                    b.HasOne("Blueprint.Api.Data.Models.PermissionEntity", "Permission")
                        .WithMany("UserPermissions")
                        .HasForeignKey("PermissionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Blueprint.Api.Data.Models.UserEntity", "User")
                        .WithMany("UserPermissions")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Permission");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Blueprint.Api.Data.Models.DataFieldEntity", b =>
                {
                    b.Navigation("DataOptions");
                });

            modelBuilder.Entity("Blueprint.Api.Data.Models.MselEntity", b =>
                {
                    b.Navigation("Cards");

                    b.Navigation("DataFields");

                    b.Navigation("Moves");

                    b.Navigation("MselTeams");

                    b.Navigation("Organizations");

                    b.Navigation("ScenarioEvents");

                    b.Navigation("UserMselRoles");
                });

            modelBuilder.Entity("Blueprint.Api.Data.Models.PermissionEntity", b =>
                {
                    b.Navigation("UserPermissions");
                });

            modelBuilder.Entity("Blueprint.Api.Data.Models.ScenarioEventEntity", b =>
                {
                    b.Navigation("DataValues");
                });

            modelBuilder.Entity("Blueprint.Api.Data.Models.TeamEntity", b =>
                {
                    b.Navigation("MselTeams");

                    b.Navigation("TeamUsers");
                });

            modelBuilder.Entity("Blueprint.Api.Data.Models.UserEntity", b =>
                {
                    b.Navigation("TeamUsers");

                    b.Navigation("UserPermissions");
                });
#pragma warning restore 612, 618
        }
    }
}
