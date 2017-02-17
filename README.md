# Rhino-Licensing
A software licensing framework [https://www.hibernatingrhinos.com/oss/rhino-licensing](https://www.hibernatingrhinos.com/oss/rhino-licensing) [http://ayende.com](http://ayende.com)

how to build
---------------
The solution can be directly compiled with visual studio
note: the psake build is currently broken

To create the nuget package run buildNuget.bat


how to run the tests
---------------
Please note that in order to run the tests you must either run them in the context of the administrator or run the following command:
netsh http add urlacl url=http://+:19292/license user=YOUR_USER_NAME


acknowledgements
---------------
Rhino Licensing is making use of the excellent log4net logging framework
