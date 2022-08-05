namespace StreetNameRegistry.Tests.AggregateTests.SnapshotTests
{
    using System.Collections.Generic;
    using AutoFixture;
    using Be.Vlaanderen.Basisregisters.AggregateSource.Snapshotting;
    using Be.Vlaanderen.Basisregisters.GrAr.Provenance;
    using FluentAssertions;
    using global::AutoFixture;
    using Municipality;
    using Municipality.Commands;
    using Municipality.DataStructures;
    using Municipality.Events;
    using Testing;
    using Xunit;
    using Xunit.Abstractions;

    public class TakeMunicipalitySnapshotTests : StreetNameRegistryTest
    {
        public TakeMunicipalitySnapshotTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
            Fixture.Customize(new InfrastructureCustomization());
            Fixture.Customize(new WithFixedMunicipalityId());
            Fixture.Customize(new WithFixedPersistentLocalId());
        }

        [Fact]
        public void MunicipalityWasImportedIsSavedInSnapshot()
        {
            var aggregate = new MunicipalityFactory(IntervalStrategy.Default).Create();

            var municipalityWasImported = Fixture.Create<MunicipalityWasImported>();

            aggregate.Initialize(new List<object>
            {
                municipalityWasImported
            });

            var snapshot = aggregate.TakeSnapshot();

            snapshot.Should().BeOfType<MunicipalitySnapshot>();
            var municipalitySnapshot = (MunicipalitySnapshot)snapshot;

            municipalitySnapshot.NisCode.Should().Be(municipalityWasImported.NisCode);
            municipalitySnapshot.MunicipalityId.Should().Be(municipalityWasImported.MunicipalityId);
            municipalitySnapshot.MunicipalityStatus.Should().BeNullOrEmpty();
            municipalitySnapshot.StreetNames.Should().BeEmpty();
            municipalitySnapshot.OfficialLanguages.Should().BeEmpty();
            municipalitySnapshot.FacilityLanguages.Should().BeEmpty();
        }

        [Fact]
        public void MunicipalityNisCodeWasChangedIsSavedInSnapshot()
        {
            var aggregate = new MunicipalityFactory(IntervalStrategy.Default).Create();

            var municipalityNisCodeWasChanged = Fixture.Create<MunicipalityNisCodeWasChanged>();
            aggregate.Initialize(new List<object>
            {
                Fixture.Create<MunicipalityWasImported>(),
                municipalityNisCodeWasChanged
            });

            var snapshot = aggregate.TakeSnapshot();

            snapshot.Should().BeOfType<MunicipalitySnapshot>();
            var municipalitySnapshot = (MunicipalitySnapshot)snapshot;

            municipalitySnapshot.NisCode.Should().Be(municipalityNisCodeWasChanged.NisCode);
        }

        [Fact]
        public void MunicipalityBecameCurrentIsSavedInSnapshot()
        {
            var aggregate = new MunicipalityFactory(IntervalStrategy.Default).Create();

            aggregate.Initialize(new List<object>
            {
                Fixture.Create<MunicipalityWasImported>(),
                Fixture.Create<MunicipalityBecameCurrent>()
            });

            var snapshot = aggregate.TakeSnapshot();

            snapshot.Should().BeOfType<MunicipalitySnapshot>();
            var municipalitySnapshot = (MunicipalitySnapshot)snapshot;

            municipalitySnapshot.MunicipalityStatus.Should().Be(MunicipalityStatus.Current);
        }

        [Fact]
        public void MunicipalityWasCorrectedToCurrentIsSavedInSnapshot()
        {
            var aggregate = new MunicipalityFactory(IntervalStrategy.Default).Create();

            aggregate.Initialize(new List<object>
            {
                Fixture.Create<MunicipalityWasImported>(),
                Fixture.Create<MunicipalityWasRetired>(),
                Fixture.Create<MunicipalityWasCorrectedToCurrent>()
            });

            var snapshot = aggregate.TakeSnapshot();

            snapshot.Should().BeOfType<MunicipalitySnapshot>();
            var municipalitySnapshot = (MunicipalitySnapshot)snapshot;

            municipalitySnapshot.MunicipalityStatus.Should().Be(MunicipalityStatus.Current);
        }

        [Fact]
        public void MunicipalityWasRetiredIsSavedInSnapshot()
        {
            var aggregate = new MunicipalityFactory(IntervalStrategy.Default).Create();

            aggregate.Initialize(new List<object>
            {
                Fixture.Create<MunicipalityWasImported>(),
                Fixture.Create<MunicipalityWasRetired>()
            });

            var snapshot = aggregate.TakeSnapshot();

            snapshot.Should().BeOfType<MunicipalitySnapshot>();
            var municipalitySnapshot = (MunicipalitySnapshot)snapshot;

            municipalitySnapshot.MunicipalityStatus.Should().Be(MunicipalityStatus.Retired);
        }

        [Fact]
        public void MunicipalityWasCorrectedToRetiredIsSavedInSnapshot()
        {
            var aggregate = new MunicipalityFactory(IntervalStrategy.Default).Create();

            aggregate.Initialize(new List<object>
            {
                Fixture.Create<MunicipalityWasImported>(),
                Fixture.Create<MunicipalityBecameCurrent>(),
                Fixture.Create<MunicipalityWasCorrectedToRetired>()
            });

            var snapshot = aggregate.TakeSnapshot();

            snapshot.Should().BeOfType<MunicipalitySnapshot>();
            var municipalitySnapshot = (MunicipalitySnapshot)snapshot;

            municipalitySnapshot.MunicipalityStatus.Should().Be(MunicipalityStatus.Retired);
        }

        [Fact]
        public void MunicipalityOfficialLanguageWasAddedIsSavedInSnapshot()
        {
            var aggregate = new MunicipalityFactory(IntervalStrategy.Default).Create();

            var municipalityDutchOfficialLanguageWasAdded = Fixture.Create<AddOfficialLanguageToMunicipality>().WithLanguage(Language.Dutch).ToEvent();
            var municipalityFrenchOfficialLanguageWasAdded = Fixture.Create<AddOfficialLanguageToMunicipality>().WithLanguage(Language.French).ToEvent();
            var municipalityGermanOfficialLanguageWasAdded = Fixture.Create<AddOfficialLanguageToMunicipality>().WithLanguage(Language.German).ToEvent();
            var municipalityEnglishOfficialLanguageWasAdded = Fixture.Create<AddOfficialLanguageToMunicipality>().WithLanguage(Language.English).ToEvent();

            aggregate.Initialize(new List<object>
            {
                Fixture.Create<MunicipalityWasImported>(),
                municipalityDutchOfficialLanguageWasAdded,
                municipalityFrenchOfficialLanguageWasAdded,
                municipalityGermanOfficialLanguageWasAdded,
                municipalityEnglishOfficialLanguageWasAdded
            });

            var snapshot = aggregate.TakeSnapshot();

            snapshot.Should().BeOfType<MunicipalitySnapshot>();
            var municipalitySnapshot = (MunicipalitySnapshot)snapshot;

            municipalitySnapshot.OfficialLanguages.Should().BeEquivalentTo(new List<Language>
            {
                Language.Dutch, Language.French, Language.German, Language.English
            });
        }

        [Theory]
        [InlineData(Language.Dutch)]
        [InlineData(Language.French)]
        [InlineData(Language.German)]
        [InlineData(Language.English)]
        public void MunicipalityOfficialLanguageWasRemovedIsSavedInSnapshot(Language languageToRemove)
        {
            var aggregate = new MunicipalityFactory(IntervalStrategy.Default).Create();

            var municipalityDutchOfficialLanguageWasAdded = Fixture.Create<AddOfficialLanguageToMunicipality>().WithLanguage(Language.Dutch).ToEvent();
            var municipalityFrenchOfficialLanguageWasAdded = Fixture.Create<AddOfficialLanguageToMunicipality>().WithLanguage(Language.French).ToEvent();
            var municipalityGermanOfficialLanguageWasAdded = Fixture.Create<AddOfficialLanguageToMunicipality>().WithLanguage(Language.German).ToEvent();
            var municipalityEnglishOfficialLanguageWasAdded = Fixture.Create<AddOfficialLanguageToMunicipality>().WithLanguage(Language.English).ToEvent();
            var municipalityOfficialLanguageWasRemoved = Fixture.Create<RemoveOfficialLanguageFromMunicipality>().WithLanguage(languageToRemove).ToEvent();

            aggregate.Initialize(new List<object>
            {
                Fixture.Create<MunicipalityWasImported>(),
                municipalityDutchOfficialLanguageWasAdded,
                municipalityFrenchOfficialLanguageWasAdded,
                municipalityGermanOfficialLanguageWasAdded,
                municipalityEnglishOfficialLanguageWasAdded,
                municipalityOfficialLanguageWasRemoved
            });

            var snapshot = aggregate.TakeSnapshot();

            snapshot.Should().BeOfType<MunicipalitySnapshot>();
            var municipalitySnapshot = (MunicipalitySnapshot)snapshot;

            var expectedLanguages = new List<Language>
            {
                Language.Dutch, Language.French, Language.German, Language.English
            };
            expectedLanguages.Remove(languageToRemove);
            municipalitySnapshot.OfficialLanguages.Should().BeEquivalentTo(expectedLanguages);
        }

        [Fact]
        public void MunicipalityFacilityLanguageWasAddedIsSavedInSnapshot()
        {
            var aggregate = new MunicipalityFactory(IntervalStrategy.Default).Create();

            var municipalityDutchFacilityLanguageWasAdded = Fixture.Create<AddFacilityLanguageToMunicipality>().WithLanguage(Language.Dutch).ToEvent();
            var municipalityFrenchFacilityLanguageWasAdded = Fixture.Create<AddFacilityLanguageToMunicipality>().WithLanguage(Language.French).ToEvent();
            var municipalityGermanFacilityLanguageWasAdded = Fixture.Create<AddFacilityLanguageToMunicipality>().WithLanguage(Language.German).ToEvent();
            var municipalityEnglishFacilityLanguageWasAdded = Fixture.Create<AddFacilityLanguageToMunicipality>().WithLanguage(Language.English).ToEvent();

            aggregate.Initialize(new List<object>
            {
                Fixture.Create<MunicipalityWasImported>(),
                municipalityDutchFacilityLanguageWasAdded,
                municipalityFrenchFacilityLanguageWasAdded,
                municipalityGermanFacilityLanguageWasAdded,
                municipalityEnglishFacilityLanguageWasAdded
            });

            var snapshot = aggregate.TakeSnapshot();

            snapshot.Should().BeOfType<MunicipalitySnapshot>();
            var municipalitySnapshot = (MunicipalitySnapshot)snapshot;

            municipalitySnapshot.FacilityLanguages.Should().BeEquivalentTo(new List<Language>
            {
                Language.Dutch, Language.French, Language.German, Language.English
            });
        }

        [Theory]
        [InlineData(Language.Dutch)]
        [InlineData(Language.French)]
        [InlineData(Language.German)]
        [InlineData(Language.English)]
        public void MunicipalityFacilityLanguageWasRemovedIsSavedInSnapshot(Language languageToRemove)
        {
            var aggregate = new MunicipalityFactory(IntervalStrategy.Default).Create();

            var municipalityDutchFacilityLanguageWasAdded = Fixture.Create<AddFacilityLanguageToMunicipality>().WithLanguage(Language.Dutch).ToEvent();
            var municipalityFrenchFacilityLanguageWasAdded = Fixture.Create<AddFacilityLanguageToMunicipality>().WithLanguage(Language.French).ToEvent();
            var municipalityGermanFacilityLanguageWasAdded = Fixture.Create<AddFacilityLanguageToMunicipality>().WithLanguage(Language.German).ToEvent();
            var municipalityEnglishFacilityLanguageWasAdded = Fixture.Create<AddFacilityLanguageToMunicipality>().WithLanguage(Language.English).ToEvent();
            var municipalityFacilityLanguageWasRemoved = Fixture.Create<RemoveFacilityLanguageFromMunicipality>().WithLanguage(languageToRemove).ToEvent();

            aggregate.Initialize(new List<object>
            {
                Fixture.Create<MunicipalityWasImported>(),
                municipalityDutchFacilityLanguageWasAdded,
                municipalityFrenchFacilityLanguageWasAdded,
                municipalityGermanFacilityLanguageWasAdded,
                municipalityEnglishFacilityLanguageWasAdded,
                municipalityFacilityLanguageWasRemoved
            });

            var snapshot = aggregate.TakeSnapshot();

            snapshot.Should().BeOfType<MunicipalitySnapshot>();
            var municipalitySnapshot = (MunicipalitySnapshot)snapshot;

            var expectedLanguages = new List<Language>
            {
                Language.Dutch, Language.French, Language.German, Language.English
            };
            expectedLanguages.Remove(languageToRemove);
            municipalitySnapshot.FacilityLanguages.Should().BeEquivalentTo(expectedLanguages);
        }

        [Fact]
        public void StreetNameWasMigratedToMunicipalityIsSavedInSnapshot()
        {
            var aggregate = new MunicipalityFactory(IntervalStrategy.Default).Create();

            var streetNameWasMigratedToMunicipality = Fixture.Create<StreetNameWasMigratedToMunicipality>();
            ((ISetProvenance)streetNameWasMigratedToMunicipality).SetProvenance(Fixture.Create<Provenance>());
            aggregate.Initialize(new List<object>
            {
                Fixture.Create<MunicipalityWasImported>(),
                streetNameWasMigratedToMunicipality
            });

            var snapshot = aggregate.TakeSnapshot();

            snapshot.Should().BeOfType<MunicipalitySnapshot>();
            var municipalitySnapshot = (MunicipalitySnapshot)snapshot;

            municipalitySnapshot.StreetNames.Should().BeEquivalentTo(new List<StreetNameData>
            {
                new StreetNameData(
                    new PersistentLocalId(streetNameWasMigratedToMunicipality.PersistentLocalId),
                    streetNameWasMigratedToMunicipality.Status,
                    new Names(streetNameWasMigratedToMunicipality.Names),
                    new HomonymAdditions(streetNameWasMigratedToMunicipality.HomonymAdditions),
                    streetNameWasMigratedToMunicipality.IsRemoved,
                    new StreetNameId(streetNameWasMigratedToMunicipality.StreetNameId),
                    streetNameWasMigratedToMunicipality.GetHash(),
                    streetNameWasMigratedToMunicipality.Provenance)
            });
        }

        [Fact]
        public void StreetNameWasProposedV2IsSavedInSnapshot()
        {
            var aggregate = new MunicipalityFactory(IntervalStrategy.Default).Create();

            var streetNameWasProposedV2 = Fixture.Create<StreetNameWasProposedV2>();
            ((ISetProvenance)streetNameWasProposedV2).SetProvenance(Fixture.Create<Provenance>());
            aggregate.Initialize(new List<object>
            {
                Fixture.Create<MunicipalityWasImported>(),
                streetNameWasProposedV2
            });

            var snapshot = aggregate.TakeSnapshot();

            snapshot.Should().BeOfType<MunicipalitySnapshot>();
            var municipalitySnapshot = (MunicipalitySnapshot)snapshot;

            municipalitySnapshot.StreetNames.Should().BeEquivalentTo(new List<StreetNameData>
            {
                new StreetNameData(
                    new PersistentLocalId(streetNameWasProposedV2.PersistentLocalId),
                    StreetNameStatus.Proposed,
                    new Names(streetNameWasProposedV2.StreetNameNames),
                    new HomonymAdditions(),
                    false,
                    null,
                    streetNameWasProposedV2.GetHash(),
                    streetNameWasProposedV2.Provenance)
            });
        }

        [Fact]
        public void StreetNameWasApprovedIsSavedInSnapshot()
        {
            var aggregate = new MunicipalityFactory(IntervalStrategy.Default).Create();

            var streetNameWasProposedV2 = Fixture.Create<StreetNameWasProposedV2>();
            var streetNameWasApproved = Fixture.Create<StreetNameWasApproved>();
            ((ISetProvenance)streetNameWasApproved).SetProvenance(Fixture.Create<Provenance>());

            aggregate.Initialize(new List<object>
            {
                Fixture.Create<MunicipalityWasImported>(),
                streetNameWasProposedV2,
                streetNameWasApproved
            });

            var snapshot = aggregate.TakeSnapshot();

            snapshot.Should().BeOfType<MunicipalitySnapshot>();
            var municipalitySnapshot = (MunicipalitySnapshot)snapshot;

            municipalitySnapshot.StreetNames.Should().BeEquivalentTo(new List<StreetNameData>
            {
                new StreetNameData(
                    new PersistentLocalId(streetNameWasApproved.PersistentLocalId),
                    StreetNameStatus.Current,
                    new Names(streetNameWasProposedV2.StreetNameNames),
                    new HomonymAdditions(),
                    false,
                    null,
                    streetNameWasApproved.GetHash(),
                    streetNameWasApproved.Provenance)
            });
        }

        [Fact]
        public void StreetNameWasRejectedIsSavedInSnapshot()
        {
            var aggregate = new MunicipalityFactory(IntervalStrategy.Default).Create();

            var streetNameWasProposedV2 = Fixture.Create<StreetNameWasProposedV2>();
            var streetNameWasRejected = Fixture.Create<StreetNameWasRejected>();
            ((ISetProvenance)streetNameWasRejected).SetProvenance(Fixture.Create<Provenance>());

            aggregate.Initialize(new List<object>
            {
                Fixture.Create<MunicipalityWasImported>(),
                streetNameWasProposedV2,
                streetNameWasRejected
            });

            var snapshot = aggregate.TakeSnapshot();

            snapshot.Should().BeOfType<MunicipalitySnapshot>();
            var municipalitySnapshot = (MunicipalitySnapshot)snapshot;

            municipalitySnapshot.StreetNames.Should().BeEquivalentTo(new List<StreetNameData>
            {
                new StreetNameData(
                    new PersistentLocalId(streetNameWasRejected.PersistentLocalId),
                    StreetNameStatus.Rejected,
                    new Names(streetNameWasProposedV2.StreetNameNames),
                    new HomonymAdditions(),
                    false,
                    null,
                    streetNameWasRejected.GetHash(),
                    streetNameWasRejected.Provenance)
            });
        }

        [Fact]
        public void StreetNameWasRetiredIsSavedInSnapshot()
        {
            var aggregate = new MunicipalityFactory(IntervalStrategy.Default).Create();

            var streetNameWasProposedV2 = Fixture.Create<StreetNameWasProposedV2>();
            var streetNameWasApproved = Fixture.Create<StreetNameWasApproved>();
            var streetNameWasRetiredV2 = Fixture.Create<StreetNameWasRetiredV2>();
            ((ISetProvenance)streetNameWasRetiredV2).SetProvenance(Fixture.Create<Provenance>());

            aggregate.Initialize(new List<object>
            {
                Fixture.Create<MunicipalityWasImported>(),
                streetNameWasProposedV2,
                streetNameWasApproved,
                streetNameWasRetiredV2
            });

            var snapshot = aggregate.TakeSnapshot();

            snapshot.Should().BeOfType<MunicipalitySnapshot>();
            var municipalitySnapshot = (MunicipalitySnapshot)snapshot;

            municipalitySnapshot.StreetNames.Should().BeEquivalentTo(new List<StreetNameData>
            {
                new StreetNameData(
                    new PersistentLocalId(streetNameWasRetiredV2.PersistentLocalId),
                    StreetNameStatus.Retired,
                    new Names(streetNameWasProposedV2.StreetNameNames),
                    new HomonymAdditions(),
                    false,
                    null,
                    streetNameWasRetiredV2.GetHash(),
                    streetNameWasRetiredV2.Provenance)
            });
        }
    }
}
