# Windows Hook Keylogger
The Internet is full of such stuff. So why not introduce to you another one? Go! This is another example of using Win32 API functions to hook some system messages.  
Just run it once to start listening to the keyboard and clipboard and writing all content into a text log file. Recording will stop the second time you execute it.  
## So what was it all for?
The format of the log file is convenient so that you can easily find the record you are looking for. For this, the entered text is saved taking into account upper and lower case letters, in accordance with the selected input language. When any other text will be copied to the clipboard, it will also be stored to the log file. Thus, you get readable text like "what you see is what you get", rather than just a list of pressed buttons, as most similar programs do. Last but not least, all records are titled with the date, time and the title of the window where it was taken from.  
_______________
And don't forget that spying is a morally dubious idea and might even be illegal in your country!
