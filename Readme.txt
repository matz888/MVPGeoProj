These files represent the 'minimum viable product' for displaying local authority data on an openlayers base map.  No real attention has been paid to either the openlayers implementation or any leveraging of the Net Topology Suites capabilities in conjunction with the REST service.  No apologies are made for the lack of finnese in these top two layers as the bulk of the exercise centred around crash coursing myself in Postgres and becoming conversant with its environment and tools which I've managed to acheive in under 24 hours.  
The back up of the database has a PostGIS database with one table that was imported from Shape Data files.  The import was acheived via the shp2pgsql-gui tool and the data was marked up with an SRID of 27700 at import.  At the point of querying the data was then transformed to the standard 4326 reference system.  The learning curve in shaping the data at the PostGIS level has been incredibly beneficial and the only outstanding task on this front is to take the dynamic SQL passed by the REST service and turn it into embedded procedural code instead.  Once that has been acheived it would then be possible to tune the database in order to serve up the data in a much faster timescale.
The return of data in the REST layer has been expedited by the use of string pre-sizing, but again there is perhaps some further work to be done such as 'passing by reference' in order to stop any unecessary and costly memory allocations given the size of the data set.
With regards the manipulation of objects and geometries in the Net Topology Suite I can see that this would be useful at an individual  local authority level, but the national local authority data is a massive data set, so given more time I would have made another endpoint for individual adminstrative areas that would have been given the ability to manipulate data within their boundaries and this could then have been sensibly leveraged by the Net Topology Suite.
As mentioned at the head the code isn't at all polished and is missing logging, validation checks, unit tests and a release build, but within the time constraints I've fleshed out the MVP against which further progress could be made.  

Running the code.

Code is ready to run in debug mode out of the box, but for the open layers client the environment may first need to be initialised using the $>npx create-ol-app command.  If this is necessary then simply copy over and replace the default index.html and main.js files once the default project has been initiated.

The C# REST Service can be run in debug on IIS Express and CORS headers have been added so http and https will play together in this instance.

In DataService.cs make sure that you change the connection string to reflect your PostGIS database settings.

The shape files to import into the database are in the folder 'BoundaryData' and can be imported using the shp2pgsql-gui.exe tool.  Go with the default settings, but table name must be LABoundsData and SRID needs to be 27700.

