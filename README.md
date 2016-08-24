# WiseNotes
A simple WIKI program I created for IT departments

Readme for WiseNotes - Making Note Taking Easy!


******** IF UPGRADING, PLEASE UNINSTALL THE OLD VERSION FIRST ********

*** NEW VERSION *** See Changelog for new version that uses config file to make file storage more customizable
To Install:

First: in the Zip file you downloaded, !!!you need to run the localdb.msi in order to install the local db version of Sql Express 2016.!!!

Second: ***Install WiseNotes. Upon completion, it you can launch it (from desktop or start menu, Windows 10, start menu only)
	*** If you are on Windows 10, navigate to c:\program files (x86)\wisetech\wisenotes\ and right click on WiseNotes.exe and run as admin. Windows 10 privileges are a little different than Win7 and in order to assign the proper 
	permissions to the folder with the database files, you need to do this step.
	

To create a note: you can either type the note directly into the rich text box or you can create it in Word, Wordpad, etc.. and copy/paste it into the rich text box and click save. type the name of the name and remember that name.

To add the note to the wiki: First create a category (unless the category for this note already exists). Do this by clicking new category and typing the name and click create.

Then, to add the note, click the "+" button and click the drop down to choose the name of the category. Type a title for the note and then click browse to find the file you just created. Click open and then you will be able to 
add the note!

That's it! Enjoy!

If you run into any issues, don't hesitate to post a question on SourceForge!



*************************** COMMON ERRORS **********************************
Database cannot be upgraded because it is read-only: This is because it did not assign the proper permissions to the c:\programdata\wisenotes folder Close Wisenotes, 
navigate to c:\program files (x86)\wisetech\wisenotes\ and right click on WiseNotes.exe and run as admin.

Unhandled exception..... A network-related or instance-specific error occurred while establishing connection.... The esrver was not found, verify the instance name, unable to locate Local Database Runtime Installation, 
verify SQL Server Express is properly installed.
** To fix this: in the zip is a file called sqllocaldb.msi run this and try again. 
