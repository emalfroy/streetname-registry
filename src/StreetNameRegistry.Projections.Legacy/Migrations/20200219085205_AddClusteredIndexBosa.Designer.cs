﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using StreetNameRegistry.Projections.Legacy;

namespace StreetNameRegistry.Projections.Legacy.Migrations
{
    [DbContext(typeof(LegacyContext))]
    [Migration("20200219085205_AddClusteredIndexBosa")]
    partial class AddClusteredIndexBosa
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Be.Vlaanderen.Basisregisters.ProjectionHandling.Runner.ProjectionStates.ProjectionStateItem", b =>
                {
                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("DesiredState")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset?>("DesiredStateChangedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<long>("Position")
                        .HasColumnType("bigint");

                    b.HasKey("Name")
                        .HasAnnotation("SqlServer:Clustered", true);

                    b.ToTable("ProjectionStates","StreetNameRegistryLegacy");
                });

            modelBuilder.Entity("StreetNameRegistry.Projections.Legacy.StreetNameDetail.StreetNameDetail", b =>
                {
                    b.Property<Guid>("StreetNameId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("Complete")
                        .HasColumnType("bit");

                    b.Property<string>("HomonymAdditionDutch")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("HomonymAdditionEnglish")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("HomonymAdditionFrench")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("HomonymAdditionGerman")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NameDutch")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NameEnglish")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NameFrench")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NameGerman")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NisCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("PersistentLocalId")
                        .HasColumnType("int");

                    b.Property<bool>("Removed")
                        .HasColumnType("bit");

                    b.Property<int?>("Status")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset>("VersionTimestampAsDateTimeOffset")
                        .HasColumnName("VersionTimestamp")
                        .HasColumnType("datetimeoffset");

                    b.HasKey("StreetNameId")
                        .HasAnnotation("SqlServer:Clustered", false);

                    b.HasIndex("PersistentLocalId")
                        .HasAnnotation("SqlServer:Clustered", true);

                    b.HasIndex("Removed");

                    b.ToTable("StreetNameDetails","StreetNameRegistryLegacy");
                });

            modelBuilder.Entity("StreetNameRegistry.Projections.Legacy.StreetNameList.StreetNameListItem", b =>
                {
                    b.Property<Guid>("StreetNameId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("Complete")
                        .HasColumnType("bit");

                    b.Property<string>("HomonymAdditionDutch")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("HomonymAdditionEnglish")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("HomonymAdditionFrench")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("HomonymAdditionGerman")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NameDutch")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NameEnglish")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NameFrench")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NameGerman")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NisCode")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int?>("PersistentLocalId")
                        .HasColumnType("int");

                    b.Property<int?>("PrimaryLanguage")
                        .HasColumnType("int");

                    b.Property<bool>("Removed")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset>("VersionTimestampAsDateTimeOffset")
                        .HasColumnName("VersionTimestamp")
                        .HasColumnType("datetimeoffset");

                    b.HasKey("StreetNameId")
                        .HasAnnotation("SqlServer:Clustered", false);

                    b.HasIndex("NisCode");

                    b.HasIndex("PersistentLocalId")
                        .HasAnnotation("SqlServer:Clustered", true);

                    b.HasIndex("Complete", "Removed");

                    b.HasIndex("Complete", "Removed", "PersistentLocalId");

                    b.ToTable("StreetNameList","StreetNameRegistryLegacy");
                });

            modelBuilder.Entity("StreetNameRegistry.Projections.Legacy.StreetNameName.StreetNameName", b =>
                {
                    b.Property<Guid>("StreetNameId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("Complete")
                        .HasColumnType("bit");

                    b.Property<bool>("IsFlemishRegion")
                        .HasColumnType("bit");

                    b.Property<string>("NameDutch")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("NameDutchSearch")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("NameEnglish")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("NameEnglishSearch")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("NameFrench")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("NameFrenchSearch")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("NameGerman")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("NameGermanSearch")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("NisCode")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("PersistentLocalId")
                        .HasColumnType("int");

                    b.Property<bool>("Removed")
                        .HasColumnType("bit");

                    b.Property<int?>("Status")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset>("VersionTimestampAsDateTimeOffset")
                        .HasColumnName("VersionTimestamp")
                        .HasColumnType("datetimeoffset");

                    b.HasKey("StreetNameId")
                        .HasAnnotation("SqlServer:Clustered", false);

                    b.HasIndex("NameDutch");

                    b.HasIndex("NameDutchSearch");

                    b.HasIndex("NameEnglish");

                    b.HasIndex("NameEnglishSearch");

                    b.HasIndex("NameFrench");

                    b.HasIndex("NameFrenchSearch");

                    b.HasIndex("NameGerman");

                    b.HasIndex("NameGermanSearch");

                    b.HasIndex("NisCode");

                    b.HasIndex("PersistentLocalId")
                        .HasAnnotation("SqlServer:Clustered", true);

                    b.HasIndex("Status");

                    b.HasIndex("VersionTimestampAsDateTimeOffset");

                    b.HasIndex("Removed", "IsFlemishRegion", "Complete");

                    b.ToTable("StreetNameName","StreetNameRegistryLegacy");
                });

            modelBuilder.Entity("StreetNameRegistry.Projections.Legacy.StreetNameSyndication.StreetNameSyndicationItem", b =>
                {
                    b.Property<long>("Position")
                        .HasColumnType("bigint");

                    b.Property<int?>("Application")
                        .HasColumnType("int");

                    b.Property<string>("ChangeType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EventDataAsXml")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("HomonymAdditionDutch")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("HomonymAdditionEnglish")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("HomonymAdditionFrench")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("HomonymAdditionGerman")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsComplete")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset>("LastChangedOnAsDateTimeOffset")
                        .HasColumnName("LastChangedOn")
                        .HasColumnType("datetimeoffset");

                    b.Property<int?>("Modification")
                        .HasColumnType("int");

                    b.Property<string>("NameDutch")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NameEnglish")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NameFrench")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NameGerman")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NisCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Operator")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Organisation")
                        .HasColumnType("int");

                    b.Property<int?>("PersistentLocalId")
                        .HasColumnType("int");

                    b.Property<string>("Reason")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("RecordCreatedAtAsDateTimeOffset")
                        .HasColumnName("RecordCreatedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<int?>("Status")
                        .HasColumnType("int");

                    b.Property<Guid>("StreetNameId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Position")
                        .HasAnnotation("SqlServer:Clustered", true);

                    b.HasIndex("Position")
                        .HasName("CI_StreetNameSyndication_Position")
                        .HasAnnotation("SqlServer:ColumnStoreIndex", "");

                    b.HasIndex("StreetNameId");

                    b.ToTable("StreetNameSyndication","StreetNameRegistryLegacy");
                });

            modelBuilder.Entity("StreetNameRegistry.Projections.Legacy.StreetNameVersion.StreetNameVersion", b =>
                {
                    b.Property<Guid>("StreetNameId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<long>("Position")
                        .HasColumnType("bigint");

                    b.Property<int?>("Application")
                        .HasColumnType("int");

                    b.Property<bool>("Complete")
                        .HasColumnType("bit");

                    b.Property<string>("HomonymAdditionDutch")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("HomonymAdditionEnglish")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("HomonymAdditionFrench")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("HomonymAdditionGerman")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Modification")
                        .HasColumnType("int");

                    b.Property<string>("NameDutch")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NameEnglish")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NameFrench")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NameGerman")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NisCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Operator")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Organisation")
                        .HasColumnType("int");

                    b.Property<int?>("PersistentLocalId")
                        .HasColumnType("int");

                    b.Property<string>("Reason")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Removed")
                        .HasColumnType("bit");

                    b.Property<int?>("Status")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset?>("VersionTimestampAsDateTimeOffset")
                        .HasColumnName("VersionTimestamp")
                        .HasColumnType("datetimeoffset");

                    b.HasKey("StreetNameId", "Position")
                        .HasAnnotation("SqlServer:Clustered", false);

                    b.HasIndex("PersistentLocalId")
                        .HasAnnotation("SqlServer:Clustered", true);

                    b.HasIndex("Removed");

                    b.ToTable("StreetNameVersions","StreetNameRegistryLegacy");
                });
#pragma warning restore 612, 618
        }
    }
}
