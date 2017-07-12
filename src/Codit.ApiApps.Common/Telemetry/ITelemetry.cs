using System;
using System.Collections.Generic;
using System.Net.Http;
using Microsoft.ApplicationInsights.DataContracts;

namespace Codit.ApiApps.Common.Telemetry
{
    public interface ITelemetry
    {
        /// <summary>
        ///     Track an unhandled exception
        /// </summary>
        /// <param name="exception">Unhandled exception that occured</param>
        /// <param name="customProperties">List of custom properties to add</param>
        void TrackException(Exception exception, Dictionary<string, string> customProperties = null);

        /// <summary>
        ///     Track a specific metric
        /// </summary>
        /// <param name="metricName">Name of the metric</param>
        /// <param name="value">Value of the metric</param>
        void TrackMetric(string metricName, double value);

        /// <summary>
        ///     Tracks a request
        /// </summary>
        /// <param name="response">Response message</param>
        /// <param name="duration">Duration of the processing, if any</param>
        /// <param name="customProperties">Custom properties, if any</param>
        void TrackRequest(HttpResponseMessage response, TimeSpan duration = default(TimeSpan),
            Dictionary<string, string> customProperties = null);

        /// <summary>
        ///     Trace a message
        /// </summary>
        /// <param name="message">Message to trace</param>
        /// <param name="severityLevel">Severity level of the trace</param>
        /// <param name="customProperties">List of custom properties</param>
        /// <param name="sequence">Sequence Id of the trace</param>
        void TrackTrace(string message, SeverityLevel severityLevel = SeverityLevel.Information,
            Dictionary<string, string> customProperties = null, string sequence = null);
    }
}