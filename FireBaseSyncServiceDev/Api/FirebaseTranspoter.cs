using System;
using System.Collections;
using System.Collections.Generic;
using FireSharpSimple;
using Gtk;

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
            if(fnotes.Count == 0){
                Logger.Info (" *** No notes to upload.  :-(   ");
                return;
            }
            Hyena.Json.JsonObject consolidated = new Hyena.Json.JsonObject();

            foreach(FirebaseNoteObject fn in fnotes){
                consolidated.Add(fn.Guid,fn.ToJsonObj());
            } 

            seril.SetInput(consolidated);

            Logger.Debug("$$$ UPDATE@UploadToServer: " + seril.Serialize());
            var r = fbc.Update("tomboynotes",seril.Serialize());
            Logger.Debug ("$$$ GOT@UploadToServer : " + r.Body);
        }

        public List<string> GetUidsFromServer (){
            String resp = fbc.Get("tomboynotes",QueryBuilder.New().Shallow(true)).Body;
            if (resp.Trim() == "null"){
                Logger.Info ("*** No notes/guids found in server");
                return new List<string>();
            }
            Logger.Debug("$$$ [GOT]@GetUidsFromServer :" + resp);
            dese.SetInput(resp);
            Hyena.Json.JsonObject uidDict = (Hyena.Json.JsonObject)dese.Deserialize();
            var keysCol = uidDict.Keys;
            var keysList = new List<String>(keysCol);
            return keysList;
        }

        public void DeleteFromSerever(List<String> guids){
            if (guids.Count == 0) {
                Logger.Info (" *** No notes to delete in server.  :-|   ");
                return;
            }
            Hyena.Json.JsonObject dele = new Hyena.Json.JsonObject ();
            foreach (var guid in guids) {
                dele.Add (guid, null);
            }
            seril.SetInput (dele);
            Logger.Debug ("$$$ UPDATE@DeleteFromSerever : " + seril.Serialize ());
            var r = fbc.Update ("tomboynotes", seril.Serialize ());
            Logger.Debug ("$$$ GOT@DeleteFromSerever : " + r.Body);
        }
	}
}
