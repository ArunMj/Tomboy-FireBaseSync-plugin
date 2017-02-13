using System;
using System.Collections;
using System.Collections.Generic;
using FireSharpSimple;

namespace Tomboy.FirebaseAddin.Api
{
	public class FirebaseTranspoter
	{
        private Hyena.Json.Serializer seril = new Hyena.Json.Serializer();
        private Hyena.Json.Deserializer dese = new Hyena.Json.Deserializer();
        private FirebaseClient fbc;
		public FirebaseTranspoter(ConnectionProps conProps)
		{
            fbc = new FirebaseClient(new FirebaseConfig(){
                BasePath = conProps.FirebaseUrl
            });
		}
        public void upload(List<FirebaseNoteObject> fnotes){
            Hyena.Json.JsonObject consolidated = new Hyena.Json.JsonObject();

            foreach(FirebaseNoteObject fn in fnotes){
                consolidated.Add(fn.Guid,fn.ToJsonObj());
            } 

            seril.SetInput(consolidated);

            Console.WriteLine(seril.Serialize());
            fbc.Update("tomboynotes",seril.Serialize());
        }

        public List<string> uidsInserver (){
            String resp = fbc.Get("tomboynotes",QueryBuilder.New().Shallow(true)).Body;
            if (resp.Trim() == "null"){
                return new List<string>();
            }
            Console.WriteLine(resp);
            dese.SetInput(resp);
            Hyena.Json.JsonObject uuidict = (Hyena.Json.JsonObject)dese.Deserialize();
            uuidict.Dump();
            var keysCol = uuidict.Keys;
            var keysList = new List<String>(keysCol);
            return keysList;
           // uuidict.
        }
	}
}
