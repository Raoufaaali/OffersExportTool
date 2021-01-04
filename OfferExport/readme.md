# Offer Export Tool
This tool is used to export Offers ( in a specific format ) from WinOasis DB to the user desktop.

## Install Prerequisites
To run this tool and export the offers you will need the following:
1. The latest installer **PlayerMaxOfferExportToolInstaller.exe**. This can be obtained from TFS drop location
2. A target machine that can connect to WinOasis DB
3. Either SQL login or Windows Login to connect to the DB

## Install
1. Copy the installer to the target machine
2. Run the installer. No specific configurations are required during the install

## Usage
The installer should create a desktop shortcut with the name **PlayerMax Offer Export Tool**
To run the tool, simply click on the desktop shortcut. 
If this is the first time the tool runs, it will ask for the DB credentials. Enter WinOasis details in the interactive terminal. You can use either windows login or sql login
if the connection is successfull, the offers csv files will be exported to the user desktop in a folder named **offers**   

The initial settings is a one-time activity. For subsequent runs (i.e. when the shortcut is clicked), the tool will attempt to connect to the DB and exoport the offers to the destination folder (overwritting existing files)

## Upgrade/Reinstall
Run the new installer as usual. No need to uninstall the existing version. 
The tool wont ask for connection string during an upgrade and will use the existing settings (if any). 
If you need to update the connection string. See [Changing Connection String](##Changing-Connection-String)

## Changing Connection String
The only settings required is DB connection info. The user is prompted to enter those the first time the tool runs. However, if you need to change those later do one of the following:
1. Delete the PlayerMaxOfferData.settings file located under C:\ProgramData. Once the file is deleted, run the tool from the desktop, OR
2. Run the tool with the "setup" option as shown below:

```bat
cd "C:\Program Files (x86)\Aristocrat\PlayerMax\ATI.PlayerMax Offer Export Tool"
OfferExport.exe setup
```
