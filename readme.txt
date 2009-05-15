Guild Wars MultiLaunch
======================
Open Source License: GPLv3

-------
Summary
-------
Launching multiple copies of Guild Wars.
(no modifications to Guild Wars files!)

------------
Requirements
------------
1) Guild Wars
2) .NET Framework 2.0 or equivalent
3) Windows XP, Vista, or 7

-----
Usage
-----
Copies of the Guild Wars folder need to be made beforehand. The number of copies depends
on how many copies you would like to have open at once. Most likely, you will only need
to make 1 extra copy to be able to run two copies side by side.

After these copies are made, add them to the list. Then, make the multi-launch enabled
shortcuts which are good for launching multiple copies of Guild Wars. Or just select
the game and launch it from the gui.

Concisely:

1) Make copies of Guild Wars folder (just need gw.exe and gw.dat)
2) Add the gw.exe(s) to the list
3) Click "Make Shortcut" and open the shortcut
   or Hit Launch button to start Guild Wars
   (hint: possible to select multiple copies to launch at once)

Guild Wars Updates: When there is an update, you will need to most likely launch a second time
if you have multiple Guild Wars updating at the same time. See gw.dat question in the FAQ.

Texmod Usage: Check the FAQ section.

--------------
Basic Controls
--------------
Add - add the location of an existing copy
Remove - remove selected copy from list
Launch - start the selected copies
Make Desktop Shortcut - add a desktop shortcut to launch selected copy

---------------------------------------------------
Expert Controls
---------------------------------------------------
Set Registry Path - path should be set before manually starting a specific install
Clear Mutex - closes the mutex handle under all live gw.exe processes
Open Texmod - attempts to locate Texmod and open

--------------------------------------------------------------
Launch Sequence Details to bypass multiple process restriction
--------------------------------------------------------------

2) Set new gw path in registry
3) Mutex cleared from all gw.exe processes
4) Launch gw copy
6) Exit GWMultiLaunch (only if using shortcut)

---
FAQ
---
Q: Do I have to always use the launcher or the special shortcuts?
A: No, you only need to use the launcher or shortcuts when you wish to launch 
   multiple copies of Guild Wars.
   
Q: Why do I get a "gw.dat is locked" message?
A: This is often the case if you launch the copy without setting the 
   registry path properly. If you are planning to launch multiple
   copies, the registry path must be set properly to avoid conflicts.
   Gw.exe will always use the path found in the registry to locate gw.dat.

Q: What is a Mutex?
A: Simple answer, a flag that gw.exe puts up so new gw.exe processes know to 
   not launch. Long answer, check wikipedia.

Q: How do I use Texmod with this?
A: If you only need one copy with Texmod helping, just launch that first via 
   Texmod. Subsequent copies can be launched with the help of the launcher.
   
   To launch a Texmoded copy after a Guild Wars instance is already running:
   1) Select the copy you will be launching and click "Set Registry Path"
   2) Click "Clear Mutex"
   3) Open Texmod and launch the copy

Q: How do I report a bug?
A: Please report all bugs at http://code.google.com/p/gwmultilaunch/issues/list.

---
URL
---
http://code.google.com/p/gwmultilaunch/

------------
Contributors
------------
IMKey@GuildWarsGuru

--------------
Special Thanks
--------------
moriz - thank you for being the brave soul to test this in windows 7

-------
History
-------
Note: This software is GPL v3. It is open source so there are no questions 
about the safety of this program. The source is available at the project url. 
It is a C# solution for Visual Studio 2005.

Date			Version		Note
--------------------------------------------------------------------------------
2009/05/14		v0.35a		Added delay back in when launching multiple copies
2009/05/14		v0.3a		Detect initial copy of Guild Wars automatically
							Error dialog on launching same copy of Guild Wars
							Added icons
							Took out registry path change back to be more
							compatible with updates.
2009/05/12		v0.2a		Fixed registry key location issue for Win Vista/7
							Fixed unicode conversion for Win7
2009/05/09:     v0.1a		Initial Release
