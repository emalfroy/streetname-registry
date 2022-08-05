namespace StreetNameRegistry.Municipality.Events
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Be.Vlaanderen.Basisregisters.EventHandling;
    using Be.Vlaanderen.Basisregisters.GrAr.Common;
    using Be.Vlaanderen.Basisregisters.GrAr.Provenance;
    using Newtonsoft.Json;
    using StreetNameRegistry.Municipality;

    [EventTags(EventTag.For.Sync, EventTag.For.Edit)]
    [EventName(EventName)]
    [EventDescription("De straatnaam werd gehistoreerd.")]
    public class StreetNameWasRetiredV2 : IMunicipalityEvent
    {
        public const string EventName = "StreetNameWasRetiredV2"; // BE CAREFUL CHANGING THIS!!

        [EventPropertyDescription("Interne GUID van de gemeente aan dewelke de straatnaam is gekoppeld.")]
        public Guid MunicipalityId { get; }

        [EventPropertyDescription("Objectidentificator van de straatnaam.")]
        public int PersistentLocalId { get; }

        [EventPropertyDescription("Metadata bij het event.")]
        public ProvenanceData Provenance { get; private set; }

        public StreetNameWasRetiredV2(
            MunicipalityId municipalityId,
            PersistentLocalId persistentLocalId)
        {
            MunicipalityId = municipalityId;
            PersistentLocalId = persistentLocalId;
        }

        [JsonConstructor]
        private StreetNameWasRetiredV2(
            Guid municipalityId,
            int persistentLocalId,
            ProvenanceData provenance
        ) :
            this(
                new MunicipalityId(municipalityId),
                new PersistentLocalId(persistentLocalId))
            => ((ISetProvenance)this).SetProvenance(provenance.ToProvenance());

        void ISetProvenance.SetProvenance(Provenance provenance) => Provenance = new ProvenanceData(provenance);

        public IEnumerable<string> GetHashFields()
        {
            var fields = Provenance.GetHashFields().ToList();
            fields.Add(MunicipalityId.ToString("D"));
            fields.Add(PersistentLocalId.ToString());
            return fields;
        }

        public string GetHash() => this.ToEventHash(EventName);
    }
}
