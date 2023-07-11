# Website URL Analyzer
## Problem Statement
### A small user-friendly website that allows the user to type a URL into an input box and then do the following:
*  1- List all images from the target URL and show them to the user in a carousel or gallery control of some kind (borrow from the internet or write your own).
* 2- Count all the words (display the total) and display the top 10 occurring words and their count (either as a table or chart of some kind, again you choose or write your own)

# Techstack used 
- c#
- clean architecture with core and infrastructure layer
- Asp.net core web project (MVC)
- OpenQA.Selenium.Chrome driver

# Steps to run the application
- Clone the repository
- Build the solution
- Select "IIS Express" in the visual studio solution
- A new browser will open the application in "https://localhost:44388/"
- click on the "URL Analyze" link from the main menu
- Enter the desired URL e.g. "https://www.bbc.co.uk/news/entertainment-arts-66159357" and click on "Analyze" button
- The desired result should show the total words count, most used words, and image list

# Scope of improvement 
- At present the system may appear a bit slow as we have used chrome driver to get the correct HTML. This is required to crawl a modern website built on a headless concept.
- Unit test framework is pending
- minor validation like the empty text is out of scope

# output
 
