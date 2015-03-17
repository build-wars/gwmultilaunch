# Guild Wars Multi-Launch #

## Purpose ##

Launch multiple copies of Guild Wars.
(no patching required!)

## Requirements ##

  1. Guild Wars
  1. Windows 2000, XP, Vista, or 7
  1. .NET Framework 2.0 or higher

## Usage ##

  1. Make copies of the Guild Wars folder
  1. Add the gw.exe(s) to the list
  1. Launch or create shortcuts for launching

  * Notes
    * Vista/Win7: May need to start GWML as admin if GW is installed to "Program Files" folder
    * Vent/Teamspeak: May need to run Vent/TS as admin or GWML/GW NOT as admin for key catching to work
    * GW Updates: Start a copy and wait for update to finish before launching another copy.
    * Texmod: See the FAQ section.

## Basic Controls ##

  * Add - add an existing copy
  * Remove - remove selected copy
  * Make Copy - create new copy of guild wars
  * Launch - start the selected copies
  * Make Shortcut - for starting selected copy
  * Make Master Shortcut - for starting next unlaunched copy
  * Argument Wizard - easy way to edit gw.exe launch switches

## Expert Controls ##

  * Set Registry Path - set gw path to selected copy in registry
  * Clear Mutex - closes the mutex handle under all active gw.exe processes
  * Start TexMod - sets path, clears mutex, locates and start Texmod
  * Force gw.dat unlock - useful if you are out of diskspace and need a quick muling session

## Launch Sequence ##

  1. Set Registry Path
  1. Mutex cleared from all gw.exe processes
  1. Launch gw copy

## FAQ ##

Q: I am using Vista/7. I am having trouble getting another copy to start.

A: Guild Wars under some circumstances may be running under admin rights under Vista/Win7. If this is the case, GWML must also be run under admin (right click gwmultilaunch.exe-> run as admin). Running GWML in admin mode allows it to close the mutex of gw.exe which were ran as admin.

Q: How do I use Texmod with this?

A: To launch a Texmoded copy:
  1. Select the copy you will be launching
  1. Click "Start TexMod"
  1. Select the same copy within TexMod and run

Q: Do I have to always use the launcher or the special shortcuts?

A: No, you only need to use the launcher or special shortcuts when you wish to launch multiple copies of Guild Wars.

Q: When I try to launch a second copy, it just takes me back to the first. Why?

A: Either, the wrong edition of the launcher is being used or gw.exe was run as admin and the launcher does not have proper access to it. If you are on a 64 bit operating system, you MUST use the 64-bit edition of the launcher. Easy way to check is to see if there is a "C:\Program Files(x86)" folder. Otherwise, if gw.exe was ran as admin (Vista/7), you need to run the launcher as admin as well. Right click the launcher shortcut and click "Run as Administrator".

Q: Why do I get a "gw.dat is locked" message?

A: This is often the case if you launch the copy without setting the registry path properly. If you are planning to launch multiple copies, the registry path must be set beforehand to avoid sharing conflicts of gw.dat. Gw.exe will always use the path found in the registry to locate gw.dat.

Q: When I try to launch muliple copies at once through the launcher, not all of them launch successfully. I get the gw.dat is locked message for a different copy. Why?

A: The default delay between copies is set to 3000 milliseconds in the "regdelay" setting of the ini file. Increase this value for slower computers since Guild Wars may need more time to read the value off the registry. The value determines the delay time between launching of multiple copies through the launcher.

Q: I get an immediate crash when I run the program.

A: There are issues with .NET framework configuration or install. If you get "Problem Signature 09: System.Security.Security" in details, you may be running this off a network drive (older .NET framework defaults disallow this). Try installing .NET framework 3.5 sp1 if you are not comfortable adjusting .NET security settings.

Q: What is a Mutex?

A: Simple answer, a flag that guild wars puts up which prevents another guild wars copy from starting. Long answer, check wikipedia.

Q: How do I report a bug?

A: Please report all bugs at http://code.google.com/p/gwmultilaunch/issues/list.

## Website ##

http://code.google.com/p/gwmultilaunch/

## Contributors ##

  * IMKey
  * satomz - icon art!

## Special Thanks ##

  * moriz - thank you for being the brave soul to test this in windows 7
  * Alexander Burn Victim - thank you for testing in vista 64-bit
  * Aciid Bu5t0r - thank you for bringing up the idea of forcefully unlocking gw.dat
  * MithranArkanere - thank you for detailing the registry issue
  * gergely.nagy - thank you for the idea and sample code to enable a cycling master shortcut

## History ##
v0.6 (2010/04/10)
  * Fixed "-character" switch argument passing for shortcuts
    * Shortcuts to launch individual copies may need to be remade to be compatible with new version
  * Allows storing multiple profiles of same path
    * Enables associating different arguments with same copy
  * Added arguments wizard
  * New art for buttons (less intense colors)

v0.6RC (2009/09/26)
  * Fixed registry path setting for certain setups of Guild Wars on 64-bit oses (requires admin)
    * It is strongly recommended not to install GW to "Programs Files (x86)" folder on 64-bit
> > operating systems since this causes GW to set keys in the Wow6432Node node which requires
> > admin access to write to.
  * Fixed edited arguments not being used if "Launch" button is clicked while cursor
> > still in arguments textbox
  * Moved "Launch" button to left side and increased size
  * Added warning message to confirm copy removal

v0.6b (2009/08/23)
  * Interface refresh
  * Changed functionality
    * Clicking "Start TexMod" will also set the path and clear mutex, saves two clicks
  * New feature
    * master shortcut: shortcut to launch additional copies, automatically picks unlaunched copy
    * compatible with gwx2 modified exes

v0.5 (2009/06/12)
  * Milestone final release. (v1.0 release supporting launching without making copies on wishlist)
  * Fixed registry writing code to write to both CURRENT\_USER and LOCAL\_MACHINE if paths exist

v0.5RC (2009/05/22)
  * Dual releases now, one for 32-bit windows, one for 64-bit windows
  * Rewrote large portion of handle managing code to be 64-bit compatible
  * Misc speed-ups in "HandleManager.cs"

v0.45b (2009/05/17)
  * Fixed bug introduced with implementing gw.dat unlocking affecting normal operations in Vista 64-bit

v0.42b (2009/05/16)
  * Fixed "Make Copy" issue with Template folders not being found under Guild Wars folder

v0.4b (2009/05/16)
  * First beta. Feature lock for now. Only bugs fixes until first stable release.
  * Added ability to drag and drop into list of copies
  * Added ability to make copies of Guild Wars from the gui.
  * Added super experimental "Force gw.dat unlock" for launching the same copy multiple times (gw.dat corruption possible)

v0.35a (2009/05/14)
  * Added delay back in when launching multiple copies

v0.3a (2009/05/14)
  * Detect initial copy of Guild Wars automatically
  * Error dialog on launching same copy of Guild Wars
  * Added icons
  * Took out registry path change back to be more compatible with updates.

v0.2a (2009/05/12)
  * Fixed registry key location issue for Win Vista/7
  * Fixed unicode conversion for Win7

v0.1a (2009/05/09)
  * Initial Release

## License ##

This software is open source licensed under GPL v3.