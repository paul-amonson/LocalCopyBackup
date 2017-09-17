# LocalCopyBackup
C# windows application (Visual Studio 2017 Community Edition) to create a compressed/encrypted local backup of folders.

## Introduction
This project was an experiment in doing 3 things in the wake of CrashPlan abandoning consumer customers:
* File copy with compression using deflate (fast with moderate results).
* File copy with encryption (AES).
* Hiding file names using GUIDs.

These are the 3 things that I felt were required for a local copy backup that is intended to sync to the cloud for backup purposes. I use One Drive only because I pay for Office 365 and a 1TB storage for free but this should work with ANY folder based cloud syncing services.

The heart of this software is a backup plan. A backup plan is simply a name, a source folder, a target folder, and _internally a key which can be exported_.

## Eye on Security
The key for encryption __must__ be kept private and saved somewhere safe. If you loose it you cannot restore a backup successfully. I recommend 2 copies on 2 seperate flash drives (redundancy against failure) in a static bag with non-static accumulating desecant store in a safe deposit box at the bank or fire-safe at home. The software contains an _Export Key_ feature for just the reason. It is important to note that the backup plan files contain the key for that plan and therefore should be kept private. The plan files and configuration are stored in the user's home folder in a new folder called __.local_backup__. This folder should be considered sensitive.

My hope is that this is only susceptible to brute force attacks which means I am relying on AES's strength. Security comments are welcome to help improve this software. I am already aware of keys being kept in memory during application execution but if your system is already compromised then really that is the least of your worries!
## LocalCopyBackup Features
![Main Application Window](MainWindow.png)
The feature set is intentionally small. I included only what I considered necessary for usability.
### Add/Edit/Delete a Backup Plan
![Add Plan Dialog](AddPlan.png)

The software will generate an encrypted index file in the target folder with GUID based names for the backup when run the first time. Encrypted file backups are in a subfolder of the target folder using the same GUID as the index filename.
### Running and Stopping Backups
Running a backup is simple, just press the _Run All Now_ button on the interface. All plans will be run, I have provided no granularity here. I am promoting a _simple design philosophy_. Backups can be stopped but this is __not__ recommended. I recogonise that sometimes you have no choice but to halt a backup and this may lead to an unknown backup state.
### Running and Stopping a Restore
Clicking the _Restore_ button will prompt you for a location to put the folder that was orginally backed up. The default folder will be the source folder for the backup but you can change the folder to another location if desired. A restore __can__ be interrupted with no ill effects.
### Timed Backups
Checking the button labeled _Enable 4 Hour Backup_ with cause the software to start a backup every 4 hours from midnight. You can change this value only by editing the configuration file. Usually every 4 hours, the default, is a good choice.
### Importing Backups for Restore
When you experience a failure it is assumed the drive with the configuration folder is lost. So saved keys and the cloud backup is all you need to recreate a backup plan and restore a backup. Use the _Import Backup_ to begin the restore process.
### Exporting Keys
This feature is designed to backup keys for catastropic failure restore.

## Summary
This software fulfulled an immediate need but I also believe others may find it useful. I did not completely sanitize the source code's style and there may still be obvious optimizations (I am not completely up to speed on all new C# features). So comments are welcome but please note not all comments will be acted upon.

___One thing to note:___ I intentionally did not use a file watcher because they do not work properly on network mounted drives.

Paul
