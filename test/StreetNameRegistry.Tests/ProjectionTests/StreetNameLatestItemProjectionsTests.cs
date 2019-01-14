namespace StreetNameRegistry.Tests.ProjectionTests
{
    using System.Threading.Tasks;
    using FluentAssertions;
    using Generate;
    using Projections.Legacy.StreetNameDetail;
    using Testing;
    using Xunit;
    using Xunit.Abstractions;

    public class StreetNameLatestItemProjectionsTests : StreetNameRegistryProjectionTest<StreetNameDetailProjections>
    {
        public StreetNameLatestItemProjectionsTests(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task StreetNameBecameCompleteSetsCompleted()
        {
            var id = Arrange(Produce.Guid());
            var provenance = Arrange(Generate.Provenance);

            await Given(Generate.EventsFor.StreetName(id))
                .Project(Generate.StreetNameBecameComplete
                    .Select(e => e.WithId(id).WithProvenance(provenance)))
                .Then(async ct => {
                    var entity = await ct.FindAsync<StreetNameDetail>(id);
                    entity.Should().NotBeNull();
                    entity.Complete.Should().BeTrue();
                });
        }

        [Fact]
        public async Task StreetNameWasRemovedRemovesRecord()
        {
            var id = Arrange(Produce.Guid());
            var provenance = Arrange(Generate.Provenance);

            await Given(Generate.EventsFor.StreetName(id))
                .Project(Generate.StreetNameWasRemoved
                    .Select(e => e.WithId(id).WithProvenance(provenance)))
                .Then(async ct =>
                {
                    var entity = await ct.FindAsync<StreetNameDetail>(id);
                    entity.Should().NotBeNull();
                    entity.Removed.Should().BeTrue();
                });
        }

        [Fact]
        public async Task StreetNameWasRetiredSetsStatus()
        {
            var id = Arrange(Produce.Guid());
            var provenance = Arrange(Generate.Provenance);

            await Given(Generate.EventsFor.StreetName(id))
                .Project(Generate.StreetNameWasRetired
                    .Select(e => e.WithId(id).WithProvenance(provenance)))
                .Then(async ct =>
                {
                    var entity = await ct.FindAsync<StreetNameDetail>(id);
                    entity.Should().NotBeNull();
                    entity.Status.Should().Be(StreetNameStatus.Retired);
                });
        }

        [Fact]
        public async Task StreetNameOsloIdWasAssignedAssignsOsloId()
        {
            var id = Arrange(Produce.Guid());
            var osloId = Arrange(Produce.Integer(10000, 15000));

            await Given(Generate.EventsFor.StreetName(id))
                .Project(Generate.StreetNameOsloIdWasAssigned
                    .Select(e => e.WithId(id)
                        .WithOsloId(osloId)))
                .Then(async ct =>
                {
                    var entity = await ct.FindAsync<StreetNameDetail>(id);
                    entity.Should().NotBeNull();
                    entity.OsloId.Should().Be(osloId);
                });
        }
    }
}
