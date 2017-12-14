# PCSS_Miniproject_MTA17339
The Programming for complex software systems mini-project made by group MTA-17339 AAU.

To use this software do following: 

Download or clone both folders:

Client: ComplexSoftwareSystems_Miniproject_Clint_MTA17339 <br />
Server: ComplexSoftwareSystems_Miniproject_MTA17339

Run the server first, then run as many clients as needed.

The server serves as a database entry reciever, use the client to enter information about an animal.
When all information has been entered the client will serialize the animal object using json and send it to the server.
When the server recieves the json-string it will deserialize it and print all of its information.
After this the server will check if a database json file exists, 
if it does it will read all of the text and convert this text to a Json Array then Deserialize into the animal objects they once were.
Then put the objects into a list and add the new entry to the list, 
and finally serialize to a json string and overwrite the json string to the file. If it dosen't exits, 
It will just make a List of the single object, serialize it and write it to a file.

To print this database write print and press enter in the server console. 
