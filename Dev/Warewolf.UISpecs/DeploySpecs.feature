﻿@Deploy 
Feature: DeploySpecs

Scenario: Deploying From Explorer Opens The Deploy With Resource Already Checked
    Given The Warewolf Studio is running
	And I Filter the Explorer with "Hello World"
	And I RightClick Explorer Localhost First Item
	And I Select Deploy FromExplorerContextMenu
	And I Select "Hello World" from the source tab 
	Then Filtered Resourse Is Checked For Deploy
	And I Click Close Deploy Tab Button
	
Scenario: Deploy ViewOnlyWorkflow to remoteConnection
	Given The Warewolf Studio is running
	When I Set Resource Permissions For "DeployViewOnly" to Group "Public" and Permissions for View to "true" and Contribute to "false" and Execute to "false"
	And I Click Deploy Ribbon Button
	And I Select RemoteConnectionIntegration From Deploy Tab Destination Server Combobox
	And I Click Deploy Tab Destination Server Connect Button		
	And I Select "DeployViewOnly" from the source tab 
	And I Wait For Explorer First Remote Server Spinner	
	Then Filtered Resourse Is Checked For Deploy
	And I Click Deploy button	
	And I Click MessageBox OK
	And I Click MessageBox OK
	And I Click MessageBox OK
	And I Click Deploy Tab Destination Server Connect Button
    

Scenario: Deploy From RemoteConnection
	Given The Warewolf Studio is running
	When I Click Deploy Ribbon Button
    And I Select RemoteConnectionIntegration From Deploy Tab Source Server Combobox
	And I Click Deploy Tab Source Server Connect Button
	And I Wait For Explorer First Remote Server Spinner	
    And Resources is visible on the tree
	And I Select "Hello World" from the source tab 
	And I Click Deploy button	
    And I Click MessageBox OK
    And I Click MessageBox OK
    And I Click MessageBox OK


Scenario: Deploy button is enabling when selecting resource in source side
	 Given The Warewolf Studio is running
	 When I Click Deploy Ribbon Button
	 And I Select RemoteConnectionIntegration From Deploy Tab Destination Server Combobox
	 And I Click Deploy Tab Destination Server Connect Button
	 And I Select "Hello world" from the source tab 
	 Then Deploy Button is enabled  "true"	
	 And I Click Deploy Tab Destination Server Connect Button
	 Then Deploy Button is enabled  "false"

Scenario: Filtering and clearing filter on source side
	Given The Warewolf Studio is running
	When I Click Deploy Ribbon Button
	And I Select RemoteConnectionIntegration From Deploy Tab Destination Server Combobox
	And I filter for "Hello World" on the source filter
	And I filter for "" on the source filter	
	And Deploy Button is enabled  "false"

Scenario: Deploying with filter enabled
     Given The Warewolf Studio is running
	 When I Click Deploy Ribbon Button
	 And I Select RemoteConnectionIntegration From Deploy Tab Destination Server Combobox
	 And I Click Deploy Tab Destination Server Connect Button
	 And I filter for "Hello World" on the source filter
	 And Resources is visible on the tree
	 And I Select "Hello World" from the source tab 
	 And I Click Deploy button	
	 And I Click Deploy button	
	 And I Click MessageBox OK
	 And I Click MessageBox OK
	 And I Click MessageBox OK
	 And I Click Deploy Tab Destination Server Connect Button

Scenario: Deploy is enabled when I change server after validation thrown
  Given The Warewolf Studio is running
  When I Click Deploy Ribbon Button
  And I Select LocalhostConnected From Deploy Tab Destination Server Combobox
  And I Select LocalhostConnected From Deploy Tab Destination Server Combobox 
  And I filter for "Hello world" on the source filter
  And Deploy Button is enabled  "false"
  And The deploy validation message is "Source and Destination cannot be the same."
  And I Select RemoteConnectionIntegration From Deploy Tab Destination Server Combobox
  And I Click Deploy Tab Destination Server Connect Button
  And I Select "Hello world" from the source tab 
  And Deploy Button is enabled  "true"

Scenario: Select All resources to deploy
  Given The Warewolf Studio is running
  When I Click Deploy Ribbon Button
  And I Select LocalhostConnected From Deploy Tab Destination Server Combobox
  And I Select RemoteConnectionIntegration From Deploy Tab Destination Server Combobox
  And I Click Deploy Tab Destination Server Connect Button
  And I filter for "DateTime" on the source filter
  And I Select localhost from the source tab 
  And Deploy Button is enabled  "true"

Scenario: Deploying From Explorer Opens The Deploy With All Resources in Folder Already Checked
	Given The Warewolf Studio is running
	And I Filter the Explorer with "Unit Tests"
	And I RightClick Explorer Localhost First Item
	And I Select Deploy FromExplorerContextMenu
	And I Enter "Unit Tests" Into Deploy Source Filter	
	Then Filtered Resourse Is Checked For Deploy
	And I Click Close Deploy Tab Button

Scenario: Cancel Deploy Returns to Deploy Tab
	Given The Warewolf Studio is running
	And I Filter the Explorer with "Unit Tests"
	And I RightClick Explorer Localhost First Item
	And I Select Deploy FromExplorerContextMenu
	And I Click Deploy Tab Destination Server Combobox
	And I Click Deploy Tab Destination Server Remote Connection Intergration Item
	And I Click Deploy Tab Destination Server Connect Button
	Then Deploy Button Is Enabled
	When I Click Deploy Tab Deploy Button
	Then Deploy Version Conflict Window Shows
	And I Click MessageBox Cancel
	And Deploy Window Is Still Open

Scenario: Deploy Disconnect Clears Destination
	Given The Warewolf Studio is running
	And I Filter the Explorer with "Unit Tests"
	And I RightClick Explorer Localhost First Item
	And I Select Deploy FromExplorerContextMenu
    And I Select RemoteConnectionIntegration From Deploy Tab Destination Server Combobox
	And I Click Deploy Tab Destination Server Connect Button
	Then Deploy Button Is Enabled
	When I Click Deploy Tab Deploy Button
	Then Deploy Version Conflict Window Shows
	And I Click MessageBox Cancel
	And Deploy Window Is Still Open
	Then I Click Deploy Tab Destination Server Connect Button

Scenario: Disconnect Remote Integration On Deploy Destination Does Not Disconnect On The Explorer
	Given The Warewolf Studio is running
	When I Try Connect To Remote Server
	And I Click Deploy Ribbon Button
	And I Click Deploy Tab Destination Server Combobox
	And I Click Deploy Tab Destination Server Remote Connection Intergration Item
	And I Click Deploy Tab Destination Server Connect Button
	And I Click Explorer Connect Remote Server Button
	Then Destination Remote Server Is Connected

