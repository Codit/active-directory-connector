using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Net.Http;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;

namespace Codit.ApiApps.Common.Telemetry
{
    public class ApplicationInsightsTelemetry : ITelemetry
    {
        /// <summary>
        ///     Application Insights Telemetry Client
        /// </summary>
        private readonly TelemetryClient _telemetryClient;

        /// <summary>
        ///     Constructor
        /// </summary>
        private ApplicationInsightsTelemetry()
        {
            TelemetryConfiguration.Active.DisableTelemetry = false;
            _telemetryClient = new TelemetryClient
            {
                InstrumentationKey = ConfigurationManager.AppSettings["APPINSIGHTS_INSTRUMENTATIONKEY"]
            };
        }

        /// <summary>
        ///     Track an unhandled exception
        /// </summary>
        /// <param name="exception">Unhandled exception that occured</param>
        /// <param name="customProperties">List of custom properties to add</param>
        public void TrackException(Exception exception, Dictionary<string, string> customProperties = null)
        {
            try
            {
                var exceptionTelemetry = new ExceptionTelemetry(exception)
                {
                    Timestamp = DateTimeOffset.UtcNow
                };

                exceptionTelemetry.Properties.Merge(customProperties, OptimizeProperty);

                _telemetryClient.TrackException(exceptionTelemetry);
            }
            catch (Exception logException)
            {
                Trace.TraceError($"Failed to log exception with value '{exception}'. Reason: {logException}.");
            }
        }

        /// <summary>
        ///     Track a specific metric
        /// </summary>
        /// <param name="metricName">Name of the metric</param>
        /// <param name="value">Value of the metric</param>
        public void TrackMetric(string metricName, double value)
        {
            try
            {
                var metricTelemetry = new MetricTelemetry
                {
                    Name = metricName,
                    Timestamp = DateTimeOffset.UtcNow,
                    Value = value
                };

                _telemetryClient.TrackMetric(metricTelemetry);
            }
            catch (Exception ex)
            {
                Trace.TraceError($"Failed to log metric '{metricName}' with value '{value}'. Reason: {ex}.");
            }
        }

        /// <summary>
        ///     Tracks a request
        /// </summary>
        /// <param name="response">Response message</param>
        /// <param name="duration">Duration of the processing, if any</param>
        /// <param name="customProperties">Custom properties, if any</param>
        public void TrackRequest(HttpResponseMessage response, TimeSpan duration = default(TimeSpan),
            Dictionary<string, string> customProperties = null)
        {
            try
            {
                var requestTelemetry = new RequestTelemetry
                {
                    HttpMethod = response.RequestMessage.Method.ToString(),
                    Url = response.RequestMessage.RequestUri,
                    ResponseCode = response.StatusCode.ToString()
                };

                if (duration != default(TimeSpan))
                {
                    requestTelemetry.Duration = duration;
                }

                requestTelemetry.Properties.Merge(customProperties, OptimizeProperty);

                _telemetryClient.TrackRequest(requestTelemetry);
            }
            catch (Exception ex)
            {
                Trace.TraceError($"Failed to log a request. Reason: {ex}.");
            }
        }

        /// <summary>
        ///     Trace a message
        /// </summary>
        /// <param name="message">Message to trace</param>
        /// <param name="severityLevel">Severity level of the trace</param>
        /// <param name="customProperties">List of custom properties</param>
        /// <param name="sequence">Sequence Id of the trace</param>
        public void TrackTrace(string message, SeverityLevel severityLevel = SeverityLevel.Information,
            Dictionary<string, string> customProperties = null, string sequence = null)
        {
            try
            {
                var traceTelemetry = new TraceTelemetry
                {
                    SeverityLevel = severityLevel,
                    Timestamp = DateTime.UtcNow,
                    Message = message
                };

                if (string.IsNullOrWhiteSpace(sequence) == false)
                    traceTelemetry.Sequence = sequence;

                traceTelemetry.Properties.Merge(customProperties, OptimizeProperty);

                _telemetryClient.TrackTrace(traceTelemetry);
            }
            catch (Exception ex)
            {
                Trace.TraceError($"Failed to trace '{message}'. Reason: {ex}.");
            }
        }

        private KeyValuePair<string, string> OptimizeProperty(KeyValuePair<string, string> property)
        {
            var propertyName = property.Key;
            var propertyValue = property.Value;

            // Avoid going over the size limit for AI
            if ((string.IsNullOrWhiteSpace(propertyValue) == false) && (propertyValue.Length > 1000))
                propertyValue = propertyValue.Substring(0, 1000);

            // Avoid having spaces in the property name for processing reasons
            if (string.IsNullOrWhiteSpace(propertyName) == false)
                propertyName = propertyName.Replace(" ", "");

            return new KeyValuePair<string, string>(propertyName, propertyValue);
        }
    }
}
