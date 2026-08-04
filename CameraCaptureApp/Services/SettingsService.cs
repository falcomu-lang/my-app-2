using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using CameraCaptureApp.Models;

namespace CameraCaptureApp.Services
{
    public class SettingsService : ISettingsService
    {
        private readonly string _settingsFilePath;

        public SettingsService()
        {
            _settingsFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "settings.ini");
        }

        public CameraSettings Load()
        {
            if (!File.Exists(_settingsFilePath))
            {
                var defaults = CameraSettings.CreateDefault();
                Save(defaults);
                return defaults.Clone();
            }

            var settings = CameraSettings.CreateDefault();
            var values = ReadIniValues();

            settings.CameraName = GetString(values, "CameraName", settings.CameraName);
            settings.ConfigFilePath = GetString(values, "ConfigFilePath", settings.ConfigFilePath);
            settings.ServerName = GetString(values, "ServerName", settings.ServerName);
            settings.ResourceIndex = GetInt(values, "ResourceIndex", settings.ResourceIndex);
            settings.Width = GetInt(values, "Width", settings.Width);
            settings.Height = GetInt(values, "Height", settings.Height);
            settings.ExposureTime = GetDecimal(values, "ExposureTime", settings.ExposureTime);
            settings.Gain = GetDecimal(values, "Gain", settings.Gain);
            settings.FrameRate = GetDecimal(values, "FrameRate", settings.FrameRate);
            settings.PixelFormat = GetString(values, "PixelFormat", settings.PixelFormat);
            settings.TriggerMode = GetTriggerMode(values, "TriggerMode", settings.TriggerMode);
            settings.AutoConnect = GetBool(values, "AutoConnect", settings.AutoConnect);
            settings.AutoSave = GetBool(values, "AutoSave", settings.AutoSave);
            settings.SaveFolder = GetString(values, "SaveFolder", settings.SaveFolder);
            settings.FileNamePattern = GetString(values, "FileNamePattern", settings.FileNamePattern);

            return settings;
        }

        public void Save(CameraSettings settings)
        {
            var lines = new[]
            {
                "; CameraCaptureApp settings",
                "[Camera]",
                "CameraName=" + settings.CameraName,
                "ConfigFilePath=" + settings.ConfigFilePath,
                "ServerName=" + settings.ServerName,
                "ResourceIndex=" + settings.ResourceIndex.ToString(CultureInfo.InvariantCulture),
                "Width=" + settings.Width.ToString(CultureInfo.InvariantCulture),
                "Height=" + settings.Height.ToString(CultureInfo.InvariantCulture),
                "ExposureTime=" + settings.ExposureTime.ToString(CultureInfo.InvariantCulture),
                "Gain=" + settings.Gain.ToString(CultureInfo.InvariantCulture),
                "FrameRate=" + settings.FrameRate.ToString(CultureInfo.InvariantCulture),
                "PixelFormat=" + settings.PixelFormat,
                "TriggerMode=" + settings.TriggerMode.ToString(),
                "AutoConnect=" + settings.AutoConnect.ToString(),
                "AutoSave=" + settings.AutoSave.ToString(),
                "SaveFolder=" + settings.SaveFolder,
                "FileNamePattern=" + settings.FileNamePattern
            };

            File.WriteAllLines(_settingsFilePath, lines, Encoding.UTF8);
        }

        private Dictionary<string, string> ReadIniValues()
        {
            var values = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            foreach (var rawLine in File.ReadAllLines(_settingsFilePath, Encoding.UTF8))
            {
                var line = rawLine.Trim();
                if (line.Length == 0 || line.StartsWith(";") || line.StartsWith("#") || line.StartsWith("["))
                {
                    continue;
                }

                var separatorIndex = line.IndexOf('=');
                if (separatorIndex <= 0)
                {
                    continue;
                }

                var key = line.Substring(0, separatorIndex).Trim();
                var value = line.Substring(separatorIndex + 1).Trim();
                values[key] = value;
            }

            return values;
        }

        private static string GetString(Dictionary<string, string> values, string key, string fallback)
        {
            string value;
            return values.TryGetValue(key, out value) ? value : fallback;
        }

        private static int GetInt(Dictionary<string, string> values, string key, int fallback)
        {
            string value;
            int parsed;
            return values.TryGetValue(key, out value) && int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out parsed)
                ? parsed
                : fallback;
        }

        private static decimal GetDecimal(Dictionary<string, string> values, string key, decimal fallback)
        {
            string value;
            decimal parsed;
            return values.TryGetValue(key, out value) && decimal.TryParse(value, NumberStyles.Number, CultureInfo.InvariantCulture, out parsed)
                ? parsed
                : fallback;
        }

        private static bool GetBool(Dictionary<string, string> values, string key, bool fallback)
        {
            string value;
            bool parsed;
            return values.TryGetValue(key, out value) && bool.TryParse(value, out parsed)
                ? parsed
                : fallback;
        }

        private static TriggerMode GetTriggerMode(Dictionary<string, string> values, string key, TriggerMode fallback)
        {
            string value;
            TriggerMode parsed;
            return values.TryGetValue(key, out value) && Enum.TryParse(value, true, out parsed)
                ? parsed
                : fallback;
        }
    }
}
