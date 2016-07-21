﻿using System.Linq;

namespace Warewolf.UITests
{
    using Microsoft.VisualStudio.TestTools.UITesting.HtmlControls;
    using Microsoft.VisualStudio.TestTools.UITesting.WinControls;
    using Microsoft.VisualStudio.TestTools.UITesting.WpfControls;
    using System;
    using System.Collections.Generic;
    using System.CodeDom.Compiler;
    using Microsoft.VisualStudio.TestTools.UITest.Extension;
    using Microsoft.VisualStudio.TestTools.UITesting;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Keyboard = Microsoft.VisualStudio.TestTools.UITesting.Keyboard;
    using Mouse = Microsoft.VisualStudio.TestTools.UITesting.Mouse;
    using MouseButtons = System.Windows.Forms.MouseButtons;
    using System.Drawing;
    using System.Windows.Input;
    using System.Text.RegularExpressions;
    using System.Reflection;
    using System.Threading;

    public partial class UIMap
    {
        public void SetGlobalPlaybackSettings()
        {
            Playback.PlaybackSettings.WaitForReadyLevel = WaitForReadyLevel.Disabled;
            Playback.PlaybackSettings.ShouldSearchFailFast = false;
            if (Environment.ProcessorCount <= 4)
            {
                Playback.PlaybackSettings.ThinkTimeMultiplier = 2;
            }
            else
            {
                Playback.PlaybackSettings.ThinkTimeMultiplier = 1;
            }
            Playback.PlaybackSettings.MaximumRetryCount = 5 * int.Parse(Playback.PlaybackSettings.ThinkTimeMultiplier.ToString());
            Playback.PlaybackSettings.SearchTimeout = 5000 * int.Parse(Playback.PlaybackSettings.ThinkTimeMultiplier.ToString());
            Playback.PlaybackSettings.MatchExactHierarchy = true;
            Playback.PlaybackSettings.SkipSetPropertyVerification = true;
            Playback.PlaybackSettings.SmartMatchOptions = SmartMatchOptions.None;
            Playback.PlaybackError -= new EventHandler<PlaybackErrorEventArgs>(PlaybackErrorHandler);
            Playback.PlaybackError += new EventHandler<PlaybackErrorEventArgs>(PlaybackErrorHandler);
        }
        
        void PlaybackErrorHandler(object sender, PlaybackErrorEventArgs e)
        {
            Console.WriteLine(e.Error.Message);
            if (e.Error is UITestControlNotFoundException)
            {
                UITestControlNotFoundException asControlNotFoundException = e.Error as UITestControlNotFoundException;
                var exceptionSource = asControlNotFoundException.ExceptionSource;
                if (exceptionSource is UITestControl)
                {
                    UITestControl parent = (exceptionSource as UITestControl).Container;
                    while (parent != null && !parent.Exists)
                    {
                        parent = parent.Container;
                    }
                    if (parent != null && parent.Exists)
                    {
                        parent.DrawHighlight();
                        e.Result = PlaybackErrorOptions.Retry;
                        return;
                    }
                }
            }
            Playback.Wait(1000 * int.Parse(Playback.PlaybackSettings.ThinkTimeMultiplier.ToString()));
            e.Result = PlaybackErrorOptions.Retry;
        }

        public void WaitIfStudioDoesNotExist()
        {
            var sleepTimer = 60;
            try
            {
                if (!this.MainStudioWindow.Exists)
                {
                    WaitForStudioStart(sleepTimer * 1000);
                }
            }
            catch (UITestControlNotFoundException)
            {
                WaitForStudioStart(sleepTimer * 1000);
            }
        }

        private void WaitForStudioStart(int timeout)
        {
            Console.WriteLine("Waiting for studio to start.");
            Playback.Wait(timeout);
            if (!this.MainStudioWindow.Exists)
            {
                throw new InvalidOperationException("Warewolf studio is not running. You are expected to run \"Dev\\TestScripts\\Studio\\Startup.bat\" as an administrator and wait for it to complete before running any coded UI tests");
            }
        }

        public void InitializeABlankWorkflow()
        {
            Click_New_Workflow_Ribbon_Button();
        }

        public void CleanupWorkflow()
        {
            try
            {
                if (MainStudioWindow.DockManager.SplitPaneLeft.Explorer.SearchTextBox.Text != string.Empty)
                {
                    Click_Clear_Toolbox_Filter_Button();
                }
                Click_Close_Tab_Button();
                Click_MessageBox_No();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error during test cleanup: " + e.Message);
            }
        }

        public void Click_Settings_Resource_Permissions_Row1_Add_Resource_Button()
        {
            Mouse.Click(this.FindAddResourceButton(this.MainStudioWindow.DockManager.SplitPaneMiddle.TabMan.SettingsTab.WorksurfaceContext.SettingsView.TabList.SecurityTab.SecurityWindow.ResourcePermissions.Row1));
            Assert.AreEqual(true, this.ServicePickerDialog.Exists, "Service picker dialog does not exist.");
        }

        public void Click_Settings_Resource_Permissions_Row1_Windows_Group_Button()
        {
            Mouse.Click(this.FindAddWindowsGroupButton(this.MainStudioWindow.DockManager.SplitPaneMiddle.TabMan.SettingsTab.WorksurfaceContext.SettingsView.TabList.SecurityTab.SecurityWindow.ResourcePermissions.Row1));
            Assert.AreEqual(true, this.SelectWindowsGroupDialog.Exists, "Select windows group dialog does not exist.");
            Assert.AreEqual(true, this.SelectWindowsGroupDialog.ItemPanel.ObjectNameTextbox.Exists, "Select windows group object name textbox does not exist.");
        }

        public UITestControl FindAddResourceButton(UITestControl row)
        {
            var firstOrDefaultCell = row.GetChildren().Where(child => child.ControlType == ControlType.Cell).ElementAtOrDefault(0);
            return firstOrDefaultCell?.GetChildren().FirstOrDefault(child => child.ControlType == ControlType.Button);
        }

        public UITestControl FindAddWindowsGroupButton(UITestControl row)
        {
            var firstOrDefaultCell = row.GetChildren().Where(child => child.ControlType == ControlType.Cell).ElementAtOrDefault(1);
            return firstOrDefaultCell?.GetChildren().FirstOrDefault(child => child.ControlType == ControlType.Button);
        }

        public UITestControl FindViewPermissionsCheckbox(UITestControl row)
        {
            var firstOrDefaultCell = row.GetChildren().Where(child => child.ControlType == ControlType.Cell).ElementAtOrDefault(2);
            return firstOrDefaultCell?.GetChildren().FirstOrDefault(child => child.ControlType == ControlType.CheckBox);
        }

        public UITestControl FindExecutePermissionsCheckbox(UITestControl row)
        {
            var firstOrDefaultCell = row.GetChildren().Where(child => child.ControlType == ControlType.Cell).ElementAtOrDefault(3);
            return firstOrDefaultCell?.GetChildren().FirstOrDefault(child => child.ControlType == ControlType.CheckBox);
        }

        public UITestControl FindContributePermissionsCheckbox(UITestControl row)
        {
            var firstOrDefaultCell = row.GetChildren().Where(child => child.ControlType == ControlType.Cell).ElementAtOrDefault(4);
            return firstOrDefaultCell?.GetChildren().FirstOrDefault(child => child.ControlType == ControlType.CheckBox);
        }
    }
}