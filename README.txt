##################### Flow of the application #####################

Log into ExileUserMgt
1 Go to Home Page
2 There is a Log On link to log into the system.
3 Log On link directs you to Log On page.
4 Provide “Account Information” if you are already a member and click Log On button to log in.
“Remember me” option allows the browser to store the credentials which can be used to log into the system again.
5 If not a user register using “Register” link providing unique username and password
6 Successful log in redirects users to the Home page

Logged in user can search his contacts, add contacts and view his contact list
Adding a contact
1 Go to Add Contact page
2 Enter contact name and address
3 Click on “Create” button 
You will be directed to Contact List page 

Search Contacts
1 Go to search page
2 Enter the string to be searched
3 Click on “Search” button
4 Search takes place on every contact name and address
5 Results are displayed at the same page

Edit and delete contact
1 Any contact displayed in Search or Contact List pages have “Edit”, “Delete” links
2 For editing a contact provide new information and click save
3 Before deletion, user is prompted to confirm the deletion
4 User can delete all the contacts using “Delete All” link on Contact List page 

##################### Deployment #####################

Installing required tools
1 MSSQL Server Management Studio 2012
2 Visual Studio 2012
3 .Net 4.0 framework

Create Databases
1 Create a database "ExileContactMgt"
	(Required tables will be created automatically)
	
#################### Updating and testing ########################

1 Complete pre requisites stated in deployment section
2 Install NuGet package manager in Visual Studio 2012
3 Use NuGet for installing required packages
  NHibernate - As an ORM
  FluentNhibernate - for runtime creation of database tables
  FluentAssertions - for unit testing
  NUnit - for unit testing 
  SimpleBrowser - for web testing
4 Use project "ContactManagementTests" for testing


 