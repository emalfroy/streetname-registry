namespace StreetNameRegistry.Projections.Legacy.StreetNameSyndication
{
    using Be.Vlaanderen.Basisregisters.ProjectionHandling.Connector;
    using Be.Vlaanderen.Basisregisters.ProjectionHandling.SqlStreamStore;
    using StreetName.Events;

    public class StreetNameSyndicationProjections : ConnectedProjection<LegacyContext>
    {
        public StreetNameSyndicationProjections()
        {
            When<Envelope<StreetNameWasRegistered>>(async (context, message, ct) =>
            {
                var streetNameSyndicationItem = new StreetNameSyndicationItem
                {
                    Position = message.Position,
                    StreetNameId = message.Message.StreetNameId,
                    NisCode = message.Message.NisCode,
                    RecordCreatedAt = message.Message.Provenance.Timestamp,
                    LastChangedOn = message.Message.Provenance.Timestamp,
                    ChangeType = message.EventName
                };

                streetNameSyndicationItem.ApplyProvenance(message.Message.Provenance);

                await context
                    .StreetNameSyndication
                    .AddAsync(streetNameSyndicationItem, ct);
            });

            When<Envelope<StreetNameNameWasNamed>>(async (context, message, ct) =>
            {
                await context.CreateNewStreetNameSyndicationItem(
                    message.Message.StreetNameId,
                    message,
                    x => UpdateNameByLanguage(x, message.Message.Name, message.Message.Language),
                    ct);
            });

            When<Envelope<StreetNameNameWasCleared>>(async (context, message, ct) =>
            {
                await context.CreateNewStreetNameSyndicationItem(
                    message.Message.StreetNameId,
                    message,
                    x => UpdateNameByLanguage(x, null, message.Message.Language),
                    ct);
            });

            When<Envelope<StreetNameNameWasCorrected>>(async (context, message, ct) =>
            {
                await context.CreateNewStreetNameSyndicationItem(
                    message.Message.StreetNameId,
                    message,
                    x => UpdateNameByLanguage(x, message.Message.Name, message.Message.Language),
                    ct);
            });

            When<Envelope<StreetNameNameWasCorrectedToCleared>>(async (context, message, ct) =>
            {
                await context.CreateNewStreetNameSyndicationItem(
                    message.Message.StreetNameId,
                    message,
                    x => UpdateNameByLanguage(x, null, message.Message.Language),
                    ct);
            });

            When<Envelope<StreetNameHomonymAdditionWasDefined>>(async (context, message, ct) =>
            {
                await context.CreateNewStreetNameSyndicationItem(
                    message.Message.StreetNameId,
                    message,
                    x => UpdateHomonymAdditionByLanguage(x, message.Message.HomonymAddition, message.Message.Language),
                    ct);
            });

            When<Envelope<StreetNameHomonymAdditionWasCleared>>(async (context, message, ct) =>
            {
                await context.CreateNewStreetNameSyndicationItem(
                    message.Message.StreetNameId,
                    message,
                    x => UpdateHomonymAdditionByLanguage(x, null, message.Message.Language),
                    ct);
            });

            When<Envelope<StreetNameHomonymAdditionWasCorrected>>(async (context, message, ct) =>
            {
                await context.CreateNewStreetNameSyndicationItem(
                    message.Message.StreetNameId,
                    message,
                    x => UpdateHomonymAdditionByLanguage(x, message.Message.HomonymAddition, message.Message.Language),
                    ct);
            });

            When<Envelope<StreetNameHomonymAdditionWasCorrectedToCleared>>(async (context, message, ct) =>
            {
                await context.CreateNewStreetNameSyndicationItem(
                    message.Message.StreetNameId,
                    message,
                    x => UpdateHomonymAdditionByLanguage(x, null, message.Message.Language),
                    ct);
            });

            When<Envelope<StreetNameBecameCurrent>>(async (context, message, ct) =>
            {
                await context.CreateNewStreetNameSyndicationItem(
                    message.Message.StreetNameId,
                    message,
                    x => x.Status = StreetNameStatus.Current,
                    ct);
            });

            When<Envelope<StreetNameWasCorrectedToCurrent>>(async (context, message, ct) =>
            {
                await context.CreateNewStreetNameSyndicationItem(
                    message.Message.StreetNameId,
                    message,
                    x => x.Status = StreetNameStatus.Current,
                    ct);
            });

            When<Envelope<StreetNameWasProposed>>(async (context, message, ct) =>
            {
                await context.CreateNewStreetNameSyndicationItem(
                    message.Message.StreetNameId,
                    message,
                    x => x.Status = StreetNameStatus.Proposed,
                    ct);
            });

            When<Envelope<StreetNameWasCorrectedToProposed>>(async (context, message, ct) =>
            {
                await context.CreateNewStreetNameSyndicationItem(
                    message.Message.StreetNameId,
                    message,
                    x => x.Status = StreetNameStatus.Proposed,
                    ct);
            });

            When<Envelope<StreetNameWasRetired>>(async (context, message, ct) =>
            {
                await context.CreateNewStreetNameSyndicationItem(
                    message.Message.StreetNameId,
                    message,
                    x => x.Status = StreetNameStatus.Retired,
                    ct);
            });

            When<Envelope<StreetNameWasCorrectedToRetired>>(async (context, message, ct) =>
            {
                await context.CreateNewStreetNameSyndicationItem(
                    message.Message.StreetNameId,
                    message,
                    x => x.Status = StreetNameStatus.Retired,
                    ct);
            });

            When<Envelope<StreetNameStatusWasRemoved>>(async (context, message, ct) =>
            {
                await context.CreateNewStreetNameSyndicationItem(
                    message.Message.StreetNameId,
                    message,
                    x => x.Status = null,
                    ct);
            });

            When<Envelope<StreetNameStatusWasCorrectedToRemoved>>(async (context, message, ct) =>
            {
                await context.CreateNewStreetNameSyndicationItem(
                    message.Message.StreetNameId,
                    message,
                    x => x.Status = null,
                    ct);
            });

            When<Envelope<StreetNameBecameComplete>>(async (context, message, ct) =>
            {
                await context.CreateNewStreetNameSyndicationItem(
                    message.Message.StreetNameId,
                    message,
                    x => x.IsComplete = true,
                    ct);
            });

            When<Envelope<StreetNameBecameIncomplete>>(async (context, message, ct) =>
            {
                await context.CreateNewStreetNameSyndicationItem(
                    message.Message.StreetNameId,
                    message,
                    x => x.IsComplete = false,
                    ct);
            });

            When<Envelope<StreetNameWasRemoved>>(async (context, message, ct) =>
            {
                await context.CreateNewStreetNameSyndicationItem(
                    message.Message.StreetNameId,
                    message,
                    x => { },
                    ct);
            });
        }

        private static void UpdateNameByLanguage(StreetNameSyndicationItem streetNameSyndicationItem, string name, Language? language)
        {
            switch (language)
            {
                case Language.Dutch:
                    streetNameSyndicationItem.NameDutch = name;
                    break;
                case Language.French:
                    streetNameSyndicationItem.NameFrench = name;
                    break;
                case Language.German:
                    streetNameSyndicationItem.NameGerman = name;
                    break;
                case Language.English:
                    streetNameSyndicationItem.NameEnglish = name;
                    break;
            }
        }

        private static void UpdateHomonymAdditionByLanguage(StreetNameSyndicationItem streetNameSyndicationItem, string homonymAddition, Language? language)
        {
            switch (language)
            {
                case Language.Dutch:
                    streetNameSyndicationItem.HomonymAdditionDutch = homonymAddition;
                    break;
                case Language.French:
                    streetNameSyndicationItem.HomonymAdditionFrench = homonymAddition;
                    break;
                case Language.German:
                    streetNameSyndicationItem.HomonymAdditionGerman = homonymAddition;
                    break;
                case Language.English:
                    streetNameSyndicationItem.HomonymAdditionEnglish = homonymAddition;
                    break;
            }
        }
    }
}
