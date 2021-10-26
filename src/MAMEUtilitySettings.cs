﻿using Playnite.SDK;
using Playnite.SDK.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAMEUtility
{
    public class MAMEUtilitySettings : ObservableObject
    {
        //////////////////////////////////////////
        //// MAME Source executable
        //////////////////////////////////////////
        private bool _useMameExecutablePath = true;
        public bool UseMameExecutable
        {
            get => _useMameExecutablePath;
            set
            {
                _useMameExecutablePath = value;
                OnPropertyChanged();
            }
        }

        private string _mameExecutableFilePath;
        public string MameExecutableFilePath
        {
            get => _mameExecutableFilePath;
            set
            {
                _mameExecutableFilePath = value;
                OnPropertyChanged();
            }
        }



        //////////////////////////////////////////
        //// MAME Source file
        //////////////////////////////////////////
        private bool _useGamelistXmlPath = false;
        public bool UseMameListFile
        {
            get => _useGamelistXmlPath;
            set
            {
                _useGamelistXmlPath = value;
                OnPropertyChanged();
            }
        }

        private string _mameGamelistXmlFilePath;
        public string MameListFilePath
        {
            get => _mameGamelistXmlFilePath;
            set
            {
                _mameGamelistXmlFilePath = value;
                OnPropertyChanged();
            }
        }


        // Playnite serializes settings object to a JSON object and saves it as text file.
        // If you want to exclude some property from being saved then use `JsonDontSerialize` ignore attribute.
        //[DontSerialize]
        //public bool OptionThatWontBeSaved { get => optionThatWontBeSaved; set => SetValue(ref optionThatWontBeSaved, value); }
    }

    public class MAMEUtilitySettingsViewModel : ObservableObject, ISettings
    {
        private readonly MAMEUtilityPlugin plugin;
        private MAMEUtilitySettings editingClone { get; set; }

        private MAMEUtilitySettings settings;
        public MAMEUtilitySettings Settings
        {
            get => settings;
            set
            {
                settings = value;
                OnPropertyChanged();
            }
        }

        public MAMEUtilitySettingsViewModel(MAMEUtilityPlugin plugin)
        {
            // Injecting your plugin instance is required for Save/Load method because Playnite saves data to a location based on what plugin requested the operation.
            this.plugin = plugin;

            // Load saved settings.
            var savedSettings = plugin.LoadPluginSettings<MAMEUtilitySettings>();

            // LoadPluginSettings returns null if not saved data is available.
            if (savedSettings != null)
            {
                Settings = savedSettings;
            }
            else
            {
                Settings = new MAMEUtilitySettings();
            }
        }

        public void BeginEdit()
        {
            // Code executed when settings view is opened and user starts editing values.
            editingClone = Serialization.GetClone(Settings);
        }

        public void CancelEdit()
        {
            // Code executed when user decides to cancel any changes made since BeginEdit was called.
            // This method should revert any changes made to Option1 and Option2.
            Settings = editingClone;
        }

        public void EndEdit()
        {
            // Code executed when user decides to confirm changes made since BeginEdit was called.
            // This method should save settings made to Option1 and Option2.
            plugin.SavePluginSettings(Settings);
        }

        public bool VerifySettings(out List<string> errors)
        {
            // Code execute when user decides to confirm changes made since BeginEdit was called.
            // Executed before EndEdit is called and EndEdit is not called if false is returned.
            // List of errors is presented to user if verification fails.
            errors = new List<string>();
            return true;
        }
    }
}