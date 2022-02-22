namespace StreetNameRegistry.Tests.ProjectionTests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoFixture;
    using FluentAssertions;
    using global::AutoFixture;
    using Municipality;
    using Municipality.Events;
    using Projections.Legacy.StreetNameListV2;
    using Xunit;

    public class StreetNameListItemProjectionsV2Tests : StreetNameLegacyProjectionTest<StreetNameListProjectionsV2>
    {
        private readonly Fixture? _fixture;

        public StreetNameListItemProjectionsV2Tests()
        {
            _fixture = new Fixture();
            _fixture.Customize(new InfrastructureCustomization());
            _fixture.Customize(new WithFixedMunicipalityId());
        }

        [Fact]
        public async Task WhenMunicipalityWasImported_ThenMunicipalityWasAdded()
        {
            var municipalityWasImported = _fixture.Create<MunicipalityWasImported>();

            await Sut
                .Given(municipalityWasImported)
                .Then(async ct =>
                {
                    var expectedMunicipality = (await ct.FindAsync<StreetNameListMunicipality>(municipalityWasImported.MunicipalityId));
                    expectedMunicipality.Should().NotBeNull();
                    expectedMunicipality.MunicipalityId.Should().Be(municipalityWasImported.MunicipalityId);
                    expectedMunicipality.NisCode.Should().Be(municipalityWasImported.NisCode);
                });
        }

        [Fact]
        public async Task WhenStreetNameWasProposed_ThenStreetNameWasAdded()
        {
            _fixture.Register(() => new Names(_fixture.CreateMany<StreetNameName>(2).ToList()));
            var streetNameWasProposedV2 = _fixture.Create<StreetNameWasProposedV2>();

            await Sut
                .Given(
                    _fixture.Create<MunicipalityWasImported>(),
                    streetNameWasProposedV2)
                .Then(async ct =>
                {
                    var expectedStreetName = (await ct.FindAsync<StreetNameListItemV2>(streetNameWasProposedV2.PersistentLocalId));
                    expectedStreetName.Should().NotBeNull();
                    expectedStreetName.MunicipalityId.Should().Be(streetNameWasProposedV2.MunicipalityId);
                    expectedStreetName.NisCode.Should().Be(streetNameWasProposedV2.NisCode);
                    expectedStreetName.PersistentLocalId.Should().Be(streetNameWasProposedV2.PersistentLocalId);
                    expectedStreetName.Removed.Should().BeFalse();
                    expectedStreetName.Status.Should().Be(StreetNameStatus.Proposed);
                    expectedStreetName.VersionTimestamp.Should().Be(streetNameWasProposedV2.Provenance.Timestamp);
                    expectedStreetName.NameDutch.Should().Be(DetermineExpectedNameForLanguage(streetNameWasProposedV2.StreetNameNames, Language.Dutch));
                    expectedStreetName.NameFrench.Should().Be(DetermineExpectedNameForLanguage(streetNameWasProposedV2.StreetNameNames, Language.French));
                    expectedStreetName.NameGerman.Should().Be(DetermineExpectedNameForLanguage(streetNameWasProposedV2.StreetNameNames, Language.German));
                    expectedStreetName.NameEnglish.Should().Be(DetermineExpectedNameForLanguage(streetNameWasProposedV2.StreetNameNames, Language.English));
                    expectedStreetName.PrimaryLanguage.Should().Be(null);
                });
        }

        [Fact]
        public async Task WhenMunicipalityNisCodeWasChanged_ThenMunicipalityNisCodeIsUpdatedAndLinkedStreetNamesHaveNewNisCode()
        {
            _fixture.Register(() => new Names(_fixture.CreateMany<StreetNameName>(2).ToList()));
            var streetNameWasProposedV2 = _fixture.Create<StreetNameWasProposedV2>();
            var streetNameWasProposedV2_2 = _fixture.Create<StreetNameWasProposedV2>();
            var municipalityWasImported = _fixture.Create<MunicipalityWasImported>();
            var municipalityNisCodeWasChanged = _fixture.Create<MunicipalityNisCodeWasChanged>();

            await Sut
                .Given(
                    municipalityWasImported,
                    streetNameWasProposedV2,
                    streetNameWasProposedV2_2,
                    municipalityNisCodeWasChanged)
                .Then(async ct =>
                {
                    var expectedMunicipality = (await ct.FindAsync<StreetNameListMunicipality>(municipalityNisCodeWasChanged.MunicipalityId));
                    expectedMunicipality.Should().NotBeNull();
                    expectedMunicipality.NisCode.Should().Be(municipalityNisCodeWasChanged.NisCode);

                    var expectedStreetNames = ct.StreetNameListV2.Where(x =>
                        x.MunicipalityId == municipalityNisCodeWasChanged.MunicipalityId);

                    expectedStreetNames.Select(x => x.NisCode)
                        .Distinct()
                        .Should()
                        .BeEquivalentTo(new List<string> { municipalityNisCodeWasChanged.NisCode });
                });
        }

        private string DetermineExpectedNameForLanguage(IEnumerable<StreetNameName> streetNameNames, Language language)
        {
            return streetNameNames.SingleOrDefault(x => x.Language == language)?.Name;
        }
    }
}
