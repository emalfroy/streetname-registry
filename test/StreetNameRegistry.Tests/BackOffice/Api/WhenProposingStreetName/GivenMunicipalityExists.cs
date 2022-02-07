namespace StreetNameRegistry.Tests.BackOffice.Api.WhenProposingStreetName
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Be.Vlaanderen.Basisregisters.Api.ETag;
    using Be.Vlaanderen.Basisregisters.CommandHandling.Idempotency;
    using Be.Vlaanderen.Basisregisters.GrAr.Legacy;
    using Be.Vlaanderen.Basisregisters.GrAr.Provenance;
    using FluentAssertions;
    using FluentValidation;
    using global::AutoFixture;
    using Infrastructure;
    using Moq;
    using StreetName;
    using StreetName.Commands.Municipality;
    using StreetNameRegistry.Api.BackOffice.StreetName;
    using StreetNameRegistry.Api.BackOffice.StreetName.Requests;
    using Testing;
    using Xunit;
    using Xunit.Abstractions;

    public class GivenMunicipalityExists : StreetNameRegistryBackOfficeTest
    {
        private readonly TestConsumerContext _consumerContext;
        private readonly StreetNameController _controller;
        private readonly Fixture _fixture;
        private readonly IdempotencyContext _idempotencyContext;

        public GivenMunicipalityExists(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
            _fixture = new Fixture();
            _controller = CreateApiBusControllerWithUser<StreetNameController>("John Doe");
            _idempotencyContext = new FakeIdempotencyContextFactory().CreateDbContext(Array.Empty<string>());
            _consumerContext = new FakeConsumerContextFactory().CreateDbContext(Array.Empty<string>());
        }

        [Fact]
        public async Task ThenTheStreetNameIsProposed()
        {
            const int expectedLocation = 5;

            //Arrange
            var municipalityLatestItem = _consumerContext.AddMunicipalityLatestItemFixture();
            var mockPersistentLocalIdGenerator = new Mock<IPersistentLocalIdGenerator>();
            mockPersistentLocalIdGenerator
                .Setup(x => x.GenerateNextPersistentLocalId())
                .Returns(new PersistentLocalId(expectedLocation));

            var importMunicipality = new ImportMunicipality(
                new MunicipalityId(municipalityLatestItem.MunicipalityId),
                new NisCode(municipalityLatestItem.NisCode),
                _fixture.Create<Provenance>());
            DispatchArrangeCommand(importMunicipality);

            var body = new StreetNameProposeRequest
            {
                GemeenteId = $"https://data.vlaanderen.be/id/gemeente/{municipalityLatestItem.NisCode}",
                Straatnamen = new Dictionary<Taal, string>
                {
                    { Taal.NL, "Rodekruisstraat" },
                    { Taal.FR, "Rue de la Croix-Rouge" }
                }
            };

            //Act
            var result = (CreatedWithLastObservedPositionAsETagResult)await _controller.Propose(ResponseOptions,
                _idempotencyContext, _consumerContext, mockPersistentLocalIdGenerator.Object,
                new StreetNameProposeRequestValidator(), body);

            //Assert
            var expectedPosition = 1;
            result.Location.Should().Be(string.Format(DetailUrl, expectedLocation));
            result.LastObservedPositionAsETag.Should().Be(expectedPosition.ToString());
        }

        [Fact]
        public async Task WhenStraatnamenIsNull_ThenBadRequestIsExpected()
        {
            var mockPersistentLocalIdGenerator = new Mock<IPersistentLocalIdGenerator>();
            mockPersistentLocalIdGenerator
                .Setup(x => x.GenerateNextPersistentLocalId())
                .Returns(new PersistentLocalId(1));

            var body = new StreetNameProposeRequest
            {
                GemeenteId = "https://data.vlaanderen.be/id/gemeente/11001",
                Straatnamen = new Dictionary<Taal, string>
                {
                    { Taal.NL, "" }
                }
            };

            //Act
            Func<Task> act = async () => await _controller.Propose(ResponseOptions, _idempotencyContext,
                _consumerContext, mockPersistentLocalIdGenerator.Object, new StreetNameProposeRequestValidator(), body);

            //Assert
            await act.Should().ThrowAsync<ValidationException>();
        }

        [Fact]
        public async Task WhenOneOfStraatnamenIsNull_ThenBadRequestIsExpected()
        {
            var mockPersistentLocalIdGenerator = new Mock<IPersistentLocalIdGenerator>();
            mockPersistentLocalIdGenerator
                .Setup(x => x.GenerateNextPersistentLocalId())
                .Returns(new PersistentLocalId(1));

            var body = new StreetNameProposeRequest
            {
                GemeenteId = "https://data.vlaanderen.be/id/gemeente/11001",
                Straatnamen = new Dictionary<Taal, string>
                {
                    { Taal.NL, "" },
                    { Taal.EN, "abc" }
                }
            };

            //Act
            Func<Task> act = async () => await _controller.Propose(ResponseOptions, _idempotencyContext,
                _consumerContext, mockPersistentLocalIdGenerator.Object, new StreetNameProposeRequestValidator(), body);

            //Assert
            await act.Should().ThrowAsync<ValidationException>();
        }

        [Fact]
        public async Task WhenOneOfStraatnamenHasExceededMaxLength_ThenBadRequestIsExpected()
        {
            var mockPersistentLocalIdGenerator = new Mock<IPersistentLocalIdGenerator>();
            mockPersistentLocalIdGenerator
                .Setup(x => x.GenerateNextPersistentLocalId())
                .Returns(new PersistentLocalId(1));

            var body = new StreetNameProposeRequest
            {
                GemeenteId = "https://data.vlaanderen.be/id/gemeente/11001",
                Straatnamen = new Dictionary<Taal, string>
                {
                    { Taal.NL, "Boulevard Louis Edelhart Lodewijk van Groothertogdom Luxemburg" },
                    { Taal.EN, "abc" }
                }
            };

            //Act
            Func<Task> act = async () => await _controller.Propose(ResponseOptions, _idempotencyContext,
                _consumerContext, mockPersistentLocalIdGenerator.Object, new StreetNameProposeRequestValidator(), body);

            //Assert
            await act.Should().ThrowAsync<ValidationException>();
        }
    }
}
