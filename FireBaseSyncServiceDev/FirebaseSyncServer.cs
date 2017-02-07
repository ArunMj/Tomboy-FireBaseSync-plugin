using System;
using Gtk;
using Tomboy.Sync;
using System.Collections.Generic;
using System.Collections;

namespace Tomboy.FirebaseAddin
{
    public class FirebaseSyncServer: SyncServer
    {
        public FirebaseSyncServer ()
        {
            //throw new Exception ("dshjfdsjhfjdgfjhgdjhg");
        }

        #region interface membes 
        //
        // Properties
        //
        public SyncLockInfo CurrentSyncLock {
            get {
                //TODO
                return null;
            }
        }

        public string Id {
            //TODO
            get;
        }

		// NOTE: Only reliable during a transaction
        public int LatestRevision {
            //TODO
            get;
        }

        //
        // Methods
        //
        public bool BeginSyncTransaction (){
            //TODO
            return true;
        }

        public bool CancelSyncTransaction (){
            //TODO
            return false;
        }

        public bool CommitSyncTransaction (){
            //TODO
            return true;
        }

        public void DeleteNotes (IList<string> deletedNoteUUIDs){}

        public IList<string> GetAllNoteUUIDs (){
            //TODO
            return new List<string>();
        }

        public IDictionary<string, NoteUpdate> GetNoteUpdatesSince (int revision){
            //TODO
            return new Dictionary<string, NoteUpdate>();
        }

        public bool UpdatesAvailableSince (int revision){
            //TODO
            return false;
        }

        public void UploadNotes (IList<Note> notes){
            //TODO
        }

        #endregion


        // ------------------ custom members -----------------
    }
}

