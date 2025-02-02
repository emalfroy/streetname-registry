namespace StreetNameRegistry.Api.BackOffice.Handlers.Lambda.Requests
{
    using System;
    using System.Collections.Generic;
    using Be.Vlaanderen.Basisregisters.GrAr.Provenance;
    using MediatR;
    using Municipality;

    public class SqsLambdaRequest : IRequest
    {
        public Guid TicketId { get; set; }
        public string MessageGroupId { get; set; }
        public string? IfMatchHeaderValue { get; set; }
        public Provenance Provenance { get; set; }
        public IDictionary<string, object> Metadata { get; set; }

        public MunicipalityId MunicipalityId => MunicipalityId.CreateFor(MessageGroupId);
    }
}
