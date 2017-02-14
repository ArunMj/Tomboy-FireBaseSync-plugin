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
        public void UploadToServer(List<FirebaseNoteObject> fnotes){
            //Console.Write (Utils.str (fnotes));Console .WriteLine (fnotes.Count);
            if(fnotes.Count == 0){
                Logger.Info (" *** No notes to upload.  :-(   ");
                return;
            }
            Hyena.Json.JsonObject consolidated = new Hyena.Json.JsonObject();

            foreach(FirebaseNoteObject fn in fnotes){
                consolidated.Add(fn.Guid,fn.ToJsonObj());
            } 

            seril.SetInput(consolidated);

            Console.WriteLine("$$$ UPDATE : " + seril.Serialize());
            var r = fbc.Update("tomboynotes",seril.Serialize());
            Console.WriteLine (" $$$ GOT : " + r.Body);
        }

        public List<string> getUidsFromServer (){
            String resp = fbc.Get("tomboynotes",QueryBuilder.New().Shallow(true)).Body;
            if (resp.Trim() == "null"){
                Logger.Info ("*** No notes/guids found in server");
                return new List<string>();
            }
            Console.WriteLine("$$$ GOT :" + resp);
            dese.SetInput(resp);
            Hyena.Json.JsonObject uidDict = (Hyena.Json.JsonObject)dese.Deserialize();
            var keysCol = uidDict.Keys;
            var keysList = new List<String>(keysCol);
            return keysList;
        }

        public void DeleteFromSerever(List<String> guids){
            Hyena.Json.JsonObject dele = new Hyena.Json.JsonObject ();
            foreach (var guid in guids) {
                dele.Add (guid, null);
            }
            seril.SetInput (dele);
            Console.WriteLine ("$$$ UPDATE : " + seril.Serialize ());
            var r = fbc.Update ("tomboynotes", seril.Serialize ());
            Console.WriteLine (" $$$ GOT : " + r.Body);
        }
	}
}
