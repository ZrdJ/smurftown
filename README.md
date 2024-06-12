# Smurftown App

Manage all of your battlenet account in one place.

> [!WARNING]
> This app needs admin priviliges to function properly. If you want the app to launch your Battlenet with other windows users, please make sure to run this app as an administrator

![image](https://github.com/ZrdJ/smurftown/assets/7228633/e6cd78b6-78e2-444f-8b39-ae002c614467)

# Key Features
* Add and edit battlenet accounts
* Manage battlenet login credentials
* Easily access and copy login credentials if needed
* Filter your accounts by name or games
* Manage dedicated windows users to launch multiple instances of battlenet at the same time

## Running as administrator
To be able to windows account features to its full extend, simply configure the app to run as administrator:
![image](https://github.com/ZrdJ/smurftown/assets/7228633/fcb668a9-f90a-4bda-9ebd-e1e940555657)

# Installation
## Download the certificate and the installer
Go to [Releases](https://github.com/ZrdJ/smurftown/releases) and download the latest bundle containing:
* SmurftownInstaller_2024.6.1.0_x64.cer
* SmurftownInstaller_2024.6.1.0_x64.msixbundle

## Install certificate on your PC
Doubleclick the downloaded certificate file `SmurftownInstaller_*_x64.cer` and hit `install`:
![image](https://github.com/ZrdJ/smurftown/assets/7228633/98a5fe90-6b1c-4d14-bcf5-5a5814ae5ec8)

## Run the installer
Run the installer file `SmurftownInstaller_*_x64.msixbundle`

# Roadmap
## Heroes of the Storm
* provide more game account details
* add hero rotation
* add capability to manage rank information
* add capability to manage (owned) hero information
## Overwatch
* provide more game account details
* add some basic stats or links for https://www.overbuff.com/
* add capability to manage rank information

# FAQ
### Where can i download the app ?
Select the latest release on the right side of this project and download the .exe Installer: [Releases](https://github.com/ZrdJ/smurftown/releases)

### Why do i need to start this app as an adminsitrator?
Thats because this app is creating windows users on your behalf when you decide to run multiple Battlenet instance at the same time

### Is this app sending or receiving data from a server on the internet?
No, this app operates only locally and does not communicate with anything online

### So where is my data stored when it does not communicate with a server?
Your data is stored in a single file called `data.yaml` and can be found in your installation directory (next to `Smurftown.exe`)

### How can I be sure you are not lying?
You can't. Lookup the source code yourself and decide on your own.

### Why do i need to install a certificate?
Because i dont have the money to buy a official certificate accepted by Micrsosoft


