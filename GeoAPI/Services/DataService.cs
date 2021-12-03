
using Npgsql;
using System.Text;

namespace GeoAPI.Services
{
    public class DataService : IDataService
    {
        //Assigning this variable is an expensive copy operation given the size of the geoJson data, so 
        //a refactor that passes by might take some pressure off of memory allocation overheads
        private string lABoundaryList = "";
        public DataService()
        {
            //IMPORTANT
            //Replace with your local postgis db settings.  Typically this infromation would be held in a properties file.
            var cs = "Host=localhost;Username=postgres;Password=T0g4d1d1mus;Database=gisdb";
            using var conn = new NpgsqlConnection(cs);            
            conn.Open();
            conn.TypeMapper.UseGeoJson();

            //This is prototyping code and should be moved down to a function in the PostGIS database now proved to be functioning
            //Additional indexes and tuning can then be undertaken on performance.
            var sql = "SELECT json_build_object(";
            sql = sql + "'type','Feature',";
            sql = sql + "'properties', json_build_object(";
            sql = sql + "'OBJECTID', objectid,";
            sql = sql + "'LAD21CD', lad21cd,";
            sql = sql + "'LAD21NM', lad21nm,";
            sql = sql + "'BNG_E', bng_e,";
            sql = sql + "'BNG_N', bng_n,";
            sql = sql + "'LONG', long,";
            sql = sql + "'LAT', lat,";
            sql = sql + "'Shape_Area', shape_area,";
            sql = sql + "'Shape_Length', shape_leng),";
            sql = sql + "'geometry',   ST_AsGeoJSON(ST_transform(geom,4326))::json)";
            sql = sql + "FROM public.\"LABoundsData\"";

            //Do an optimisation to avoid any repartitioning of memory during the append operations
            StringBuilder boundaryGeoJsonStr = new StringBuilder("{\"type\":\"FeatureCollection\",\"features\":[", 184000000);

            using (var cmd = new NpgsqlCommand(sql, conn))
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    boundaryGeoJsonStr.Append(reader.GetFieldValue<string>(0));
                    //JObject newJStuff = JObject.Parse(someValue);
                    boundaryGeoJsonStr.Append(",");

                }
               
                boundaryGeoJsonStr.Length--;
            }
            boundaryGeoJsonStr.Replace('\"', '"');
            boundaryGeoJsonStr.Append("]}");
            
            lABoundaryList = boundaryGeoJsonStr.ToString();
            conn.Close();
        }
        public string Get()
        {
            if (lABoundaryList != null)
                return lABoundaryList;
            return null;
        }
        
    }

}
