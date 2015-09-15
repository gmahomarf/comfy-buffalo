using System;
using Ironhide.Api.Host;
using Machine.Specifications;

namespace Ironhide.Api.Specs.Integration
{
    [Ignore("Integration spec.")]
    public class when_sending_a_notification_to_slack
    {
        static SlackHiringTeamNotifications _notifier;

        Establish context =
            () => { _notifier = new SlackHiringTeamNotifications(); };

        Because of =
            () =>
                _notifier.Notify("byron@acklenavenue.com", "Byron Sommardahl", "http://github.com", "645436",
                    new TimeSpan(50000));

        It should_work =
            () => { };
    }
}