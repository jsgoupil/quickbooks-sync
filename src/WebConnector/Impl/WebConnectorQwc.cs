using QbSync.WebConnector.Core;
using QbSync.WebConnector.Models;
using System;
using System.Text;

namespace QbSync.WebConnector.Impl
{
    /// <summary>
    /// Service handling the generation of the QWC File.
    /// </summary>
    public class WebConnectorQwc : IWebConnectorQwc
    {
        /// <summary>
        /// Gets the QWC file as a string.
        /// </summary>
        /// <param name="model">The options to configure the QWC file.</param>
        /// <returns>XML string.</returns>
        public string GetQwcFile(WebConnectorQwcModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            if (string.IsNullOrEmpty(model.AppDescription))
            {
                throw new ArgumentException("The AppDescription is required.", nameof(WebConnectorQwcModel.AppDescription));
            }

            if (string.IsNullOrEmpty(model.AppName))
            {
                throw new ArgumentException("The AppName is required.", nameof(WebConnectorQwcModel.AppName));
            }

            if (string.IsNullOrEmpty(model.AppSupport))
            {
                throw new ArgumentException("The AppSupport is required.", nameof(WebConnectorQwcModel.AppSupport));
            }

            if (string.IsNullOrEmpty(model.AppURL))
            {
                throw new ArgumentException("The AppURL is required.", nameof(WebConnectorQwcModel.AppURL));
            }

            if (model.FileID == default)
            {
                throw new ArgumentException("The FileID is required.", nameof(WebConnectorQwcModel.FileID));
            }

            if (model.OwnerID == default)
            {
                throw new ArgumentException("The OwnerID is required.", nameof(WebConnectorQwcModel.OwnerID));
            }

            if (string.IsNullOrEmpty(model.UserName))
            {
                throw new ArgumentException("The UserName is required.", nameof(WebConnectorQwcModel.UserName));
            }

            var sb = new StringBuilder();
            sb.Append(@"<?xml version=""1.0""?>");
            sb.Append("<QBWCXML>");
            sb.Append($"<AppDescription>{model.AppDescription}</AppDescription>");

            if (!string.IsNullOrEmpty(model.AppDisplayName))
            {
                sb.Append($"<AppDisplayName>{model.AppDisplayName}</AppDisplayName>");
            }

            sb.Append($"<AppID>{model.AppID}</AppID>");
            sb.Append($"<AppName>{model.AppName}</AppName>");
            sb.Append($"<AppSupport>{model.AppSupport}</AppSupport>");

            if (!string.IsNullOrEmpty(model.AppUniqueName))
            {
                sb.Append($"<AppUniqueName>{model.AppUniqueName}</AppUniqueName>");
            }

            sb.Append($"<AppURL>{model.AppURL}</AppURL>");

            if (model.AuthFlags != null)
            {
                var v = GetFlagValue(model.AuthFlags);
                sb.Append($"<AuthFlags>0x{v:X1}</AuthFlags>");
            }

            sb.Append($"<FileID>{model.FileID:B}</FileID>");

            if (model.IsReadOnly)
            {
                sb.Append($"<IsReadOnly>true</IsReadOnly>");
            }

            if (model.Notify)
            {
                sb.Append($"<Notify>true</Notify>");
            }

            sb.Append($"<OwnerID>{model.OwnerID:B}</OwnerID>");

            if (model.PersonalDataPrep.HasValue)
            {
                sb.Append("<PersonalDataPrep>");
                if (model.PersonalDataPrep.Value == PersonalDataPref.PdpOptional)
                {
                    sb.Append("pdpOptional");
                }
                else if (model.PersonalDataPrep.Value == PersonalDataPref.PdpRequired)
                {
                    sb.Append("pdpRequired");
                }

                sb.Append("</PersonalDataPrep>");
            }

            sb.Append("<QBType>");
            if (model.QBType == QBType.QBFS)
            {
                sb.Append("QBFS");
            }
            else if (model.QBType == QBType.QBPOS)
            {
                sb.Append("QBPOS");
            }

            sb.Append("</QBType>");

            // If the timespan is smaller than 1 minute, we will use the RunEveryNSeconds
            // Otherwise we round with minutes.
            if (model.RunEvery.HasValue)
            {
                sb.Append("<Scheduler>");
                if (model.RunEvery.Value.TotalSeconds < 60)
                {
                    sb.Append($"<RunEveryNSeconds>{(int)Math.Round(model.RunEvery.Value.TotalSeconds, MidpointRounding.AwayFromZero)}</RunEveryNSeconds>");
                }
                else
                {
                    sb.Append($"<RunEveryNMinutes>{(int)Math.Round(model.RunEvery.Value.TotalMinutes, MidpointRounding.AwayFromZero)}</RunEveryNMinutes>");

                }

                sb.Append("</Scheduler>");
            }

            // Style not supported!

            if (model.UnattendedModePref.HasValue)
            {
                sb.Append("<UnattendedModePref>");
                if (model.UnattendedModePref.Value == UnattendedModePref.UmpOptional)
                {
                    sb.Append("umpOptional");
                }
                else if (model.UnattendedModePref.Value == UnattendedModePref.UmpRequired)
                {
                    sb.Append("umpRequired");
                }

                sb.Append("</UnattendedModePref>");
            }

            sb.Append($"<UserName>{model.UserName}</UserName>");

            sb.Append("</QBWCXML>");

            return sb.ToString();
        }

        private static int GetFlagValue(Enum input)
        {
            int v = 0;
            foreach (Enum? value in Enum.GetValues(input.GetType()))
            {
                if (value != null && input.HasFlag(value))
                {
                    var tempV = (int)(object)value;
                    v += tempV;
                }
            }

            return v;
        }
    }
}
